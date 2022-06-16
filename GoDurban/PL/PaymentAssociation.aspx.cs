using GoDurban.BL;
using GoDurban.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class PaymentAssociation : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        private static string constr = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
        private static string ODBCconstr = ConfigurationManager.ConnectionStrings["ODBCConnection"].ToString();

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

                if (type == "Supervisor")
                {
                    btnExportTo.Visible = true;
                }
                else
                {
                    btnExportTo.Visible = false;
                    lblSent.Visible = false;
                }
            }

            lblError.Visible = false;
            lblSent.Visible = false;

            //LoadButtonVisible();

            if (!IsPostBack)
            {
                LoadData();
                LoadDataVoided();

                if (gvPayment.Rows.Count > 0)
                {
                    divPayment.Visible = true;
                }
                else
                {
                    divPayment.Visible = false;
                }
            }
        }
        private void LoadButtonVisible()
        {
            string currentdate = DateTime.Now.ToShortDateString();
            var noClickedList = db.NoOfClicksPerMonths.Where(x => x.ColumnName == "Association").ToList();
            if (noClickedList.Count() > 0)
            {
                var monthCount = noClickedList.LastOrDefault();

                if (monthCount.RangeDate == currentdate)
                {
                    lblSent.Visible = false;
                    btnExportTo.Enabled = true;
                }
                else
                {
                    lblSent.Visible = true;
                    btnExportTo.Enabled = false;
                }
            }
        }
        private void LoadBtnClickedOnce()
        {
            string currentdate = DateTime.Now.ToShortDateString();
            var noClickedList = db.NoOfClicksPerMonths.Where(x => x.ColumnName == "Association").ToList();
            if (noClickedList.Count() > 0)
            {
                var noClicked = noClickedList.LastOrDefault();

                if (noClicked.MonthClicked < DateTime.Now.Month && noClicked.ColumnName == "Association")
                {
                    lblSent.Visible = false;
                    btnExportTo.Enabled = true;
                }
                else
                {
                    lblSent.Visible = true;
                    btnExportTo.Enabled = false;
                }
            }
        }

        private void LoadDate()
        {
            DateTime today = DateTime.Today;
            DateTime lastDayOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            if (today != lastDayOfMonth)
            {
                divExport.Visible = false;
            }
            else
            {
                divExport.Visible = true;
            }
        }

        protected void gvPayment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPayment.PageIndex = e.NewPageIndex;
            //gvServiceArea.DataBind();
            LoadData();
        }

        //public void LoadDataVoided()
        //{
        //    try
        //    {
        //        string query = "Select VCALKY from F5604005 where VCALKY LIKE 'A%' ";
        //        using (OdbcConnection con = new OdbcConnection(ODBCconstr))
        //        {
        //            using (OdbcCommand cmd = new OdbcCommand(query, con))
        //            {
        //                con.Open();

        //                // Execute the DataReader and access the data.
        //                OdbcDataReader reader = cmd.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    Payment p = new Payment();
        //                    PaymentBL pbl = new PaymentBL();

        //                    string accNo = string.Empty;
        //                    accNo = reader.GetString(reader.GetOrdinal("VCALKY"));

        //                    var assId = db.Associations.FirstOrDefault(x => x.AssociationNo == accNo);
        //                    var pay = db.Payments.ToList().FirstOrDefault(x => x.AssociationID == assId.AssociationID);
        //                    if (pay.AccountNo != null)
        //                    {
        //                        pay.VoidedPayment = true;
        //                        pbl.UpdatePayment(pay);
        //                    }
        //                }
        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        divsuccess.Style.Add("display", "none");
        //        divdanger.Style.Add("display", "none");
        //        divinfo.Style.Add("display", "none");
        //        divwarning.Style.Add("display", "none");

        //        lblError.Visible = true;
        //        //lblError.Text = "Connection Problem: " + ex.Message.ToString();
        //        lblError.Text = "Can't load data from JDE system!";
        //    }
        //}

        public void LoadDataVoided()
        {
            try
            {
                string query = "Select VPALKY from F5604004 where VPALKY LIKE 'A%' ";
                using (OdbcConnection con = new OdbcConnection(ODBCconstr))
                {
                    using (OdbcCommand cmd = new OdbcCommand(query, con))
                    {
                        con.Open();

                        // Execute the DataReader and access the data.
                        OdbcDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Payment p = new Payment();
                            PaymentBL pbl = new PaymentBL();

                            string accNo = string.Empty;
                            accNo = reader.GetString(reader.GetOrdinal("VPALKY"));

                            var assId = db.Associations.FirstOrDefault(x => x.AssociationNo == accNo);
                            var pay = db.Payments.ToList().FirstOrDefault(x => x.AssociationID == assId.AssociationID);
                            if (pay.AccountNo != null)
                            {
                                pay.VoidedPayment = false;
                                pbl.UpdatePayment(pay);
                            }
                        }
                        reader.Close();
                    }
                }

                string query1 = "Select VCALKY from F5604005 where VCALKY LIKE 'A%' ";
                using (OdbcConnection con = new OdbcConnection(ODBCconstr))
                {
                    using (OdbcCommand cmd = new OdbcCommand(query1, con))
                    {
                        con.Open();

                        // Execute the DataReader and access the data.
                        OdbcDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Payment TBL = new Payment();
                            PaymentBL pbl = new PaymentBL();

                            string accNo = string.Empty;
                            accNo = reader.GetString(reader.GetOrdinal("VCALKY"));
                            int lastMonth = Convert.ToInt16(DateTime.Now.Month - 1);

                            var assId = db.Associations.FirstOrDefault(x => x.AssociationNo == accNo);
                            var pay = db.Payments.ToList().FirstOrDefault(x => x.AssociationID == assId.AssociationID);
                            if (pay.AccountNo != null && pay.VoidedPayment == false && pay.UserID == null)
                            {
                                pay.UserID = 1;
                                pay.IsPaymentSent = "No";
                                pay.VoidedPayment = true;
                                pbl.UpdatePayment(pay);

                                if (pay.AccountNo != null && pay.Date.Value.Month >= lastMonth)
                                {
                                    pay.Date = DateTime.Now;
                                    pay.AccountName = pay.AccountName;
                                    pay.AccountNo = pay.AccountNo;
                                    pay.AddressLine1 = pay.AddressLine1;
                                    pay.AddressLine2 = pay.AddressLine2;
                                    pay.AddressLine3 = pay.AddressLine3;
                                    pay.AddressLine4 = pay.AddressLine4;
                                    pay.AddressNo = pay.AddressNo;
                                    pay.Amount = pay.Amount;
                                    pay.BankAccNo = pay.BankAccNo;
                                    pay.BankName = pay.BankName;
                                    pay.BankTransit = pay.BankTransit;
                                    pay.City = pay.City;
                                    pay.DocTy = pay.DocTy;
                                    pay.ID1Code = pay.ID1Code;
                                    pay.IDIssuer = pay.IDIssuer;
                                    pay.IsPaymentSent = "No";
                                    pay.LongAddress = pay.LongAddress;
                                    pay.PaymentMethod = pay.PaymentMethod;
                                    pay.PhoneNo = pay.PhoneNo;
                                    pay.PostalCode = pay.PostalCode;
                                    pay.SP = pay.SP;
                                    pay.STSCD = pay.STSCD;
                                    pay.TaxID = pay.TaxID;
                                    pay.TransNo = pay.TransNo;
                                    pay.UserID = 2;
                                    pay.OwnerID = pay.OwnerID;
                                    pay.RegionID = pay.RegionID;
                                    pay.AssociationID = pay.AssociationID;
                                    pay.EntityType = "BNA";
                                    pay.VoidedPayment = true;
                                    pbl.CreatePayment(pay);
                                }
                            }
                        }
                        reader.Close();
                    }
                }

                using (var conn = new SqlConnection(constr))
                using (var command = new SqlCommand("CheckVoid", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.ToString();
                lblError.Text = "Can't load data from JDE system!";
            }
        }

        private void LoadData()
        {
            DateTime firstDateOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDateOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            int lastMonth = Convert.ToInt16(DateTime.Now.Month - 1);

            var list = (from a in db.Payments
                        join b in db.Associations on a.AssociationID equals b.AssociationID

                        //where ((a.Date >= firstDateOfMonth && a.Date <= lastDateOfMonth) || ((a.Date.Value.Month >= lastMonth && a.VoidedPayment == true) && (a.Date.Value.Month >= lastMonth && a.IsPaymentSent == "No")))
                        where ((a.Date >= firstDateOfMonth && a.Date <= lastDateOfMonth) || (a.Date.Value.Month >= lastMonth && a.IsPaymentSent == "No"))

                        where (b.tbl_Status.StatusDescription == "Approved")
                        where (a.AssociationID == b.AssociationID)
                        where (a.Amount != null && a.Amount != 0)
                        where (a.AccountNo.StartsWith("A"))

                        select new
                        {
                            a.Date,
                            a.PaymentID,
                            a.TransNo,
                            a.LineNo,
                            a.BatchNo,
                            a.DocTy,
                            a.SP,
                            a.STSCD,
                            a.LongAddress,
                            a.AccountName,
                            a.AccountNo,
                            a.IDIssuer,
                            a.ID1Code,
                            a.TaxID,
                            a.PhoneNo,
                            //a.AddressLine1,
                            //a.AddressLine2,
                            //a.AddressLine3,
                            //a.AddressLine4,
                            //a.City,
                            //a.PostalCode,
                            a.DocNo,
                            a.Amount,
                            a.AddressNo,
                            a.PaymentMethod,
                            a.BankName,
                            a.BankTransit,
                            a.BankAccNo,
                            a.IsPaymentSent,
                            VoidedPayment = a.VoidedPayment == true ? "Yes" : "No"
                            //address = a.AddressLine1 + ", " + a.AddressLine2 + ", " + a.City + ", " + a.PostalCode,
                        });
            gvPayment.DataSource = list.ToList();
            gvPayment.DataBind();
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
        //private bool ValidateCheckBox()
        //{
        //    bool valid = true;

        //    divsuccess.Style.Add("display", "none");
        //    divdanger.Style.Add("display", "none");
        //    divinfo.Style.Add("display", "none");
        //    divwarning.Style.Add("display", "none");

        //    foreach (GridViewRow row in gvPayment.Rows)
        //    {
        //        CheckBox Chbox = (CheckBox)row.FindControl("chkRow");
        //        if (Chbox.Checked == false)
        //        {
        //            lblCheck.Visible = true;
        //            //Chbox.Checked = true;
        //        }
        //        else
        //        {
        //            lblCheck.Visible = false;
        //        }
        //    }

        //    return valid;
        //}

        protected void btnExportTo_Click(object sender, EventArgs e)
        {
            try
            {
                //CheckBox chkAll = (CheckBox)gvPayment.HeaderRow.Cells[0].FindControl("chkAll");

                int a = gvPayment.PageIndex;
                for (int i = 0; i < gvPayment.PageCount; i++)
                {
                    gvPayment.SetPageIndex(i);

                    foreach (GridViewRow row in gvPayment.Rows)
                    {
                        //CheckBox Chbox = (CheckBox)row.FindControl("chkRow");
                        //if (Chbox.Checked == true)
                        //{

                        //var payreg = db.Payments.ToList().Find(x => x.Amount == Convert.ToDecimal(5000.00) && x.IsPaymentSent == "No");

                        GridView gvPayment = (GridView)row.FindControl("gvPayment");
                        int id = Convert.ToInt32(row.Cells[1].Text);

                        var payreg = db.Payments.ToList().Find(x => x.PaymentID == id);

                        if (payreg.IsPaymentSent == "Yes")
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                        }
                        else
                        {
                            int ownassregid = Convert.ToInt16(payreg.AssociationID);
                            DateTime date = Convert.ToDateTime(payreg.Date);
                            int userid = Convert.ToInt16(payreg.UserID);
                            string transno = payreg.TransNo;
                            decimal lineno = Convert.ToDecimal(payreg.LineNo);
                            string batchno = payreg.BatchNo;
                            string docty = payreg.DocTy;
                            string sp = payreg.SP;
                            string stscd = payreg.STSCD;
                            string longaddress = payreg.LongAddress;
                            string accountname = payreg.AccountName;
                            string accountno = payreg.AccountNo;
                            string idissuer = payreg.IDIssuer;
                            string id1code = payreg.ID1Code;
                            string taxid = payreg.TaxID;
                            string phoneno = payreg.PhoneNo;
                            string addressline1 = payreg.AddressLine1;
                            string addressline2 = payreg.AddressLine2;
                            string addressline3 = payreg.AddressLine3;
                            string addressline4 = payreg.AddressLine4;
                            string city = payreg.City;
                            string postalcode = payreg.PostalCode;
                            string docno = payreg.DocNo;
                            decimal amount = Convert.ToDecimal(payreg.Amount) * 100;
                            string addressno = payreg.AddressNo;
                            string paymentmethod = payreg.PaymentMethod;
                            string bankname = payreg.BankName;
                            string banktransit = payreg.BankTransit;
                            string bankaccno = payreg.BankAccNo;
                            string ispaymentsent = payreg.IsPaymentSent;

                            using (OdbcConnection con = new OdbcConnection(ODBCconstr))
                            {
                                using (OdbcCommand cmd = new OdbcCommand("INSERT INTO F5604003 (DPEDUS," +
                                    " DPEDTN, DPEDLN, DPEDBT, DPEDSP, DPALKY, DPALPH, DPAA18, DPAA, DPIDMTHD," +
                                    " DPDL01, DPTNST, DPCBNK) " +
                                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", con))
                                {
                                    try
                                    {
                                        cmd.Parameters.Add("@DPEDUS", OdbcType.VarChar, 10).Value = "135100";
                                        cmd.Parameters.Add("@DPEDTN", OdbcType.VarChar, 22).Value = "1";
                                        cmd.Parameters.Add("@DPEDLN", OdbcType.Numeric, 7).Value = lineno;
                                        cmd.Parameters.Add("@DPEDBT", OdbcType.VarChar, 15).Value = batchno;
                                        cmd.Parameters.Add("@DPEDSP", OdbcType.VarChar, 1).Value = "P";
                                        cmd.Parameters.Add("@DPALKY", OdbcType.VarChar, 20).Value = accountno;
                                        cmd.Parameters.Add("@DPALPH", OdbcType.VarChar, 40).Value = accountname;
                                        //cmd.Parameters.Add("@DPTRMNUM", OdbcType.VarChar, 11).Value = "66520000162";
                                        cmd.Parameters.Add("@DPAA18", OdbcType.VarChar, 18).Value = docno;
                                        cmd.Parameters.Add("@DPAA", OdbcType.Numeric, 15).Value = amount;
                                        cmd.Parameters.Add("@DPIDMTHD", OdbcType.VarChar, 6).Value = "T";
                                        cmd.Parameters.Add("@DPDL01", OdbcType.VarChar, 30).Value = bankname;
                                        cmd.Parameters.Add("@DPTNST", OdbcType.VarChar, 20).Value = banktransit;
                                        cmd.Parameters.Add("@DPCBNK", OdbcType.VarChar, 20).Value = bankaccno;
                                        //cmd.Parameters.Add("@DPCKSV", OdbcType.Char, 1).Value = "0";

                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        con.Close();

                                        PaymentBL pBL = new PaymentBL();
                                        Payment pTBL = new Payment();
                                        var payId = db.Payments.ToList().Find(x => x.AssociationID == ownassregid && x.PaymentID == id);
                                        payId.IsPaymentSent = "Yes";
                                        //payId.VoidedPayment = false;
                                        pBL.UpdatePayment(payId);

                                        NoOfClicksPerMonth noOf = new NoOfClicksPerMonth();
                                        NoOfClicksPerMonthBL noOfBL = new NoOfClicksPerMonthBL();
                                        var numOfClicks = db.NoOfClicksPerMonths.ToList().FindAll(x => x.MonthClicked == DateTime.Now.Month && x.ColumnName == "Association");
                                        if (numOfClicks.Count == 0)
                                        {
                                            noOf.RangeDate = DateTime.Now.AddDays(15).ToShortDateString();
                                            noOf.MonthClicked = DateTime.Now.Month;
                                            noOf.IsPaymentSent = "Yes";
                                            noOf.ColumnName = "Association";
                                            db.NoOfClicksPerMonths.Add(noOf);
                                            db.SaveChanges();
                                        }

                                        LoadBtnClickedOnce();
                                        LoadData();

                                        lblSent.Visible = true;
                                        lblError.Visible = false;

                                        divsuccess.Style.Add("display", "inline");
                                        divdanger.Style.Add("display", "none");
                                        divinfo.Style.Add("display", "none");
                                        divwarning.Style.Add("display", "none");
                                    }
                                    catch (Exception ex)
                                    {
                                        divsuccess.Style.Add("display", "none");
                                        divdanger.Style.Add("display", "none");
                                        divinfo.Style.Add("display", "none");
                                        divwarning.Style.Add("display", "none");

                                        lblError.Visible = true;
                                        //lblError.Text = "Connection Problem: " + ex.ToString();
                                        lblError.Text = "Error inserting data into JDE system!";
                                    }
                                }
                            }
                        }
                    }
                }
                gvPayment.SetPageIndex(a);

                using (SqlConnection connection = new SqlConnection(constr))
                using (SqlCommand command = connection.CreateCommand())
                {
                    string seqname = "BNA";
                    command.CommandText = "update tbl_DriverCount set last_value = last_value +1 , updated_date = getdate() where seq_name = '" + seqname + "'";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.ToString();
                lblError.Text = "Error inserting data into JDE system!";
            }
        }

        protected void gvPayment_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvPayment.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvPayment.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvPayment.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvPayment.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvPayment_RowCreated(object sender, GridViewRowEventArgs e)
        {
            // Adding a column manually once the header created
            if (e.Row.RowType == DataControlRowType.Header) // If header created
            {
                GridView PaymentGrid = (GridView)sender;

                // Creating a Row
                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                //Adding Year Column
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Line No.";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2; // For merging first, second row cells to one
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding Period Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "Batch No.";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding Audited By Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "Document No.";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding Audited By Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "SP";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding Revenue Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "Account Name";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 3; // For merging three columns (Direct, Referral, Total)
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding Revenue Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "Account Number";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 3; // For merging three columns (Direct, Referral, Total)
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding Revenue Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "Phone Number";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 3; // For merging three columns (Direct, Referral, Total)
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding the Row at the 0th position (first row) in the Grid
                PaymentGrid.Controls[0].Controls.AddAt(0, HeaderRow);
            }
        }

        protected void gvPayment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //// Invisibling the first three columns of second row header (normally created on binding)
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    e.Row.Cells[0].Visible = false; // Invisibiling Year Header Cell
            //    e.Row.Cells[1].Visible = false; // Invisibiling Period Header Cell
            //    e.Row.Cells[2].Visible = false; // Invisibiling Audited By Header Cell
            //    e.Row.Cells[3].Visible = false; // Invisibiling Audited By Header Cell
            //    e.Row.Cells[4].Visible = false; // Invisibiling Audited By Header Cell
            //}


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string st = e.Row.Cells[10].Text.ToString();
                if (st == "Yes")
                {
                    CheckBox Chbox = (CheckBox)e.Row.Cells[1].FindControl("chkRow");
                    Chbox.Visible = false;
                }
                else if (st == "No")
                {
                    CheckBox Chbox = (CheckBox)e.Row.Cells[1].FindControl("chkRow");
                    Chbox.Visible = true;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gvBank = (GridView)e.Row.FindControl("gvBank");
                int id = Convert.ToInt32(e.Row.Cells[1].Text);

                var list = (from a in db.Payments
                            where a.PaymentID == id
                            select new
                            {
                                a.PaymentID,
                                a.BankTransit,
                                a.BankName,
                                a.BankAccNo,
                                a.LineNo,
                                a.BatchNo,
                                a.DocNo,
                            });
                gvBank.DataSource = list.ToList();
                gvBank.DataBind();
            }
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

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}