using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Users : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        
        public static int userID, userpageID = 0;
        public static int  id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                Response.Redirect("~/Index.aspx?ShowDialog=yes");
            }
            else
            {
                string type = Session["UserRole"].ToString();

                if (type != "Admin" && type != "Supervisor")
                {
                    Response.Redirect("~/Error.aspx");
                }
            }

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");
            divwarning1.Style.Add("display", "none");

            if (!Page.IsPostBack)
            {
                LoadGender();
                LoadRace();
                //LoadRole();
            }

            lblError.Visible = false;
            lblCheck.Visible = false;
            lblID.Visible = false;
            divAdd.Style.Add("display", "inline");
            //divUpdate.Style.Add("display", "none");
        }

        //private void LoadRole()
        //{
        //    RoleBL BL = new RoleBL();
        //    List<tbl_Role> list = BL.LoadRole();
        //    ddlRole.DataSource = list;
        //    ddlRole.DataValueField = "RoleID";
        //    ddlRole.DataTextField = "RoleDescription";
        //    ddlRole.DataBind();
        //    ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByText("User"));
        //}

        public void LoadRace()
        {
            RaceBL BL = new RaceBL();
            List<tbl_Race> list = BL.LoadRace();
            ddlRace.DataSource = list;
            ddlRace.DataValueField = "RaceID";
            ddlRace.DataTextField = "RaceDescription";
            ddlRace.DataBind();
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

        protected void RemoveBorder()
        {
            txtName.Style.Add("border", "");
            txtSurname.Style.Add("border", "");
            txtCellNo.Style.Add("border", "");
            txtIDNo.Style.Add("border", "");
            txtEmail.Style.Add("border", "");
            ddlGender.Style.Add("border", "");
            ddlRace.Style.Add("border", "");
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

            return valid;
        }
        
        protected void Clear()
        {
            //txtSearch.Text = string.Empty;
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtIDNo.Text = string.Empty;
            txtCellNo.Text = string.Empty;
            txtSurname.Text = string.Empty;
            txtIDNo.Text = string.Empty;
            ddlRace.SelectedValue = "0";
            ddlGender.SelectedValue = "0";

            chkSupervisor.Checked = false;
            chkAll.Checked = false;
            chkAssociation.Checked = false;
            chkAssociationL.Checked = false;
            chkAssociationR.Checked = false;
            chkAssociationSA.Checked = false;
            chkOwner.Checked = false;
            chkOwnerA.Checked = false;
            chkOwnerSA.Checked = false;
            chkRegion.Checked = false;
            chkRegionL.Checked = false;
            chkRegionSA.Checked = false;
            chkVehicle.Checked = false;
            chkDriver.Checked = false;

            RemoveBorder();
        }
        protected void GetUserID()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;

            SqlConnection csdb = new SqlConnection(constr);

            SqlDataAdapter date = new SqlDataAdapter("SELECT max(UserID)  FROM[tbl_User]", csdb);
            SqlDataAdapter adap = new SqlDataAdapter("SELECT UserID  FROM[tbl_User] where UserID = (SELECT max(UserID)  FROM[tbl_User])", csdb);

            //  SqlDataAdapter adap = new SqlDataAdapter("SELECT max(ApplicationID) as ApplicationID FROM Application", csdb);
            DataSet MyDataSet = new DataSet();

            adap.Fill(MyDataSet);
            int vall = 0;
            for (int i = 0; i < MyDataSet.Tables[0].Rows.Count; i++)
            {
                vall = Convert.ToInt16(MyDataSet.Tables[0].Rows[i]["UserID"].ToString());
            }
            userID = vall;
        }

        private bool ValidateCheckBox()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if (!chkSupervisor.Checked)
            {
                if (!chkAssociation.Checked && !chkAssociationL.Checked && !chkAssociationR.Checked && !chkAssociationSA.Checked &&
                    !chkDriver.Checked && !chkOwner.Checked && !chkOwnerA.Checked && !chkOwnerSA.Checked &&
                    !chkRegion.Checked && !chkRegionL.Checked && !chkRegionSA.Checked && !chkVehicle.Checked)
                {
                    lblCheck.Visible = true;
                    valid = false;
                }
                else
                {
                    lblCheck.Visible = false;
                }
            }

            return valid;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            RemoveBorder();
            Clear();
            if ((txtSearch.Text != ""))
            {
                string search = txtSearch.Text.ToLower().Trim();

                var user = db.tbl_User.ToList().FirstOrDefault(x => x.IDNo == search);

                if (user == null || user.UserRole == "Admin")
                {
                    lblID.Visible = true;
                    txtSearch.Text = string.Empty;
                    txtSearch.Focus();
                }
                else
                {
                    id = user.UserID;

                    lblID.Visible = false;
                    divAdd.Style.Add("display", "inline");
                    //divUpdate.Style.Add("display", "inline");

                    btnAdd.Text = "Update";

                    txtName.Text = user.Name;
                    txtSurname.Text = user.Surname;
                    txtCellNo.Text = user.CellNo;
                    txtEmail.Text = user.Email;
                    txtIDNo.Text = user.IDNo;
                    ddlGender.SelectedValue = user.GenderID.ToString();
                    ddlRace.SelectedValue = user.RaceID.ToString();
                    chkSupervisor.Checked = Convert.ToBoolean(user.IsSupervisor);

                    userID = Convert.ToInt16(user.UserID);
                    var userid = db.tbl_User.ToList().FirstOrDefault(x => x.UserID == Convert.ToInt16(userID));
                    int ID = userid.UserID;
                    var userpage = db.tbl_UserPage.ToList().FirstOrDefault(x => x.UserID == Convert.ToInt16(ID));
                    userpageID = userpage.UserPageID;

                    if (userpage.UserID == null)
                    {
                        divsuccess.Style.Add("display", "none");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "none");
                        divwarning.Style.Add("display", "none");

                        lblError.Visible = true;
                        lblError.Text = "No Record Was Checked";
                    }
                    else if (userpage.UserID != null)
                    {
                        chkAssociation.Checked = Convert.ToBoolean(userpage.Association);
                        chkAssociationL.Checked = Convert.ToBoolean(userpage.AssociationL);
                        chkAssociationR.Checked = Convert.ToBoolean(userpage.AssociationR);
                        chkAssociationSA.Checked = Convert.ToBoolean(userpage.AssociationSA);
                        chkOwner.Checked = Convert.ToBoolean(userpage.Owner);
                        chkOwnerA.Checked = Convert.ToBoolean(userpage.OwnerA);
                        chkOwnerSA.Checked = Convert.ToBoolean(userpage.OwnerSA);
                        chkRegion.Checked = Convert.ToBoolean(userpage.Region);
                        chkRegionL.Checked = Convert.ToBoolean(userpage.RegionL);
                        chkRegionSA.Checked = Convert.ToBoolean(userpage.RegionSA);
                        chkVehicle.Checked = Convert.ToBoolean(userpage.Vehicle);
                        chkDriver.Checked = Convert.ToBoolean(userpage.Driver);
                    }
                }
            }
            else
            {
                Clear();
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int clear = 0;
            try
            {
                UserBL BL = new UserBL();
                UserPageBL upBL = new UserPageBL();
                tbl_User TBL = new tbl_User();
                tbl_UserPage upTBL = new tbl_UserPage();

                if (ValidateCheckBox() && ValidateUser())
                {
                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        using (GoDurbanEntities db = new GoDurbanEntities())
                        {
                            //var idno = db.tbl_User.ToList().FindAll(x => String.Compare(x.IDNo, (string)txtIDNo.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0 && x.IsActive == true);
                            //var email = db.tbl_User.ToList().FindAll(x => String.Compare(x.Email, (string)txtEmail.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0 && x.IsActive == true);

                            var idnoemail = db.tbl_User.FirstOrDefault(x => x.Email == txtEmail.Text.Trim() || x.IDNo == txtIDNo.Text.Trim());

                            if (idnoemail != null)
                            {
                                if (idnoemail.Email.ToLower() == txtEmail.Text.ToLower().Trim())
                                {
                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "inline");
                                    divwarning1.Style.Add("display", "none");

                                    txtEmail.Text = string.Empty;
                                    txtEmail.Focus();
                                }
                                else if (idnoemail.IDNo.ToLower() == txtIDNo.Text.ToLower().Trim())
                                {
                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "none");
                                    divwarning1.Style.Add("display", "inline");

                                    txtIDNo.Text = string.Empty;
                                    txtIDNo.Focus();
                                }
                            }
                            else
                            {
                                clear = 1;
                                TBL.CellNo = txtCellNo.Text.Trim();
                                TBL.Email = txtEmail.Text.ToLower().Trim();
                                TBL.IDNo = txtIDNo.Text.ToLower().Trim();
                                TBL.Name = txtName.Text.Trim();
                                TBL.Surname = txtSurname.Text.Trim();
                                TBL.UserName = txtIDNo.Text.ToLower().Trim();
                                TBL.Password = txtPassword.Text.Trim();
                                TBL.PasswordConfirm = txtConfirmPassword.Text.Trim();
                                //TBL.UserRole = ddlRole.SelectedItem.Text;
                                //TBL.RoleID = Convert.ToInt16(ddlRole.SelectedValue);
                                TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                                TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                                TBL.IsSupervisor = chkSupervisor.Checked;
                                if(chkSupervisor.Checked==true)
                                {
                                    TBL.UserRole = "Supervisor";
                                }
                                else
                                {
                                    TBL.UserRole = "User";
                                }
                                BL.CreateUser(TBL);
                                //if (ValidateSelectedCheckBox())
                                //{

                                GetUserID();
                                upTBL.UserID = Convert.ToInt16(userID);
                                upTBL.Association = chkAssociation.Checked;
                                upTBL.AssociationL = chkAssociationL.Checked;
                                upTBL.AssociationR = chkAssociationR.Checked;
                                upTBL.AssociationSA = chkAssociationSA.Checked;
                                upTBL.Owner = chkOwner.Checked;
                                upTBL.OwnerA = chkOwnerA.Checked;
                                upTBL.OwnerSA = chkOwnerSA.Checked;
                                upTBL.Region = chkRegion.Checked;
                                upTBL.RegionL = chkRegionL.Checked;
                                upTBL.RegionSA = chkRegionSA.Checked;
                                upTBL.Vehicle = chkVehicle.Checked;
                                upTBL.Driver = chkDriver.Checked;
                                upBL.CreateUserPage(upTBL);
                                //}


                                try
                                {
                                    EmailBL EML = new EmailBL();
                                    EML.Email("Moja Cruise System Registration", "Hello " + txtName.Text + " " + txtSurname.Text + "<br/> <br/>"
                                        + "You have been successfully registered on the Moja Cruise System." + "<br/> "
                                        + "Please use your ID No as a username and your password is 'MojaCruise' to login." + "<br/>" +
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

                                    lblError.Visible = true;
                                    //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                                    lblError.Text = "Can't send email. Network problem or services are currently down, please contact support team.";
                                }

                                db.SaveChanges();

                                divsuccess.Style.Add("display", "inline");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "none");

                                lblCheck.Visible = false;
                                Clear();
                            }
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        var idnoemail = db.tbl_User.FirstOrDefault(x => x.Email == txtEmail.Text.Trim() && id !=x.UserID);
                        var idsearch = db.tbl_User.FirstOrDefault(x => x.IDNo == txtIDNo.Text.Trim() && id != x.UserID);

                        if (idnoemail != null || idsearch!=null)
                        {
                           
                            if (idnoemail!=null)
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "inline");
                                divwarning1.Style.Add("display", "none");

                                txtEmail.Text = string.Empty;
                                txtEmail.Focus();
                            }
                            else if (idsearch!=null)
                            {
                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "none");
                                divwarning.Style.Add("display", "none");
                                divwarning1.Style.Add("display", "inline");

                                txtIDNo.Text = string.Empty;
                                txtIDNo.Focus();
                            }
                        }
                        else
                        {
                            RemoveBorder();
                            clear = 1;
                            TBL.UserID = userID;
                            TBL.CellNo = txtCellNo.Text;
                            TBL.Email = txtEmail.Text.ToLower().Trim();
                            TBL.IDNo = txtIDNo.Text.ToLower().Trim();
                            TBL.Name = txtName.Text;
                            TBL.Surname = txtSurname.Text;
                            TBL.UserName = txtIDNo.Text.ToLower().Trim();
                            TBL.RaceID = Convert.ToInt16(ddlRace.SelectedValue);
                            TBL.GenderID = Convert.ToInt16(ddlGender.SelectedValue);
                            //TBL.UserRole = ddlRole.SelectedItem.Text;
                            //TBL.RoleID = Convert.ToInt16(ddlRole.SelectedValue);
                            TBL.IsSupervisor = chkSupervisor.Checked;
                            if (chkSupervisor.Checked == true)
                            {
                                TBL.UserRole = "Supervisor";
                            }
                            else
                            {
                                TBL.UserRole = "User";
                            }
                            BL.UpdateUser(TBL);

                            upTBL.UserPageID = userpageID;
                            upTBL.UserID = userID;
                            upTBL.Association = chkAssociation.Checked;
                            upTBL.AssociationL = chkAssociationL.Checked;
                            upTBL.AssociationR = chkAssociationR.Checked;
                            upTBL.AssociationSA = chkAssociationSA.Checked;
                            upTBL.Owner = chkOwner.Checked;
                            upTBL.OwnerA = chkOwnerA.Checked;
                            upTBL.OwnerSA = chkOwnerSA.Checked;
                            upTBL.Region = chkRegion.Checked;
                            upTBL.RegionL = chkRegionL.Checked;
                            upTBL.RegionSA = chkRegionSA.Checked;
                            upTBL.Vehicle = chkVehicle.Checked;
                            upTBL.Driver = chkDriver.Checked;

                            upBL.UpdateUserPage(upTBL);
                            lblCheck.Visible = false;

                            try
                            {
                                EmailBL EML = new EmailBL();
                                EML.Email("Moja Cruise System Registration", "Hello " + txtName.Text + " " + txtSurname.Text + "<br/> <br/>"
                                    + "You have been successfully registered on the Moja Cruise System." + "<br/> "
                                    + "Please use your ID No as a username and password is 'MojaCruise' to login." + "<br/>"
                                    + "For more details feel free to contact the Ethekwini Transport Authority" + "<br/>"
                                    + "Regards" + "<br/>"
                                    + "Ethekwini Transport Authority", txtEmail.Text.ToLower(), "Ethekwini Transport Authority", "ethekwini@oneconnectgroup.com");
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

                            divUser.Style.Add("display", "inline");
                            //divUser1.Style.Add("display", "inline");

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "inline");
                            divwarning.Style.Add("display", "none");

                            btnAdd.Text = "Add";

                            Clear();
                        }
                    }
                }

                //Refresh Grid
                //LoadData();

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

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Network problem or services are currently down, please contact support team.";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            divAdd.Style.Add("display", "inline");
            //divUpdate.Style.Add("display", "none");
            Response.Redirect("~/PL/Users.aspx");
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
                            a.IsActive,
                            a.IsSupervisor
                        });
            gvUsers.DataSource = list.ToList();
            gvUsers.DataBind();
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

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            divUser.Style.Add("display", "inline");
            divUser1.Style.Add("display", "inline");
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
                    ddlGender.SelectedValue = temp.GenderID.ToString();
                    ddlRace.SelectedValue = temp.RaceID.ToString();
                    chkSupervisor.Checked = temp.IsSupervisor.Value;

                    btnAdd.Text = "Update";
                }
            }
        }


    }
}