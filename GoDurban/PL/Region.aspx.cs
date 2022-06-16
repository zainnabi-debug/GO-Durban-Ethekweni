using GoDurban.BL;
using GoDurban.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Region : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        
        public static int accID;

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

            if (!IsPostBack)
            {
                LoadData();
                LoadAccountType();
                LoadBanks();
                LoadBranches();
                LoadStatusAll();
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

        public void LoadAccountType()
        {
            //ddlAccountType.DataSource = db.AccountTypes.ToList();
            //ddlAccountType.DataTextField = "AccountTypeName";
            //ddlAccountType.DataValueField = "AccountTypeID";
            //ddlAccountType.DataBind();
        }

        public void LoadBanks()
        {
            BankBL BL = new BankBL();
            ddlBank.DataSource = BL.LoadBanks();
            ddlBank.DataTextField = "BankName";
            ddlBank.DataValueField = "BankID";
            ddlBank.DataBind();
        }

        public void LoadBranches()
        {
            BankBL BL = new BankBL();
            ddlBranch.DataSource = BL.LoadBranchs();
            ddlBranch.DataTextField = "BranchCode";
            ddlBranch.DataValueField = "BankID";
            ddlBranch.DataBind();
        }

        public void LoadBanksAndBranches()
        {
            ddlBank.DataSource = db.Banks.ToList();
            ddlBank.DataTextField = "BankName";
            ddlBank.DataValueField = "BankID";
            ddlBank.DataBind();

            ddlBranch.DataSource = db.Banks.ToList();
            ddlBranch.DataTextField = "BranchCode";
            ddlBranch.DataValueField = "BankID";
            ddlBranch.DataBind();
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

        protected void GetPrimaryKey()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            SqlConnection csdb = new SqlConnection(constr);

            //SqlDataAdapter date = new SqlDataAdapter("SELECT max(AccountID)  FROM[Account]", csdb);
            //SqlDataAdapter adap = new SqlDataAdapter("SELECT AccountID  FROM[Account] where AccountNo = '"+txtAccNo.Text+"' )", csdb);
            SqlDataAdapter adap = new SqlDataAdapter("SELECT AccountID FROM [Account] WHERE (AccountNo LIKE'" + txtAccNo.Text + "')", csdb);

            //  SqlDataAdapter adap = new SqlDataAdapter("SELECT max(ApplicationID) as ApplicationID FROM Application", csdb);
            DataSet MyDataSet = new DataSet();

            adap.Fill(MyDataSet);
            int vall = 0;
            for (int i = 0; i < MyDataSet.Tables[0].Rows.Count; i++)
            {
                vall = Convert.ToInt16(MyDataSet.Tables[0].Rows[i]["AccountID"].ToString());
            }
            accID = vall;
        }

        public string GenerateLineNo()
        {
            int NumberofDrivers = db.Payments.ToList().Count;
            string refno = "LN";
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

        public string GenerateBatchNo()
        {
            int NumberofDrivers = db.Payments.ToList().Count;
            string refno = "BN";
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

        public string GenerateDocumentNo()
        {
            int NumberofDrivers = db.Payments.ToList().Count;
            string refno = "DN";
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
        public string GenerateRegNo()
        {
            int NumberofDrivers = db.Payments.ToList().Count;
            string refno = "REG";
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
                if (ValidateRegion())
                {
                    // creating an object for business logic

                    AccountBL acBL = new AccountBL();
                    Models.Account acTBL = new Models.Account();

                    RegionBL rBL = new RegionBL();
                    Models.Region rTBL = new Models.Region();

                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        //var Account = db.tbl_Account.ToList().FindAll(x => String.Compare(x.AccountName, (string)txtAccNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        //if (Account.Count > 0 || string.IsNullOrWhiteSpace(txtAccNo.Text))
                        //{
                        //    txtAccNo.Text = string.Empty;
                        //    txtAccNo.Focus();

                        //    divsuccess.Style.Add("display", "none");
                        //    divdanger.Style.Add("display", "none");
                        //    divinfo.Style.Add("display", "none");
                        //    divwarning.Style.Add("display", "inline");
                        //}


                        var reg = db.Regions.ToList().FindAll(x => String.Compare(x.RegionName, (string)txtRegion.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                        var acc = db.Accounts.ToList().FindAll(x => String.Compare(x.AccountNo, (string)txtAccNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                        string regi = txtRegion.Text.Trim();

                        if (reg.Count > 0 || string.IsNullOrWhiteSpace(txtRegion.Text))
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                            divwarning1.Style.Add("display", "none");

                            txtRegion.Text = string.Empty;
                            txtRegion.Focus();
                        }
                        else if (regi == string.Empty)
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "none");

                            txtRegion.Text = string.Empty;
                            txtRegion.Focus();
                        }
                        else if (acc.Count > 0 || string.IsNullOrWhiteSpace(txtAccNo.Text))
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "inline");

                            txtAccNo.Text = string.Empty;
                            txtAccNo.Focus();
                        }
                        else
                        {
                            acTBL.AccountNo = txtAccNo.Text.Trim();
                            acTBL.BankID = Convert.ToInt16(ddlBank.SelectedValue);
                            acBL.CreateAccount(acTBL);

                            rTBL.RegionName = txtRegion.Text.Trim();
                            rTBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                            GetPrimaryKey();
                            rTBL.AccountID = accID;

                            if (ddlStatus.SelectedIndex == ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending")))
                            {
                                rBL.CreateRegion(rTBL);

                                PaymentBL pBL = new PaymentBL();
                                Models.Payment pTBL = new Models.Payment();
                                //pTBL.LineNo = GenerateLineNo();
                                //pTBL.BatchNo = GenerateBatchNo();
                                //pTBL.DocNo = GenerateDocumentNo();
                                pTBL.SP = "P";
                                pTBL.PaymentMethod = "T";
                                pTBL.Date = DateTime.Now;
                                pTBL.IsPaymentSent = "No";
                                pTBL.EntityType = "BNR";
                                //pTBL.VoidedPayment = false;

                                var regid = db.Regions.ToList().FirstOrDefault(x => x.RegionName == txtRegion.Text.ToString());
                                pTBL.RegionID = regid.RegionID;
                                pTBL.AccountNo = regid.RegionNo;
                                pTBL.AccountName = regid.RegionName;

                                var accno = db.Accounts.ToList().FirstOrDefault(x => x.AccountID == regid.AccountID);
                                pTBL.BankAccNo = accno.AccountNo;

                                var bnkno = db.Banks.ToList().FirstOrDefault(x => x.BankID == accno.BankID);
                                pTBL.BankName = bnkno.BankName;
                                pTBL.BankTransit = bnkno.BranchCode;

                                var regled = db.tbl_RegionalLeadership.FirstOrDefault(x => x.RegionID == regid.RegionID);
                                if (regled != null)
                                {
                                    pTBL.PhoneNo = regled.CellNo;
                                    pTBL.AddressLine1 = regled.AddressStreet;
                                    pTBL.AddressLine2 = regled.AddressSuburb;
                                    pTBL.AddressLine3 = regled.AddressCode;
                                    pTBL.City = regled.AddressCity;
                                    pTBL.PostalCode = regled.AddressCode;
                                }

                                pBL.CreatePayment(pTBL);

                                SendEmailPending();

                                Clear();

                                divsuccess.Style.Add("display", "inline");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "none");
                                divwarning1.Style.Add("display", "none");
                            }
                            else
                            {
                                rBL.CreateRegion(rTBL);

                                Clear();

                                divsuccess.Style.Add("display", "inline");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "none");
                                divwarning1.Style.Add("display", "none");
                            }
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        int rId = int.Parse(ViewState["RegionID"].ToString());
                        rTBL.RegionID = rId;
                        rTBL.RegionName = txtRegion.Text.Trim();
                        rTBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);

                        var reg = db.Regions.ToList().Find(x => String.Compare(x.RegionName, (string)txtRegion.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                        var id = db.Regions.ToList().FirstOrDefault(x => x.RegionID == rId);

                        #region account
                        var aa = db.Accounts.ToList().Find(x => x.AccountNo == txtAccNo.Text.Trim());
                        var a = db.Accounts.FirstOrDefault(x => x.AccountID == id.AccountID);
                        if ((aa == null) || (a.AccountNo == txtAccNo.Text.Trim()))
                        {
                            a.AccountNo = txtAccNo.Text.Trim();
                            a.BankID = Convert.ToInt16(ddlBank.SelectedValue);
                            acBL.UpdateAccount(a);

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "none");

                            if ((reg == null) || (id.RegionName == txtRegion.Text.Trim()))
                            {
                                rBL.UpdateRegion(rTBL);
                                
                                PaymentBL pBL = new PaymentBL();
                                Payment pTBL = new Payment();
                                var regId = db.Regions.FirstOrDefault(x => x.RegionID == rId);
                                var payId = db.Payments.ToList().FirstOrDefault(x => x.RegionID == regId.RegionID);
                                if (payId != null)
                                {
                                    payId.Amount = null;
                                    payId.IsPaymentSent = "No"; 
                                    //payId.VoidedPayment = false;
                                    payId.AccountName = txtRegion.Text;
                                    payId.BankAccNo = txtAccNo.Text;
                                    payId.BankName = ddlBank.SelectedItem.Text;
                                    payId.BankTransit = ddlBranch.SelectedItem.Text;
                                    pBL.UpdatePayment(payId);
                                }
                                else
                                {
                                    pTBL.SP = "P";
                                    pTBL.PaymentMethod = "T";
                                    pTBL.Date = DateTime.Now;
                                    pTBL.IsPaymentSent = "No";
                                    pTBL.EntityType = "BNR";
                                    //pTBL.VoidedPayment = false;

                                    var regid = db.Regions.ToList().FirstOrDefault(x => x.RegionName == txtRegion.Text.ToString());
                                    pTBL.RegionID = regid.RegionID;
                                    pTBL.AccountNo = regid.RegionNo;
                                    pTBL.AccountName = regid.RegionName;

                                    var accno = db.Accounts.ToList().FirstOrDefault(x => x.AccountID == regid.AccountID);
                                    pTBL.BankAccNo = accno.AccountNo;

                                    var bnkno = db.Banks.ToList().FirstOrDefault(x => x.BankID == accno.BankID);
                                    pTBL.BankName = bnkno.BankName;
                                    pTBL.BankTransit = bnkno.BranchCode;

                                    var regled = db.tbl_RegionalLeadership.FirstOrDefault(x => x.RegionID == regid.RegionID);
                                    if (regled != null)
                                    {
                                        pTBL.PhoneNo = regled.CellNo;
                                        pTBL.AddressLine1 = regled.AddressStreet;
                                        pTBL.AddressLine2 = regled.AddressSuburb;
                                        pTBL.AddressLine3 = regled.AddressCode;
                                        pTBL.City = regled.AddressCity;
                                        pTBL.PostalCode = regled.AddressCode;
                                    }

                                    pBL.CreatePayment(pTBL);
                                }

                                SendEmailPendingEdited();

                                Clear();

                                btnAdd.Text = "Add";

                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "inline");
                                divwarning.Style.Add("display", "none");
                                divwarning1.Style.Add("display", "none");

                            }
                            else
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "inline");
                                divwarning1.Style.Add("display", "none");

                                txtRegion.Text = string.Empty;
                                txtRegion.Focus();

                                //lblError.Visible = true;
                                ////lblError.Text = "Connection Problem: " + ex.Message.ToString();
                                //lblError.Text = "Id number exists.";
                            }
                        }
                        else
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "inline");

                            txtAccNo.Text = string.Empty;
                            txtAccNo.Focus();
                        }
                        #endregion
                    }
                    //Refresh Grid
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblError.Visible = true;
                lblError.Text = "Connection Problem: " + ex.ToString();
                //lblError.Text = "Network problem or services are currently down, please contact support team.";
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
                    EML.Email("Moja Cruise System - Region Registration", "Hello " + name + " " + surname + "<br/> <br/>"
                        + "Please check the pending record of Account: " + txtAccNo.Text + " " + "<br/> <br/>"
                        + "<a href = '" + uri + "'>Please click the link to login.</a>" + " " + "<br/> <br/>"
                         + "For more details feel free to contact the Ethekwini Transport Authority" + "<br/>" +
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
                    lblError.Text = "Network problem or services are currently down, please contact support team.";
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
                    EML.Email("Moja Cruise System - Region Edited", "Hello " + name + " " + surname + "<br/> <br/>"
                        + "Please check the pending record of Account: " + txtAccNo.Text + " " + "<br/> <br/>"
                        + "<a href = '" + uri + "'>Please click the link to login.</a>" + " " + "<br/> <br/>"
                         + "For more details feel free to contact the Ethekwini Transport Authority" + "<br/>" +
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
                    lblError.Text = "Network problem or services are currently down, please contact support team.";
                }

                db.SaveChanges();
            }
        }

        private bool ValidateRegion()
        {
            bool valid = true;

            if ((txtRegion.Text == string.Empty))
            {
                txtRegion.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtRegion.Style.Add("border", "");
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
            if ((txtAccNo.Text == string.Empty))
            {
                txtAccNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtAccNo.Style.Add("border", "");
            }
            if ((ddlBank.SelectedValue == "0"))
            {
                ddlBank.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlBank.Style.Add("border", "");
            }
            if ((ddlBranch.SelectedValue == "0"))
            {
                ddlBranch.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlBranch.Style.Add("border", "");
            }

            return valid;
        }

        protected void Clear()
        {
            txtRegion.Text = string.Empty;
            txtAccNo.Text = string.Empty;
            ddlBank.SelectedValue = "0";
            ddlBranch.SelectedValue = "0";
            //ddlStatus.SelectedValue = "0";
            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtRegion.Style.Add("border", "");
            txtAccNo.Style.Add("border", "");
            ddlBank.Style.Add("border", "");
            ddlBranch.Style.Add("border", "");
            ddlStatus.Style.Add("border", "");

            divwarning.Style.Add("display", "none");
            divwarning1.Style.Add("display", "none");
        }

        private void LoadData()
        {
            var data = (from a in db.Regions
                        join b in db.Accounts on a.AccountID equals b.AccountID
                        join c in db.Banks on b.BankID equals c.BankID
                        join d in db.tbl_Status on a.StatusID equals d.StatusID
                        select new
                        {
                            a.RegionID,
                            a.RegionName,
                            a.RegionNo,
                            b.AccountID,
                            b.AccountNo,
                            c.BankID,
                            c.BankName,
                            c.BranchCode,
                            d.StatusDescription,
                        });
            gvRegion.DataSource = data.ToList().OrderByDescending(x => x.RegionID);
            gvRegion.DataBind();
        }

        //objListOrder.OrderBy(o=>o.OrderDate).ToList()
        protected void gvRegion_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvRegion.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvRegion.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvRegion.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvRegion.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvRegion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvRegion.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvRegion.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["RegionID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);

                var temp = db.Regions.ToList().FirstOrDefault(x => x.RegionID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");

                    RemoveBorder();

                    Session["RegionID"] = Id;
                    txtRegion.Text = temp.RegionName.ToString();

                    //var sts = db.tbl_Status.FirstOrDefault(x => x.StatusID == temp.StatusID);
                    //ddlStatus.SelectedItem.Text = sts.StatusDescription.ToString();

                    var acc = db.Accounts.FirstOrDefault(x => x.AccountID == temp.AccountID);
                    txtAccNo.Text = acc.AccountNo.ToString();

                    var bank = db.Banks.FirstOrDefault(x => x.BankID == acc.BankID);
                    ddlBranch.SelectedValue = bank.BankID.ToString();
                    ddlBank.SelectedValue = bank.BankID.ToString();

                    if (temp.tbl_Status.StatusDescription == "Approved")
                    {
                        ddlStatus.Items.Clear();
                        LoadStatusAll();
                        RemoveDuplicateItems(ddlStatus);
                        ddlStatus.SelectedValue = temp.StatusID.ToString();
                    }
                    else if ((temp.tbl_Status.StatusDescription == "Inactive") || (temp.tbl_Status.StatusDescription == "Cancelled") || (temp.tbl_Status.StatusDescription == "Pending"))
                    {
                        ddlStatus.Items.Clear();
                        LoadStatusNotAll();
                        RemoveDuplicateItems(ddlStatus);
                        ddlStatus.SelectedValue = temp.StatusID.ToString();
                    }

                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
                    ddlStatus.Enabled = false;

                    btnAdd.Text = "Update";
                }
            }

            else if (commandName == "DeleteItem")
            {
                string Source = gvRegion.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["AcountID"] = Id;

                try
                {
                    string html = string.Empty;
                    string url = @"http://r6efmprdbw.durban.gov.za:44007/V1/esbapi/DeleteAccount?AcountID=" + Id;

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
                    throw new Exception("Can't delete record.", ex);
                }

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
            Response.Redirect("~/PL/Region.aspx");
        }

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBranch.SelectedValue = ddlBank.SelectedValue;
            ddlBranch.DataBind();
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBank.SelectedValue = ddlBranch.SelectedValue;
            ddlBank.DataBind();
        }
    }
}