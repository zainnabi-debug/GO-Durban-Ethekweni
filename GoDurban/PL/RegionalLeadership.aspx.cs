using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GoDurban.SMSService;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace GoDurban.PL
{
    public partial class RegionalLeadership : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        string currentDate = DateTime.Now.ToString("dd MMM yyyy");

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
            RemoveBorder();

            if (!IsPostBack)
            {
                LoadGender();
                LoadRace();
                LoadRegion();
                LoadData();
                //LoadStatus();
                LoadStatusNotAll();
                LoadReason();
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

        //public void LoadStatus()
        //{
        //    StatusBL BL = new StatusBL();
        //    ddlStatus.DataSource = BL.LoadStatus();
        //    ddlStatus.DataTextField = "StatusDescription";
        //    ddlStatus.DataValueField = "StatusID";
        //    ddlStatus.DataBind();
        //}

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

        public void LoadReason()
        {
            ReasonBL BL = new ReasonBL();
            ddlReason.DataSource = BL.LoadReason();
            ddlReason.DataTextField = "ReasonDescription";
            ddlReason.DataValueField = "ReasonID";
            ddlReason.DataBind();
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

        public void LoadRegion()
        {
            var data = from a in db.Regions
                       join b in db.tbl_Status on a.StatusID equals b.StatusID
                       where b.StatusDescription == "Approved"
                       select new
                       {
                           a.RegionID,
                           a.RegionName
                       };
            ddlRegion.DataSource = data.ToList();
            ddlRegion.DataValueField = "RegionID";
            ddlRegion.DataTextField = "RegionName";
            ddlRegion.DataBind();
        }        

        private bool ValidateRegionalLeadership()
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
            //if ((txtEmail.Text == string.Empty))
            //{
            //    txtEmail.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    txtEmail.Style.Add("border", "");
            //}
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
            if ((ddlRegion.SelectedValue == "0"))
            {
                ddlRegion.Style.Add("border", "1px solid red");
                valid = false;
                lblRegion.Text = "Required";
            }
            else
            {
                ddlRegion.Style.Add("border", "");
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

            if (!chkPassport.Checked)
            {
                txtPassport.Style.Add("border", "");
                if ((txtIDNo.Text == string.Empty))
                {
                    txtIDNo.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    txtIDNo.Style.Add("border", "");
                }
            }
            else if(chkPassport.Checked)
            {
                txtIDNo.Style.Add("border", "");
                if ((txtPassport.Text == string.Empty))
                {
                    txtPassport.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    txtPassport.Style.Add("border", "");
                }
            }

            return valid;
        }

        private bool ValidateRegionalLeadershipEdit()
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
            //if ((txtEmail.Text == string.Empty))
            //{
            //    txtEmail.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    txtEmail.Style.Add("border", "");
            //}
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
            if ((ddlRegion.SelectedValue == "0"))
            {
                ddlRegion.Style.Add("border", "1px solid red");
                valid = false;
                lblRegion.Text = "Required";
            }
            else
            {
                ddlRegion.Style.Add("border", "");
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

            if (!chkPassport.Checked)
            {
                txtPassport.Style.Add("border", "");
                if ((txtIDNo.Text == string.Empty))
                {
                    txtIDNo.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    txtIDNo.Style.Add("border", "");
                }
            }
            else if (chkPassport.Checked)
            {
                txtIDNo.Style.Add("border", "");
                if ((txtPassport.Text == string.Empty))
                {
                    txtPassport.Style.Add("border", "1px solid red");
                    valid = false;
                }
                else
                {
                    txtPassport.Style.Add("border", "");
                }
            }

            return valid;
        }

        protected void RemoveBorder()
        {
            txtName.Style.Add("border", "");
            txtSurname.Style.Add("border", "");
            txtIDNo.Style.Add("border", "");
            txtPassport.Style.Add("border", "");
            txtCellNo.Style.Add("border", "");
            txtOfficeNo.Style.Add("border", "");
            txtEmail.Style.Add("border", "");
            txtStreetNo.Style.Add("border", "");
            txtSuburb.Style.Add("border", "");
            txtCity.Style.Add("border", "");
            txtCode.Style.Add("border", "");
            ddlRace.Style.Add("border", "");
            ddlGender.Style.Add("border", "");
            ddlRegion.Style.Add("border", "");
            ddlStatus.Style.Add("border", "");
            ddlReason.Style.Add("border", "");
            chkTermsConditions.Style.Add("border", "");
            chkReviewContract.Style.Add("border", "");
        }
        
        private void LoadData()
        {
            var list = (from a in db.tbl_RegionalLeadership
                        join b in db.tbl_Gender on a.GenderID equals b.GenderID
                        join c in db.Regions on a.RegionID equals c.RegionID
                        join d in db.tbl_Race on a.RaceID equals d.RaceID
                        join e in db.tbl_Status on a.StatusID equals e.StatusID

                        where ((a.RegionID == c.RegionID) && c.StatusID == 15)

                        select new
                        {
                            a.RegionalLeaderID,
                            a.AddressCity,
                            a.AddressCode,
                            a.AddressStreet,
                            a.AddressSuburb,
                            a.CalcBirthDate,
                            a.CellNo,
                            a.Email,
                            b.GenderDescription,
                            a.IDNo,
                            a.Passport,
                            a.Name,
                            a.OfficeNo,
                            d.RaceDescription,
                            c.RegionName,
                            a.Surname,
                            e.StatusDescription,
                            //f.ReasonDescription
                        });
            gvRegionalLeader.DataSource = list.ToList().OrderByDescending(x => x.RegionalLeaderID);
            gvRegionalLeader.DataBind();
        }


        public void Clear()
        {
            txtName.Text = string.Empty;
            txtSurname.Text = string.Empty;
            txtIDNo.Text = string.Empty;
            txtPassport.Text = string.Empty;
            txtCellNo.Text = string.Empty;
            txtOfficeNo.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtStreetNo.Text = string.Empty;
            txtSuburb.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtCode.Text = string.Empty;
            ddlRace.SelectedValue = "0";
            ddlGender.SelectedValue = "0";
            ddlRegion.SelectedValue = "0";
            //ddlStatus.SelectedValue = "0";
            ddlReason.SelectedValue = "0";
            txtIDNo.Enabled = true;
            txtPassport.Enabled = false;
            chkReviewContract.Checked = false;
            chkTermsConditions.Checked = false;
            chkPassport.Checked = false;
            spAsterik.Style.Add("display", "none");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRegionalLeadership())
                {
                    // creating an object for business logic
                    RegionalLeadershipBL BL = new RegionalLeadershipBL();
                    tbl_RegionalLeadership TBL = new tbl_RegionalLeadership();

                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        if (!chkPassport.Checked)
                        {
                            var regionalleadership = db.tbl_RegionalLeadership.ToList().FindAll(x => String.Compare(x.IDNo, (string)txtIDNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                            
                            if (regionalleadership.Count > 0)
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "inline");
                                divwarning1.Style.Add("display", "none");

                                txtName.Focus();
                            }
                            else
                            {
                                TBL.AddressCity = txtCity.Text;
                                TBL.AddressCode = txtCode.Text;
                                TBL.AddressStreet = txtStreetNo.Text;
                                TBL.AddressSuburb = txtSuburb.Text;
                                //TBL.CalcBirthDate = model.CalcBirthDate;
                                TBL.CellNo = txtCellNo.Text;
                                TBL.Email = txtEmail.Text;
                                TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                                TBL.IDNo = txtIDNo.Text.Trim();
                                //TBL.Passport = txtPassport.Text.Trim();
                                TBL.IsPassport = false;
                                TBL.Name = txtName.Text;
                                TBL.OfficeNo = txtOfficeNo.Text;
                                TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                                TBL.RegionID = Convert.ToInt16(ddlRegion.SelectedValue);
                                TBL.Surname = txtSurname.Text;
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
                                    BL.CreateRegionalLeadership(TBL);

                                    SendEmailPending();

                                    //string reg = "Regional Leadership";
                                    //SendSmsApproved(reg);

                                    divsuccess.Style.Add("display", "inline");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "none");

                                    Clear();
                                }
                                else
                                {
                                    BL.CreateRegionalLeadership(TBL);

                                    divsuccess.Style.Add("display", "inline");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "none");

                                    Clear();
                                }
                            }
                        }
                        else 
                        {
                            var regionalleadership1 = db.tbl_RegionalLeadership.ToList().FindAll(x => String.Compare(x.Passport, (string)txtPassport.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                            if (regionalleadership1.Count > 0)
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "inline");
                                divwarning1.Style.Add("display", "none");

                                txtName.Focus();
                            }
                            else
                            {
                                TBL.AddressCity = txtCity.Text;
                                TBL.AddressCode = txtCode.Text;
                                TBL.AddressStreet = txtStreetNo.Text;
                                TBL.AddressSuburb = txtSuburb.Text;
                                //TBL.CalcBirthDate = model.CalcBirthDate;
                                TBL.CellNo = txtCellNo.Text;
                                TBL.Email = txtEmail.Text;
                                TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                                //TBL.IDNo = txtIDNo.Text.Trim();
                                TBL.Passport = txtPassport.Text.Trim();
                                TBL.IsPassport = true;
                                TBL.Name = txtName.Text;
                                TBL.OfficeNo = txtOfficeNo.Text;
                                TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                                TBL.RegionID = Convert.ToInt16(ddlRegion.SelectedValue);
                                TBL.Surname = txtSurname.Text;
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
                                    BL.CreateRegionalLeadership(TBL);

                                    SendEmailPending();

                                    //string reg = "Regional Leadership";
                                    //SendSmsApproved(reg);

                                    divsuccess.Style.Add("display", "inline");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "none");

                                    Clear();
                                }
                                else
                                {
                                    BL.CreateRegionalLeadership(TBL);

                                    divsuccess.Style.Add("display", "inline");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "none");

                                    Clear();
                                }
                            }
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        //var regionalleadership = db.tbl_RegionalLeadership.ToList().FindAll(x => String.Compare(x.IDNo, (string)txtIDNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        //if (regionalleadership.Count > 0)
                        //{
                        //    divsuccess.Style.Add("display", "none");
                        //    divdanger.Style.Add("display", "none");
                        //    divinfo.Style.Add("display", "none");
                        //    divwarning.Style.Add("display", "inline");

                        //    Clear();
                        //    txtName.Focus();
                        //}
                        //else
                        {
                            int Id = int.Parse(ViewState["RegionalLeaderID"].ToString());
                            TBL.RegionalLeaderID = Id;
                            TBL.AddressCity = txtCity.Text;
                            TBL.AddressCode = txtCode.Text;
                            TBL.AddressStreet = txtStreetNo.Text;
                            TBL.AddressSuburb = txtSuburb.Text;
                            //TBL.CalcBirthDate = model.CalcBirthDate;
                            TBL.CellNo = txtCellNo.Text;
                            TBL.Email = txtEmail.Text;
                            TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                            
                            if (chkPassport.Checked)
                            {
                                TBL.Passport = txtPassport.Text.Trim();
                                TBL.IsPassport = true;
                            }
                            else
                            {
                                TBL.IDNo = txtIDNo.Text.Trim();
                                TBL.IsPassport = false;
                            }

                            TBL.Name = txtName.Text;
                            TBL.OfficeNo = txtOfficeNo.Text;
                            TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                            TBL.RegionID = Convert.ToInt16(ddlRegion.SelectedValue);
                            TBL.Surname = txtSurname.Text;
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

                            var r = db.tbl_RegionalLeadership.ToList().Find(x => x.IDNo == txtIDNo.Text.ToString());

                            var id = db.tbl_RegionalLeadership.ToList().FirstOrDefault(x => x.RegionalLeaderID == Id);
                            
                            var p = db.tbl_RegionalLeadership.ToList().Find(x => x.Passport == txtPassport.Text.ToString());

                            var pp = db.tbl_RegionalLeadership.ToList().FirstOrDefault(x => x.RegionalLeaderID == Id);

                            if ((r == null) || (id.IDNo == txtIDNo.Text.Trim()))
                            {
                                if (ddlStatus.SelectedItem.Text == "Pending")
                                {
                                    BL.UpdateRegionalLeadership(TBL);

                                    SendEmailPendingEdited();

                                    //string reg = "Regional Leadership";
                                    //SendSmsApproved(reg);

                                    btnAdd.Text = "Add";

                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "inline");
                                    divwarning.Style.Add("display", "none");
                                    divwarning1.Style.Add("display", "none");

                                    Clear();
                                }
                                else
                                {
                                    BL.UpdateRegionalLeadership(TBL);

                                    btnAdd.Text = "Add";

                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "inline");
                                    divwarning.Style.Add("display", "none");
                                    divwarning1.Style.Add("display", "none");

                                    Clear();
                                }
                            }
                            else if ((p == null) || (pp.Passport == txtPassport.Text.Trim()))
                            {
                                if (ddlStatus.SelectedItem.Text == "Pending")
                                {
                                    BL.UpdateRegionalLeadership(TBL);

                                    SendEmailPendingEdited();

                                    //string reg = "Regional Leadership";
                                    //SendSmsApproved(reg);

                                    btnAdd.Text = "Add";

                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "inline");
                                    divwarning.Style.Add("display", "none");
                                    divwarning1.Style.Add("display", "none");

                                    Clear();
                                }
                                else
                                {
                                    BL.UpdateRegionalLeadership(TBL);

                                    btnAdd.Text = "Add";

                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "inline");
                                    divwarning.Style.Add("display", "none");
                                    divwarning1.Style.Add("display", "none");

                                    Clear();
                                }
                            }
                            else
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "none");
                                divwarning1.Style.Add("display", "inline");

                                //lblError.Visible = true;
                                ////lblError.Text = "Connection Problem: " + ex.Message.ToString();
                                //lblError.Text = "Id number exists.";
                            }
                        }
                    }
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
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Network problem or services are currently down, please contact support team.";
            }
        }

        protected void gvRegionalLeader_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvRegionalLeader.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvRegionalLeader.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["RegionalLeaderID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);

                var temp = db.tbl_RegionalLeadership.ToList().FirstOrDefault(x => x.RegionalLeaderID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    RemoveBorder();
                    Session["RegionalLeaderID"] = Id;
                    txtName.Text = temp.Name;
                    txtSurname.Text = temp.Surname;
                    txtCellNo.Text = temp.CellNo;
                    txtOfficeNo.Text = temp.OfficeNo;
                    txtEmail.Text = temp.Email;
                    txtStreetNo.Text = temp.AddressStreet;
                    txtSuburb.Text = temp.AddressSuburb;
                    txtCity.Text = temp.AddressCity;
                    txtCode.Text = temp.AddressCode;
                    ddlRace.SelectedValue = temp.RaceID.ToString();
                    ddlGender.SelectedValue = temp.GenderID.ToString();
                    ddlRegion.SelectedValue = temp.RegionID.ToString();
                    //ddlStatus.SelectedValue = temp.StatusID.ToString();

                    if (temp.IsPassport.Value == true)
                    {
                        txtIDNo.Enabled = false;
                        txtIDNo.Text = string.Empty;
                        txtPassport.Enabled = true;
                        chkPassport.Checked = Convert.ToBoolean(temp.IsPassport);
                        txtPassport.Text = temp.Passport;
                        spAsterik.Style.Add("display", "inline");
                        spAsterik1.Style.Add("display", "none");
                    }
                    else
                    {
                        txtPassport.Text = string.Empty;
                        txtPassport.Enabled = false;
                        txtIDNo.Text = temp.IDNo;
                        txtIDNo.Enabled = true;
                        chkPassport.Checked = false;
                        spAsterik.Style.Add("display", "none");
                        spAsterik1.Style.Add("display", "inline");
                    }

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

                    chkReviewContract.Checked = temp.ReviewContract.Value;
                    chkTermsConditions.Checked = temp.TermsAndConditions.Value;

                    btnAdd.Text = "Update";
                }
            }
            else if (commandName == "Print")
            {
                LoadStatusAll();

                //string Source = gvRegionalLeader.Rows[myRow.RowIndex].Cells[0].Text;

                //ViewState["RegionalLeaderID"] = Id;

                //var temp = db.tbl_RegionalLeadership.ToList().FirstOrDefault(x => x.RegionalLeaderID == Convert.ToInt16(Id));

                //db.tbl_RegionalLeadership.Remove(temp);
                //db.SaveChanges();
                //RemoveBorder();
                //LoadData();
                //Clear();
                //btnAdd.Text = "Add";

                //divsuccess.Style.Add("display", "none");
                //divdanger.Style.Add("display", "inline");
                //divinfo.Style.Add("display", "none");
                //divwarning.Style.Add("display", "none");

                RegionalLeadershipBL BL = new RegionalLeadershipBL();
                tbl_RegionalLeadership TBL = new tbl_RegionalLeadership();

                ViewState["RegionalLeaderID"] = Id;

                var temp = db.tbl_RegionalLeadership.ToList().FirstOrDefault(x => x.RegionalLeaderID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    RemoveBorder();
                    Session["RegionalLeaderID"] = Id;
                    txtName.Text = temp.Name;
                    txtSurname.Text = temp.Surname;
                    txtCellNo.Text = temp.CellNo;
                    txtOfficeNo.Text = temp.OfficeNo;
                    txtEmail.Text = temp.Email;
                    txtStreetNo.Text = temp.AddressStreet;
                    txtSuburb.Text = temp.AddressSuburb;
                    txtCity.Text = temp.AddressCity;
                    txtCode.Text = temp.AddressCode;
                    ddlRace.SelectedValue = temp.RaceID.ToString();
                    ddlGender.SelectedValue = temp.GenderID.ToString();
                    ddlRegion.SelectedValue = temp.RegionID.ToString();
                    chkReviewContract.Checked = temp.ReviewContract.Value;
                    chkTermsConditions.Checked = temp.TermsAndConditions.Value;

                    ddlStatus.SelectedValue = temp.StatusID.ToString();

                    if (temp.ReasonID != null)
                    {
                        ddlReason.SelectedValue = temp.ReasonID.ToString();
                    }
                    else
                    {
                        ddlReason.SelectedItem.Text = "N/A";
                    }

                    if (temp.Passport != null)
                    {
                        txtPassport.Text = temp.Passport;
                        txtIDNo.Text = "N/A";
                    }
                    else
                    {
                        txtPassport.Text = "N/A";
                        txtIDNo.Text = temp.IDNo;
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

        protected void gvRegionalLeader_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvRegionalLeader.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvRegionalLeader.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvRegionalLeader.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvRegionalLeader.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/RegionalLeadership.aspx");
        }

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegion.SelectedValue == "0")
            {
                lblRegion.Text = "Required";
            }
            else
            {
                lblRegion.Text = "";
            }
        }

        public bool SendSmsPending(string regional)
        {
            bool valid = true;

            try
            {
                string status = null;

                SMSService.PortTypeClient smsserv = new SMSService.PortTypeClient();
                SMSService.SendSMSReq_T req = new SMSService.SendSMSReq_T();

                req.MobileNo = txtCellNo.Text;
                req.TemplateId = "GDRegL";

                Parameters_T objp = new Parameters_T();

                req.Parameters = objp;
                req.Parameters.Parameter1 = new Parameter1_T();
                req.Parameters.Parameter1.Name = "Parameter1";
                req.Parameters.Parameter1.Value = regional;

                req.Parameters = objp;
                req.Parameters.Parameter2 = new Parameter2_T();
                req.Parameters.Parameter2.Name = "Parameter2";
                req.Parameters.Parameter2.Value = "Pending";

                SMSService.SendSMSResp_T obj = smsserv.SendSMSOperation(req);

                status = obj.Status.StatusMessage;

                valid = true;
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

                valid = false;
            }

            return valid;
        }

        public void SendSmsInactive(string regional)
        {
            try
            {
                string status = null;

                SMSService.PortTypeClient smsserv = new SMSService.PortTypeClient();
                SMSService.SendSMSReq_T req = new SMSService.SendSMSReq_T();

                req.MobileNo = txtCellNo.Text;
                req.TemplateId = "GDRegL";

                Parameters_T objp = new Parameters_T();

                req.Parameters = objp;
                req.Parameters.Parameter1 = new Parameter1_T();
                req.Parameters.Parameter1.Name = "Parameter1";
                req.Parameters.Parameter1.Value = regional;

                req.Parameters = objp;
                req.Parameters.Parameter2 = new Parameter2_T();
                req.Parameters.Parameter2.Name = "Parameter2";
                req.Parameters.Parameter2.Value = "Inactive";

                SMSService.SendSMSResp_T obj = smsserv.SendSMSOperation(req);

                status = obj.Status.StatusMessage;
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

        public void SendSmsCancelled(string regional)
        {
            try
            {
                string status = null;

                SMSService.PortTypeClient smsserv = new SMSService.PortTypeClient();
                SMSService.SendSMSReq_T req = new SMSService.SendSMSReq_T();

                req.MobileNo = txtCellNo.Text;
                req.TemplateId = "GDRegL";

                Parameters_T objp = new Parameters_T();

                req.Parameters = objp;
                req.Parameters.Parameter1 = new Parameter1_T();
                req.Parameters.Parameter1.Name = "Parameter1";
                req.Parameters.Parameter1.Value = regional;

                req.Parameters = objp;
                req.Parameters.Parameter2 = new Parameter2_T();
                req.Parameters.Parameter2.Name = "Parameter2";
                req.Parameters.Parameter2.Value = "Cancelled";

                SMSService.SendSMSResp_T obj = smsserv.SendSMSOperation(req);

                status = obj.Status.StatusMessage;
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

        public bool SendSmsApproved(string regional)
        {
            bool valid = true;

            try
            {
                string status = null;

                SMSService.PortTypeClient smsserv = new SMSService.PortTypeClient();
                SMSService.SendSMSReq_T req = new SMSService.SendSMSReq_T();

                req.MobileNo = txtCellNo.Text;
                req.TemplateId = "GDRegL";

                Parameters_T objp = new Parameters_T();

                req.Parameters = objp;
                req.Parameters.Parameter1 = new Parameter1_T();
                req.Parameters.Parameter1.Name = "Parameter1";
                req.Parameters.Parameter1.Value = regional;

                req.Parameters = objp;
                req.Parameters.Parameter2 = new Parameter2_T();
                req.Parameters.Parameter2.Name = "Parameter2";
                req.Parameters.Parameter2.Value = "Approved";

                SMSService.SendSMSResp_T obj = smsserv.SendSMSOperation(req);

                status = obj.Status.StatusMessage;

                valid = true;
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

                valid = false;
            }

            return valid;
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

            document.Add(new Paragraph("Regional Leadership", titleFont));

            document.Add(new Paragraph("Personal Details", subTitleFont));
            var personaldetails = new PdfPTable(2);
            personaldetails.HorizontalAlignment = 0;
            personaldetails.SpacingBefore = 5;
            personaldetails.SpacingAfter = 5;
            personaldetails.DefaultCell.Border = 0;

            //personaldetails.SetWidths(new int[] { 1, 4 });
            personaldetails.AddCell(new Phrase("Name:", bodyFont));
            personaldetails.AddCell(txtName.Text);
            personaldetails.AddCell(new Phrase("Surname:", bodyFont));
            personaldetails.AddCell(txtSurname.Text);
            personaldetails.AddCell(new Phrase("ID No:", bodyFont));
            personaldetails.AddCell(txtIDNo.Text);
            personaldetails.AddCell(new Phrase("Passport No:", bodyFont));
            personaldetails.AddCell(txtPassport.Text);
            personaldetails.AddCell(new Phrase("Cell No:", bodyFont));
            personaldetails.AddCell(txtCellNo.Text);
            personaldetails.AddCell(new Phrase("Office No:", bodyFont));
            personaldetails.AddCell(txtOfficeNo.Text);
            personaldetails.AddCell(new Phrase("Email:", bodyFont));
            personaldetails.AddCell(txtEmail.Text);
            personaldetails.AddCell(new Phrase("Gender:", bodyFont));
            personaldetails.AddCell(ddlGender.SelectedItem.Text);
            personaldetails.AddCell(new Phrase("Race:", bodyFont));
            personaldetails.AddCell(ddlRace.SelectedItem.Text);
            document.Add(personaldetails);

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

            process.AddCell(new Phrase("Region:", bodyFont));
            process.AddCell(ddlRegion.SelectedItem.Text);
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


            //document.Add(new Paragraph("\n"));

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
            Response.AddHeader("Content-Disposition", "attachment; filename=RegionalLeadership.pdf");
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

                    if (chkPassport.Checked)
                    {
                        EmailBL EML = new EmailBL();
                        EML.Email("Moja Cruise System - Regional Leadership Registration", "Hello " + name + " " + surname + "<br/> <br/>"
                                                        + "Please check the pending record of Regional Leadership: " + txtName.Text + " " + txtSurname.Text + ", " + "Passport No.: " + txtPassport.Text + "<br/> <br/>"
                                                        + "<a href = '" + uri + "'>Please click the link to login.</a>" + " " + "<br/> <br/>"
                                                        + "For more details feel free to contact the Ethekwini Transport Authority." + "<br/>" +
                                                        "Regards" + "<br/>" +
                                                        "Ethekwini Transport Authority", email, "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
                    }
                    else
                    {
                        EmailBL EML = new EmailBL();
                        EML.Email("Moja Cruise System - Regional Leadership Registration", "Hello " + name + " " + surname + "<br/> <br/>"
                                                        + "Please check the pending record of Regional Leadership: " + txtName.Text + " " + txtSurname.Text + ", " + "ID No.: " + txtIDNo.Text + "<br/> <br/>"
                                                        + "<a href = '" + uri + "'>Please click the link to login.</a>" + " " + "<br/> <br/>"
                                                        + "For more details feel free to contact the Ethekwini Transport Authority." + "<br/>" +
                                                        "Regards" + "<br/>" +
                                                        "Ethekwini Transport Authority", email, "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
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

                    if (chkPassport.Checked)
                    {
                        EmailBL EML = new EmailBL();
                        EML.Email("Moja Cruise System - Regional Leadership Edited", "Hello " + name + " " + surname + "<br/> <br/>"
                                                        + "Please check the pending record of Regional Leadership: " + txtName.Text + " " + txtSurname.Text + ", " + "Passport No.: " + txtPassport.Text + "<br/> <br/>"
                                                        + "<a href = '" + uri + "'>Please click the link to login.</a>" + " " + "<br/> <br/>"
                                                        + "For more details feel free to contact the Ethekwini Transport Authority." + "<br/>" +
                                                        "Regards" + "<br/>" +
                                                        "Ethekwini Transport Authority", email, "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
                    }
                    else
                    {
                        EmailBL EML = new EmailBL();
                        EML.Email("Moja Cruise System - Regional Leadership Edited", "Hello " + name + " " + surname + "<br/> <br/>"
                                                        + "Please check the pending record of Regional Leadership: " + txtName.Text + " " + txtSurname.Text + ", " + "ID No.: " + txtIDNo.Text + "<br/> <br/>"
                                                        + "<a href = '" + uri + "'>Please click the link to login.</a>" + " " + "<br/> <br/>"
                                                        + "For more details feel free to contact the Ethekwini Transport Authority." + "<br/>" +
                                                        "Regards" + "<br/>" +
                                                        "Ethekwini Transport Authority", email, "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
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
                    lblError.Text = "Can't send email. Network problem or services are currently down, please contact support team.";
                }

                db.SaveChanges();
            }
        }

        protected void chkPassport_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPassport.Checked)
            {
                txtIDNo.Text = string.Empty;
                txtIDNo.Enabled = false;
                txtPassport.Enabled = true;
                spAsterik.Style.Add("display", "inline");
                spAsterik1.Style.Add("display", "none");
            }
            else
            {
                txtPassport.Text = string.Empty;
                txtIDNo.Enabled = true;
                txtPassport.Enabled = false;
                spAsterik.Style.Add("display", "none");
                spAsterik1.Style.Add("display", "inline");
            }
        }

        protected void gvRegionalLeader_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string st = e.Row.Cells[12].Text.ToString();
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