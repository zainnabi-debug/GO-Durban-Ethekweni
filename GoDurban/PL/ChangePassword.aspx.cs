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
    public partial class ChangePassword : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            txtEmail.Text = Session["Email"].ToString();
            
            lblError.Visible = false;
            
            if (!IsPostBack)
            {
            }
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
            txtCurrentPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var pass = db.tbl_User.ToList().Find(x => x.Email.ToLower() == txtEmail.Text.ToLower().Trim());

                if (pass.PasswordHash == Encrypt(txtCurrentPassword.Text))
                {
                    if (txtNewPassword.Text == txtConfirmPassword.Text)
                    {
                        pass.Password = Encrypt(txtNewPassword.Text);
                        pass.PasswordHash = Encrypt(txtNewPassword.Text);

                        if (pass.FirstTimeLogin == true)
                        {
                            pass.FirstTimeLogin = false;
                            db.SaveChanges();
                        }
                        else
                        {
                            db.SaveChanges();
                        }
                        
                        try
                        {
                            EmailBL EML = new EmailBL();
                            EML.Email("Moja Cruise System Password Changed ", "Hello " + pass.Name + " " + pass.Surname + "<br/> <br/>"
                                + "You have changed your password successfully on the Moja Cruise System." + "<br/> "
                                + "Please use your ID No as a username and your new password to login." + "<br/>" +
                                    "For more details feel free to contact the Ethekwini Transport Authority" + "<br/>" +
                                    "Regards" + "<br/>" +
                                    "Ethekwini Transport Authority", txtEmail.Text.ToLower(), "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
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
                        divsuccess.Style.Add("display", "inline");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "none");
                        divwarning.Style.Add("display", "none");
                        Clear();
                    }
                    else
                    {
                        divsuccess.Style.Add("display", "none");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "none");
                        divwarning.Style.Add("display", "none");

                        lblError.Visible = true;
                        lblError.Text = "Current Password Is Incorrect";
                    }
                }
                else
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");

                    lblError.Visible = true;
                    lblError.Text = "Current Password Is Incorrect";
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