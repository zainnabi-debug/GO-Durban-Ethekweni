using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class VehicleInfo : System.Web.UI.Page
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
                
                if (type == "User")
                {
                    DisableForm(Page.Controls);
                }
                else if ((type != "Admin" && type != "Supervisor"))
                {
                    Response.Redirect("~/Register.aspx");
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

        public void Clear()
        {
            txtCapacity.Text = string.Empty;
            txtMake.Text = string.Empty;
            txtModel.Text = string.Empty;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateVehicleInfo())
                {
                    // creating an object for business logic

                    VehicleInfoBL BL = new VehicleInfoBL();
                    tbl_VehicleInfo TBL = new tbl_VehicleInfo();


                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var make = db.tbl_VehicleInfo.ToList().FindAll(x => String.Compare(x.Make, (string)txtMake.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                        var model = db.tbl_VehicleInfo.ToList().FindAll(x => String.Compare(x.Model, (string)txtModel.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                        var capacity = db.tbl_VehicleInfo.ToList().FindAll(x => String.Compare(x.Capacity, (string)txtCapacity.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (make.Count > 0 && model.Count > 0 && capacity.Count > 0 || string.IsNullOrWhiteSpace(txtMake.Text) || string.IsNullOrWhiteSpace(txtModel.Text) || string.IsNullOrWhiteSpace(txtCapacity.Text))
                        {
                            txtMake.Text = string.Empty;
                            txtMake.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            TBL.Capacity = txtCapacity.Text.Trim();
                            TBL.Make = txtMake.Text.Trim();
                            TBL.Model = txtModel.Text.Trim();

                            //Call create method
                            BL.CreateVehicleInfo(TBL);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        var make = db.tbl_VehicleInfo.FirstOrDefault(x => x.Make==txtMake.Text.Trim() && x.Model==txtModel.Text.Trim() && x.Capacity==txtCapacity.Text.Trim());

                        if (make!=null)
                        {
                            txtMake.Text = string.Empty;
                            txtMake.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            int VehicleInfoID = int.Parse(ViewState["VehicleInfoID"].ToString());

                            tbl_VehicleInfo vehicle =db.tbl_VehicleInfo.FirstOrDefault(x => x.VehicleInfoID== VehicleInfoID); 
                            vehicle.VehicleInfoID = VehicleInfoID;
                            vehicle.Capacity = txtCapacity.Text;
                            vehicle.Make = txtMake.Text;
                            vehicle.Model = txtModel.Text;

                            BL.UpdateVehicleInfo(vehicle);

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
        private bool ValidateVehicleInfo()
        {
            bool valid = true;

            if ((txtCapacity.Text == string.Empty))
            {
                txtCapacity.Style.Add("border", "1px solid red");
                valid = false;
            }
            else if ((txtCapacity.Text == "0"))
            {
                txtCapacity.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCapacity.Style.Add("border", "");
            }
            if ((txtMake.Text == string.Empty))
            {
                txtMake.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtMake.Style.Add("border", "");
            }
            if ((txtModel.Text == string.Empty))
            {
                txtModel.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtModel.Style.Add("border", "");
            }
            return valid;
        }

        protected void RemoveBorder()
        {
            txtCapacity.Style.Add("border", "");
            txtMake.Style.Add("border", "");
            txtModel.Style.Add("border", "");
        }

        private void LoadData()
        {
            VehicleInfoBL BL = new VehicleInfoBL();
            List<tbl_VehicleInfo> list = BL.LoadVehicleInfo();
            gvVehicleInfo.DataSource = list;
            gvVehicleInfo.DataBind();
        }

        protected void gvVehicleInfo_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvVehicleInfo.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvVehicleInfo.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvVehicleInfo.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvVehicleInfo.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvVehicleInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            VehicleInfoBL BL = new VehicleInfoBL();
            tbl_VehicleInfo TBL = new tbl_VehicleInfo();

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvVehicleInfo.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvVehicleInfo.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["VehicleInfoID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);
                using (GoDurbanEntities db = new GoDurbanEntities())
                {
                    var temp = db.tbl_VehicleInfo.ToList().FirstOrDefault(x => x.VehicleInfoID == Convert.ToInt16(Id));

                    if (temp != null)
                    {
                        divsuccess.Style.Add("display", "none");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "none");
                        divwarning.Style.Add("display", "none");
                        RemoveBorder();
                        Session["VehicleInfoID"] = Id;
                        txtCapacity.Text = temp.Capacity;
                        txtMake.Text = temp.Make;
                        txtModel.Text = temp.Model;
                        btnAdd.Text = "Update";

                    }
                }
            }

            //else if (commandName == "DeleteItem")
            //{
            //    string Source = gvVehicleInfo.Rows[myRow.RowIndex].Cells[0].Text;

            //    ViewState["VehicleInfoID"] = Id;

            //    using (GoDurbanEntities db = new GoDurbanEntities())
            //    {
            //        var temp = db.tbl_VehicleInfo.ToList().FirstOrDefault(x => x.VehicleInfoID == Convert.ToInt16(Id));

            //        db.tbl_VehicleInfo.Remove(temp);
            //        db.SaveChanges();
            //        RemoveBorder();
            //        LoadData();
            //        Clear();
            //        btnAdd.Text = "Add";

            //        divsuccess.Style.Add("display", "none");
            //        divdanger.Style.Add("display", "inline");
            //        divinfo.Style.Add("display", "none");
            //        divwarning.Style.Add("display", "none");

            //    }
            //}
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/VehicleInfo.aspx");
        }
    }
}