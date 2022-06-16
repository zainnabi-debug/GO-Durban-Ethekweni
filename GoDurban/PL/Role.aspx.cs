using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Role : System.Web.UI.Page
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
                if (ValidateRole())
                {
                    // creating an object for business logic

                    RoleBL BL = new RoleBL();
                    tbl_Role TBL = new tbl_Role();


                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var role = db.tbl_Role.ToList().FindAll(x => String.Compare(x.RoleDescription, (string)txtRole.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (role.Count > 0 || string.IsNullOrWhiteSpace(txtRole.Text))
                        {
                            txtRole.Text = string.Empty;
                            txtRole.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            TBL.RoleDescription = txtRole.Text.Trim();

                            //Call create method
                            BL.Add(TBL);
                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        var role = db.tbl_Role.ToList().FindAll(x => String.Compare(x.RoleDescription, (string)txtRole.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (role.Count > 0 || string.IsNullOrWhiteSpace(txtRole.Text))
                        {
                            txtRole.Text = string.Empty;
                            txtRole.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            int Id = int.Parse(ViewState["RoleID"].ToString());
                            TBL.RoleID = Id;
                            TBL.RoleDescription = txtRole.Text.Trim();

                            BL.Update(TBL);
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

        private bool ValidateRole()
        {
            bool valid = true;

            if ((txtRole.Text == string.Empty))
            {
                txtRole.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtRole.Style.Add("border", "");
            }
            return valid;
        }

        protected void Clear()
        {
            txtRole.Text = string.Empty;
            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtRole.Style.Add("border", "");
        }

        private void LoadData()
        {
            RoleBL BL = new RoleBL();
            List<tbl_Role> list = BL.LoadRole();
            gvRole.DataSource = list;
            gvRole.DataBind();
        }

        protected void gvRole_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvRole.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvRole.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvRole.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvRole.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            RoleBL BL = new RoleBL();
            tbl_Role TBL = new tbl_Role();

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvRole.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvRole.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["RoleID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);

                var temp = db.tbl_Role.ToList().FirstOrDefault(x => x.RoleID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");
                    RemoveBorder();
                    Session["RoleID"] = Id;
                    txtRole.Text = temp.RoleDescription;
                    btnAdd.Text = "Update";
                }
            }

            //else if (commandName == "DeleteItem")
            //{
            //    string Source = gvRole.Rows[myRow.RowIndex].Cells[0].Text;

            //    ViewState["RoleID"] = Id;

            //    var temp = db.tbl_Role.ToList().FirstOrDefault(x => x.RoleID == Convert.ToInt16(Id));

            //    db.tbl_Role.Remove(temp);
            //    db.SaveChanges();
            //    RemoveBorder();
            //    LoadData();
            //    Clear();
            //    btnAdd.Text = "Add";

            //    divsuccess.Style.Add("display", "none");
            //    divdanger.Style.Add("display", "inline");
            //    divinfo.Style.Add("display", "none");
            //    divwarning.Style.Add("display", "none");

            //}
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/Role.aspx");
        }
    }
}