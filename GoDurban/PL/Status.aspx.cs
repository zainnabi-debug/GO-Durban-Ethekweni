using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Status : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
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
                if (ValidateStatus())
                {
                    // creating an object for business logic

                    StatusBL BL = new StatusBL();
                    tbl_Status TBL = new tbl_Status();


                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var status = db.tbl_Status.ToList().FindAll(x => String.Compare(x.StatusDescription, (string)txtStatus.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (status.Count > 0 || string.IsNullOrWhiteSpace(txtStatus.Text))
                        {
                            txtStatus.Text = string.Empty;
                            txtStatus.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        { 
                            TBL.StatusDescription = txtStatus.Text.Trim();

                            //Call create method
                            BL.CreateStatus(TBL);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        var status = db.tbl_Status.ToList().FindAll(x => String.Compare(x.StatusDescription, (string)txtStatus.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (status.Count > 0 || string.IsNullOrWhiteSpace(txtStatus.Text))
                        {
                            txtStatus.Text = string.Empty;
                            txtStatus.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            int Id = int.Parse(ViewState["StatusID"].ToString());
                            TBL.StatusID = Id;
                            TBL.StatusDescription = txtStatus.Text.Trim();

                            BL.UpdateStatus(TBL);

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "inline");
                            divwarning.Style.Add("display", "none");

                            btnAdd.Text = "Add";
                        }
                    }

                    //Refresh Grid
                    LoadData();

                    Clear();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "autoclosable-btn-success", "show();", true);
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

        private bool ValidateStatus()
        {
            bool valid = true;

            if ((txtStatus.Text == string.Empty))
            {
                txtStatus.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtStatus.Style.Add("border", "");
            }
            return valid;
        }

        protected void Clear()
        {
            txtStatus.Text = string.Empty;
            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtStatus.Style.Add("border", "");
        }

        private void LoadData()
        {
            StatusBL BL = new StatusBL();
            List<tbl_Status> list = BL.LoadStatus();
            gvStatus.DataSource = list;
            gvStatus.DataBind();
        }

        protected void gvStatus_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvStatus.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvStatus.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvStatus.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvStatus.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            StatusBL BL = new StatusBL();
            tbl_Status TBL = new tbl_Status();

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvStatus.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvStatus.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["StatusID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);

                var temp = db.tbl_Status.ToList().FirstOrDefault(x => x.StatusID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");
                    RemoveBorder();
                    Session["StatusID"] = Id;
                    txtStatus.Text = temp.StatusDescription;
                    btnAdd.Text = "Update";

                }
            }

            //else if (commandName == "DeleteItem")
            //{
            //    string Source = gvStatus.Rows[myRow.RowIndex].Cells[0].Text;

            //    ViewState["StatusID"] = Id;

            //    var temp = db.tbl_Status.ToList().FirstOrDefault(x => x.StatusID == Convert.ToInt16(Id));

            //    db.tbl_Status.Remove(temp);
            //    db.SaveChanges();
            //    RemoveBorder();
            //    LoadData();
            //    Clear();
            //}
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/Status.aspx");
        }
    }
}