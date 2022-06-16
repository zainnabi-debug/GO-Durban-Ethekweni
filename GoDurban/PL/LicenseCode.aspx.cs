using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class LicenseCode : System.Web.UI.Page
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
                if (ValidateLicenseCode())
                {
                    // creating an object for business logic

                    LicenseCodeBL BL = new LicenseCodeBL();
                    tbl_LicenseCode TBL = new tbl_LicenseCode();


                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var liccode = db.tbl_LicenseCode.ToList().FindAll(x => String.Compare(x.LicenseCode, (string)txtLicenseCode.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (liccode.Count > 0 || string.IsNullOrWhiteSpace(txtLicenseCode.Text.Trim()))
                        {
                            txtLicenseCode.Text = string.Empty;
                            txtLicenseCode.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            TBL.LicenseCode = txtLicenseCode.Text.Trim();
                            TBL.LicenseCodeDescription = txtLicenseCodeDescription.Text.Trim();

                            //Call create method
                            BL.CreateLicenseCode(TBL);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        var liccode = db.tbl_LicenseCode.FirstOrDefault(x => x.LicenseCode == txtLicenseCode.Text.Trim() && x.LicenseCodeDescription == txtLicenseCodeDescription.Text.Trim());

                        if (liccode != null)
                        {
                            txtLicenseCode.Text = string.Empty;
                            txtLicenseCode.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            int Id = int.Parse(ViewState["LicenseCodeID"].ToString());
                            TBL.LicenseCodeID = Id;
                            TBL.LicenseCode = txtLicenseCode.Text.Trim();
                            TBL.LicenseCodeDescription = txtLicenseCodeDescription.Text.Trim();

                            BL.UpdateLicenseCode(TBL);

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

        private bool ValidateLicenseCode()
        {
            bool valid = true;

            if ((txtLicenseCode.Text == string.Empty))
            {
                txtLicenseCode.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtLicenseCode.Style.Add("border", "");
            }
            if ((txtLicenseCodeDescription.Text == string.Empty))
            {
                txtLicenseCodeDescription.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtLicenseCodeDescription.Style.Add("border", "");
            }
            return valid;
        }

        protected void Clear()
        {
            txtLicenseCode.Text = string.Empty;
            txtLicenseCodeDescription.Text = string.Empty;
            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtLicenseCode.Style.Add("border", "");
            txtLicenseCodeDescription.Style.Add("border", "");
        }

        private void LoadData()
        {
            LicenseCodeBL BL = new LicenseCodeBL();
            List<tbl_LicenseCode> list = BL.LoadLicenseCode();
            gvLicenseCode.DataSource = list;
            gvLicenseCode.DataBind();
        }

        protected void gvLicenseCode_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvLicenseCode.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvLicenseCode.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvLicenseCode.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvLicenseCode.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvLicenseCode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LicenseCodeBL BL = new LicenseCodeBL();
            tbl_LicenseCode TBL = new tbl_LicenseCode();

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvLicenseCode.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvLicenseCode.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["LicenseCodeID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);
                
                var temp = db.tbl_LicenseCode.ToList().FirstOrDefault(x => x.LicenseCodeID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");
                    RemoveBorder();
                    Session["LicenseCodeID"] = Id;
                    txtLicenseCode.Text = temp.LicenseCode;
                    txtLicenseCodeDescription.Text = temp.LicenseCodeDescription;
                    btnAdd.Text = "Update";

                }
            }

            //else if (commandName == "DeleteItem")
            //{
            //    string Source = gvLicenseCode.Rows[myRow.RowIndex].Cells[0].Text;

            //    ViewState["LicenseCodeID"] = Id;
                
            //    var temp = db.tbl_LicenseCode.ToList().FirstOrDefault(x => x.LicenseCodeID == Convert.ToInt16(Id));

            //    db.tbl_LicenseCode.Remove(temp);
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
            Response.Redirect("~/PL/LicenseCode.aspx");
        }
    }
}