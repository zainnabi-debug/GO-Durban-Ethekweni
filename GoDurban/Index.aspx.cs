using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban
{
    public partial class Index1 : System.Web.UI.Page
    {
        private UserBL BL = new UserBL();
        private GoDurbanEntities db = new GoDurbanEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ShowDialog = Request.QueryString["ShowDialog"];

            if (ShowDialog == "yes")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "TimeOut();", true);
            }
            if (!IsPostBack)
            {
                txtPassword.Text = string.Empty;
                txtUsername.Text = string.Empty;
            }
            lblError.Visible = false;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (GoDurbanEntities db = new GoDurbanEntities())
                {
                    //string username = txtUsername.Text;
                    string password = txtPassword.Text.Trim();

                    var user = db.tbl_User.ToList().FirstOrDefault(x => x.UserName.ToLower() == txtUsername.Text.ToLower().Trim());
                    var userprofile = db.tbl_UserPage.ToList().FirstOrDefault(x => x.UserID == user.UserID);

                    if (user != null)
                    {
                        Session["UserID"] = user.UserID;

                        //Decrypt password

                        Session["UserName"] = user.UserName;
                        Session["UserRole"] = user.UserRole;
                        Session["Name"] = user.Name;
                        Session["Surname"] = user.Surname;
                        Session["Email"] = user.Email;

                        if (userprofile != null)
                        {
                            Session["Association"] = userprofile.Association;
                            Session["AssociationL"] = userprofile.AssociationL;
                            Session["AssociationR"] = userprofile.AssociationR;
                            Session["AssociationSA"] = userprofile.AssociationSA;
                            Session["Owner"] = userprofile.Owner;
                            Session["OwnerA"] = userprofile.OwnerA;
                            Session["OwnerSA"] = userprofile.OwnerSA;
                            Session["Region"] = userprofile.Region;
                            Session["RegionL"] = userprofile.RegionL;
                            Session["RegionSA"] = userprofile.RegionSA;
                            Session["Driver"] = userprofile.Driver;
                            Session["Vehicle"] = userprofile.Vehicle;
                        }

                        string dencryptedPassword = BL.Decrypt(user.PasswordHash);

                        //var pass = db.tbl_User.ToList().FirstOrDefault(x => x.UserName == txtUsername.Text.ToLower() && x.IsActive == true && x.PasswordHash == password);

                        if (password == dencryptedPassword)
                        {
                            //Store user login details in Session 

                            Session["UserName"] = user.UserName;
                            Session["UserRole"] = user.UserRole;
                            Session["UserID"] = user.UserID;
                            Session["Name"] = user.Name;
                            Session["Surname"] = user.Surname;
                            Session["Email"] = user.Email;

                            if (user.FirstTimeLogin == true)
                            {
                                Response.Redirect("PL/ChangePassword");
                            }
                            else
                            {
                                if (user.UserRole == "Admin")
                                {
                                    Session["UserRole"] = "Admin";
                                    Response.Redirect("PL/Home");
                                }
                                else if (user.UserRole == "Supervisor")
                                {
                                    Session["UserRole"] = "Supervisor";
                                    Response.Redirect("PL/Home");
                                }
                                else if (user.UserRole == "User")
                                {
                                    Session["UserRole"] = "User";
                                    Response.Redirect("PL/Home");
                                }

                                else
                                {
                                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please enter Correct UserName and Password!')", true);

                                    lblError.Visible = true;
                                    //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                                    lblError.Text = "Please enter correct username and password to login or please contact support team.";
                                }
                            }
                        }

                        else
                        {
                            lblError.Visible = true;
                            //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                            lblError.Text = "Please enter correct username and password to login or please contact support team.";
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                        lblError.Text = "Please enter correct username and password to login or please contact support team.";
                    }
                }
            }
            catch (Exception ex)
            {
                //divsuccess.Style.Add("display", "none");
                //divdanger.Style.Add("display", "none");
                //divinfo.Style.Add("display", "none");
                //divwarning.Style.Add("display", "none");

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Please enter correct username and password to login or please contact support team.";
            }
        }
    }
}