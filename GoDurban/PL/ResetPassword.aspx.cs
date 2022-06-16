using GoDurban.BL;
using GoDurban.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Email"] != null)
            {
                string email = Request.QueryString["Email"].ToString();
                email = email.Replace(" ", "+");
                txtEmail.Text = Decrypt(email);
                txtEmail.ReadOnly = true;
                txtNewPassword.Focus();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Could not find the email address";
            }
        }

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
            txtConfirmPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
        }
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var pass = db.tbl_User.ToList().Find(x => x.Email.ToLower() == txtEmail.Text.ToLower().Trim());
            if(pass!=null)
            {
                pass.Password = Encrypt(txtNewPassword.Text);
                pass.PasswordHash = Encrypt(txtNewPassword.Text);
                db.SaveChanges();
                
                try
                {
                    EmailBL EML = new EmailBL();
                    EML.Email("Reset Password", "Hello " + pass.Name + " " + pass.Surname + "<br/> <br/>"
                        + "You have changed your password successfully on the Moja Cruise System." + "<br/> "
                        + "Please use your ID Number as a username and your new password to login." + "<br/>" +
                            "For more details feel free to contact the Ethekwini Transport Authority" + "<br/>" +
                            "Regards" + "<br/>" +
                            "Ethekwini Transport Authority", txtEmail.Text.ToLower(), "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");

                    divsuccess.Style.Add("display", "inline");
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
                Clear();
            }
            else
            {
                lblError.Visible = true;
                 lblError.Text = "User Is Not Registered";
            }
        }
    }
}