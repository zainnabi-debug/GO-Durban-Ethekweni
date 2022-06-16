using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class User : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
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
            
            if (!Page.IsPostBack)
            {
                LoadRole();
                LoadGender();
                LoadRace();
                LoadData();
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

        public void LoadRole()
        {
            RoleBL BL = new RoleBL();
            List<tbl_Role> list = BL.LoadRole();
            ddlRole.DataSource = list;
            ddlRole.DataValueField = "RoleID";
            ddlRole.DataTextField = "RoleDescription";
            ddlRole.DataBind();
        }

        public void LoadGender()
        {
            GenderBL BL = new GenderBL();
            List<tbl_Gender> list = BL.LoadGender();
            ddlGender.DataSource = list;
            ddlGender.DataValueField = "GenderID";
            ddlGender.DataTextField = "GenderDescription";
            ddlGender.DataBind();
        }

        public void LoadRace()
        {
            RaceBL BL = new RaceBL();
            List<tbl_Race> list = BL.LoadRace();
            ddlRace.DataSource = list;
            ddlRace.DataValueField = "RaceID";
            ddlRace.DataTextField = "RaceDescription";
            ddlRace.DataBind();
        }
       
      
        protected void RemoveBorder()
        {
            txtName.Style.Add("border", "");
            txtSurname.Style.Add("border", "");
            txtCellNo.Style.Add("border", "");
            txtIDNo.Style.Add("border", "");
            txtPassword.Style.Add("border", "");
            txtEmail.Style.Add("border", "");
            txtConfirmPassword.Style.Add("border", "");
            ddlGender.Style.Add("border", "");
            ddlRace.Style.Add("border", "");
            ddlRole.Style.Add("border", "");
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

            //if ((txtPassword.Text == string.Empty))
            //{
            //    txtPassword.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    txtPassword.Style.Add("border", "");
            //}

            //if ((txtConfirmPassword.Text == string.Empty))
            //{
            //    txtConfirmPassword.Style.Add("border", "1px solid red");
            //    valid = false;
            //}
            //else
            //{
            //    txtConfirmPassword.Style.Add("border", "");
            //}

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

                    if (temp.Contains("Add"))
                    {
                        using (GoDurbanEntities db = new GoDurbanEntities())
                        {
                            var username = db.tbl_User.ToList().FindAll(x => String.Compare(x.UserName, (string)txtIDNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0 && x.IsActive == true);

                            if (username.Count > 0 || string.IsNullOrWhiteSpace(txtIDNo.Text))
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "inline");
                                txtIDNo.Text = string.Empty;
                                txtIDNo.Focus();
                            }
                            else
                            {
                                clear = 1;
                                TBL.CellNo = txtCellNo.Text;
                                TBL.Email = txtEmail.Text;
                                TBL.IDNo = txtIDNo.Text;
                                TBL.Name = txtName.Text;
                                TBL.Surname = txtSurname.Text;
                                TBL.UserName = txtIDNo.Text.Trim();
                                TBL.Password = txtPassword.Text;
                                TBL.PasswordConfirm = txtConfirmPassword.Text;
                                TBL.RoleID = Convert.ToInt16(ddlRole.SelectedValue);
                                TBL.UserRole = ddlRole.SelectedItem.Text;
                                TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                                TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);

                                BL.CreateUser(TBL);

                                //divsuccess.Style.Add("display", "inline");
                                //divdanger.Style.Add("display", "none");
                                //divinfo.Style.Add("display", "none");
                                //divwarning.Style.Add("display", "none");

                                try
                                {
                                    EmailBL EML = new EmailBL();
                                    EML.Email("Moja Cruise System Registration", "Hello " + txtName.Text + " " + txtSurname.Text + "<br/> <br/>"
                                        + "You have been successfully registered on the Moja Cruise System." + "<br/> "
                                        + "Please use your ID No as a username and your password to login." + "<br/>" +
                                            //"For more details feel free to contact the Ethekwini Transport Authority" + "<br/>" +
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
                                    lblError.Text = "Can't send email. Network problem or services are currently down, please contact support team.";
                                }

                                db.SaveChanges();

                                divsuccess.Style.Add("display", "inline");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "none");

                                Clear();
                            }
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        RemoveBorder();
                        clear = 1;
                        int Id = int.Parse(ViewState["UserID"].ToString());
                        TBL.UserID = Id;
                        TBL.CellNo = txtCellNo.Text;
                        TBL.Email = txtEmail.Text;
                        TBL.IDNo = txtIDNo.Text;
                        TBL.Name = txtName.Text;
                        TBL.Surname = txtSurname.Text;
                        TBL.UserName = txtIDNo.Text.Trim();
                        TBL.Password = txtPassword.Text;
                        TBL.PasswordConfirm = txtConfirmPassword.Text;
                        TBL.UserRole = ddlRole.SelectedItem.Text;
                        TBL.RoleID = Convert.ToInt16(ddlRole.SelectedValue);
                        TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                        TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                        TBL.IsActive = Convert.ToBoolean(chkStatus.Checked);
                        BL.UpdateUser(TBL);

                        divUser.Style.Add("display", "inline");
                        divUser1.Style.Add("display", "inline");

                        divsuccess.Style.Add("display", "none");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "inline");
                        divwarning.Style.Add("display", "none");

                        btnAdd.Text = "Add";

                        Clear();
                    }
                }

                //Refresh Grid
                LoadData();
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

       

        protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //    if (ddlUserType.SelectedItem.Text == "Driver")
            //    {
            //        dvDriver.Style.Add("display", "inline");
            //        dvPassenger.Style.Add("display", "none");
            //    }
            //    else if (ddlUserType.SelectedItem.Text == "Passenger")
            //    {
            //        dvDriver.Style.Add("display", "none");
            //        dvPassenger.Style.Add("display", "inline");
            //    }
            //    else
            //    {
            //        dvDriver.Style.Add("display", "none");
            //        dvPassenger.Style.Add("display", "none");
            //    }
        }

        
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            divUser.Style.Add("display", "inline");
            divUser1.Style.Add("display", "inline");
            divactive.Style.Add("display", "inline");
            divactive1.Style.Add("display", "inline");
            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvUsers.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvUsers.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["UserID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);

                var temp = db.tbl_User.ToList().FirstOrDefault(x => x.UserID == Convert.ToInt16(Id));

                if (temp != null)
                {

                    RemoveBorder();
                    Session["Id"] = Id;
                    txtName.Text = temp.Name;
                    txtSurname.Text = temp.Surname;
                    txtIDNo.Text = temp.IDNo;
                    txtCellNo.Text = temp.CellNo;
                    txtEmail.Text = temp.Email;
                    txtPassword.Text = temp.Password;
                    txtConfirmPassword.Text = temp.PasswordConfirm;
                    ddlGender.SelectedValue = temp.GenderID.ToString();
                    ddlRace.SelectedValue = temp.RaceID.ToString();
                    ddlRole.SelectedValue = temp.RoleID.ToString();
                    chkStatus.Checked = temp.IsActive.Value;

                    btnAdd.Text = "Update";
                }
            }
            //else if (commandName == "DeleteItem")
            //{
            //    string Source = gvUsers.Rows[myRow.RowIndex].Cells[0].Text;

            //    ViewState["Id"] = Id;

            //    var temp = db.tbl_User.ToList().FirstOrDefault(x => x.UserID == Convert.ToInt16(Id));

            //    temp.IsActive = false;
            //    db.SaveChanges();
            //    LoadData();
            //    Response.Redirect("~/PL/Register");
            //}

        }

        protected void gvUsers_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();


            if (gvUsers.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvUsers.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvUsers.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvUsers.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }


        public void LoadData()
        {
            //string loggedUserName = Session["UserName"].ToString();

            var list = (from a in db.tbl_User
                            join b in db.tbl_Gender on a.GenderID equals b.GenderID
                            join c in db.tbl_Race on a.RaceID equals c.RaceID
                            join d in db.tbl_Role on a.RoleID equals d.RoleID
                            //join e in db.tbl_Status on a.StatusID equals e.StatusID
                            //where a.UserName == loggedUserName 
                             select new
                            {
                                a.UserID,
                                a.CellNo,
                                a.Email,
                                a.DateCreated,
                                b.GenderDescription,
                                a.IDNo,
                                a.Name,
                                c.RaceDescription,
                                d.RoleDescription,
                                //e.StatusDescription,
                                a.Surname,
                                a.UserName,
                                a.IsActive
                            });
                gvUsers.DataSource = list.ToList();
                gvUsers.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/User.aspx");
        }
    }
}