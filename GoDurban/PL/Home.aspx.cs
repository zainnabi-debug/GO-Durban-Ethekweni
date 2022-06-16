using GoDurban.BL;
using GoDurban.Models;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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
    public partial class Home : System.Web.UI.Page
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
            }

            lblError.Visible = false;

            if (!IsPostBack)
            {
                //LoadDataVoidedA();
                //LoadDataVoidedO();
                //LoadDataVoidedR();
            }
        }
               
        public void LoadDataVoidedR()
        {
            try
            {
                string query = "Select VPALKY from F5604004 where VPALKY LIKE 'R%' ";
                using (OdbcConnection con = new OdbcConnection(ODBCconstr))
                {
                    using (OdbcCommand cmd = new OdbcCommand(query, con))
                    {
                        con.Open();

                        // Execute the DataReader and access the data.
                        OdbcDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            PaymentBL pbl = new PaymentBL();

                            string accNo = string.Empty;
                            accNo = reader.GetString(reader.GetOrdinal("VPALKY"));

                            var regId = db.Regions.FirstOrDefault(x => x.RegionNo == accNo);
                            var pay = db.Payments.ToList().FirstOrDefault(x => x.RegionID == regId.RegionID);
                            if (pay.AccountNo != null)
                            {
                                pay.VoidedPayment = false;
                                pbl.UpdatePayment(pay);
                            }
                        }
                        reader.Close();
                    }
                }

                string query1 = "Select VCALKY from F5604005 where VCALKY LIKE 'R%' ";
                using (OdbcConnection con = new OdbcConnection(ODBCconstr))
                {
                    using (OdbcCommand cmd = new OdbcCommand(query1, con))
                    {
                        con.Open();

                        // Execute the DataReader and access the data.
                        OdbcDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            PaymentBL pbl = new PaymentBL();

                            string accNo = string.Empty;
                            accNo = reader.GetString(reader.GetOrdinal("VCALKY"));

                            var regId = db.Regions.FirstOrDefault(x => x.RegionNo == accNo);
                            var pay = db.Payments.ToList().FirstOrDefault(x => x.RegionID == regId.RegionID);
                            if (pay.AccountNo != null)
                            {
                                //pay.Amount = Convert.ToDecimal(8000.00) * 2;
                                pay.VoidedPayment = true;
                                pbl.UpdatePayment(pay);
                            }
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Can't load data from JDE system!";
            }
        }

        public void LoadDataVoidedO()
        {
            try
            {
                string query = "Select VPALKY from F5604004 where VPALKY NOT LIKE 'A%' AND VPALKY NOT LIKE 'R%'";
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

                            var ownId = db.tbl_Owner.FirstOrDefault(x => x.IDNo == accNo);
                            var pay = db.Payments.ToList().FirstOrDefault(x => x.OwnerID == ownId.OwnerID);
                            if (pay.AccountNo != null)
                            {
                                pay.VoidedPayment = false;
                                pbl.UpdatePayment(pay);
                            }
                        }
                        reader.Close();
                    }
                }

                string query1 = "Select VCALKY from F5604005 where VCALKY NOT LIKE 'A%' AND VCALKY NOT LIKE 'R%'";
                using (OdbcConnection con = new OdbcConnection(ODBCconstr))
                {
                    using (OdbcCommand cmd = new OdbcCommand(query1, con))
                    {
                        con.Open();

                        // Execute the DataReader and access the data.
                        OdbcDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Payment p = new Payment();
                            PaymentBL pbl = new PaymentBL();

                            string accNo = string.Empty;
                            accNo = reader.GetString(reader.GetOrdinal("VCALKY"));

                            var ownId = db.tbl_Owner.FirstOrDefault(x => x.IDNo == accNo);
                            var pay = db.Payments.ToList().FirstOrDefault(x => x.OwnerID == ownId.OwnerID);
                            if (pay.AccountNo != null)
                            {
                                pay.VoidedPayment = true;
                                pbl.UpdatePayment(pay);
                            }
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Connection Problem: " + ex.Message.ToString();
                //lblError.Text = "Can't load data from JDE system!";
            }
        }

        public void LoadDataVoidedA()
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
                            Payment p = new Payment();
                            PaymentBL pbl = new PaymentBL();

                            string accNo = string.Empty;
                            accNo = reader.GetString(reader.GetOrdinal("VCALKY"));

                            var assId = db.Associations.FirstOrDefault(x => x.AssociationNo == accNo);
                            var pay = db.Payments.ToList().FirstOrDefault(x => x.AssociationID == assId.AssociationID);
                            if (pay.AccountNo != null)
                            {
                                //pay.Amount = Convert.ToDecimal(5000.00) * 2;
                                pay.VoidedPayment = true;
                                pbl.UpdatePayment(pay);
                            }
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Can't load data from JDE system!";
            }
        }
    }
}