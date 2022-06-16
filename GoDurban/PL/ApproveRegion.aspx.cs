using GoDurban.BL;
using GoDurban.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class ApproveRegion : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        SqlCommand cmd;

        int regID = 0;

        public static int accID;
        protected void Page_Load(object sender, EventArgs e)
        {
            regID = Convert.ToInt32(Request.QueryString["RegionID"].ToString());

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
                LoadStatus();
                LoadBanksAndBranches();
                LoadFields();
            }
            lblError.Visible = false;
            //divRegion.Style.Add("display", "none");
        }

        public void LoadStatus()
        {
            StatusBL BL = new StatusBL();
            ddlStatus.DataSource = BL.LoadStatus();
            ddlStatus.DataTextField = "StatusDescription";
            ddlStatus.DataValueField = "StatusID";
            ddlStatus.DataBind();
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
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
        //public string GenerateRegionNo()
        //{
        //    int NumberofRegions = db.tbl_Region.ToList().Count;
        //    string regno = "R";
        //    if (NumberofRegions < 10)
        //    {
        //        int NewNumberofDrivers = NumberofRegions + 1;
        //        if (NewNumberofDrivers < 10)
        //        {
        //            regno += "000" + NewNumberofDrivers;
        //        }
        //        else
        //        {
        //            regno += "00" + NewNumberofDrivers;
        //        }
        //    }
        //    else if (NumberofRegions < 100)
        //    {
        //        int NewNumberofProjects = NumberofRegions + 1;
        //        if (NewNumberofProjects < 10)
        //        {
        //            regno += "000" + NewNumberofProjects;
        //        }
        //        else
        //        {
        //            regno += "00" + NewNumberofProjects;
        //        }
        //    }
        //    else
        //    {
        //        int NewNumberofProjects = NumberofRegions + 1;
        //        regno += NewNumberofProjects;
        //    }
        //    return regno;
        //}

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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRegion())
                {
                    // creating an object for business logic

                    RegionBL BL = new RegionBL();
                    Models.Region TBL = new Models.Region();

                    AccountBL acBL = new AccountBL();
                    Models.Account acTBL = new Models.Account();

                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var region = db.Regions.ToList().FindAll(x => String.Compare(x.RegionName, (string)txtRegion.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        var acc = db.Accounts.ToList().FindAll(x => String.Compare(x.AccountNo, (string)txtAccNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                        string regi = txtRegion.Text.Trim();

                        if (region.Count > 0 || string.IsNullOrWhiteSpace(txtRegion.Text))
                        {
                            txtRegion.Text = string.Empty;
                            txtRegion.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divinfo1.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                            divwarning1.Style.Add("display", "none");
                        }
                        else if (regi == string.Empty)
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divinfo1.Style.Add("display", "none");
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
                            divinfo1.Style.Add("display", "none");
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

                            TBL.RegionName = txtRegion.Text.Trim();
                            TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                            GetPrimaryKey();
                            TBL.AccountID = accID;
                            BL.CreateRegion(TBL);

                            Clear();

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divinfo1.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "none");
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        //int Id = int.Parse(ViewState["RegionID"].ToString());
                        TBL.RegionID = regID;
                        TBL.RegionName = txtRegion.Text.Trim();
                        TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                        GetPrimaryKey();
                        TBL.AccountID = accID;
                        TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);

                        var reg = db.Regions.ToList().Find(x => String.Compare(x.RegionName, (string)txtRegion.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                        var id = db.Regions.ToList().FirstOrDefault(x => x.RegionID == regID);

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
                            divinfo1.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "none");

                            if ((reg == null) || (id.RegionName == txtRegion.Text.Trim()))
                            {
                                if (ddlStatus.SelectedItem.Text == "Approved")
                                {
                                    BL.UpdateRegion(TBL);

                                    PaymentBL pBL = new PaymentBL();
                                    Payment pTBL = new Payment();
                                    var regId = db.Regions.FirstOrDefault(x => x.RegionID == regID);
                                    var payId = db.Payments.ToList().FirstOrDefault(x => x.RegionID == regId.RegionID);                                    
                                    payId.Amount = Convert.ToDecimal(8000.00);
                                    payId.VoidedPayment = false;
                                    pBL.UpdatePayment(payId);

                                    Clear();

                                    btnAdd.Text = "Update";

                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "inline");
                                    divinfo1.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "none");
                                    divwarning1.Style.Add("display", "none");
                                }
                                else
                                {
                                    BL.UpdateRegion(TBL);

                                    PaymentBL pBL = new PaymentBL();
                                    Payment pTBL = new Payment();
                                    var regId = db.Regions.FirstOrDefault(x => x.RegionID == regID);
                                    var payId = db.Payments.ToList().FirstOrDefault(x => x.RegionID == regId.RegionID);
                                    if (payId != null)
                                    {
                                        payId.Amount = null;
                                        payId.VoidedPayment = false;
                                        pBL.UpdatePayment(payId);
                                    }

                                    Clear();

                                    btnAdd.Text = "Update";

                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divinfo1.Style.Add("display", "inline");
                                    divwarning.Style.Add("display", "none");
                                    divwarning1.Style.Add("display", "none");
                                }
                            }
                            else
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divinfo1.Style.Add("display", "none");
                                divwarning.Style.Add("display", "inline");
                                divwarning1.Style.Add("display", "none");

                                txtRegion.Text = string.Empty;
                                txtRegion.Focus(); 
                            }
                        }
                        else
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divinfo1.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "inline");

                            txtAccNo.Text = string.Empty;
                            txtAccNo.Focus();
                        }
                        #endregion
                    }

                    //Refresh Grid
                    LoadData();

                    //ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Pending"));
                    //ddlStatus.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divinfo1.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
                divwarning1.Style.Add("display", "none");

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Network problem or services are currently down, please contact support team";
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
            if ((ddlBank.SelectedIndex == 0))
            {
                ddlBank.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlBank.Style.Add("border", "");
            }
            if ((ddlBranch.SelectedIndex == 0))
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
            ddlStatus.SelectedValue = "0";
            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtRegion.Style.Add("border", "");
            txtAccNo.Style.Add("border", "");
            ddlBank.Style.Add("border", "");
            ddlBranch.Style.Add("border", "");
            ddlStatus.Style.Add("border", "");
        }

        private void LoadData()
        {
            var data = (from a in db.Regions
                        join b in db.Accounts on a.AccountID equals b.AccountID
                        join c in db.Banks on b.BankID equals c.BankID
                        join d in db.tbl_Status on a.StatusID equals d.StatusID

                        where a.RegionID == regID
                        select new
                        {
                            a.RegionID,
                            a.RegionNo,
                            a.RegionName,
                            b.AccountID,
                            b.AccountNo,
                            c.BankID,
                            c.BankName,
                            c.BranchCode,
                            d.StatusDescription,
                        });
            gvRegion.DataSource = data.ToList();
            gvRegion.DataBind();
        }


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
            divRegion.Style.Add("display", "inline");

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvRegion.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "ApproveItem")
            {
                ddlStatus.Enabled = true;

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
                    divinfo1.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");
                    divwarning1.Style.Add("display", "none");

                    RemoveBorder();

                    Session["RegionID"] = Id;
                    txtRegion.Text = temp.RegionName;

                    //txtAccNo.Text = temp.AccountNo;
                    //var bank = db.tbl_Bank.FirstOrDefault(x => x.BankName == temp.BankName);
                    //ddlBranch.SelectedValue = bank.BankId.ToString();
                    //ddlBank.SelectedValue = bank.BankId.ToString();

                    var acc = db.Accounts.FirstOrDefault(x => x.AccountID == temp.AccountID);
                    txtAccNo.Text = acc.AccountNo.ToString();

                    var bank = db.Banks.FirstOrDefault(x => x.BankID == acc.BankID);
                    ddlBranch.SelectedValue = bank.BankID.ToString();
                    ddlBank.SelectedValue = bank.BankID.ToString();

                    ddlStatus.SelectedValue = temp.StatusID.ToString();

                    btnAdd.Text = "Update";
                }
            }

            else if (commandName == "Print")
            {
                RegionBL BL = new RegionBL();
                Region TBL = new Region();

                ViewState["RegionID"] = Id;

                var temp = db.Regions.ToList().FirstOrDefault(x => x.RegionID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    RemoveBorder();
                    Session["RegionID"] = Id;
                    //txtAccNo.Text = temp.AccountNo;
                    //var bank = db.Banks.FirstOrDefault(x => x.BankName == temp.BankName);
                    //ddlBranch.SelectedValue = bank.BankId.ToString();
                    //ddlBank.SelectedValue = bank.BankId.ToString();
                    txtRegion.Text = temp.RegionName;

                    var acc = db.Accounts.FirstOrDefault(x => x.AccountID == temp.AccountID);
                    txtAccNo.Text = acc.AccountNo.ToString();

                    var bank = db.Banks.FirstOrDefault(x => x.BankID == acc.BankID);
                    ddlBranch.SelectedValue = bank.BankID.ToString();
                    ddlBank.SelectedValue = bank.BankID.ToString();
                }

                CreatePDF();
                Clear();

                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divinfo1.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
                divwarning1.Style.Add("display", "none");
            }

            else if (commandName == "DeleteItem")
            {
                string Source = gvRegion.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["RegionID"] = Id;

                try
                {
                    string html = string.Empty;
                    string url = @"http://r6efmprdbw.durban.gov.za:44007/V1/esbapi/DeleteRegion?RegionID=" + Id;

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
                //catch (WebException e)
                //{
                //    string pageContent = new StreamReader(e.Response.GetResponseStream()).ReadToEnd().ToString();
                //    return pageContent;
                //}

                RemoveBorder();
                LoadData();
                Clear();

                btnAdd.Text = "Add";

                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "inline");
                divinfo.Style.Add("display", "none");
                divinfo1.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
                divwarning1.Style.Add("display", "none");
            }
        }


        public void CreatePDF()
        {
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

            document.Add(new Paragraph("Approve Region", titleFont));

            document.Add(new Paragraph("Region Details", subTitleFont));
            var personaldetails = new PdfPTable(2);
            personaldetails.HorizontalAlignment = 0;
            personaldetails.SpacingBefore = 5;
            personaldetails.SpacingAfter = 5;
            personaldetails.DefaultCell.Border = 0;

            int regionID = Convert.ToInt16(Session["RegionID"].ToString());
            var data = db.Regions.ToList().FirstOrDefault(x => x.RegionID == regionID);
            string regno = data.RegionNo;

            string status = data.StatusID.Value.ToString();

            //personaldetails.SetWidths(new int[] { 1, 4 });
            personaldetails.AddCell(new Phrase("Region:", bodyFont));
            personaldetails.AddCell(txtRegion.Text);
            personaldetails.AddCell(new Phrase("Region No:", bodyFont));
            personaldetails.AddCell(regno);
            personaldetails.AddCell(new Phrase("Bank Name:", bodyFont));
            personaldetails.AddCell(ddlBank.SelectedItem.Text);
            personaldetails.AddCell(new Phrase("Branch Name:", bodyFont));
            personaldetails.AddCell(ddlBranch.SelectedItem.Text);
            personaldetails.AddCell(new Phrase("Account No:", bodyFont));
            personaldetails.AddCell(txtAccNo.Text);
            personaldetails.AddCell(new Phrase("Status:", bodyFont));
            personaldetails.AddCell(status);
            document.Add(personaldetails);

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
            Response.AddHeader("Content-Disposition", "attachment; filename=ApproveRegion.pdf");
            Response.BinaryWrite(output.ToArray());

        }
        //public bool DeleteRegionById(int regionID)
        //{
        //    using (GoDurbanEntities db = new GoDurbanEntities())
        //    {
        //        try
        //        {
        //            foreach (var regionSA in db.tbl_RegionServiceArea.Where(o => o.RegionSAreaID == regionID))
        //                db.tbl_RegionServiceArea.Remove(regionSA);
        //            db.SaveChanges();

        //            //var regionSA = db.tbl_RegionServiceArea.SingleOrDefault(o => o.RegionSAreaID == regionID);
        //            //db.tbl_RegionServiceArea.Remove(regionSA);
        //            //db.SaveChanges();

        //            var assosiationregion = db.tbl_AssociationRegion.SingleOrDefault(o => o.RegionID == regionID);
        //                db.tbl_AssociationRegion.Remove(assosiationregion);
        //            db.SaveChanges();

        //            var region = db.tbl_Region.SingleOrDefault(o => o.RegionID == regionID);
        //            db.tbl_Region.Remove(region);
        //            db.SaveChanges();

        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            //throw new Exception();
        //            return false;
        //        }
        //    }
        //}


        //public string DeleteRegion()
        //{
        //    {
        //        string msg = "";
        //        // Connection Object
        //        using (SqlConnection con = new SqlConnection(connStr))
        //        {
        //            //selecting region ID
        //            String name = txtRegion.ToString();
        //            String sql = "select [RegionID] from tbl_Region where RegionName like '" + txtRegion.Text + "'";
        //            DataSet ds = con.QuerySelect(sql, "tbl_Region");

        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                int id = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        //                String sql2 = "select [AssociationServiceAreaID] from [dbo].[tbl_AssociationServiceArea]  join [dbo].[tbl_RegionServiceArea] on [tbl_AssociationServiceArea].RegionSAreaID = [dbo].tbl_AssociationServiceArea.RegionSAreaID  join tbl_Region on [tbl_RegionServiceArea].RegionID = tbl_Region.RegionID  where tbl_Region.RegionID = '" + id + "'";
        //                DataSet ds2 = con.QuerySelect(sql2, "tbl_Region");
        //                int id2 = 0;// Convert.ToInt32(ds2.Tables[0].Rows[0][0]);
        //                if (ds2.Tables[0].Rows.Count > 0)
        //                {
        //                    id2 = 0; Convert.ToInt32(ds2.Tables[0].Rows[0][0]);
        //                    label1.Text = id2.ToString();
        //                    sql2 = "delete from GoDurban.dbo.tbl_AssociationServiceArea where AssociationServiceAreaID = '" + id2 + "'";
        //                    con.queryModify(sql2);
        //                }
        //                else if (id > 0)
        //                {
        //                    sql = "delete from [dbo].[tbl_RegionServiceArea] where [RegionSAreaID] = '" + id + "'";
        //                    con.queryModify(sql2);

        //                    label1.Text = id.ToString();
        //                    sql = "delete from [dbo].[tbl_AssociationRegion] where[RegionID] = '" + id + "'";
        //                    con.queryModify(sql);

        //                    sql = "delete from [dbo].[tbl_RegionalLeadership] where[RegionID] = '" + id + "'";
        //                    con.queryModify(sql);

        //                    sql = "delete from [dbo].[tbl_RegionServiceArea] where[RegionID] = '" + id + "'";
        //                    con.queryModify(sql);

        //                    sql = "delete from [dbo].[tbl_Region] where [RegionID] = '" + id + "'";
        //                    con.queryModify(sql);
        //                    msg = "delted";
        //                }
        //            }
        //            else
        //            {
        //                msg = "wewe";
        //            }
        //            return msg;
        //        }

        //    }
        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/ApproveRegion.aspx?RegionID=" + regID);
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/Supervisor.aspx");
        }

        public void LoadFields()
        {
            var temp = db.Regions.ToList().FirstOrDefault(x => x.RegionID == regID);

            if (temp != null)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                RemoveBorder();

                Session["RegionID"] = regID;
                txtRegion.Text = temp.RegionName;

                var sts = db.tbl_Status.FirstOrDefault(x => x.StatusID == temp.StatusID);
                ddlStatus.SelectedItem.Text = sts.StatusDescription.ToString();

                var acc = db.Accounts.FirstOrDefault(x => x.AccountID == temp.AccountID);
                txtAccNo.Text = acc.AccountNo.ToString();

                var bank = db.Banks.FirstOrDefault(x => x.BankID == acc.BankID);
                ddlBranch.SelectedValue = bank.BankID.ToString();
                ddlBank.SelectedValue = bank.BankID.ToString();

                //txtAccNo.Text = temp.AccountNo;
                //var bank = db.tbl_Bank.FirstOrDefault(x => x.BankName == temp.BankName);
                //ddlBranch.SelectedValue = bank.BankId.ToString();
                //ddlBank.SelectedValue = bank.BankId.ToString();
                //ddlStatus.SelectedValue = temp.StatusID.ToString();

                btnAdd.Text = "Update";
            }
        }
    }
}