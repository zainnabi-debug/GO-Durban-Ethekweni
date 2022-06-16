using GoDurban.BL;
using GoDurban.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class ServiceArea : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                Response.Redirect("~/Index.aspx?ShowDialog=yes");
            }
            else
            {
                string type = Session["UserRole"].ToString();

                if (type != "Admin" && type != "User" && type != "Supervisor")
                {
                    Response.Redirect("~/Error.aspx");
                }
            }

            if (!IsPostBack)
            {
                LoadData();
            }
            lblError.Visible = false;
        }

        public void DisableForm(ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Enabled = false;
                if (ctrl is Button)
                    ((Button)ctrl).Enabled = false;
                else if (ctrl is DropDownList)
                    ((DropDownList)ctrl).Enabled = false;
                else if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Enabled = false;
                else if (ctrl is RadioButton)
                    ((RadioButton)ctrl).Enabled = false;
                else if (ctrl is RadioButtonList)
                    ((RadioButtonList)ctrl).Enabled = false;
                else if (ctrl is LinkButton)
                    ((LinkButton)ctrl).Enabled = false;
                else if (ctrl is FileUpload)
                    ((FileUpload)ctrl).Enabled = false;
                else if (ctrl is GridView)
                    ((GridView)ctrl).Enabled = false;

                DisableForm(ctrl.Controls);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateServiceArea())
                {
                    ServiceAreaBL BL = new ServiceAreaBL();
                    tbl_ServiceArea TBL = new tbl_ServiceArea();

                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var servicearea = db.tbl_ServiceArea.ToList().FindAll(x => String.Compare(x.ServiceAreaDescription, (string)txtServiceArea.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (servicearea.Count > 0 || string.IsNullOrWhiteSpace(txtServiceArea.Text))
                        {
                            txtServiceArea.Text = string.Empty;
                            txtServiceArea.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            TBL.ServiceAreaDescription = txtServiceArea.Text.Trim();
                            BL.CreateServiceArea(TBL);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        try
                        {
                            var servicearea = db.tbl_ServiceArea.ToList().FindAll(x => String.Compare(x.ServiceAreaDescription, (string)txtServiceArea.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                            if (servicearea.Count > 0 || string.IsNullOrWhiteSpace(txtServiceArea.Text))
                            {
                                txtServiceArea.Text = string.Empty;
                                txtServiceArea.Focus();

                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "inline");
                            }
                            else
                            {
                                int Id = int.Parse(ViewState["ServiceAreaID"].ToString());
                                TBL.ServiceAreaID = Id;
                                TBL.ServiceAreaDescription = txtServiceArea.Text.Trim();
                                BL.UpdateServiceArea(TBL);
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "inline");
                                divwarning.Style.Add("display", "none");

                                btnAdd.Text = "Add";
                            }
                        }
                        catch (Exception ex)
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");

                            lblError.Visible = true;
                            //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                            lblError.Text = "Network problem or services are currently down, please contact support team.";
                        }
                    }

                    LoadData();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Network problem or services are currently down, please contact support team.";
            }
        }

        private bool ValidateServiceArea()
        {
            bool valid = true;

            if ((txtServiceArea.Text == string.Empty))
            {
                txtServiceArea.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtServiceArea.Style.Add("border", "");
            }
            return valid;
        }

        protected void Clear()
        {
            txtServiceArea.Text = string.Empty;
            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtServiceArea.Style.Add("border", "");
        }

        private void LoadData()
        {
            ServiceAreaBL BL = new ServiceAreaBL();
            List<tbl_ServiceArea> list = BL.LoadServiceArea();
            gvServiceArea.DataSource = list.ToList().OrderByDescending(x => x.ServiceAreaID);
            gvServiceArea.DataBind();
        }


        protected void gvServiceArea_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ServiceAreaBL BL = new ServiceAreaBL();
            tbl_ServiceArea TBL = new tbl_ServiceArea();

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvServiceArea.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvServiceArea.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["ServiceAreaID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);
               
                var temp = db.tbl_ServiceArea.ToList().FirstOrDefault(x => x.ServiceAreaID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");

                    RemoveBorder();
                    Session["ServiceAreaID"] = Id;
                    txtServiceArea.Text = temp.ServiceAreaDescription;
                    btnAdd.Text = "Update";
                }
            }

            else if (commandName == "DeleteItem")
            {
                string Source = gvServiceArea.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["ServiceAreaID"] = Id;
                
                try
                {
                    string html = string.Empty;
                    string url = @"http://r6efmprdbw.durban.gov.za:44007/V1/esbapi/DeleteServiceArea?ServiceAreaID=" + Id;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip;


                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream stream = response.GetResponseStream())
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                html = reader.ReadToEnd();
                            }
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", html, true);

                            dynamic stuff = JsonConvert.DeserializeObject(html);

                            string RoleName = stuff.ToString();

                            //if(RoleName== "1")
                            // {


                            //}
                        }
                }
                catch (Exception ex)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");

                    throw new Exception("Can't delete record.", ex);
                }

                RemoveBorder();
                LoadData();
                Clear();

                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "inline");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
            }
        }
               

        protected void gvServiceArea_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvServiceArea.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvServiceArea.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvServiceArea.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvServiceArea.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/ServiceArea.aspx");
        }
    }
}