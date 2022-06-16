using GoDurban.BL;
using GoDurban.Models;
using GoDurban.SMSService;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Vehicle : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        string currentDate = DateTime.Now.ToString("dd MMM yyyy");

        private bool btnCORUpload = false;
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

            if (ddlStatus.SelectedItem.Text == "Approved")
            {
                divReason.Style.Add("display", "none");
                divReason1.Style.Add("display", "none");
            }
            else
            {
                divReason.Style.Add("display", "inline");
                divReason1.Style.Add("display", "inline");
            }

            lblError.Visible = false;
            lblCORScan.Visible = false;
            lblCORScan1.Visible = false;

            RemoveBorder();

            if (!IsPostBack)
            {
                lblError.Visible = false;
                LoadOwner();
                LoadVehicleInfo();
                LoadData();
                LoadStatusAll();
                LoadReason();
                //RemoveDuplicateItems(ddlOwner);

                int year = DateTime.Now.Year;
                for (int i = 1900; i <= year; i++)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(i.ToString());
                    ddlYear.Items.Add(li);
                    ddlYearRegistered.Items.Add(li);
                }
                ddlYear.Items.FindByText(year.ToString()).Selected = true;
                ddlYearRegistered.Items.FindByText(year.ToString()).Selected = true;

                txtVehNoPlate.Style.Add("border", "");

                if (gvUploadVehicle.Rows.Count > 0)
                {
                    lblError.Visible = true;
                }
                else
                {
                    lblError.Visible = false;
                }
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

        public void RemoveDuplicateItems(DropDownList ddl)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                ddl.SelectedIndex = i;
                string str = ddl.SelectedItem.ToString();
                for (int counter = i + 1; counter < ddl.Items.Count; counter++)
                {
                    ddl.SelectedIndex = counter;
                    string compareStr = ddl.SelectedItem.ToString();
                    if (str == compareStr)
                    {
                        ddl.Items.RemoveAt(counter);
                        counter = counter - 1;
                    }
                }
            }
        }

        public void LoadStatusAll()
        {
            StatusBL BL = new StatusBL();
            ddlStatus.DataSource = BL.LoadStatus();
            ddlStatus.DataTextField = "StatusDescription";
            ddlStatus.DataValueField = "StatusID";
            ddlStatus.DataBind();
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
        }

        public void LoadStatusNotAll()
        {
            StatusBL BL = new StatusBL();
            ddlStatus.DataSource = BL.LoadStatusNotAll();
            ddlStatus.DataTextField = "StatusDescription";
            ddlStatus.DataValueField = "StatusID";
            ddlStatus.DataBind();
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
        }

        //public DataTable GetCertainStatuses()
        //{
        //    using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString))
        //    using (var cmd = new SqlCommand("SELECT StatusID, StatusDescription FROM tbl_Status WHERE StatusDescription <>'Approved'", conn))
        //    using (var adapter = new SqlDataAdapter(cmd))
        //    {
        //        var products = new DataTable();
        //        adapter.Fill(products);
        //        return products;
        //    }
        //}

        public void LoadReason()
        {
            ReasonBL BL = new ReasonBL();
            ddlReason.DataSource = BL.LoadReason();
            ddlReason.DataTextField = "ReasonDescription";
            ddlReason.DataValueField = "ReasonID";
            ddlReason.DataBind();
        }

        public void LoadVehicleInfo()
        {
            var data = from v in db.tbl_VehicleInfo
                       select new
                       {
                           VehicleInfoID = v.VehicleInfoID,
                           Make = v.Make + " " + v.Model + " " + v.Capacity
                       };
            ddlVehicleInfo.DataSource = data.ToList();
            ddlVehicleInfo.DataValueField = "VehicleInfoID";
            ddlVehicleInfo.DataTextField = "Make";
            ddlVehicleInfo.DataBind();
        }


        public void LoadOwner()
        {
            var data = from o in db.tbl_Owner
                       join s in db.tbl_Status on o.StatusID equals s.StatusID
                       where s.StatusDescription == "Approved"
                       select new
                       {
                           OwnerID = o.OwnerID,
                           Name = o.Name + " " + o.Surname + " - " + o.IDNo
                       };
            ddlOwner.DataSource = data.ToList();
            ddlOwner.DataValueField = "OwnerID";
            ddlOwner.DataTextField = "Name";
            ddlOwner.DataBind();
        }

        public void LoadData()
        {
            var list = (from a in db.tbl_Vehicle
                        join b in db.tbl_Owner on a.OwnerID equals b.OwnerID
                        join d in db.tbl_VehicleInfo on a.VehicleInfoID equals d.VehicleInfoID
                        join e in db.tbl_Status on a.StatusID equals e.StatusID

                        where ((a.OwnerID == b.OwnerID) && b.StatusID == 15)

                        select new
                        {
                            a.VehicleID,
                            a.CORExpiry,
                            a.CORScanID,
                            a.EngineNumber,
                            a.LastServiced,
                            a.NumberPlate,
                            a.OperatingLicenseExpiry,
                            b.IDNo,
                            a.VehicleAccreditation,
                            a.VINNumber,
                            a.OperatingLicense,
                            d.Make,
                            d.Model,
                            a.Capacity,
                            a.Year,
                            a.YearRegistered,
                            e.StatusDescription,
                            NameSurname = b.Name + " " + b.Surname
                        });
            gvVehicle.DataSource = list.ToList().OrderByDescending(x => x.VehicleID);
            gvVehicle.DataBind();
        }

        protected void RemoveBorder()
        {
            txtVehCORExpiry.Style.Add("border", "");
            txtVehEngineNO.Style.Add("border", "");
            txtVehLastServiced.Style.Add("border", "");
            txtVehNoPlate.Style.Add("border", "");
            txtVehOperatingLicenseExpiry.Style.Add("border", "");
            txtVehVinNo.Style.Add("border", "");
            ddlYearRegistered.Style.Add("border", "");
            txtCapacity.Style.Add("border", "");
            ddlYear.Style.Add("border", "");
            txtOperatingLicense.Style.Add("border", "");
            ddlOwner.Style.Add("border", "");
            ddlVehicleInfo.Style.Add("border", "");
            ddlStatus.Style.Add("border", "");
            ddlReason.Style.Add("border", "");
            fuCORScan.Style.Add("border", "");
            //lblVehicle.Visible = false;
        }

        private bool ValidateVehicle()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if ((txtVehCORExpiry.Text == string.Empty))
            {
                txtVehCORExpiry.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehCORExpiry.Style.Add("border", "");
            }
            if ((txtVehEngineNO.Text == string.Empty))
            {
                txtVehEngineNO.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehEngineNO.Style.Add("border", "");
            }
            if ((txtVehLastServiced.Text == string.Empty))
            {
                txtVehLastServiced.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehLastServiced.Style.Add("border", "");
            }
            if ((txtVehNoPlate.Text == string.Empty))
            {
                txtVehNoPlate.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehNoPlate.Style.Add("border", "");
            }
            if ((txtVehOperatingLicenseExpiry.Text == string.Empty))
            {
                txtVehOperatingLicenseExpiry.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehOperatingLicenseExpiry.Style.Add("border", "");
            }
            if ((txtVehVinNo.Text == string.Empty))
            {
                txtVehVinNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehVinNo.Style.Add("border", "");
            }

            //if ((ddlYear.SelectedIndex == 0))
            //{
            //    ddlYear.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    ddlYear.Style.Add("border", "");
            //}
            if ((ddlYearRegistered.SelectedValue == "0"))
            {
                ddlYearRegistered.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlYearRegistered.Style.Add("border", "");
            }
            if ((txtOperatingLicense.Text == string.Empty))
            {
                txtOperatingLicense.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtOperatingLicense.Style.Add("border", "");
            }
            if ((ddlOwner.SelectedValue == "0"))
            {
                ddlOwner.Style.Add("BorderStyle", "1px solid red");
                valid = false;
                lblOwner.Text = "Required";


            }
            else if ((ddlOwner.SelectedValue != "0"))
            {
                lblOwner1.Text = "";
                ddlOwner.Style.Add("border", "");
            }
            if ((ddlVehicleInfo.SelectedValue == "0"))
            {
                ddlVehicleInfo.Style.Add("border", "1px solid red");
                valid = false;
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Vehicle Type!');", true);
                lblVehicle.Text = "Required";
            }
            else if ((ddlVehicleInfo.SelectedValue != "0"))
            {
                txtCapacity.Style.Add("border", "1px solid red");
                lblVehicle.Text = "";
                ddlVehicleInfo.Style.Add("border", "");
                if ((ddlVehicleInfo.SelectedValue != "0") && (txtCapacity.Text == string.Empty))
                {
                    txtCapacity.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    txtCapacity.Style.Add("border", "");
                }
            }

            if ((txtCapacity.Text == string.Empty))
            {
                txtCapacity.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCapacity.Style.Add("border", "");
            }

            if ((ddlStatus.SelectedValue == "0"))
            {
                ddlStatus.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlStatus.Style.Add("border", "");
            }

            if ((ddlStatus.SelectedValue != "0"))
            {
                if ((ddlReason.SelectedValue == "0"))
                {
                    ddlReason.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    ddlReason.Style.Add("border", "");
                }
            }
            else
            {
                ddlReason.Style.Add("border", "");
            }

            string cor = "0";
            cor = txtCOR.Text;

            //if (fuCORScan.HasFile == false && txtCOR.Text == string.Empty)
            //{
            //    fuCORScan.Style.Add("border", "1px solid red");
            //    valid = false;
            //}

            //else
            //{
            //    fuCORScan.Style.Add("border", "");
            //    valid = true;
            //}

            return valid;
        }

        private bool ValidateFileUpload()
        {
            bool valid = true;

            string cor1 = "0";
            cor1 = txtCOR1.Text;

            if (!btnCORUpload && txtCOR1.Text == string.Empty)
            {
                fuCORScan.Style.Add("border", "1px solid red");
                lblCORScan1.Visible = true;
                valid = false;
                btnCORUpload = false;
            }
            else
            {
                fuCORScan.Style.Add("border", "");
                valid = true;
            }
            return valid;
        }


        protected void btnCOR_ServerClick(object sender, EventArgs e)
        {
            if (ValidateVehicleUpload())
            {
                UploadDMSCOR();
                divUploadDocuments.Style.Add("display", "inline");
                txtCOR.Text = "1";
                ValidateVehicle();
                lblCORScan.Visible = true;
                btnCORUpload = true;
                txtCOR1.Text = "1";
            }
            RemoveBorder();
            fuCORScan.Style.Add("color", "#ffffff");
        }


        private bool ValidateVehicleEdit()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if ((txtVehCORExpiry.Text == string.Empty))
            {
                txtVehCORExpiry.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehCORExpiry.Style.Add("border", "");
            }
            if ((txtVehEngineNO.Text == string.Empty))
            {
                txtVehEngineNO.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehEngineNO.Style.Add("border", "");
            }
            if ((txtVehLastServiced.Text == string.Empty))
            {
                txtVehLastServiced.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehLastServiced.Style.Add("border", "");
            }
            if ((txtVehNoPlate.Text == string.Empty))
            {
                txtVehNoPlate.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehNoPlate.Style.Add("border", "");
            }
            if ((txtVehOperatingLicenseExpiry.Text == string.Empty))
            {
                txtVehOperatingLicenseExpiry.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehOperatingLicenseExpiry.Style.Add("border", "");
            }

            if ((ddlYearRegistered.SelectedIndex == 0))
            {
                ddlYearRegistered.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlYearRegistered.Style.Add("border", "");
            }

            if ((txtVehVinNo.Text == string.Empty))
            {
                txtVehVinNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtVehVinNo.Style.Add("border", "");
            }

            if ((txtOperatingLicense.Text == string.Empty))
            {
                txtOperatingLicense.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtOperatingLicense.Style.Add("border", "");
            }

            if ((txtCapacity.Text == string.Empty))
            {
                txtCapacity.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCapacity.Style.Add("border", "");
            }

            if ((ddlOwner.SelectedValue == "0"))
            {
                ddlOwner.Style.Add("BorderStyle", "1px solid red");
                valid = false;
                lblOwner.Text = "Required";
            }
            else if ((ddlOwner.SelectedValue != "0"))
            {
                lblOwner1.Text = "";
                ddlOwner.Style.Add("border", "");
            }

            if ((ddlVehicleInfo.SelectedValue == "0"))
            {
                ddlVehicleInfo.Style.Add("border", "1px solid red");
                valid = false;
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Vehicle Type!');", true);
                lblVehicle.Text = "Required";
            }
            else if ((ddlVehicleInfo.SelectedValue != "0"))
            {
                txtCapacity.Style.Add("border", "1px solid red");
                lblVehicle.Text = "";
                ddlVehicleInfo.Style.Add("border", "");
                if ((ddlVehicleInfo.SelectedValue != "0") && (txtCapacity.Text == string.Empty))
                {
                    txtCapacity.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    txtCapacity.Style.Add("border", "");
                }
            }

            if ((ddlStatus.SelectedValue == "0"))
            {
                ddlStatus.Style.Add("border", "1px solid red");
                valid = false;
            }
            else if ((ddlStatus.SelectedItem.Text == "Approved"))
            {
                ddlStatus.Style.Add("border", "");
                ddlReason.Style.Add("border", "");
            }
            else if ((ddlStatus.SelectedItem.Text == "Inactive") || (ddlStatus.SelectedItem.Text == "Cancelled") || (ddlStatus.SelectedItem.Text == "Pending"))
            {
                if ((ddlReason.SelectedValue == "0"))
                {
                    ddlReason.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    ddlReason.Style.Add("border", "");
                }
            }
            else
            {
                ddlStatus.Style.Add("border", "");
                ddlReason.Style.Add("border", "");
            }

            return valid;
        }

        public int countCars(int id)
        {
            var count = (from v in db.tbl_Vehicle
                         where v.OwnerID == Convert.ToInt16(ddlOwner.SelectedValue)
                         select v).Count();
            return count;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                VehicleBL BL = new VehicleBL();
                tbl_Vehicle TBL = new tbl_Vehicle();

                string temp = btnAdd.Text;

                if (ValidateVehicle() && temp.Contains("Add"))
                {
                    var numberplate = db.tbl_Vehicle.ToList().FindAll(x => String.Compare(x.NumberPlate, (string)txtVehNoPlate.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                    if (numberplate.Count > 0 || string.IsNullOrWhiteSpace(txtVehNoPlate.Text))
                    {
                        divsuccess.Style.Add("display", "none");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "none");
                        divwarning.Style.Add("display", "inline");

                        txtVehNoPlate.Focus();
                    }
                    else
                    {
                        //TBL.CORScanID =Convert.ToBoolean(fuCORScan.PostedFile.FileName);
                        TBL.EngineNumber = txtVehEngineNO.Text;
                        TBL.NumberPlate = txtVehNoPlate.Text.Trim();
                        DateTime txtCORExpiry = DateTime.ParseExact(txtVehCORExpiry.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        TBL.CORExpiry = txtCORExpiry;
                        DateTime txtLastServiced = DateTime.ParseExact(txtVehLastServiced.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        TBL.LastServiced = txtLastServiced;
                        DateTime txtOperatingLicenseExpiry = DateTime.ParseExact(txtVehOperatingLicenseExpiry.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        TBL.OperatingLicenseExpiry = txtOperatingLicenseExpiry;

                        TBL.OperatingLicense = txtOperatingLicense.Text;
                        TBL.VehicleInfoID = Convert.ToInt16(ddlVehicleInfo.SelectedValue);
                        TBL.VINNumber = txtVehVinNo.Text;
                        TBL.YearRegistered = ddlYearRegistered.SelectedValue;
                        TBL.OwnerID = Convert.ToInt16(ddlOwner.SelectedValue);
                        TBL.Year = ddlYear.SelectedValue;
                        TBL.Capacity = txtCapacity.Text;

                        if ((ddlStatus.SelectedItem.Text == "Approved"))
                        {
                            TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                        }
                        else if ((ddlStatus.SelectedItem.Text == "Inactive") || (ddlStatus.SelectedItem.Text == "Cancelled") || (ddlStatus.SelectedItem.Text == "Pending"))
                        {
                            TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                            TBL.ReasonID = Convert.ToInt16(ddlReason.SelectedValue);
                        }

                        if (ddlStatus.SelectedItem.Text == "Pending")
                        {
                            BL.CreateVehicle(TBL);

                            SendEmailPending();

                            string veh = "Vehicle";
                            SendSmsPending(veh);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");

                            divUploadDocuments.Style.Add("display", "none");
                            //divVehicle.Style.Add("display", "none");

                            Clear();
                        }
                        else
                        {
                            BL.CreateVehicle(TBL);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");

                            divUploadDocuments.Style.Add("display", "none");
                            //divVehicle.Style.Add("display", "none");

                            Clear();
                        }
                    }
                }
                else if (ValidateVehicleEdit() && temp.Contains("Update"))
                {
                    fuCORScan.Style.Add("border", "");

                    int Id = int.Parse(ViewState["VehicleID"].ToString());
                    TBL.VehicleID = Id;
                    //TBL.CORScanID =Convert.ToBoolean(fuCORScan.PostedFile.FileName);
                    TBL.EngineNumber = (txtVehEngineNO.Text);
                    TBL.NumberPlate = txtVehNoPlate.Text.Trim();

                    DateTime txtCORExpiry = DateTime.ParseExact(txtVehCORExpiry.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    TBL.CORExpiry = txtCORExpiry;
                    DateTime txtLastServiced = DateTime.ParseExact(txtVehLastServiced.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    TBL.LastServiced = txtLastServiced;
                    DateTime txtOperatingLicenseExpiry = DateTime.ParseExact(txtVehOperatingLicenseExpiry.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    TBL.OperatingLicenseExpiry = txtOperatingLicenseExpiry;

                    TBL.OperatingLicense = txtOperatingLicense.Text;
                    TBL.VINNumber = txtVehVinNo.Text;
                    TBL.YearRegistered = ddlYearRegistered.SelectedValue;
                    TBL.OwnerID = Convert.ToInt16(ddlOwner.SelectedValue);
                    TBL.VehicleInfoID = Convert.ToInt16(ddlVehicleInfo.SelectedValue);
                    TBL.Year = ddlYear.SelectedValue;
                    TBL.Capacity = txtCapacity.Text;
                    TBL.VehicleInfoID = Convert.ToInt16(ddlVehicleInfo.SelectedValue);

                    if ((ddlStatus.SelectedItem.Text == "Approved"))
                    {
                        TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                    }
                    else if ((ddlStatus.SelectedItem.Text == "Inactive") || (ddlStatus.SelectedItem.Text == "Cancelled") || (ddlStatus.SelectedItem.Text == "Pending"))
                    {
                        TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                        TBL.ReasonID = Convert.ToInt16(ddlReason.SelectedValue);
                    }

                    var v = db.tbl_Vehicle.ToList().Find(x => x.NumberPlate == txtVehNoPlate.Text.ToString());
                    var id = db.tbl_Vehicle.ToList().FirstOrDefault(x => x.VehicleID == Id);
                    if ((v == null) || (id.NumberPlate == txtVehNoPlate.Text.Trim()))
                    {
                        if (ddlStatus.SelectedItem.Text == "Pending")
                        {
                            PaymentBL pBL = new PaymentBL();
                            Payment pTBL = new Payment();
                            var vehid = db.tbl_Vehicle.ToList().FirstOrDefault(x => x.NumberPlate == txtVehNoPlate.Text);
                            var ownId = db.tbl_Owner.FirstOrDefault(x => x.OwnerID == vehid.OwnerID);
                            var payId = db.Payments.ToList().FirstOrDefault(x => x.OwnerID == ownId.OwnerID);
                            if (payId.AccountNo == ownId.IDNo)
                            {
                                if (payId.Amount == null)
                                {
                                    //payId.Amount = Convert.ToDecimal(1000.00);
                                    pBL.UpdatePayment(payId);
                                }
                                else if (payId.Amount != null)
                                {
                                    if (payId.Amount == Convert.ToDecimal(0.00))
                                    {
                                        //payId.Amount = Convert.ToDecimal(1000.00);
                                        pBL.UpdatePayment(payId);
                                    }
                                    else
                                    {
                                        if (v.PaymentAdded == true)
                                        {
                                            var vehId = db.tbl_Vehicle.ToList().Find(x => x.NumberPlate == txtVehNoPlate.Text);
                                            vehId.PaymentAdded = false;
                                            BL.UpdateVehicle(vehId);

                                            double amount = double.Parse("1000.00", CultureInfo.InvariantCulture);
                                            payId.Amount = payId.Amount - (decimal)amount;
                                            pBL.UpdatePayment(payId);
                                        }
                                    }
                                }
                            }

                            BL.UpdateVehicle(TBL);
                            
                            SendEmailPendingEdited();

                            string veh = "Vehicle";
                            SendSmsPending(veh);

                            btnAdd.Text = "Add";

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "inline");
                            divwarning.Style.Add("display", "none");

                            divUploadDocuments.Style.Add("display", "none");
                            //divVehicle.Style.Add("display", "none");
                            divDoc.Style.Add("display", "none");

                            Clear();
                        }
                        else
                        {
                            BL.UpdateVehicle(TBL); ;

                            btnAdd.Text = "Add";

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "inline");
                            divwarning.Style.Add("display", "none");

                            divUploadDocuments.Style.Add("display", "none");
                            //divVehicle.Style.Add("display", "none");
                            divDoc.Style.Add("display", "none");

                            Clear();
                        }
                    }
                    else
                    {
                        divsuccess.Style.Add("display", "none");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "none");
                        divwarning.Style.Add("display", "inline");

                        txtVehNoPlate.Text = string.Empty;
                        txtVehNoPlate.Focus();

                        //lblError.Visible = true;
                        ////lblError.Text = "Connection Problem: " + ex.Message.ToString();
                        //lblError.Text = "Id number exists.";
                    }
                    ddlStatus.Enabled = false;
                }
                LoadData();
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
        public void Clear()
        {
            txtVehCORExpiry.Text = string.Empty;
            txtVehEngineNO.Text = string.Empty;
            txtVehLastServiced.Text = string.Empty;
            txtVehNoPlate.Text = string.Empty;
            txtVehOperatingLicenseExpiry.Text = string.Empty;
            txtVehVinNo.Text = string.Empty;
            //ddlYearRegistered.SelectedValue = "0";
            txtCapacity.Text = string.Empty;
            ddlStatus.SelectedItem.Text = "Pending";
            txtOperatingLicense.Text = string.Empty;
            ddlVehicleInfo.SelectedValue = "0";
            ddlOwner.SelectedValue = "0";
            ddlReason.SelectedValue = "0";
        }

        protected void gvVehicle_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvVehicle.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvVehicle.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvVehicle.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvVehicle.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvVehicle.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                ddlStatus.Enabled = true;
                RemoveBorder();
                LoadDMSVehicleDoc();

                //Accessing BoundField Column
                string Source = gvVehicle.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["VehicleID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);

                var temp = db.tbl_Vehicle.ToList().FirstOrDefault(x => x.VehicleID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    divDoc.Style.Add("display", "inline");
                    divUploadDocuments.Style.Add("display", "none");

                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");

                    RemoveBorder();

                    Session["VehicleID"] = Id;
                    txtVehCORExpiry.Text = temp.CORExpiry.ToString();
                    txtVehEngineNO.Text = temp.EngineNumber;
                    txtVehLastServiced.Text = temp.LastServiced.ToString();
                    txtVehNoPlate.Text = temp.NumberPlate;
                    txtVehOperatingLicenseExpiry.Text = temp.OperatingLicenseExpiry.ToString();
                    txtVehVinNo.Text = temp.VINNumber;
                    ddlYearRegistered.Text = temp.YearRegistered;
                    txtOperatingLicense.Text = temp.OperatingLicense.ToString();
                    ddlOwner.SelectedValue = temp.OwnerID.ToString();
                    ddlVehicleInfo.SelectedValue = temp.VehicleInfoID.ToString();

                    //string date = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtVehLastServiced.Text = date;
                    //date = temp.LastServiced.ToString();

                    //string date1 = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtVehCORExpiry.Text = date1;
                    //date1 = temp.CORExpiry.ToString();

                    //string date2 = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtVehOperatingLicenseExpiry.Text = date2;
                    //date2 = temp.OperatingLicenseExpiry.ToString();

                    var data2 = db.tbl_Vehicle.ToList().FirstOrDefault(x => x.VehicleID == Convert.ToInt16(Id));

                    DateTime date = DateTime.Now;
                    Convert.ToDateTime(date).ToShortDateString();

                    DateTime opexpiry = date;
                    opexpiry = Convert.ToDateTime(data2.OperatingLicenseExpiry);
                    txtVehOperatingLicenseExpiry.Text = data2.OperatingLicenseExpiry.Value.ToString("MM/dd/yyyy");

                    DateTime corexpiry = date;
                    corexpiry = Convert.ToDateTime(data2.CORExpiry);
                    txtVehCORExpiry.Text = data2.CORExpiry.Value.ToString("MM/dd/yyyy");

                    DateTime lastdateserviced = date;
                    lastdateserviced = Convert.ToDateTime(data2.LastServiced);
                    txtVehLastServiced.Text = data2.LastServiced.Value.ToString("MM/dd/yyyy");

                    if (ddlVehicleInfo.SelectedValue != "0")
                    {
                        divCapacity.Style.Add("display", "inline");
                        divCapacity1.Style.Add("display", "inline");
                        divYear.Style.Add("display", "none");
                        divYear1.Style.Add("display", "none");
                        ddlYear.Text = temp.Year;
                        txtCapacity.Text = temp.Capacity;
                    }

                    //if (temp.tbl_Status.StatusDescription == "Approved")
                    //{
                    //    divReason.Style.Add("display", "none");
                    //    divReason1.Style.Add("display", "none");
                    //    ddlStatus.Items.Clear();
                    //    LoadStatusAll();
                    //    RemoveDuplicateItems(ddlStatus);
                    //    ddlStatus.SelectedValue = temp.StatusID.ToString();
                    //}
                    //else if ((temp.tbl_Status.StatusDescription == "Inactive") || (temp.tbl_Status.StatusDescription == "Cancelled") || (temp.tbl_Status.StatusDescription == "Pending"))
                    //{
                    //    divReason.Style.Add("display", "inline");
                    //    divReason1.Style.Add("display", "inline");
                    //    ddlStatus.Items.Clear();
                    //    LoadStatusNotAll();
                    //    RemoveDuplicateItems(ddlStatus);
                    //    ddlStatus.SelectedValue = temp.StatusID.ToString();
                    //    ddlReason.SelectedValue = temp.ReasonID.ToString();
                    //}

                    if (temp.tbl_Status.StatusDescription == "Approved")
                    {
                        ddlStatus.Items.Clear();
                        LoadStatusAll();
                        RemoveDuplicateItems(ddlStatus);
                        ddlStatus.SelectedValue = temp.StatusID.ToString();
                        divReason.Style.Add("display", "inline");
                        divReason1.Style.Add("display", "inline");
                    }
                    else if ((temp.tbl_Status.StatusDescription == "Inactive") || (temp.tbl_Status.StatusDescription == "Cancelled") || (temp.tbl_Status.StatusDescription == "Pending"))
                    {
                        ddlStatus.Items.Clear();
                        LoadStatusNotAll();
                        //LoadReason();
                        RemoveDuplicateItems(ddlStatus);
                        ddlStatus.SelectedValue = temp.StatusID.ToString();
                        ddlReason.SelectedValue = temp.ReasonID.ToString();
                        divReason.Style.Add("display", "inline");
                        divReason1.Style.Add("display", "inline");
                    }

                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
                    ddlStatus.Enabled = false;

                    btnAdd.Text = "Update";
                }
            }
            else if (commandName == "Print")
            {
                LoadStatusAll();

                //string Source = gvVehicle.Rows[myRow.RowIndex].Cells[0].Text;

                //ViewState["VehicleID"] = Id;

                //var temp = db.tbl_Vehicle.ToList().FirstOrDefault(x => x.VehicleID == Convert.ToInt16(Id));

                //db.tbl_Vehicle.Remove(temp);
                //db.SaveChanges();
                //RemoveBorder();
                //LoadData(Convert.ToInt32(ddlOwner.SelectedValue));
                //Clear();
                //btnAdd.Text = "Add";

                //divsuccess.Style.Add("display", "none");
                //divdanger.Style.Add("display", "inline");
                //divinfo.Style.Add("display", "none");
                //divwarning.Style.Add("display", "none");

                VehicleBL BL = new VehicleBL();
                tbl_Vehicle TBL = new tbl_Vehicle();

                ViewState["VehicleID"] = Id;

                var temp = db.tbl_Vehicle.ToList().FirstOrDefault(x => x.VehicleID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    RemoveBorder();
                    Session["VehicleID"] = Id;
                    txtVehCORExpiry.Text = temp.CORExpiry.ToString();
                    txtVehEngineNO.Text = temp.EngineNumber;
                    txtVehLastServiced.Text = temp.LastServiced.ToString();
                    txtVehNoPlate.Text = temp.NumberPlate;
                    txtVehOperatingLicenseExpiry.Text = temp.OperatingLicenseExpiry.ToString();
                    txtVehVinNo.Text = temp.VINNumber;
                    ddlYearRegistered.Text = temp.YearRegistered;
                    txtOperatingLicense.Text = temp.OperatingLicense.ToString();
                    ddlOwner.SelectedValue = temp.OwnerID.ToString();
                    ddlVehicleInfo.SelectedValue = temp.VehicleInfoID.ToString();

                    //string date = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtVehLastServiced.Text = date;
                    //date = temp.LastServiced.ToString();

                    //string date1 = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtVehCORExpiry.Text = date1;
                    //date1 = temp.CORExpiry.ToString();

                    //string date2 = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtVehOperatingLicenseExpiry.Text = date2;
                    //date2 = temp.OperatingLicenseExpiry.ToString();

                    var data2 = db.tbl_Vehicle.ToList().FirstOrDefault(x => x.VehicleID == Convert.ToInt16(Id));

                    DateTime date = DateTime.Now;
                    Convert.ToDateTime(date).ToShortDateString();

                    DateTime opexpiry = date;
                    opexpiry = Convert.ToDateTime(data2.OperatingLicenseExpiry);
                    txtVehOperatingLicenseExpiry.Text = data2.OperatingLicenseExpiry.Value.ToString("MM/dd/yyyy");

                    DateTime corexpiry = date;
                    corexpiry = Convert.ToDateTime(data2.CORExpiry);
                    txtVehCORExpiry.Text = data2.CORExpiry.Value.ToString("MM/dd/yyyy");

                    DateTime lastdateserviced = date;
                    lastdateserviced = Convert.ToDateTime(data2.LastServiced);
                    txtVehLastServiced.Text = data2.LastServiced.Value.ToString("MM/dd/yyyy");

                    if (ddlVehicleInfo.SelectedValue != "0")
                    {
                        divCapacity.Style.Add("display", "inline");
                        divCapacity1.Style.Add("display", "inline");
                        divYear.Style.Add("display", "none");
                        divYear1.Style.Add("display", "none");
                        ddlYear.Text = temp.Year;
                        txtCapacity.Text = temp.Capacity;
                    }

                    ddlStatus.SelectedValue = temp.StatusID.ToString();

                    if (temp.ReasonID != null)
                    {
                        ddlReason.SelectedValue = temp.ReasonID.ToString();
                    }
                    else
                    {
                        ddlReason.SelectedItem.Text = "N/A";
                    }
                }

                CreatePDF();
                Clear();

                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
            }
        }

        protected void ddlVehicleInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVehicleInfo.SelectedValue == "0")
            {
                lblVehicle.Text = "Required";
                divCapacity.Style.Add("display", "none");
                divCapacity1.Style.Add("display", "none");
                divYear.Style.Add("display", "none");
                divYear1.Style.Add("display", "none");
            }
            else
            {
                lblVehicle.Text = "";
                divCapacity.Style.Add("display", "inline");
                divCapacity1.Style.Add("display", "inline");
                divYear.Style.Add("display", "none");
                divYear1.Style.Add("display", "none");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/Vehicle.aspx");
        }

        //protected void View(object sender, EventArgs e)
        //{
        //    string url = string.Format("Vehicle.aspx?FN={0}.pdf", (sender as LinkButton).CommandName);
        //    string script = "<script type='text/javascript'>window.open('" + url + "')</script>";
        //    this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);
        //}

        protected void gvUploadVehicle_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;
            if (commandName == "View")
            {
                LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
                GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
                GridView myGrid = (GridView)sender; // the gridview 
                string Fname = gvUploadVehicle.DataKeys[myRow.RowIndex].Values[0].ToString();

                string url = gvUploadVehicle.Rows[myRow.RowIndex].Cells[3].Text;
                //Response.Redirect(url);

                lnkBtn.Attributes.Add("onclick", "javascript:window.open('" + url + "'); return false;");
            }
            else if (commandName == "DeleteFile")
            {
                try
                {
                    SharepointService.DocumentManagementSystemClient serviceSharepoint = new SharepointService.DocumentManagementSystemClient();

                    SharepointService.DeleteRequest request = new SharepointService.DeleteRequest();

                    LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
                    GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
                    GridView myGrid = (GridView)sender; // the gridview 
                    string Fname = gvUploadVehicle.DataKeys[myRow.RowIndex].Values[0].ToString();

                    request.documentName = Fname;

                    SharepointService.DeleteDocumentResponse res = new SharepointService.DeleteDocumentResponse();

                    res.DeleteResponse = serviceSharepoint.DeleteDocument(request);

                    string status = res.DeleteResponse.Status;

                    SharepointService.Search_Request rqt = new SharepointService.Search_Request();
                    rqt.FolderName = ddlOwner.SelectedItem.Text + "-" + txtVehNoPlate.Text;

                    SharepointService.SearchDocumentResponse ser = new SharepointService.SearchDocumentResponse();

                    ser.response = serviceSharepoint.SearchDocument(rqt);

                    string fname = null;
                    int count = ser.response.Length;
                    for (int i = 0; i < count; i++)
                    {
                        string filename = ser.response[i].NAME;
                        string link = ser.response[i].Link_URL;

                        string category = ser.response[i].REFERENCE_NUMBER;
                        //string reference = ser.response[i].REFERENCE_NUMBER;
                        string creationdate = ser.response[i].CREATION_DATE;
                        gvUploadVehicle.DataSource = ser.response;
                        gvUploadVehicle.DataBind();
                        fname = filename;
                    }
                    if (fname == null)
                    {
                        gvUploadVehicle.Visible = false;

                    }
                    else
                    {
                        gvUploadVehicle.Visible = true;
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
        }

        private bool ValidateVehicleUpload()
        {
            bool valid = true;

            if ((ddlOwner.SelectedValue == "0") && (txtVehNoPlate.Text == string.Empty))
            {
                ddlOwner.Style.Add("border", "1px solid red");
                txtVehNoPlate.Style.Add("border", "1px solid red");
                //fuCORScan.Style.Add("border", "1px solid red");
                valid = false;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Owner And Number Plate!');", true);
            }
            else
            {
                ddlOwner.Style.Add("border", "");
                txtVehNoPlate.Style.Add("border", "");
                //fuCORScan.Style.Add("border", "");
            }

            return valid;
        }


        //upload
        public void UploadDMSCOR()
        {
            try
            {
                SharepointService.DocumentManagementSystemClient serviceSharepoint = new SharepointService.DocumentManagementSystemClient();

                SharepointService.Upload_Request request = new SharepointService.Upload_Request();

                request.fileName = fuCORScan.FileName;
                request.fileContent = fuCORScan.FileBytes;
                request.destinationLocation = ddlOwner.SelectedItem.Text + "-" + txtVehNoPlate.Text;
                request.sourceLocation = "COR";

                SharepointService.Upload_Response response = serviceSharepoint.UploadDocument(request);

                string status = response.optStatus;

                string desURL = response.UpladDetails.UploadResult.link_url;

                SharepointService.Search_Request rqt = new SharepointService.Search_Request();
                rqt.FolderName = ddlOwner.SelectedItem.Text + "-" + txtVehNoPlate.Text;

                SharepointService.SearchDocumentResponse ser = new SharepointService.SearchDocumentResponse();

                ser.response = serviceSharepoint.SearchDocument(rqt);
                string fname = null;
                int count = ser.response.Length;
                for (int i = 0; i < count; i++)
                {
                    string filename = ser.response[i].NAME;
                    string category = ser.response[i].REFERENCE_NUMBER;
                    string link = ser.response[i].Link_URL;
                    //string  = ser.response[i].REFERENCE_NUMBER;
                    string creationdate = ser.response[i].CREATION_DATE;
                    gvUploadVehicle.DataSource = ser.response;
                    gvUploadVehicle.DataBind();
                    fname = filename;
                }
                if (fname == null)
                {
                    gvUploadVehicle.Visible = false;
                    //divDocMsge.Visible = true;
                    //divDocs.Visible = false;
                }
                else
                {
                    gvUploadVehicle.Visible = true;
                    //divDocMsge.Visible = false;
                    //divDocs.Visible = true;
                }
                serviceSharepoint.Close();

            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Can't upload document. Network problem or services are currently down, please contact support team.";
            }
        }

        protected void ddlOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOwner.SelectedValue == "0")
            {
                lblOwner.Text = "Required";
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                divUploadDocuments.Style.Add("display", "none");
                divDoc.Style.Add("display", "none");
            }
            else
            {
                lblOwner.Text = "";
                LoadData();
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                divUploadDocuments.Style.Add("display", "none");
                divDoc.Style.Add("display", "none");
            }
        }

        protected void btnDoc_Click(object sender, EventArgs e)
        {
            divUploadDocuments.Style.Add("display", "inline");
            divDoc.Style.Add("display", "none");
            LoadDMSVehicleDoc();
        }


        public void LoadDMSVehicleDoc()
        {
            try
            {
                SharepointService.DocumentManagementSystemClient serviceSharepoint = new SharepointService.DocumentManagementSystemClient();

                SharepointService.Search_Request rqt = new SharepointService.Search_Request();
                rqt.FolderName = ddlOwner.SelectedItem.Text + "-" + txtVehNoPlate.Text;

                SharepointService.SearchDocumentResponse ser = new SharepointService.SearchDocumentResponse();

                ser.response = serviceSharepoint.SearchDocument(rqt);
                string fname = null;
                int count = ser.response.Length;
                for (int i = 0; i < count; i++)
                {

                    string filename = ser.response[i].NAME;
                    string category = ser.response[i].REFERENCE_NUMBER;
                    string link = ser.response[i].Link_URL;
                    //string  = ser.response[i].REFERENCE_NUMBER;
                    string creationdate = ser.response[i].CREATION_DATE;

                    gvUploadVehicle.DataSource = ser.response;
                    gvUploadVehicle.DataBind();
                    fname = filename;
                }
                if (fname == null)
                {
                    gvUploadVehicle.Visible = false;
                    //divDocMsge.Visible = true;
                    //divDocs.Visible = false;
                }
                else
                {
                    gvUploadVehicle.Visible = true;
                    //divDocMsge.Visible = false;
                    //divDocs.Visible = true;
                }
                serviceSharepoint.Close();

            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblError.Visible = false;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Can't download document. Network problem or services are currently down, please contact support team.";
            }
        }

        public void SendEmailPending()
        {
            string email, name, surname;
            var data = db.tbl_User.ToList().FindAll(x => x.IsSupervisor == true);

            for (int i = 0; i < data.Count; i++)
            {
                email = data[i].Email;
                name = data[i].Name;
                surname = data[i].Surname;

                try
                {
                    String URL = HttpContext.Current.Request.Url.AbsoluteUri;
                    Uri uri = new Uri(URL);

                    EmailBL EML = new EmailBL();
                    EML.Email("Moja Cruise System - Vehicle Registration", "Hello " + name + " " + surname + "<br/> <br/>"
                                                    + "Please check the pending record of Vehicle: " + ddlVehicleInfo.SelectedItem.Text + ", " + "Reg No: " + txtVehNoPlate.Text + "<br/> <br/>"
                                                    + "<a href = '" + uri + "'>Please click the link to login.</a>" + " " + "<br/> <br/>"
                                                    + "For more details feel free to contact the Ethekwini Transport Authority." + "<br/>" +
                                                    "Regards" + "<br/>" +
                                                    "Ethekwini Transport Authority", email, "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
                }
                catch (Exception ex)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");

                    lblError.Visible = true;
                    //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                    lblError.Text = "Can't send email. Network problem or services are currently down, please contact support team.";
                }

                db.SaveChanges();
            }
        }

        public void SendEmailPendingEdited()
        {
            string email, name, surname;
            var data = db.tbl_User.ToList().FindAll(x => x.IsSupervisor == true);

            for (int i = 0; i < data.Count; i++)
            {
                email = data[i].Email;
                name = data[i].Name;
                surname = data[i].Surname;

                try
                {
                    String URL = HttpContext.Current.Request.Url.AbsoluteUri;
                    Uri uri = new Uri(URL);

                    EmailBL EML = new EmailBL();
                    EML.Email("Moja Cruise System - Vehicle Edited", "Hello " + name + " " + surname + "<br/> <br/>"
                                                    + "Please check the pending record of Vehicle: " + ddlVehicleInfo.SelectedItem.Text + ", " + "Reg No: " + txtVehNoPlate.Text + "<br/> <br/>"
                                                    + "<a href = '" + uri + "'>Please click the link to login.</a>" + " " + "<br/> <br/>"
                                                    + "For more details feel free to contact the Ethekwini Transport Authority." + "<br/>" +
                                                    "Regards" + "<br/>" +
                                                    "Ethekwini Transport Authority", email, "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
                }
                catch (Exception ex)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");

                    lblError.Visible = true;
                    //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                    lblError.Text = "Can't send email. Network problem or services are currently down, please contact support team.";
                }

                db.SaveChanges();
            }
        }

        public void SendSmsPending(string vehicle)
        {
            try
            {
                if (ddlOwner.SelectedValue != "0")
                {
                    string cellno;
                    var data = db.tbl_Owner.ToList().FirstOrDefault(x => x.OwnerID == Convert.ToInt16(ddlOwner.SelectedValue));
                    cellno = data.CellNo;

                    string status = null;

                    SMSService.PortTypeClient smsserv = new SMSService.PortTypeClient();
                    SMSService.SendSMSReq_T req = new SMSService.SendSMSReq_T();

                    req.MobileNo = cellno;
                    req.TemplateId = "GDVehicle";

                    Parameters_T objp = new Parameters_T();

                    req.Parameters = objp;
                    req.Parameters.Parameter1 = new Parameter1_T();
                    req.Parameters.Parameter1.Name = "Parameter1";
                    req.Parameters.Parameter1.Value = vehicle;

                    req.Parameters = objp;
                    req.Parameters.Parameter2 = new Parameter2_T();
                    req.Parameters.Parameter2.Name = "Parameter2";
                    req.Parameters.Parameter2.Value = "Pending";

                    SMSService.SendSMSResp_T obj = smsserv.SendSMSOperation(req);

                    status = obj.Status.StatusMessage;
                }
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblError.Visible = true;
                //blError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Can't send sms. Network problem or services are currently down, please contact support team.";
            }
        }

        public void SendSmsApproved(string vehicle)
        {
            try
            {
                if (ddlOwner.SelectedValue != "0")
                {
                    string cellno;
                    var data = db.tbl_Owner.ToList().FirstOrDefault(x => x.OwnerID == Convert.ToInt16(ddlOwner.SelectedValue));
                    cellno = data.CellNo;

                    string status = null;

                    SMSService.PortTypeClient smsserv = new SMSService.PortTypeClient();
                    SMSService.SendSMSReq_T req = new SMSService.SendSMSReq_T();

                    req.MobileNo = cellno;
                    req.TemplateId = "GDVehicle";

                    Parameters_T objp = new Parameters_T();

                    req.Parameters = objp;
                    req.Parameters.Parameter1 = new Parameter1_T();
                    req.Parameters.Parameter1.Name = "Parameter1";
                    req.Parameters.Parameter1.Value = vehicle;

                    req.Parameters = objp;
                    req.Parameters.Parameter2 = new Parameter2_T();
                    req.Parameters.Parameter2.Name = "Parameter2";
                    req.Parameters.Parameter2.Value = "Approved";

                    SMSService.SendSMSResp_T obj = smsserv.SendSMSOperation(req);

                    status = obj.Status.StatusMessage;
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
                lblError.Text = "Can't send sms. Network problem or services are currently down, please contact support team.";
            }
        }

        public void CreatePDF()
        {
            // Create a Document object
            var document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Create a new PdfWrite object, writing the output to a MemoryStream
            var output = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            // Open the Document for writing
            document.Open();

            var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
            var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
            var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            document.Add(new Paragraph("", titleFont));
            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Vehicle", titleFont));

            document.Add(new Paragraph("Owner Details", subTitleFont));
            var personaldetails = new PdfPTable(2);
            personaldetails.HorizontalAlignment = 0;
            personaldetails.SpacingBefore = 5;
            personaldetails.SpacingAfter = 5;
            personaldetails.DefaultCell.Border = 0;

            string cellno, name, surname, idno, officeno, email, gender, race;
            var data = db.tbl_Owner.ToList().FirstOrDefault(x => x.OwnerID == Convert.ToInt16(ddlOwner.SelectedValue));
            cellno = data.CellNo;
            name = data.Name;
            surname = data.Surname;
            officeno = data.OfficeNo;
            email = data.Email;
            idno = data.IDNo;

            gender = Convert.ToString(data.GenderID);
            gender = data.tbl_Gender.GenderDescription;
            race = Convert.ToString(data.RaceID);
            race = data.tbl_Race.RaceDescription;

            //personaldetails.SetWidths(new int[] { 1, 4 });
            personaldetails.AddCell(new Phrase("Name:", bodyFont));
            personaldetails.AddCell(name);
            personaldetails.AddCell(new Phrase("Surname:", bodyFont));
            personaldetails.AddCell(surname);
            personaldetails.AddCell(new Phrase("ID No:", bodyFont));
            personaldetails.AddCell(idno);
            personaldetails.AddCell(new Phrase("Cell No:", bodyFont));
            personaldetails.AddCell(cellno);
            personaldetails.AddCell(new Phrase("Office No:", bodyFont));
            personaldetails.AddCell(officeno);
            personaldetails.AddCell(new Phrase("Email:", bodyFont));
            personaldetails.AddCell(email);
            personaldetails.AddCell(new Phrase("Gender:", bodyFont));
            personaldetails.AddCell(gender);
            personaldetails.AddCell(new Phrase("Race:", bodyFont));
            personaldetails.AddCell(race);
            document.Add(personaldetails);

            // Add the "Address" subtitle

            document.Add(new Paragraph("Vehicle Info", subTitleFont));
            var vehicleinfo = new PdfPTable(2);
            vehicleinfo.HorizontalAlignment = 0;
            vehicleinfo.SpacingBefore = 5;
            vehicleinfo.SpacingAfter = 5;
            vehicleinfo.DefaultCell.Border = 0;

            string make, model;
            var data1 = db.tbl_VehicleInfo.ToList().FirstOrDefault(x => x.VehicleInfoID == Convert.ToInt16(ddlVehicleInfo.SelectedValue));
            make = data1.Make;
            model = data1.Model;

            int vehicleID = Convert.ToInt16(Session["VehicleID"].ToString());
            var data2 = db.tbl_Vehicle.ToList().FirstOrDefault(x => x.VehicleID == vehicleID);

            DateTime date = DateTime.Now;
            Convert.ToDateTime(date).ToShortDateString();

            DateTime opexpiry = date;
            opexpiry = Convert.ToDateTime(data2.OperatingLicenseExpiry);
            txtVehOperatingLicenseExpiry.Text = data2.OperatingLicenseExpiry.Value.ToString("MM/dd/yyyy");

            DateTime corexpiry = date;
            corexpiry = Convert.ToDateTime(data2.CORExpiry);
            txtVehCORExpiry.Text = data2.CORExpiry.Value.ToString("MM/dd/yyyy");

            DateTime lastdateserviced = date;
            lastdateserviced = Convert.ToDateTime(data2.LastServiced);
            txtVehLastServiced.Text = data2.LastServiced.Value.ToString("MM/dd/yyyy");

            vehicleinfo.AddCell(new Phrase("No Plate:", bodyFont));
            vehicleinfo.AddCell(txtVehNoPlate.Text);
            vehicleinfo.AddCell(new Phrase("Make:", bodyFont));
            vehicleinfo.AddCell(make);
            vehicleinfo.AddCell(new Phrase("Model:", bodyFont));
            vehicleinfo.AddCell(model);
            vehicleinfo.AddCell(new Phrase("Capacity:", bodyFont));
            vehicleinfo.AddCell(txtCapacity.Text);
            vehicleinfo.AddCell(new Phrase("Year Registered:", bodyFont));
            vehicleinfo.AddCell(ddlYearRegistered.SelectedValue);
            vehicleinfo.AddCell(new Phrase("Last Date Serviced:", bodyFont));
            vehicleinfo.AddCell(lastdateserviced.ToShortDateString());
            vehicleinfo.AddCell(new Phrase("Engine No:", bodyFont));
            vehicleinfo.AddCell(txtVehEngineNO.Text);
            vehicleinfo.AddCell(new Phrase("VIN No:", bodyFont));
            vehicleinfo.AddCell(txtVehVinNo.Text);
            vehicleinfo.AddCell(new Phrase("COR Scan:", bodyFont));
            vehicleinfo.AddCell(lblCORScan.Text);
            vehicleinfo.AddCell(new Phrase("COR Expiry:", bodyFont));
            vehicleinfo.AddCell(corexpiry.ToShortDateString());
            vehicleinfo.AddCell(new Phrase("Operating License:", bodyFont));
            vehicleinfo.AddCell(txtOperatingLicense.Text);
            vehicleinfo.AddCell(new Phrase("OL Expiry:", bodyFont));
            vehicleinfo.AddCell(opexpiry.ToShortDateString());
            document.Add(vehicleinfo);

            // Add the "Process Info" subtitle

            document.Add(new Paragraph("Process Info", subTitleFont));
            var process = new PdfPTable(2);
            process.HorizontalAlignment = 0;
            process.SpacingBefore = 5;
            process.SpacingAfter = 5;
            process.DefaultCell.Border = 0;

            process.AddCell(new Phrase("Status:", bodyFont));
            process.AddCell(ddlStatus.SelectedItem.Text);
            process.AddCell(new Phrase("Reason:", bodyFont));
            process.AddCell(ddlReason.SelectedItem.Text);
            process.AddCell(new Phrase("Print Date:", bodyFont));
            process.AddCell(currentDate);
            document.Add(process);

            var godbn = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/godurban.png"));
            godbn.SetAbsolutePosition(10, 800);
            godbn.ScaleAbsolute(200f, 40f);
            godbn.ScaleToFit(500f, 40f);
            godbn.ScaledHeight.ToString();
            godbn.ScaledWidth.ToString();

            //godbn.ScaleToFit(105f, 105f);
            //godbn.Alignment = Element.ALIGN_LEFT;

            document.Add(godbn);

            var muvo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/moja.png"));
            muvo.SetAbsolutePosition(415, 800);
            muvo.ScaleAbsolute(170f, 40f);
            godbn.ScaleToFit(300f, 40f);
            godbn.ScaledHeight.ToString();
            godbn.ScaledWidth.ToString();

            //muvo.ScaleToFit(105f, 105f);
            //muvo.Alignment = Element.ALIGN_RIGHT;

            document.Add(muvo);

            document.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Vehicle.pdf");
            Response.BinaryWrite(output.ToArray());
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ddlStatus.SelectedItem.Text == "Inactive") || (ddlStatus.SelectedItem.Text == "Cancelled") || (ddlStatus.SelectedItem.Text == "Pending"))
            {
                divReason.Style.Add("display", "inline");
                divReason1.Style.Add("display", "inline");
            }
            else
            {
                divReason.Style.Add("display", "none");
                divReason1.Style.Add("display", "none");
            }
        }

        protected void gvVehicle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string st = e.Row.Cells[15].Text.ToString();
                if (st == "Approved")
                {
                    LinkButton lnkPrint = (LinkButton)e.Row.Cells[2].FindControl("lnkPrint");
                    lnkPrint.Visible = true;
                }
                else
                {
                    LinkButton lnkPrint = (LinkButton)e.Row.Cells[2].FindControl("lnkPrint");
                    lnkPrint.Visible = false;
                }
            }
        }
    }
}