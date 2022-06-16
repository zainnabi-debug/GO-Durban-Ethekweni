using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Reason : System.Web.UI.Page
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
                if (ValidateReason())
                {
                    ReasonBL BL = new ReasonBL();
                    tbl_Reason TBL = new tbl_Reason();

                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var Reason = db.tbl_Reason.ToList().FindAll(x => String.Compare(x.ReasonDescription, (string)txtReason.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (Reason.Count > 0 || string.IsNullOrWhiteSpace(txtReason.Text))
                        {
                            txtReason.Text = string.Empty;
                            txtReason.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            TBL.ReasonDescription = txtReason.Text.Trim();
                            BL.CreateReason(TBL);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        var Reason = db.tbl_Reason.ToList().FindAll(x => String.Compare(x.ReasonDescription, (string)txtReason.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (Reason.Count > 0 || string.IsNullOrWhiteSpace(txtReason.Text))
                        {
                            txtReason.Text = string.Empty;
                            txtReason.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            int Id = int.Parse(ViewState["ReasonID"].ToString());
                            TBL.ReasonID = Id;
                            TBL.ReasonDescription = txtReason.Text.Trim();
                            BL.UpdateReason(TBL);
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "inline");
                            divwarning.Style.Add("display", "none");

                            btnAdd.Text = "Add";
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

        private bool ValidateReason()
        {
            bool valid = true;

            if ((txtReason.Text == string.Empty))
            {
                txtReason.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtReason.Style.Add("border", "");
            }
            return valid;
        }

        protected void Clear()
        {
            txtReason.Text = string.Empty;
            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtReason.Style.Add("border", "");
        }

        private void LoadData()
        {
            ReasonBL BL = new ReasonBL();
            List<tbl_Reason> list = BL.LoadReason();
            gvReason.DataSource = list;
            gvReason.DataBind();
        }

        protected void gvReason_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvReason.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvReason.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvReason.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvReason.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvReason_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ReasonBL BL = new ReasonBL();
            tbl_Reason TBL = new tbl_Reason();

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvReason.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvReason.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["ReasonID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);

                var temp = db.tbl_Reason.ToList().FirstOrDefault(x => x.ReasonID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");
                    RemoveBorder();
                    Session["ReasonID"] = Id;
                    txtReason.Text = temp.ReasonDescription;
                    btnAdd.Text = "Update";

                }
            }

            //else if (commandName == "DeleteItem")
            //{
            //    string Source = gvReason.Rows[myRow.RowIndex].Cells[0].Text;

            //    tbl_Owner TBL2 = new tbl_Owner();
            //    OwnerBL BL2 = new OwnerBL();
            //    tbl_Driver TBL1 = new tbl_Driver();
            //    DriverBL BL1 = new DriverBL();
            //    tbl_User TBL3 = new tbl_User();
            //    UserBL BL3 = new UserBL();

            //    ViewState["ReasonID"] = Id;

            //    var temp = db.tbl_Reason.ToList().FirstOrDefault(x => x.ReasonID == Convert.ToInt16(Id));
            //    db.tbl_Reason.Remove(temp);

            //    db.SaveChanges();
            //    RemoveBorder();
            //    LoadData();
            //    Clear();
            //    btnAdd.Text = "Add";
            //}
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/Reason.aspx");
        }
    }
}