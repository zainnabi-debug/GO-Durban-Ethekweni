using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class OperatingLicense : System.Web.UI.Page
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

        private bool ValidateOperatingLicense()
        {
            bool valid = true;

            if ((txtDescription.Text == string.Empty))
            {
                txtDescription.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtDescription.Style.Add("border", "");
            }

            if ((txtOLCode.Text == string.Empty))
            {
                txtOLCode.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtOLCode.Style.Add("border", "");
            }

            if ((txtRouteCode.Text == string.Empty))
            {
                txtRouteCode.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtRouteCode.Style.Add("border", "");
            }
            return valid;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateOperatingLicense())
            {
                OperatingLicenseBL BL = new OperatingLicenseBL();
                tbl_OperatingLicense TBL = new tbl_OperatingLicense();

                //Check button text to add or update
                string temp = btnAdd.Text;

                if (temp.Contains("Add"))
                {
                    var operatinglicense = db.tbl_OperatingLicense.ToList().FindAll(x => String.Compare(x.OLCode, (string)txtOLCode.Text, StringComparison.OrdinalIgnoreCase) == 0);

                    if (operatinglicense.Count > 0)
                    {
                        txtOLCode.Text = string.Empty;
                        txtOLCode.Focus();

                        divsuccess.Style.Add("display", "none");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "none");
                        divwarning.Style.Add("display", "inline");
                    }
                    else
                    {
                        TBL.Description = txtDescription.Text;
                        TBL.OLCode = txtOLCode.Text;
                        TBL.RouteCode = txtRouteCode.Text;
                        BL.CreateOperatingLicense(TBL);

                        divsuccess.Style.Add("display", "inline");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "none");
                        divwarning.Style.Add("display", "none");
                    }
                }
                else if (temp.Contains("Update"))
                {
                    int Id = int.Parse(ViewState["OperatingLicenseID"].ToString());
                    TBL.OperatingLicenseID = Id;
                    TBL.Description = txtDescription.Text;
                    TBL.OLCode = txtOLCode.Text;
                    TBL.RouteCode = txtRouteCode.Text;
                    BL.UpdateOperatingLicense(TBL);

                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "inline");
                    divwarning.Style.Add("display", "none");

                    btnAdd.Text = "Add";
                }

                LoadData();
                Clear();
            }
        }

        public void Clear()
        {
            txtDescription.Text = string.Empty;
            txtOLCode.Text = string.Empty;
            txtRouteCode.Text = string.Empty;
        }

        protected void RemoveBorder()
        {
            txtDescription.Style.Add("border", "");
            txtOLCode.Style.Add("border", "");
            txtRouteCode.Style.Add("border", "");
        }

        private void LoadData()
        {
            OperatingLicenseBL BL = new OperatingLicenseBL();
            List<tbl_OperatingLicense> list = BL.LoadOperatingLicense();
            gvOL.DataSource = list;
            gvOL.DataBind();
        }

        protected void gvOL_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvOL.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvOL.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvOL.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvOL.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvOL_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OperatingLicenseBL BL = new OperatingLicenseBL();
            tbl_OperatingLicense TBL = new tbl_OperatingLicense();

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvOL.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvOL.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["OperatingLicenseID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);
                
                    var temp = db.tbl_OperatingLicense.ToList().FirstOrDefault(x => x.OperatingLicenseID == Convert.ToInt16(Id));

                    if (temp != null)
                    {
                        RemoveBorder();
                        Session["OperatingLicenseID"] = Id;
                        txtDescription.Text = temp.Description;
                        txtOLCode.Text = temp.OLCode;
                        txtRouteCode.Text = temp.RouteCode;
                        BL.UpdateOperatingLicense(temp);
                        btnAdd.Text = "Update";
                    }
            }

            else if (commandName == "DeleteItem")
            {
                string Source = gvOL.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["OperatingLicenseID"] = Id;
                
                var temp = db.tbl_OperatingLicense.ToList().FirstOrDefault(x => x.OperatingLicenseID == Convert.ToInt16(Id));

                db.tbl_OperatingLicense.Remove(temp);
                db.SaveChanges();
                RemoveBorder();
                LoadData();
                Clear();
                btnAdd.Text = "Add";

                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "inline");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/OperatingLicense.aspx");
        }
    }
}