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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class ApproveAssociation : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        SqlCommand com;

        int assID = 0;

        public static int accID;
        protected void Page_Load(object sender, EventArgs e)
        {
            assID = Convert.ToInt32(Request.QueryString["AssociationID"].ToString());

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
            //divAssociation.Style.Add("display", "none");

            if (!IsPostBack)
            {
                LoadData();
                LoadStatus();
                LoadBanksAndBranches();
                LoadFields();
            }
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

        //public string GenerateAssociationNo()
        //{
        //    int NumberofAssociations = db.tbl_Association.ToList().Count;
        //    string assno = "A";
        //    if (NumberofAssociations < 10)
        //    {
        //        int NewNumberofDrivers = NumberofAssociations + 1;
        //        if (NewNumberofDrivers < 10)
        //        {
        //            assno += "000" + NewNumberofDrivers;
        //        }
        //        else
        //        {
        //            assno += "00" + NewNumberofDrivers;
        //        }
        //    }
        //    else if (NumberofAssociations < 100)
        //    {
        //        int NewNumberofProjects = NumberofAssociations + 1;
        //        if (NewNumberofProjects < 10)
        //        {
        //            assno += "000" + NewNumberofProjects;
        //        }
        //        else
        //        {
        //            assno += "00" + NewNumberofProjects;
        //        }
        //    }
        //    else
        //    {
        //        int NewNumberofProjects = NumberofAssociations + 1;
        //        assno += NewNumberofProjects;
        //    }
        //    return assno;
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
                if (ValidateAssociation())
                {
                    // creating an object for business logic

                    AssociationBL BL = new AssociationBL();
                    Models.Association TBL = new Models.Association();

                    AccountBL acBL = new AccountBL();
                    Models.Account acTBL = new Models.Account();

                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var association = db.Associations.ToList().FindAll(x => String.Compare(x.AssociationName, (string)txtAssociation.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        var acc = db.Accounts.ToList().FindAll(x => String.Compare(x.AccountNo, (string)txtAccNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                        string asso = txtAssociation.Text.Trim();

                        if (association.Count > 0 || string.IsNullOrWhiteSpace(txtAssociation.Text))
                        {
                            txtAssociation.Text = string.Empty;
                            txtAssociation.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divinfo1.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                            divwarning1.Style.Add("display", "none");
                        }
                        else if (asso == string.Empty)
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divinfo1.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                            divwarning1.Style.Add("display", "none");

                            txtAssociation.Text = string.Empty;
                            txtAssociation.Focus();
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

                            TBL.AssociationName = txtAssociation.Text.Trim();
                            TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                            GetPrimaryKey();
                            TBL.AccountID = accID;
                            BL.CreateAssociation(TBL);

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
                        TBL.AssociationID = assID;
                        TBL.AssociationName = txtAssociation.Text.Trim();
                        TBL.StatusID = Convert.ToInt16(ddlStatus.SelectedValue);
                        GetPrimaryKey();
                        TBL.AccountID = accID;

                        var ass = db.Associations.ToList().Find(x => String.Compare(x.AssociationName, (string)txtAssociation.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);
                        var id = db.Associations.ToList().FirstOrDefault(x => x.AssociationID == assID);

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

                            if ((ass == null) || (id.AssociationName == txtAssociation.Text.Trim()))
                            {
                                if (ddlStatus.SelectedItem.Text == "Approved")
                                {
                                    BL.UpdateAssociation(TBL);

                                    PaymentBL pBL = new PaymentBL();
                                    Payment pTBL = new Payment();
                                    var assId = db.Associations.FirstOrDefault(x => x.AssociationID == assID);
                                    var payId = db.Payments.ToList().FirstOrDefault(x => x.AssociationID == assId.AssociationID);
                                    payId.Amount = Convert.ToDecimal(5000.00);
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
                                    BL.UpdateAssociation(TBL);

                                    PaymentBL pBL = new PaymentBL();
                                    Payment pTBL = new Payment();
                                    var assId = db.Associations.FirstOrDefault(x => x.AssociationID == assID);
                                    var payId = db.Payments.ToList().FirstOrDefault(x => x.AssociationID == assId.AssociationID);
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

                                txtAssociation.Text = string.Empty;
                                txtAssociation.Focus();
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

        private bool ValidateAssociation()
        {
            bool valid = true;

            if ((txtAssociation.Text == string.Empty))
            {
                txtAssociation.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtAssociation.Style.Add("border", "");
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
            txtAssociation.Text = string.Empty;
            txtAccNo.Text = string.Empty;
            ddlBranch.SelectedValue = "0";
            ddlBank.SelectedValue = "0";
            ddlStatus.SelectedValue = "0";
            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtAssociation.Style.Add("border", "");
            txtAccNo.Style.Add("border", "");
            ddlBank.Style.Add("border", "");
            ddlBranch.Style.Add("border", "");
            ddlStatus.Style.Add("border", "");
        }

        private void LoadData()
        {
            var data = (from a in db.Associations
                        join b in db.Accounts on a.AccountID equals b.AccountID
                        join c in db.Banks on b.BankID equals c.BankID
                        join d in db.tbl_Status on a.StatusID equals d.StatusID

                        where a.AssociationID == assID

                        select new
                        {
                            a.AssociationID,
                            a.AssociationNo,
                            a.AssociationName,
                            b.AccountID,
                            b.AccountNo,
                            c.BankID,
                            c.BankName,
                            c.BranchCode,
                            d.StatusDescription,
                        });
            gvAssociation.DataSource = data.ToList();
            gvAssociation.DataBind();
        }

        //objListOrder.OrderBy(o=>o.OrderDate).ToList()
        protected void gvAssociation_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvAssociation.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvAssociation.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvAssociation.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvAssociation.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvAssociation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            divAssociation.Style.Add("display", "inline");

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvAssociation.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "ApproveItem")
            {
                ddlStatus.Enabled = true;

                //Accessing BoundField Column
                string Source = gvAssociation.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["AssociationID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);

                var temp = db.Associations.ToList().FirstOrDefault(x => x.AssociationID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divinfo1.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");
                    divwarning1.Style.Add("display", "none");

                    RemoveBorder();

                    Session["AssociationID"] = Id;
                    txtAssociation.Text = temp.AssociationName;

                    var acc = db.Accounts.FirstOrDefault(x => x.AccountID == temp.AccountID);
                    txtAccNo.Text = acc.AccountNo.ToString();

                    var bank = db.Banks.FirstOrDefault(x => x.BankID == acc.BankID);
                    ddlBranch.SelectedValue = bank.BankID.ToString();
                    ddlBank.SelectedValue = bank.BankID.ToString();

                    ddlStatus.SelectedValue = temp.StatusID.ToString();

                    btnAdd.Text = "Update";
                }
            }

            else if (commandName == "DeleteItem")
            {
                string Source = gvAssociation.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["AssociationID"] = Id;

                try
                {
                    string html = string.Empty;
                    string url = @"http://r6efmprdbw.durban.gov.za:44007/V1/esbapi/DeleteAssociation?AssociationID=" + Id;

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
                divinfo1.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
                divwarning1.Style.Add("display", "none");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/ApproveAssociation.aspx?AssociationID=" + assID);
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
            var temp = db.Associations.ToList().FirstOrDefault(x => x.AssociationID == assID);

            if (temp != null)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divinfo1.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
                divwarning1.Style.Add("display", "none");

                RemoveBorder();

                Session["AssociationID"] = assID;
                txtAssociation.Text = temp.AssociationName;

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