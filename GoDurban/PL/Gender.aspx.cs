using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Gender : System.Web.UI.Page
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

                if (ValidateGender())
                {
                    // creating an object for business logic

                    GenderBL BL = new GenderBL();
                    tbl_Gender TBL = new tbl_Gender();


                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var gender = db.tbl_Gender.ToList().FindAll(x => String.Compare(x.GenderDescription, (string)txtGender.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (gender.Count > 0 || string.IsNullOrWhiteSpace(txtGender.Text))
                        {
                            txtGender.Text = string.Empty;
                            txtGender.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            TBL.GenderDescription = txtGender.Text.Trim();

                            //Call create method
                            BL.CreateGender(TBL);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        var gender = db.tbl_Gender.ToList().FindAll(x => String.Compare(x.GenderDescription, (string)txtGender.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (gender.Count > 0 || string.IsNullOrWhiteSpace(txtGender.Text))
                        {
                            txtGender.Text = string.Empty;
                            txtGender.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            int Id = int.Parse(ViewState["GenderID"].ToString());
                            TBL.GenderID = Id;
                            TBL.GenderDescription = txtGender.Text.Trim();

                            BL.UpdateGender(TBL);

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
            catch(Exception ex)
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

        private bool ValidateGender()
        {
            bool valid = true;

            if ((txtGender.Text == string.Empty))
            {
                txtGender.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtGender.Style.Add("border", "");
            }
            return valid;
        }

        protected void Clear()
        {
            txtGender.Text = string.Empty;
            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtGender.Style.Add("border", "");
        }

        private void LoadData()
        {
            GenderBL BL = new GenderBL();
            List<tbl_Gender> list = BL.LoadGender();
            gvGender.DataSource = list;
            gvGender.DataBind();
        }

        protected void gvGender_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvGender.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvGender.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvGender.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvGender.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvGender_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GenderBL BL = new GenderBL();
            tbl_Gender TBL = new tbl_Gender();

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvGender.DataKeys[myRow.RowIndex].Value.ToString();

            //if (commandName == "EditItem")
            //{
            //    //Accessing BoundField Column
            //    string Source = gvGender.Rows[myRow.RowIndex].Cells[0].Text;

            //    ViewState["GenderID"] = Id;
            //    //int Id = Convert.ToInt32(e.CommandArgument);
                
            //    var temp = db.tbl_Gender.ToList().FirstOrDefault(x => x.GenderID == Convert.ToInt16(Id));

            //    if (temp != null)
            //    {
            //        divsuccess.Style.Add("display", "none");
            //        divdanger.Style.Add("display", "none");
            //        divinfo.Style.Add("display", "none");
            //        divwarning.Style.Add("display", "none");
            //        RemoveBorder();
            //        Session["GenderID"] = Id;
            //        txtGender.Text = temp.GenderDescription;
            //        btnAdd.Text = "Update";

            //    }
            //}

            //else if (commandName == "DeleteItem")
            //{
            //    string Source = gvGender.Rows[myRow.RowIndex].Cells[0].Text;

            //    ViewState["GenderID"] = Id;
                                
            //    var temp = db.tbl_Gender.ToList().FirstOrDefault(x => x.GenderID == Convert.ToInt16(Id));

            //    db.tbl_Gender.Remove(temp);
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
            Response.Redirect("~/PL/Gender.aspx");
        }
    }
}