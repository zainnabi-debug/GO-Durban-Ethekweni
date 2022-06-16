using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Register : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        protected void Page_Load(object sender, EventArgs e)
        {   
            lblError.Visible = false;

            if (!IsPostBack)
            {
                if (!IsPostBack)
                {
                    LoadRole();
                    LoadGender();
                    LoadRace();
                }
            }
        }

        private void LoadRole()
        {
            RoleBL BL = new RoleBL();
            List<tbl_Role> list = BL.LoadRole();
            ddlRole.DataSource = list;
            ddlRole.DataValueField = "RoleID";
            ddlRole.DataTextField = "RoleDescription";
            ddlRole.DataBind();
            //ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByText("User C"));
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

        private bool ValidateUser()
        {
            bool valid = true;

            if ((txtName.Text == string.Empty))
            {
                txtName.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtName.Style.Add("border", "");

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

            if ((txtIDNo.Text == string.Empty))
            {
                txtIDNo.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtIDNo.Style.Add("border", "");
            }

            if ((txtPassword.Text == string.Empty))
            {
                txtPassword.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtPassword.Style.Add("border", "");
            }

            if ((txtConfirmPassword.Text == string.Empty))
            {
                txtConfirmPassword.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtConfirmPassword.Style.Add("border", "");
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

            if ((txtEmail.Text == string.Empty))
            {
                txtEmail.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtEmail.Style.Add("border", "");
            }
            
            //if ((txtAddress.Text == string.Empty))
            //{
            //    txtAddress.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    txtAddress.Style.Add("border", "");
            //}
            
            if ((ddlGender.SelectedValue== "0"))
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

            if ((ddlRole.SelectedValue == "0"))
            {
                ddlRole.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                ddlRole.Style.Add("border", "");
            }

            return valid;
        }

        protected void Clear()
        {
            txtName.Text = string.Empty;
            //txtAddress.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtIDNo.Text = string.Empty;
            txtCellNo.Text = string.Empty;
            //txtOfficeNo.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtSurname.Text = string.Empty;
            txtIDNo.Text = string.Empty;
            ddlRole.SelectedValue = "0";
            ddlRace.SelectedValue = "0";
            ddlGender.SelectedValue = "0";

            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtName.Style.Add("border", "");
            txtSurname.Style.Add("border", "");
            txtCellNo.Style.Add("border", "");
            txtIDNo.Style.Add("border", "");
            txtConfirmPassword.Style.Add("border", "");
            txtPassword.Style.Add("border", "");
            //txtAddress.Style.Add("border", "");
            txtEmail.Style.Add("border", "");
            txtIDNo.Style.Add("border", "");
            ddlRole.Style.Add("border", "");
            ddlRace.Style.Add("border", "");
            ddlGender.Style.Add("border", "");
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int clear = 0;
            try
            {
                UserBL BL = new UserBL();
                tbl_User TBL = new tbl_User();

                if (ValidateUser())
                {
                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Submit"))
                    {
                        using (GoDurbanEntities db = new GoDurbanEntities())
                        {
                            var username = db.tbl_User.ToList().FindAll(x => String.Compare(x.UserName, (string)txtIDNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0 && x.IsActive == true);
                            var email = db.tbl_User.ToList().FindAll(x => String.Compare(x.Email, (string)txtEmail.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0 && x.IsActive == true);

                            if (email.Count > 0 || string.IsNullOrWhiteSpace(txtEmail.Text))
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "none");
                                divwarning1.Style.Add("display", "inline");

                                txtEmail.Text = string.Empty;
                                txtEmail.Focus();
                            }
                            else if (username.Count > 0 || string.IsNullOrWhiteSpace(txtIDNo.Text))
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "inline");
                                divwarning1.Style.Add("display", "none");

                                txtIDNo.Text = string.Empty;
                                txtIDNo.Focus();
                            }
                            else
                            {
                                clear = 1;
                                TBL.CellNo = txtCellNo.Text.Trim();
                                TBL.Email = txtEmail.Text.Trim();
                                TBL.IDNo = txtIDNo.Text.Trim();
                                TBL.Name = txtName.Text.Trim();
                                TBL.Surname = txtSurname.Text.Trim();
                                TBL.UserName = txtIDNo.Text.Trim();
                                TBL.Password = txtPassword.Text.Trim();
                                TBL.PasswordConfirm = txtConfirmPassword.Text.Trim();
                                TBL.UserRole = ddlRole.SelectedItem.Text;
                                TBL.RoleID = Convert.ToInt16(ddlRole.SelectedValue);
                                TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                                TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                                BL.CreateUser(TBL);
                                
                                try
                                {
                                    EmailBL EML = new EmailBL();
                                    EML.Email("You have been successfully registered on the Moja Cruise System", "Hello " + txtName.Text + " " + txtSurname.Text + "<br/> <br/>"
                                        + "You have been successfully registered on the Moja Cruise System." + "<br/> "
                                        + "Please use your ID No as a username and your password to login." + "<br/>" +
                                            "For more details feel free to contact the Ethekwini Transport Authority." + "<br/>" +
                                            "Regards" + "<br/>" +
                                            "Ethekwini Transport Authority", txtEmail.Text.ToLower(), "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
                                }
                                catch (Exception ex)
                                {
                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "none");
                                    divwarning1.Style.Add("display", "none");

                                    lblError.Visible = true;
                                    //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                                    lblError.Text = "Can't send email. Network problem or services are currently down, please contact support team.";
                                }

                                db.SaveChanges();
                                
                                divsuccess.Style.Add("display", "inline");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "none");
                                divwarning1.Style.Add("display", "none");

                                //// modal popup
                                //lblCreateUser.Text = "Thank you for registering on the System " + "<br/>" + "Username and Password will be emailed to E-mail Address:" + txtUsername.Text.ToLower();
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "1Pop", "openModal();", true);
                            }
                        }
                    }
                }
            
            if (clear == 1)
            {
                Clear();
            }
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
                divwarning1.Style.Add("display", "none");

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Network problem or services are currently down, please contact support team.";
            }
        }

        protected void DisplayUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/Register");
        }
    }
}