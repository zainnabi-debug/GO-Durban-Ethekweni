using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
        }

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        UserBL BL = new UserBL();
        //        tbl_User TBL = new tbl_User();
        //        string temp = btnAdd.Text;

        //        int Id = 0;

        //        if (temp.Contains("Reset"))
        //        {
        //            ViewState["UserID"] = Id;

        //            var user = db.tbl_User.ToList().FirstOrDefault(x => x.UserID == Convert.ToInt16(Id));

        //            user.Password = txtIDNo.Text.Trim();

        //            user.PasswordConfirm = txtIDNo.Text.Trim();
        //                db.SaveChanges();
        //                divsuccess.Style.Add("display", "none");
        //                divdanger.Style.Add("display", "none");
        //                divinfo.Style.Add("display", "inline");
        //                divwarning.Style.Add("display", "none");

        //                btnAdd.Text = "Add";

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        divsuccess.Style.Add("display", "none");
        //        divdanger.Style.Add("display", "inline");
        //        divinfo.Style.Add("display", "none");
        //        divwarning.Style.Add("display", "none");
        //    }
        //}

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public void Clear()
        {
            txtEmail.Text = string.Empty;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string mail = txtEmail.Text.ToLower().Trim();
                var user = db.tbl_User.ToList().FirstOrDefault(x => x.Email.ToLower() == mail);
                string servername = ConfigurationManager.AppSettings["ServerName"].ToString();
                if (user != null)
                {
                    if (user.Email.ToLower() == txtEmail.Text.ToLower().Trim())
                    {
                        try
                        {
                            EmailBL EML = new EmailBL();
                            EML.Email("Reset Password", "Hello " + "<br/> <br/>"
                                    + "You have requested to reset a password." + "<br/> " +
                                    "Click <a href =\"http://w8mglassfish01:93/PL/ResetPassword?Email=" + Encrypt(txtEmail.Text) + "\">Here</a>" + " to reset your password." + "<br/> " +
                                    "For more details feel free to contact the Ethekwini Transport Authority" + "<br/>" +
                                    "Regards" + "<br/>" +
                                    "Ethekwini Transport Authority", txtEmail.Text.ToLower(), "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");

                            divwarning.Style.Add("display", "inline");
                        }
                        catch (SmtpFailedRecipientException ex)
                        {
                            SmtpStatusCode statusCode = ex.StatusCode;

                            if (statusCode == SmtpStatusCode.MailboxBusy || statusCode == SmtpStatusCode.MailboxUnavailable || statusCode == SmtpStatusCode.TransactionFailed)
                            {
                                // wait 5 seconds, try a second time

                                EmailBL EML = new EmailBL();
                                EML.Email("Reset Password", "Hello " + "<br/> <br/>"
                                    + "You have requested to reset a password." + "<br/> " +
                                   "Click < a href =\"http://w8mglassfish01:93/PL/ResetPassword?Email=" + Encrypt(txtEmail.Text) + "\">Here</a>" + " to reset your password." + "<br/> " +
                                        "For more details feel free to contact the Ethekwini Transport Authority" + "<br/>" +
                                        "Regards" + "<br/>" +
                                        "Ethekwini Transport Authority", txtEmail.Text.ToLower(), "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                    Clear();
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Email address is not registered";
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
}