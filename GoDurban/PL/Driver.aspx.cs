using GoDurban.BL;
using GoDurban.Models;
using GoDurban.SMSService;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;

namespace GoDurban.PL
{
    public partial class Driver : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        string currentDate = DateTime.Now.ToString("dd MMM yyyy");

        private bool btnRecommendationLetterScan1 = false;
        private bool btnEmpContScan1 = false;
        private bool btnDriverLicense1 = false;
        private bool btnPRDPScan1 = false;

        public static int driverID;

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

            if (rblEmploymentContract.SelectedValue == "No")
            {
                divEmpContScan.Style.Add("display", "none");
                divEmpContScan1.Style.Add("display", "none");
            }
            else if (rblEmploymentContract.SelectedValue == "Yes")
            {
                divEmpContScan.Style.Add("display", "inline");
                divEmpContScan1.Style.Add("display", "inline");
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
            lblEmpScan1.Visible = false;
            lblPRDPScan1.Visible = false;
            lblRecLetter1.Visible = false;
            lblDriLicense1.Visible = false;

            lblEmpScan2.Visible = false;
            lblPRDPScan2.Visible = false;
            lblRecLetter2.Visible = false;
            lblDriLicense2.Visible = false;

            RemoveBorder();

            if (!IsPostBack)
            {
                LoadGender();
                LoadRace();
                LoadLicenseCode();
                LoadData();
                LoadOwner();
                LoadStatusAll();
                LoadReason();
                //RemoveDuplicateItems(ddlOwner);
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

        public void LoadLicenseCode()
        {
            LicenseCodeBL BL = new LicenseCodeBL();
            ddlLicenseCode.DataSource = BL.LoadLicenseCode();
            ddlLicenseCode.DataTextField = "LicenseCode";
            ddlLicenseCode.DataValueField = "LicenseCodeID";
            ddlLicenseCode.DataBind();
        }
        public void LoadGender()
        {
            GenderBL BL = new GenderBL();
            ddlGender.DataSource = BL.LoadGender();
            ddlGender.DataTextField = "GenderDescription";
            ddlGender.DataValueField = "GenderID";
            ddlGender.DataBind();
        }

        public void LoadRace()
        {
            RaceBL BL = new RaceBL();
            ddlRace.DataSource = BL.LoadRace();
            ddlRace.DataTextField = "RaceDescription";
            ddlRace.DataValueField = "RaceID";
            ddlRace.DataBind();
        }

        public void Clear()
        {
            txtName.Text = string.Empty;
            txtSurname.Text = string.Empty;
            txtIDNo.Text = string.Empty;
            txtCellNo.Text = string.Empty;
            txtOfficeNo.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtStreetNo.Text = string.Empty;
            txtSuburb.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtCode.Text = string.Empty;
            txtEmploymentContractExpiry.Text = string.Empty;
            txtLicenseExpiry.Text = string.Empty;
            txtPRDPCode.Text = string.Empty;
            txtPRDPExpiry.Text = string.Empty;
            rblEmploymentContract.ClearSelection();
            ddlLicenseCode.SelectedValue = "0";
            ddlRace.SelectedValue = "0";
            ddlGender.SelectedValue = "0";
            ddlOwner.SelectedValue = "0";
            ddlStatus.SelectedItem.Text = "Pending";
            ddlReason.SelectedValue = "0";
            spAsterik.Style.Add("display", "none");

            chkReviewContract.Checked = false;
            chkTermsConditions.Checked = false;
        }


        private bool ValidateDriver()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if ((txtName.Text == string.Empty))
            {
                txtName.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtName.Style.Add("border", "");
            }
            if ((txtStreetNo.Text == string.Empty))
            {
                txtStreetNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtStreetNo.Style.Add("border", "");
            }
            if ((txtSuburb.Text == string.Empty))
            {
                txtSuburb.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtSuburb.Style.Add("border", "");
            }
            if ((txtCity.Text == string.Empty))
            {
                txtCity.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCity.Style.Add("border", "");
            }
            if ((txtCode.Text == string.Empty))
            {
                txtCode.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCode.Style.Add("border", "");
            }
            if ((txtSurname.Text == string.Empty))
            {
                txtSurname.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtSurname.Style.Add("border", "");
            }
            if ((txtIDNo.Text == string.Empty))
            {
                txtIDNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtIDNo.Style.Add("border", "");
            }

            if ((txtCellNo.Text == string.Empty))
            {
                txtCellNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCellNo.Style.Add("border", "");
            }

            if ((txtOfficeNo.Text == string.Empty))
            {
                txtOfficeNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtOfficeNo.Style.Add("border", "");
            }


            if ((txtLicenseExpiry.Text == string.Empty))
            {
                txtLicenseExpiry.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtLicenseExpiry.Style.Add("border", "");
            }
            if ((txtPRDPCode.Text == string.Empty))
            {
                txtPRDPCode.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtPRDPCode.Style.Add("border", "");
            }
            if ((txtPRDPExpiry.Text == string.Empty))
            {
                txtPRDPExpiry.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtPRDPExpiry.Style.Add("border", "");
            }
            if ((ddlOwner.SelectedValue == "0"))
            {
                ddlOwner.Style.Add("border", "1px solid red");
                valid = false;
                lblOwner.Text = "Required";
                // ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Owner!');", true);
            }
            else
            {
                ddlOwner.Style.Add("border", "");
            }
            if ((ddlGender.SelectedValue == "0"))
            {
                ddlGender.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlGender.Style.Add("border", "");
            }
            if ((ddlRace.SelectedValue == "0"))
            {
                ddlRace.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlRace.Style.Add("border", "");
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
            if ((ddlLicenseCode.SelectedValue == "0"))
            {
                ddlLicenseCode.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlLicenseCode.Style.Add("border", "");
            }

            if ((!chkReviewContract.Checked))
            {
                chkReviewContract.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                chkReviewContract.Style.Add("border", "");
            }
            if ((!chkTermsConditions.Checked))
            {
                chkTermsConditions.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                chkTermsConditions.Style.Add("border", "");
            }
            string driverlicense = "0";
            driverlicense = txtDriverLicense.Text;

            //if (fuDriverLicenseScan.HasFile == false && txtDriverLicense.Text == string.Empty)
            //{
            //    fuDriverLicenseScan.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    fuDriverLicenseScan.Style.Add("border", "");
            //}

            //if ((fuRecommendationLetterScan.HasFile == false && txtRecommendationLetterScan.Text == string.Empty))
            //{
            //    fuRecommendationLetterScan.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    fuRecommendationLetterScan.Style.Add("border", "");
            //}

            //if ((fuPRDPScan.HasFile == false && txtPRDPScan.Text == string.Empty))
            //{
            //    fuPRDPScan.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    fuPRDPScan.Style.Add("border", "");
            //}

            string empcontract = "0";
            empcontract = txtEmpContScan.Text;
            if (rblEmploymentContract.SelectedValue == "Yes")
            {
                if ((txtEmploymentContractExpiry.Text == string.Empty))
                {
                    txtEmploymentContractExpiry.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    txtEmploymentContractExpiry.Style.Add("border", "");
                }

                spAsterik.Style.Add("display", "inline");

                //if ((fuEmploymentContractScan.HasFile == false && txtEmpContScan.Text == string.Empty))
                //{
                //    fuEmploymentContractScan.Style.Add("border", "1px solid red");
                //    valid = false;
                //}
                //else
                //{
                //    fuEmploymentContractScan.Style.Add("border", "");
                //}
            }
            else if (rblEmploymentContract.SelectedValue == "No")
            {
                fuEmploymentContractScan.Style.Add("border", "");
                txtEmploymentContractExpiry.Style.Add("border", "");
                spAsterik.Style.Add("display", "none");
                //valid = true;
            }

            return valid;
        }

        private bool ValidateFileUpload()
        {
            bool valid = true;

            string recletter1 = "0";
            recletter1 = txtRecommendationLetterScan1.Text;

            string drilicense1 = "0";
            drilicense1 = txtDriverLicense1.Text;

            string prdp1 = "0";
            prdp1 = txtPRDPScan1.Text;

            string empcontract = "0";
            empcontract = txtEmpContScan1.Text;

            if (!btnRecommendationLetterScan1 && txtRecommendationLetterScan1.Text == string.Empty)
            {
                fuRecommendationLetterScan.Style.Add("border", "1px solid red");
                lblRecLetter1.Visible = true;
                valid = false;
                btnRecommendationLetterScan1 = false;
            }
            else
            {
                fuRecommendationLetterScan.Style.Add("border", "");
                valid = true;
            }

            if (!btnDriverLicense1 && txtDriverLicense1.Text == string.Empty)
            {
                fuDriverLicenseScan.Style.Add("border", "1px solid red");
                lblDriLicense1.Visible = true;
                valid = false;
                btnDriverLicense1 = false;
            }
            else
            {
                fuDriverLicenseScan.Style.Add("border", "");
                valid = true;
            }

            if (!btnPRDPScan1 && txtPRDPScan1.Text == string.Empty)
            {
                fuPRDPScan.Style.Add("border", "1px solid red");
                lblPRDPScan1.Visible = true;
                valid = false;
                btnPRDPScan1 = false;
            }
            else
            {
                fuPRDPScan.Style.Add("border", "");
                valid = true;
            }

            //if (rblEmploymentContract.SelectedValue == "Yes")
            //{
            //    if (!btnEmpContScan1 && txtEmpContScan1.Text == string.Empty)
            //    {
            //        fuEmploymentContractScan.Style.Add("border", "1px solid red");
            //        lblEmpScan1.Visible = true;
            //        valid = false;
            //        btnEmpContScan1 = false;
            //    }
            //    else
            //    {
            //        fuEmploymentContractScan.Style.Add("border", "");
            //        valid = true;
            //    }
            //}
            //else if (rblEmploymentContract.SelectedValue == "No")
            //{
            //    fuEmploymentContractScan.Style.Add("border", "");
            //    lblEmpScan1.Visible = false;
            //    btnEmpContScan1 = false;
            //}

            return valid;
        }

        private bool ValidateDriverEdit()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if ((txtName.Text == string.Empty))
            {
                txtName.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtName.Style.Add("border", "");
            }
            if ((txtStreetNo.Text == string.Empty))
            {
                txtStreetNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtStreetNo.Style.Add("border", "");
            }
            if ((txtSuburb.Text == string.Empty))
            {
                txtSuburb.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtSuburb.Style.Add("border", "");
            }
            if ((txtCity.Text == string.Empty))
            {
                txtCity.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCity.Style.Add("border", "");
            }
            if ((txtCode.Text == string.Empty))
            {
                txtCode.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCode.Style.Add("border", "");
            }
            if ((txtSurname.Text == string.Empty))
            {
                txtSurname.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtSurname.Style.Add("border", "");
            }
            if ((txtIDNo.Text == string.Empty))
            {
                txtIDNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtIDNo.Style.Add("border", "");
            }

            if ((txtCellNo.Text == string.Empty))
            {
                txtCellNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtCellNo.Style.Add("border", "");
            }

            if ((txtOfficeNo.Text == string.Empty))
            {
                txtOfficeNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtOfficeNo.Style.Add("border", "");
            }


            if ((txtLicenseExpiry.Text == string.Empty))
            {
                txtLicenseExpiry.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtLicenseExpiry.Style.Add("border", "");
            }
            if ((txtPRDPCode.Text == string.Empty))
            {
                txtPRDPCode.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtPRDPCode.Style.Add("border", "");
            }
            if ((txtPRDPExpiry.Text == string.Empty))
            {
                txtPRDPExpiry.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtPRDPExpiry.Style.Add("border", "");
            }
            if ((ddlOwner.SelectedValue == "0"))
            {
                ddlOwner.Style.Add("border", "1px solid red");
                valid = false;
                lblOwner.Text = "Required";
                // ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Owner!');", true);
            }
            else
            {
                ddlOwner.Style.Add("border", "");
            }
            if ((ddlGender.SelectedValue == "0"))
            {
                ddlGender.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlGender.Style.Add("border", "");
            }
            if ((ddlRace.SelectedValue == "0"))
            {
                ddlRace.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlRace.Style.Add("border", "");
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

            if ((ddlLicenseCode.SelectedValue == "0"))
            {
                ddlLicenseCode.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlLicenseCode.Style.Add("border", "");
            }

            if ((!chkReviewContract.Checked))
            {
                chkReviewContract.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                chkReviewContract.Style.Add("border", "");
            }
            if ((!chkTermsConditions.Checked))
            {
                chkTermsConditions.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                chkTermsConditions.Style.Add("border", "");
            }

            if (rblEmploymentContract.SelectedValue == "Yes")
            {
                if ((txtEmploymentContractExpiry.Text == string.Empty))
                {
                    txtEmploymentContractExpiry.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    txtEmploymentContractExpiry.Style.Add("border", "");
                }

                spAsterik.Style.Add("display", "inline");

            }
            else if (rblEmploymentContract.SelectedValue == "No")
            {
                txtEmploymentContractExpiry.Style.Add("border", "");
                spAsterik.Style.Add("display", "none");
                //valid = true;
            }

            return valid;
        }

        //private bool ValidateRemoveBorder()
        //{
        //    bool valid = true;
        //    if (fuPRDPScan.HasFile == false)
        //    {
        //        fuPRDPScan.Style.Add("border", "");
        //        valid = false;
        //    }
        //    if (fuRecommendationLetterScan.HasFile == false)
        //    {
        //        fuRecommendationLetterScan.Style.Add("border", "");
        //        valid = false;
        //    }
        //    if (fuDriverLicenseScan.HasFile == false)
        //    {
        //        fuDriverLicenseScan.Style.Add("border", "");
        //        valid = false;
        //    }
        //    if (fuEmploymentContractScan.HasFile == false)
        //    {
        //        fuEmploymentContractScan.Style.Add("border", "");
        //        valid = false;
        //    }
        //    return valid;
        //}

        private bool ValidateEmpContract()
        {
            bool valid = true;
            if (rblEmploymentContract.SelectedValue == "0")
            {
                divEmpContScan.Style.Add("display", "none");
                divEmpContScan1.Style.Add("display", "none");
            }
            else
            {
                divEmpContScan.Style.Add("display", "inline");
                divEmpContScan1.Style.Add("display", "inline");
            }
            return valid;

        }

        public string GenerateDriverNo()
        {
            int NumberofDrivers = db.tbl_Driver.ToList().Count;
            string refno = "D";
            if (NumberofDrivers < 10)
            {
                int NewNumberofDrivers = NumberofDrivers + 1;
                if (NewNumberofDrivers < 10)
                {
                    refno += "000" + NewNumberofDrivers;
                }
                else
                {
                    refno += "00" + NewNumberofDrivers;
                }
            }
            else if (NumberofDrivers < 100)
            {
                int NewNumberofProjects = NumberofDrivers + 1;
                if (NewNumberofProjects < 10)
                {
                    refno += "000" + NewNumberofProjects;
                }
                else
                {
                    refno += "00" + NewNumberofProjects;
                }
            }
            else
            {
                int NewNumberofProjects = NumberofDrivers + 1;
                refno += NewNumberofProjects;
            }
            return refno;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DriverBL BL = new DriverBL();
                tbl_Driver TBL = new tbl_Driver();

                //Check button text to add or update
                string temp = btnAdd.Text;

                if (ValidateDriver() && temp.Contains("Add"))
                {
                    var driver = db.tbl_Driver.ToList().FindAll(x => String.Compare(x.IDNo, (string)txtIDNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                    if (driver.Count > 0 || string.IsNullOrWhiteSpace(txtIDNo.Text))
                    {
                        divsuccess.Style.Add("display", "none");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "none");
                        divwarning.Style.Add("display", "inline");

                        txtName.Focus();
                    }
                    else
                    {
                        TBL.AddressCity = txtCity.Text;
                        TBL.AddressCode = (txtCode.Text);
                        TBL.AddressStreet = txtStreetNo.Text;
                        TBL.AddressSuburb = txtSuburb.Text;
                        //TBL.CalcBirthDate = Convert.ToDateTime(txtDOB.Text);
                        TBL.CellNo = (txtCellNo.Text);
                        TBL.Email = txtEmail.Text;
                        TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                        TBL.IDNo = txtIDNo.Text.Trim();
                        TBL.Name = txtName.Text;
                        TBL.OfficeNo = (txtOfficeNo.Text);
                        TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                        TBL.Surname = txtSurname.Text;
                        TBL.EmploymentContract = rblEmploymentContract.SelectedValue;
                        TBL.LicenseCodeID = Convert.ToInt16(ddlLicenseCode.SelectedValue);
                        TBL.OwnerID = Convert.ToInt16(ddlOwner.SelectedValue);
                        //TBL.CalcDriverNO = GenerateDriverNo();
                        TBL.PRDPCode = txtPRDPCode.Text;

                        DateTime prdpexpiry = DateTime.ParseExact(txtPRDPExpiry.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        TBL.PRDPExpiry = prdpexpiry;
                        DateTime licenseexpiry = DateTime.ParseExact(txtLicenseExpiry.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        TBL.LicenseExpiry = licenseexpiry;

                        if (txtEmploymentContractExpiry.Text == string.Empty)
                        {
                            DateTime? dt = string.IsNullOrEmpty(txtEmploymentContractExpiry.Text) ? (DateTime?)null : DateTime.Parse(txtEmploymentContractExpiry.Text);
                            TBL.EmploymentContractExpiry = dt;
                        }
                        else
                        {
                            DateTime empcontrexpiry = DateTime.ParseExact(txtEmploymentContractExpiry.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                            TBL.EmploymentContractExpiry = empcontrexpiry;
                        }

                        //TBL.DriverLicenseScanID = Convert.ToInt16(fuDriverLicenseScan.PostedFile.FileName);
                        //TBL.EmploymentContractScanID = Convert.ToInt16(fuEmploymentContractScan.PostedFile.FileName);
                        //TBL.PRDPScanID = Convert.ToInt16(fuPRDPScan.PostedFile.FileName);
                        //TBL.RecommendationLetterScanID = Convert.ToInt16(fuRecommendationLetterScan.PostedFile.FileName);
                        TBL.ReviewContract = chkReviewContract.Checked;
                        TBL.TermsAndConditions = chkTermsConditions.Checked;

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
                            BL.CreateDriver(TBL);

                            int ownID = Convert.ToInt16(ddlOwner.SelectedValue);
                            var dr = db.tbl_Driver.Where(x => x.OwnerID == ownID && x.StatusID != 15).ToList();
                            if (dr.Count == 0)
                            {
                                OwnerBL ownerBL = new OwnerBL();
                                tbl_Owner owna = new tbl_Owner();
                                owna.OwnerID = ownID;
                                owna.PendingDriver = false;
                                ownerBL.UpdateOwnerPendingDriver(owna);
                            }
                            else
                            {
                                OwnerBL ownerBL = new OwnerBL();
                                tbl_Owner owna = new tbl_Owner();
                                owna.OwnerID = ownID;
                                owna.PendingDriver = true;

                                ownerBL.UpdateOwnerPendingDriver(owna);
                            }

                            SendEmailPending();

                            string dri = "Driver";
                            SendSmsPending(dri);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");

                            //rblEmploymentContract.SelectedValue = "No";
                            divEmpContScan.Style.Add("display", "none");
                            divEmpContScan1.Style.Add("display", "none");

                            divUploadDocuments.Style.Add("display", "none");
                            //divDriver.Style.Add("display", "none");
                            txtEmploymentContractExpiry.Style.Add("border", "");

                            Clear();
                            RemoveBorder();

                            lblEmpScan.Visible = false;
                            lblPRDPScan.Visible = false;
                            lblRecLetter.Visible = false;
                            lblDriLicense.Visible = false;

                            rblEmploymentContract.SelectedValue = "No";

                        }
                        else
                        {
                            BL.CreateDriver(TBL);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");

                            //rblEmploymentContract.SelectedValue = "No";
                            divEmpContScan.Style.Add("display", "none");
                            divEmpContScan1.Style.Add("display", "none");

                            divUploadDocuments.Style.Add("display", "none");
                            //divDriver.Style.Add("display", "none");
                            txtEmploymentContractExpiry.Style.Add("border", "");

                            Clear();
                            RemoveBorder();

                            lblEmpScan.Visible = false;
                            lblPRDPScan.Visible = false;
                            lblRecLetter.Visible = false;
                            lblDriLicense.Visible = false;

                            rblEmploymentContract.SelectedValue = "No";
                        }
                    }
                }
                else if (ValidateDriverEdit() && temp.Contains("Update"))
                {
                    {
                        int Id = int.Parse(ViewState["DriverID"].ToString());
                        TBL.DriverID = Id;
                        TBL.AddressCity = txtCity.Text;
                        TBL.AddressCode = (txtCode.Text);
                        TBL.AddressStreet = txtStreetNo.Text;
                        TBL.AddressSuburb = txtSuburb.Text;
                        //TBL.CalcBirthDate = Convert.ToDateTime(txtDOB.Text);
                        TBL.CellNo = (txtCellNo.Text);
                        TBL.Email = txtEmail.Text;
                        TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                        TBL.IDNo = (txtIDNo.Text);
                        TBL.Name = txtName.Text;
                        TBL.OfficeNo = (txtOfficeNo.Text);
                        TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                        TBL.Surname = txtSurname.Text;
                        TBL.EmploymentContract = rblEmploymentContract.SelectedValue;
                        TBL.LicenseCodeID = Convert.ToInt16(ddlLicenseCode.SelectedValue);
                        TBL.OwnerID = Convert.ToInt16(ddlOwner.SelectedValue);
                        TBL.PRDPCode = txtPRDPCode.Text;

                        DateTime prdpexpiry = DateTime.ParseExact(txtPRDPExpiry.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        TBL.PRDPExpiry = prdpexpiry;
                        DateTime licenseexpiry = DateTime.ParseExact(txtLicenseExpiry.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        TBL.LicenseExpiry = licenseexpiry;

                        if (txtEmploymentContractExpiry.Text == string.Empty)
                        {
                            DateTime? dt = string.IsNullOrEmpty(txtEmploymentContractExpiry.Text) ? (DateTime?)null : DateTime.Parse(txtEmploymentContractExpiry.Text);
                            TBL.EmploymentContractExpiry = dt;
                        }
                        else
                        {
                            DateTime empcontrexpiry = DateTime.ParseExact(txtEmploymentContractExpiry.Text, "M/d/yyyy", CultureInfo.InvariantCulture);
                            TBL.EmploymentContractExpiry = empcontrexpiry;
                        }

                        //TBL.DriverLicenseScanID = Convert.ToInt16(fuDriverLicenseScan.PostedFile.FileName);
                        //TBL.EmploymentContractScanID = Convert.ToInt16(fuEmploymentContractScan.PostedFile.FileName);
                        //TBL.PRDPScanID = Convert.ToInt16(fuPRDPScan.PostedFile.FileName);
                        //TBL.RecommendationLetterScanID = Convert.ToInt16(fuRecommendationLetterScan.PostedFile.FileName);
                        TBL.ReviewContract = chkReviewContract.Checked;
                        TBL.TermsAndConditions = chkTermsConditions.Checked;

                        if (ddlStatus.SelectedItem.Text == "Approved")
                        {
                            TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                        }
                        else if ((ddlStatus.SelectedItem.Text == "Inactive") || (ddlStatus.SelectedItem.Text == "Cancelled") || (ddlStatus.SelectedItem.Text == "Pending"))
                        {
                            TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                            TBL.ReasonID = Convert.ToInt16(ddlReason.SelectedValue);
                        }

                        var d = db.tbl_Driver.ToList().Find(x => x.IDNo == txtIDNo.Text.ToString());
                        var id = db.tbl_Driver.ToList().FirstOrDefault(x => x.DriverID == Id);
                        if ((d == null) || (id.IDNo == txtIDNo.Text.Trim()))
                        {
                            if (ddlStatus.SelectedItem.Text == "Pending")
                            {
                                BL.UpdateDriver(TBL);

                                int ownID = Convert.ToInt16(ddlOwner.SelectedValue);
                                var dr = db.tbl_Driver.Where(x => x.OwnerID == ownID && x.StatusID != 15).ToList();
                                if (dr.Count == 0)
                                {
                                    OwnerBL ownerBL = new OwnerBL();
                                    tbl_Owner owna = new tbl_Owner();
                                    owna.OwnerID = ownID;
                                    owna.PendingDriver = false;
                                    ownerBL.UpdateOwnerPendingDriver(owna);
                                }
                                else
                                {
                                    OwnerBL ownerBL = new OwnerBL();
                                    tbl_Owner owna = new tbl_Owner();
                                    owna.OwnerID = ownID;
                                    owna.PendingDriver = true;
                                    ownerBL.UpdateOwnerPendingDriver(owna);
                                }

                                SendEmailPendingEdited();

                                string dri = "Driver";
                                SendSmsPending(dri);

                                btnAdd.Text = "Add";

                                RemoveBorder();

                                lblEmpScan.Visible = false;
                                lblPRDPScan.Visible = false;
                                lblRecLetter.Visible = false;
                                lblDriLicense.Visible = false;

                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "inline");
                                divwarning.Style.Add("display", "none");
                                divUploadDocuments.Style.Add("display", "none");
                                //divDriver.Style.Add("display", "none");

                                divUploadDocuments.Style.Add("display", "none");
                                divDoc.Style.Add("display", "none");

                                Clear();

                                rblEmploymentContract.SelectedValue = "No";
                            }
                            else
                            {
                                BL.UpdateDriver(TBL);

                                btnAdd.Text = "Add";

                                RemoveBorder();

                                lblEmpScan.Visible = false;
                                lblPRDPScan.Visible = false;
                                lblRecLetter.Visible = false;
                                lblDriLicense.Visible = false;

                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "inline");
                                divwarning.Style.Add("display", "none");
                                divUploadDocuments.Style.Add("display", "none");
                                //divDriver.Style.Add("display", "none");

                                divUploadDocuments.Style.Add("display", "none");
                                divDoc.Style.Add("display", "none");

                                Clear();

                                rblEmploymentContract.SelectedValue = "No";
                            }
                        }
                        else
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");

                            txtIDNo.Text = string.Empty;
                            txtIDNo.Focus();
                        }
                    }

                    ddlStatus.Enabled = false;

                    if (rblEmploymentContract.SelectedValue == "Yes")
                    {
                        divEmpContScan.Style.Add("display", "inline");
                        divEmpContScan1.Style.Add("display", "inline");
                    }
                    else
                    {
                        divEmpContScan.Style.Add("display", "none");
                        divEmpContScan1.Style.Add("display", "none");
                    }
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

        protected void RemoveBorder()
        {
            txtName.Style.Add("border", "");
            txtSurname.Style.Add("border", "");
            txtIDNo.Style.Add("border", "");
            txtCellNo.Style.Add("border", "");
            txtOfficeNo.Style.Add("border", "");
            txtEmail.Style.Add("border", "");
            txtStreetNo.Style.Add("border", "");
            txtSuburb.Style.Add("border", "");
            txtCity.Style.Add("border", "");
            txtCode.Style.Add("border", "");
            rblEmploymentContract.Style.Add("border", "");
            txtEmploymentContractExpiry.Style.Add("border", "");
            txtLicenseExpiry.Style.Add("border", "");
            txtPRDPCode.Style.Add("border", "");
            txtPRDPExpiry.Style.Add("border", "");
            ddlRace.Style.Add("border", "");
            ddlGender.Style.Add("border", "");
            ddlOwner.Style.Add("border", "");
            ddlStatus.Style.Add("border", "");
            ddlLicenseCode.Style.Add("border", "");
            chkTermsConditions.Style.Add("border", "");
            chkReviewContract.Style.Add("border", "");
            ddlReason.Style.Add("border", "");
            fuPRDPScan.Style.Add("border", "");
            fuRecommendationLetterScan.Style.Add("border", "");
            fuDriverLicenseScan.Style.Add("border", "");
            fuEmploymentContractScan.Style.Add("border", "");
            //lblOwner.Visible = false;
        }

        private void LoadData()
        {
            var list = (from a in db.tbl_Driver
                        join b in db.tbl_Gender on a.GenderID equals b.GenderID
                        join c in db.tbl_Owner on a.OwnerID equals c.OwnerID
                        join d in db.tbl_Race on a.RaceID equals d.RaceID
                        join e in db.tbl_Status on a.StatusID equals e.StatusID

                        where ((a.OwnerID == c.OwnerID) && c.StatusID == 15)

                        select new
                        {
                            a.DriverID,
                            a.OwnerID,
                            a.AddressCity,
                            a.AddressCode,
                            a.AddressStreet,
                            a.AddressSuburb,
                            a.CalcDriverNO,
                            a.CellNo,
                            a.Email,
                            b.GenderDescription,
                            a.IDNo,
                            a.Name,
                            a.OfficeNo,
                            d.RaceDescription,
                            a.Surname,
                            OwnerName = c.Name + " " + c.Surname,
                            e.StatusDescription,
                            //f.ReasonDescription
                        });
            gvDriver.DataSource = list.ToList().OrderByDescending(x => x.DriverID);
            gvDriver.DataBind();
        }

        protected void gvDriver_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvDriver.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvDriver.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvDriver.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvDriver.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvDriver_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvDriver.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                Clear();
                ddlStatus.Enabled = true;
                RemoveBorder();
                LoadDMSEmpContract();

                //Accessing BoundField Column
                string Source = gvDriver.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["DriverID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);

                var temp = db.tbl_Driver.ToList().FirstOrDefault(x => x.DriverID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    divDoc.Style.Add("display", "inline");
                    divUploadDocuments.Style.Add("display", "none");

                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");

                    RemoveBorder();

                    Session["DriverID"] = Id;
                    txtName.Text = temp.Name;
                    txtSurname.Text = temp.Surname;
                    txtIDNo.Text = temp.IDNo;
                    txtCellNo.Text = temp.CellNo;
                    txtOfficeNo.Text = temp.OfficeNo;
                    txtEmail.Text = temp.Email;
                    txtStreetNo.Text = temp.AddressStreet;
                    txtSuburb.Text = temp.AddressSuburb;
                    txtCity.Text = temp.AddressCity;
                    txtCode.Text = temp.AddressCode;
                    txtPRDPCode.Text = temp.PRDPCode;

                    //string date = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtPRDPExpiry.Text = date;
                    //date = temp.PRDPExpiry.ToString();

                    //string date1 = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtLicenseExpiry.Text = date1;
                    //date1 = temp.LicenseExpiry.ToString();

                    //string date2 = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtEmploymentContractExpiry.Text = date2;
                    //date2 = temp.EmploymentContractExpiry.ToString();

                    var data2 = db.tbl_Driver.ToList().FirstOrDefault(x => x.DriverID == Convert.ToInt16(Id));

                    DateTime date = DateTime.Now;
                    Convert.ToDateTime(date).ToShortDateString();

                    DateTime prdpexpiry = date;
                    prdpexpiry = Convert.ToDateTime(data2.PRDPExpiry);
                    txtPRDPExpiry.Text = data2.PRDPExpiry.Value.ToString("MM/dd/yyyy");

                    DateTime licenseexpiry = date;
                    licenseexpiry = Convert.ToDateTime(data2.LicenseExpiry);
                    txtLicenseExpiry.Text = data2.LicenseExpiry.Value.ToString("MM/dd/yyyy");

                    DateTime empcontexpiry = date;
                    empcontexpiry = Convert.ToDateTime(data2.EmploymentContractExpiry);

                    if (data2.EmploymentContractExpiry != null)
                    {
                        txtEmploymentContractExpiry.Text = data2.EmploymentContractExpiry.Value.ToString("MM/dd/yyyy");
                    }

                    ddlRace.SelectedValue = temp.RaceID.ToString();
                    ddlGender.SelectedValue = temp.GenderID.ToString();
                    ddlOwner.SelectedValue = temp.OwnerID.ToString();
                    chkReviewContract.Checked = temp.ReviewContract.Value;
                    chkTermsConditions.Checked = temp.TermsAndConditions.Value;
                    ddlLicenseCode.SelectedValue = temp.LicenseCodeID.ToString();
                    rblEmploymentContract.SelectedValue = temp.EmploymentContract;

                    if (rblEmploymentContract.SelectedValue == "Yes")
                    {
                        divEmpContScan.Style.Add("display", "inline");
                        divEmpContScan1.Style.Add("display", "inline");
                        spAsterik.Style.Add("display", "inline");
                        //fuEmploymentContractScan.HasFile = temp.EmploymentContractScanID;
                    }
                    else
                    {
                        divEmpContScan.Style.Add("display", "none");
                        divEmpContScan1.Style.Add("display", "none");
                        spAsterik.Style.Add("display", "none");
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

                //string Source = gvDriver.Rows[myRow.RowIndex].Cells[0].Text;

                //ViewState["DriverID"] = Id;

                //var temp = db.tbl_Driver.ToList().FirstOrDefault(x => x.DriverID == Convert.ToInt16(Id));

                //db.tbl_Driver.Remove(temp);
                //db.SaveChanges();
                //RemoveBorder();
                //LoadData(Convert.ToInt32(ddlOwner.SelectedValue));
                //Clear();
                //btnAdd.Text = "Add";

                //divsuccess.Style.Add("display", "none");
                //divdanger.Style.Add("display", "inline");
                //divinfo.Style.Add("display", "none");
                //divwarning.Style.Add("display", "none");

                DriverBL BL = new DriverBL();
                tbl_Driver TBL = new tbl_Driver();

                ViewState["DriverID"] = Id;

                var temp = db.tbl_Driver.ToList().FirstOrDefault(x => x.DriverID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    RemoveBorder();
                    Session["DriverID"] = Id;
                    txtName.Text = temp.Name;
                    txtSurname.Text = temp.Surname;
                    txtIDNo.Text = temp.IDNo;
                    txtCellNo.Text = temp.CellNo;
                    txtOfficeNo.Text = temp.OfficeNo;
                    txtEmail.Text = temp.Email;
                    txtStreetNo.Text = temp.AddressStreet;
                    txtSuburb.Text = temp.AddressSuburb;
                    txtCity.Text = temp.AddressCity;
                    txtCode.Text = temp.AddressCode;
                    txtPRDPCode.Text = temp.PRDPCode;

                    //string date = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtPRDPExpiry.Text = date;
                    //date = temp.PRDPExpiry.ToString();

                    //string date1 = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtLicenseExpiry.Text = date1;
                    //date1 = temp.LicenseExpiry.ToString();

                    //string date2 = DateTime.Now.ToString("dd/MMM/yyyy");
                    //txtEmploymentContractExpiry.Text = date2;
                    //date2 = temp.EmploymentContractExpiry.ToString();

                    var data2 = db.tbl_Driver.ToList().FirstOrDefault(x => x.DriverID == Convert.ToInt16(Id));

                    DateTime date = DateTime.Now;
                    Convert.ToDateTime(date).ToShortDateString();

                    DateTime prdpexpiry = date;
                    prdpexpiry = Convert.ToDateTime(data2.PRDPExpiry);
                    txtPRDPExpiry.Text = data2.PRDPExpiry.Value.ToString("MM/dd/yyyy");

                    DateTime licenseexpiry = date;
                    licenseexpiry = Convert.ToDateTime(data2.LicenseExpiry);
                    txtLicenseExpiry.Text = data2.LicenseExpiry.Value.ToString("MM/dd/yyyy");

                    DateTime empcontexpiry = date;
                    empcontexpiry = Convert.ToDateTime(data2.EmploymentContractExpiry);
                    if (data2.EmploymentContractExpiry != null)
                    {
                        txtEmploymentContractExpiry.Text = data2.EmploymentContractExpiry.Value.ToString("MM/dd/yyyy");
                    }

                    ddlRace.SelectedValue = temp.RaceID.ToString();
                    ddlGender.SelectedValue = temp.GenderID.ToString();
                    ddlOwner.SelectedValue = temp.OwnerID.ToString();
                    chkReviewContract.Checked = temp.ReviewContract.Value;
                    chkTermsConditions.Checked = temp.TermsAndConditions.Value;
                    ddlLicenseCode.SelectedValue = temp.LicenseCodeID.ToString();
                    rblEmploymentContract.SelectedValue = temp.EmploymentContract;

                    if (rblEmploymentContract.SelectedValue == "Yes")
                    {
                        divEmpContScan.Style.Add("display", "inline");
                        divEmpContScan1.Style.Add("display", "inline");
                        //fuEmploymentContractScan.HasFile = temp.EmploymentContractScanID;
                    }
                    else
                    {
                        divEmpContScan.Style.Add("display", "none");
                        divEmpContScan1.Style.Add("display", "none");
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/Driver.aspx");
        }

        private bool ValidateDriverUpload()
        {
            bool valid = true;

            if ((ddlOwner.SelectedValue == "0") && (txtIDNo.Text == string.Empty))
            {
                ddlOwner.Style.Add("border", "1px solid red");
                txtIDNo.Style.Add("border", "1px solid red");
                valid = false;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Owner And ID No!');", true);
            }
            else
            {
                ddlOwner.Style.Add("border", "");
                txtIDNo.Style.Add("border", "");
            }

            return valid;
        }

        //upload commands

        protected void gvAttachments_Commands(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;
            if (commandName == "View")
            {
                LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
                GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
                GridView myGrid = (GridView)sender; // the gridview 
                string Fname = gvUpload.DataKeys[myRow.RowIndex].Values[0].ToString();

                string url = gvUpload.Rows[myRow.RowIndex].Cells[3].Text;
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
                    string Fname = gvUpload.DataKeys[myRow.RowIndex].Values[0].ToString();

                    request.documentName = Fname;

                    SharepointService.DeleteDocumentResponse res = new SharepointService.DeleteDocumentResponse();

                    res.DeleteResponse = serviceSharepoint.DeleteDocument(request);

                    string status = res.DeleteResponse.Status;

                    SharepointService.Search_Request rqt = new SharepointService.Search_Request();
                    rqt.FolderName = ddlOwner.SelectedItem.Text + "-" + txtIDNo.Text;

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
                        gvUpload.DataSource = ser.response;
                        gvUpload.DataBind();
                        fname = filename;
                    }
                    if (fname == null)
                    {
                        gvUpload.Visible = false;

                    }
                    else
                    {
                        gvUpload.Visible = true;
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



        public void LoadDMSEmpContract()
        {
            try
            {
                SharepointService.DocumentManagementSystemClient serviceSharepoint = new SharepointService.DocumentManagementSystemClient();

                SharepointService.Search_Request rqt = new SharepointService.Search_Request();
                rqt.FolderName = ddlOwner.SelectedItem.Text + "-" + txtIDNo.Text;

                SharepointService.SearchDocumentResponse ser = new SharepointService.SearchDocumentResponse();

                ser.response = serviceSharepoint.SearchDocument(rqt);
                string fname = null;
                int count = ser.response.Length;
                for (int i = 0; i < count; i++)
                {
                    string filename = ser.response[i].NAME;
                    string reference = ser.response[i].REFERENCE_NUMBER;
                    string link = ser.response[i].Link_URL;
                    string creationdate = ser.response[i].CREATION_DATE;
                    gvUpload.DataSource = ser.response;
                    gvUpload.DataBind();
                }
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

        protected void btnDoc_Click(object sender, EventArgs e)
        {
            LoadDMSEmpContract();

            if (gvUpload.Rows.Count > 0)
            {
                divUploadDocuments.Style.Add("display", "inline");
                divDoc.Style.Add("display", "none");
            }
            else
            {
                divUploadDocuments.Style.Add("display", "none");
                divDoc.Style.Add("display", "inline");
            }
        }


        //upload
        public void UploadDMSEmpContract()
        {
            try
            {
                //if ((fuEmploymentContractScan.FileName != null)&& (fuPRDPScan.FileName != null)&& (fuDriverLicenseScan.FileName != null)&& (fuRecommendationLetterScan.FileName != null))
                //{
                //    txtEmpContScan1.Text = "0";
                //    lblEmpScan.Visible = false;
                //    lblEmpScan1.Visible = false;
                //    lblEmpScan2.Visible = true;
                //}
                //else
                //{
                SharepointService.DocumentManagementSystemClient serviceSharepoint = new SharepointService.DocumentManagementSystemClient();

                SharepointService.Upload_Request request = new SharepointService.Upload_Request();

                request.fileName = fuEmploymentContractScan.FileName;
                request.fileContent = fuEmploymentContractScan.FileBytes;
                request.destinationLocation = ddlOwner.SelectedItem.Text + "-" + txtIDNo.Text;
                request.sourceLocation = "Employment Contract";

                SharepointService.Upload_Response response = serviceSharepoint.UploadDocument(request);

                string status = response.optStatus;

                string desURL = response.UpladDetails.UploadResult.link_url;

                SharepointService.Search_Request rqt = new SharepointService.Search_Request();
                rqt.FolderName = ddlOwner.SelectedItem.Text + "-" + txtIDNo.Text;

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
                    gvUpload.DataSource = ser.response;
                    gvUpload.DataBind();
                    fname = filename;
                }
                if (fname == null)
                {
                    gvUpload.Visible = false;
                    //divDocMsge.Visible = true;
                    //divDocs.Visible = false;
                }
                else
                {
                    gvUpload.Visible = true;
                    //divDocMsge.Visible = false;
                    //divDocs.Visible = true;
                }
                serviceSharepoint.Close();
                //}
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

        public void UploadDMSRecLetter()
        {
            try
            {
                //if ((fuEmploymentContractScan.FileName != null) && (fuPRDPScan.FileName != null) && (fuDriverLicenseScan.FileName != null) && (fuRecommendationLetterScan.FileName != null))
                //{
                //    txtRecommendationLetterScan1.Text = "0";
                //    lblRecLetter.Visible = false;
                //    lblRecLetter1.Visible = false;
                //    lblRecLetter2.Visible = true;
                //}
                //else
                //{
                SharepointService.DocumentManagementSystemClient serviceSharepoint = new SharepointService.DocumentManagementSystemClient();

                SharepointService.Upload_Request request = new SharepointService.Upload_Request();

                request.fileName = fuRecommendationLetterScan.FileName;
                request.fileContent = fuRecommendationLetterScan.FileBytes;
                request.destinationLocation = ddlOwner.SelectedItem.Text + "-" + txtIDNo.Text;
                request.sourceLocation = "Recommendation Letter";
                //request.destinationLocation = "ref123";


                //request.referenceNo = Convert.ToString(Session["OwnerID"]);


                SharepointService.Upload_Response response = serviceSharepoint.UploadDocument(request);

                string status = response.optStatus;

                string desURL = response.UpladDetails.UploadResult.link_url;


                SharepointService.Search_Request rqt = new SharepointService.Search_Request();
                rqt.FolderName = ddlOwner.SelectedItem.Text + "-" + txtIDNo.Text;

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
                    gvUpload.DataSource = ser.response;
                    gvUpload.DataBind();
                    fname = filename;
                }
                if (fname == null)
                {
                    gvUpload.Visible = false;
                    //divDocMsge.Visible = true;
                    //divDocs.Visible = false;
                }
                else
                {
                    gvUpload.Visible = true;
                    //divDocMsge.Visible = false;
                    //divDocs.Visible = true;
                }
                serviceSharepoint.Close();
                //}
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

        public void UploadDMSDriverLicense()
        {
            string driverstatus = "1";
            driverstatus = txtDriverLicense.Text;
            try
            {
                //if ((fuEmploymentContractScan.FileName != null) && (fuPRDPScan.FileName != null) && (fuDriverLicenseScan.FileName != null) && (fuRecommendationLetterScan.FileName != null))
                //{
                //    txtDriverLicense1.Text = "0";
                //    lblDriLicense.Visible = false;
                //    lblDriLicense1.Visible = false;
                //    lblDriLicense2.Visible = true;
                //}
                //else
                //{
                SharepointService.DocumentManagementSystemClient serviceSharepoint = new SharepointService.DocumentManagementSystemClient();

                SharepointService.Upload_Request request = new SharepointService.Upload_Request();

                request.fileName = fuDriverLicenseScan.FileName;
                request.fileContent = fuDriverLicenseScan.FileBytes;
                request.destinationLocation = ddlOwner.SelectedItem.Text + "-" + txtIDNo.Text;
                request.sourceLocation = "Drivers License";
                //request.destinationLocation = "ref123";


                //request.referenceNo = Convert.ToString(Session["OwnerID"]);


                SharepointService.Upload_Response response = serviceSharepoint.UploadDocument(request);

                string status = response.optStatus;

                string desURL = response.UpladDetails.UploadResult.link_url;


                SharepointService.Search_Request rqt = new SharepointService.Search_Request();
                rqt.FolderName = ddlOwner.SelectedItem.Text + "-" + txtIDNo.Text;

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
                    gvUpload.DataSource = ser.response;
                    gvUpload.DataBind();
                    fname = filename;
                }
                if (fname == null)
                {
                    gvUpload.Visible = false;
                    //divDocMsge.Visible = true;
                    //divDocs.Visible = false;
                }
                else
                {
                    gvUpload.Visible = true;
                    //divDocMsge.Visible = false;
                    //divDocs.Visible = true;
                }
                serviceSharepoint.Close();
                //}
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

        public void UploadDMSPRDP()
        {
            try
            {
                //if ((fuEmploymentContractScan.FileName != null) && (fuPRDPScan.FileName != null) && (fuDriverLicenseScan.FileName != null) && (fuRecommendationLetterScan.FileName != null))
                //{
                //    txtPRDPScan1.Text = "0";
                //    lblPRDPScan.Visible = false;
                //    lblPRDPScan1.Visible = false;
                //    lblPRDPScan2.Visible = true;
                //}
                //else
                //{
                SharepointService.DocumentManagementSystemClient serviceSharepoint = new SharepointService.DocumentManagementSystemClient();

                SharepointService.Upload_Request request = new SharepointService.Upload_Request();

                request.fileName = fuPRDPScan.FileName;
                request.fileContent = fuPRDPScan.FileBytes;
                request.destinationLocation = ddlOwner.SelectedItem.Text + "-" + txtIDNo.Text;
                request.sourceLocation = "PRDP";
                //request.destinationLocation = "ref123";


                //request.referenceNo = Convert.ToString(Session["OwnerID"]);


                SharepointService.Upload_Response response = serviceSharepoint.UploadDocument(request);

                string status = response.optStatus;

                string desURL = response.UpladDetails.UploadResult.link_url;


                SharepointService.Search_Request rqt = new SharepointService.Search_Request();
                rqt.FolderName = ddlOwner.SelectedItem.Text + "-" + txtIDNo.Text;

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
                    gvUpload.DataSource = ser.response;
                    gvUpload.DataBind();
                    fname = filename;
                }
                if (fname == null)
                {
                    gvUpload.Visible = false;
                    //divDocMsge.Visible = true;
                    //divDocs.Visible = false;
                }
                else
                {
                    gvUpload.Visible = true;
                    //divDocMsge.Visible = false;
                    //divDocs.Visible = true;
                }
                serviceSharepoint.Close();
                //}
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

        protected void btnRecLetter_ServerClick(object sender, EventArgs e)
        {
            if ((ValidateDriverUpload()))
            {
                UploadDMSRecLetter();
                divUploadDocuments.Style.Add("display", "inline");
                txtRecommendationLetterScan.Text = "1";

                ValidateDriver();
                lblRecLetter.Visible = true;
                lblRecLetter2.Visible = false;

                btnRecommendationLetterScan1 = true;
                txtRecommendationLetterScan1.Text = "1";

                fuRecommendationLetterScan.Style.Add("border", "");
                fuPRDPScan.Style.Add("border", "");
                fuDriverLicenseScan.Style.Add("border", "");
                fuEmploymentContractScan.Style.Add("border", "");
            }
            //else
            //{
            //    txtRecommendationLetterScan.Text = "0";
            //    txtRecommendationLetterScan1.Text = "0";
            //    btnRecommendationLetterScan1 = false;
            //    lblRecLetter.Visible = false;
            //    lblRecLetter1.Visible = false;
            //    lblRecLetter2.Visible = true;
            //}
            RemoveBorder();
            fuRecommendationLetterScan.Style.Add("color", "#ffffff");
        }

        protected void btnDriverLicense_ServerClick(object sender, EventArgs e)
        {
            if ((ValidateDriverUpload()))
            {
                UploadDMSDriverLicense();
                divUploadDocuments.Style.Add("display", "inline");
                txtDriverLicense.Text = "1";

                ValidateDriver();
                lblDriLicense.Visible = true;
                lblDriLicense2.Visible = false;

                btnDriverLicense1 = true;
                txtDriverLicense1.Text = "1";

                fuDriverLicenseScan.Style.Add("border", "");
                fuPRDPScan.Style.Add("border", "");
                fuRecommendationLetterScan.Style.Add("border", "");
                fuEmploymentContractScan.Style.Add("border", "");
            }
            //else
            //{
            //    txtDriverLicense.Text = "0";
            //    txtDriverLicense1.Text = "0";
            //    btnDriverLicense1 = false;
            //    lblDriLicense.Visible = false;
            //    lblDriLicense1.Visible = false;
            //    lblDriLicense2.Visible = true;
            //}
            RemoveBorder();
            fuDriverLicenseScan.Style.Add("color", "#ffffff");
        }

        protected void btnPRDPScan_ServerClick(object sender, EventArgs e)
        {
            if ((ValidateDriverUpload()))
            {
                UploadDMSPRDP();
                divUploadDocuments.Style.Add("display", "inline");
                txtPRDPScan.Text = "1";
                ValidateDriver();
                lblPRDPScan.Visible = true;
                lblPRDPScan2.Visible = false;

                btnPRDPScan1 = true;
                txtPRDPScan1.Text = "1";

                fuPRDPScan.Style.Add("border", "");
                fuRecommendationLetterScan.Style.Add("border", "");
                fuDriverLicenseScan.Style.Add("border", "");
                fuEmploymentContractScan.Style.Add("border", "");
            }
            //else
            //{
            //    txtPRDPScan.Text = "0";
            //    txtPRDPScan1.Text = "0";
            //    btnPRDPScan1 = false;
            //    lblPRDPScan.Visible = false;
            //    lblPRDPScan1.Visible = false;
            //    lblPRDPScan2.Visible = true;
            //}
            RemoveBorder();
            fuPRDPScan.Style.Add("color", "#ffffff");
        }

        protected void btnEmpContract_ServerClick(object sender, EventArgs e)
        {
            if ((ValidateDriverUpload()))
            {
                UploadDMSEmpContract();
                divUploadDocuments.Style.Add("display", "inline");
                txtEmpContScan.Text = "1";
                ValidateDriver();
                lblEmpScan.Visible = true;
                lblEmpScan2.Visible = false;

                btnEmpContScan1 = true;
                txtEmpContScan1.Text = "1";

                fuEmploymentContractScan.Style.Add("border", "");
                fuPRDPScan.Style.Add("border", "");
                fuRecommendationLetterScan.Style.Add("border", "");
                fuDriverLicenseScan.Style.Add("border", "");
            }
            //else
            //{
            //    txtEmpContScan.Text = "0";
            //    txtEmpContScan1.Text = "0";
            //    btnEmpContScan1 = false;
            //    lblEmpScan.Visible = false;
            //    lblEmpScan1.Visible = false;
            //    lblEmpScan2.Visible = true;
            //}
            RemoveBorder();
            fuEmploymentContractScan.Style.Add("color", "#ffffff");
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

        protected void rblEmploymentContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ValidateEmpContract();
        }

        protected void btnDeleteDoc_Click(object sender, EventArgs e)
        {
            try
            {
                SharepointService.DocumentManagementSystemClient serviceSharepoint = new SharepointService.DocumentManagementSystemClient();

                SharepointService.DeleteRequest request = new SharepointService.DeleteRequest();

                Button lnkBtn = new Button();

                GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
                GridView myGrid = (GridView)sender; // the gridview 
                string Fname = gvUpload.DataKeys[myRow.RowIndex].Values[0].ToString();

                request.documentName = Fname;

                SharepointService.DeleteDocumentResponse res = new SharepointService.DeleteDocumentResponse();

                res.DeleteResponse = serviceSharepoint.DeleteDocument(request);

                string status = res.DeleteResponse.Status;

                SharepointService.Search_Request rqt = new SharepointService.Search_Request();
                rqt.FolderName = ddlOwner.SelectedItem.Text.Trim();

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
                    gvUpload.DataSource = ser.response;
                    gvUpload.DataBind();
                    fname = filename;
                }
                if (fname == null)
                {
                    gvUpload.Visible = false;

                }
                else
                {
                    gvUpload.Visible = true;
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
                lblError.Text = "Can't delete document. Network problem or services are currently down, please contact support team.";
            }
        }

        protected void GetDriverNo()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            SqlConnection csdb = new SqlConnection(constr);

            SqlDataAdapter date = new SqlDataAdapter("SELECT max(DriverID)  FROM[tbl_Driver]", csdb);
            SqlDataAdapter adap = new SqlDataAdapter("SELECT DriverID  FROM[tbl_Driver] where DriverID = (SELECT max(DriverID)  FROM[tbl_Driver])", csdb);

            //  SqlDataAdapter adap = new SqlDataAdapter("SELECT max(ApplicationID) as ApplicationID FROM Application", csdb);
            DataSet MyDataSet = new DataSet();

            adap.Fill(MyDataSet);
            int vall = 0;
            for (int i = 0; i < MyDataSet.Tables[0].Rows.Count; i++)
            {
                vall = Convert.ToInt16(MyDataSet.Tables[0].Rows[i]["DriverID"].ToString());
            }
            driverID = vall;
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
                    EML.Email("Moja Cruise System - Driver Registration", "Hello " + name + " " + surname + "<br/> <br/>"
                        + "Please check the pending record of Driver: " + txtName.Text + " " + txtSurname.Text + ", " + "ID No: " + txtIDNo.Text + "<br/> <br/>"
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
                    EML.Email("Moja Cruise System - Driver Edited", "Hello " + name + " " + surname + "<br/> <br/>"
                        + "Please check the pending record of Driver: " + txtName.Text + " " + txtSurname.Text + ", " + "ID No: " + txtIDNo.Text + "<br/> <br/>"
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

        public void SendSmsApproved(string driver)
        {
            try
            {
                if (ddlOwner.SelectedValue != "0")
                {

                    string driverID = Session["DriverID"].ToString();
                    string driverno, cellno;
                    var data = db.tbl_Driver.ToList().FirstOrDefault(x => x.DriverID == Convert.ToInt16(driverID));
                    driverno = data.CalcDriverNO;
                    cellno = data.CellNo;

                    string status = null;

                    SMSService.PortTypeClient smsserv = new SMSService.PortTypeClient();
                    SMSService.SendSMSReq_T req = new SMSService.SendSMSReq_T();

                    req.MobileNo = cellno;
                    req.TemplateId = "GDDriver";

                    Parameters_T objp = new Parameters_T();

                    req.Parameters = objp;
                    req.Parameters.Parameter1 = new Parameter1_T();
                    req.Parameters.Parameter1.Name = "Parameter1";
                    req.Parameters.Parameter1.Value = driver;

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

        public void SendSmsPending(string driver)
        {
            try
            {
                if (ddlOwner.SelectedValue != "0")
                {
                    GetDriverNo();

                    var data1 = db.tbl_Driver.ToList().FirstOrDefault(x => x.DriverID == driverID);
                    string driverno = data1.CalcDriverNO;
                    string cellno = data1.CellNo;

                    string status = null;

                    SMSService.PortTypeClient smsserv = new SMSService.PortTypeClient();
                    SMSService.SendSMSReq_T req = new SMSService.SendSMSReq_T();

                    req.MobileNo = cellno;
                    req.TemplateId = "GDDriver";

                    Parameters_T objp = new Parameters_T();

                    req.Parameters = objp;
                    req.Parameters.Parameter1 = new Parameter1_T();
                    req.Parameters.Parameter1.Name = "Parameter1";
                    req.Parameters.Parameter1.Value = driver;

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

            var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
            var subTitleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);

            document.Add(new Paragraph("", titleFont));
            document.Add(Chunk.NEWLINE);

            document.Add(new Paragraph("Driver", titleFont));

            document.Add(new Paragraph("Owner Details", subTitleFont));
            var ownerdetails = new PdfPTable(2);
            ownerdetails.HorizontalAlignment = 0;
            ownerdetails.SpacingBefore = 5;
            ownerdetails.SpacingAfter = 5;
            ownerdetails.DefaultCell.Border = 0;

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

            //ownerdetails.SetWidths(new int[] { 1, 4 });
            ownerdetails.AddCell(new Phrase("Name:", bodyFont));
            ownerdetails.AddCell(name);
            ownerdetails.AddCell(new Phrase("Surname:", bodyFont));
            ownerdetails.AddCell(surname);
            ownerdetails.AddCell(new Phrase("ID No:", bodyFont));
            ownerdetails.AddCell(idno);
            ownerdetails.AddCell(new Phrase("Cell No:", bodyFont));
            ownerdetails.AddCell(txtCellNo.Text);
            ownerdetails.AddCell(new Phrase("Office No:", bodyFont));
            ownerdetails.AddCell(officeno);
            ownerdetails.AddCell(new Phrase("Email:", bodyFont));
            ownerdetails.AddCell(email);
            ownerdetails.AddCell(new Phrase("Gender:", bodyFont));
            ownerdetails.AddCell(gender);
            ownerdetails.AddCell(new Phrase("Race:", bodyFont));
            ownerdetails.AddCell(race);
            document.Add(ownerdetails);

            document.Add(new Paragraph("Driver Personal Details", subTitleFont));
            var driverdetails = new PdfPTable(2);
            driverdetails.HorizontalAlignment = 0;
            driverdetails.SpacingBefore = 5;
            driverdetails.SpacingAfter = 5;
            driverdetails.DefaultCell.Border = 0;

            //personaldetails.SetWidths(new int[] { 1, 4 });
            driverdetails.AddCell(new Phrase("Name:", bodyFont));
            driverdetails.AddCell(txtName.Text);
            driverdetails.AddCell(new Phrase("Surname:", bodyFont));
            driverdetails.AddCell(txtSurname.Text);
            driverdetails.AddCell(new Phrase("ID No:", bodyFont));
            driverdetails.AddCell(txtIDNo.Text);
            driverdetails.AddCell(new Phrase("Cell No:", bodyFont));
            driverdetails.AddCell(txtCellNo.Text);
            driverdetails.AddCell(new Phrase("Office No:", bodyFont));
            driverdetails.AddCell(txtOfficeNo.Text);
            driverdetails.AddCell(new Phrase("Email:", bodyFont));
            driverdetails.AddCell(txtEmail.Text);
            driverdetails.AddCell(new Phrase("Gender:", bodyFont));
            driverdetails.AddCell(ddlGender.SelectedItem.Text);
            driverdetails.AddCell(new Phrase("Race:", bodyFont));
            driverdetails.AddCell(ddlRace.SelectedItem.Text);
            document.Add(driverdetails);

            document.Add(new Paragraph("Driver Info", subTitleFont));
            var vehicleinfo = new PdfPTable(2);
            vehicleinfo.HorizontalAlignment = 0;
            vehicleinfo.SpacingBefore = 5;
            vehicleinfo.SpacingAfter = 5;
            vehicleinfo.DefaultCell.Border = 0;

            int driverID = Convert.ToInt16(Session["DriverID"].ToString());
            var data1 = db.tbl_Driver.ToList().FirstOrDefault(x => x.DriverID == driverID);
            string driverno = data1.CalcDriverNO;

            DateTime licenseexpiry, prdpexpiry, empcontractexpiry;
            licenseexpiry = Convert.ToDateTime(data1.LicenseExpiry);
            prdpexpiry = Convert.ToDateTime(data1.PRDPExpiry);
            empcontractexpiry = Convert.ToDateTime(data1.EmploymentContractExpiry);

            vehicleinfo.AddCell(new Phrase("Driver No:", bodyFont));
            vehicleinfo.AddCell(driverno);
            vehicleinfo.AddCell(new Phrase("License Code:", bodyFont));
            vehicleinfo.AddCell(ddlLicenseCode.SelectedItem.Text);
            vehicleinfo.AddCell(new Phrase("License Expiry:", bodyFont));
            vehicleinfo.AddCell(licenseexpiry.ToShortDateString());
            vehicleinfo.AddCell(new Phrase("PRDP Code:", bodyFont));
            vehicleinfo.AddCell(txtPRDPCode.Text);
            vehicleinfo.AddCell(new Phrase("PRDP Expiry:", bodyFont));
            vehicleinfo.AddCell(prdpexpiry.ToShortDateString());
            vehicleinfo.AddCell(new Phrase("Employment Contract:", bodyFont));
            vehicleinfo.AddCell(rblEmploymentContract.Text);
            vehicleinfo.AddCell(new Phrase("Employment Contract Expiry:", bodyFont));

            if (data1.EmploymentContractExpiry != null)
            { vehicleinfo.AddCell(empcontractexpiry.ToShortDateString()); }
            else
            { vehicleinfo.AddCell("N/A"); }

            vehicleinfo.AddCell(new Phrase("PRDP Scan:", bodyFont));
            vehicleinfo.AddCell(lblPRDPScan.Text);
            vehicleinfo.AddCell(new Phrase("Recommendation Letter Scan:", bodyFont));
            vehicleinfo.AddCell(lblRecLetter.Text);

            vehicleinfo.AddCell(new Phrase("Employment Contract Scan:", bodyFont));

            if (rblEmploymentContract.SelectedValue == "Yes")
            {
                vehicleinfo.AddCell(lblEmpScan.Text);
            }
            else
            {
                vehicleinfo.AddCell("N/A");
            }

            vehicleinfo.AddCell(new Phrase("Driver License Scan:", bodyFont));
            vehicleinfo.AddCell(lblDriLicense.Text);
            document.Add(vehicleinfo);

            // Add the "Address" subtitle

            document.Add(new Paragraph("Address", subTitleFont));
            var address = new PdfPTable(2);
            address.HorizontalAlignment = 0;
            address.SpacingBefore = 5;
            address.SpacingAfter = 5;
            address.DefaultCell.Border = 0;

            address.AddCell(new Phrase("Street No & Name:", bodyFont));
            address.AddCell(txtStreetNo.Text);
            address.AddCell(new Phrase("City:", bodyFont));
            address.AddCell(txtCity.Text);
            address.AddCell(new Phrase("Suburb:", bodyFont));
            address.AddCell(txtSuburb.Text);
            address.AddCell(new Phrase("Code:", bodyFont));
            address.AddCell(txtCode.Text);
            document.Add(address);

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
            process.AddCell(new Phrase("Accept Terms & Conditions:", bodyFont));
            process.AddCell("Yes");
            process.AddCell(new Phrase("Review Contract:", bodyFont));
            process.AddCell("Yes");
            process.AddCell(new Phrase("Print Date:", bodyFont));
            process.AddCell(currentDate);
            document.Add(process);

            //// Add ending message
            //var endingMessage = new Paragraph("Thank you for your business! If you have any questions about your order, please contact us at 800-555-NORTH.", endingMessageFont);
            //endingMessage.SetAlignment("Center");
            //document.Add(endingMessage);

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
            Response.AddHeader("Content-Disposition", "attachment; filename=Driver.pdf");
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

        protected void gvDriver_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string st = e.Row.Cells[11].Text.ToString();
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