using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Race : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        SqlCommand cmd;
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

            if (!IsPostBack)
            {
                LoadData();
            }
            lblError.Visible = false;
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRace())
                {
                    RaceBL BL = new RaceBL();
                    tbl_Race TBL = new tbl_Race();

                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Add"))
                    {
                        var race = db.tbl_Race.ToList().FindAll(x => String.Compare(x.RaceDescription, (string)txtRace.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (race.Count > 0 || string.IsNullOrWhiteSpace(txtRace.Text))
                        {
                            txtRace.Text = string.Empty;
                            txtRace.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            TBL.RaceDescription = txtRace.Text.Trim();
                            BL.CreateRace(TBL);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        var race = db.tbl_Race.ToList().FindAll(x => String.Compare(x.RaceDescription, (string)txtRace.Text.Trim(), StringComparison.OrdinalIgnoreCase) == 0);

                        if (race.Count > 0 || string.IsNullOrWhiteSpace(txtRace.Text))
                        {
                            txtRace.Text = string.Empty;
                            txtRace.Focus();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            int Id = int.Parse(ViewState["RaceID"].ToString());
                            TBL.RaceID = Id;
                            TBL.RaceDescription = txtRace.Text.Trim();
                            BL.UpdateRace(TBL);
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "inline");
                            divwarning.Style.Add("display", "none");

                            btnAdd.Text = "Add";
                        }
                        
                    }

                    LoadData();
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

        private bool ValidateRace()
        {
            bool valid = true;

            if ((txtRace.Text == string.Empty))
            {
                txtRace.Style.Add("border", "1px solid red");
                valid = false;
            }
            else
            {
                txtRace.Style.Add("border", "");
            }
            return valid;
        }

        protected void Clear()
        {
            txtRace.Text = string.Empty;
            RemoveBorder();
        }

        protected void RemoveBorder()
        {
            txtRace.Style.Add("border", "");
        }

        private void LoadData()
        {
            RaceBL BL = new RaceBL();
            List<tbl_Race> list = BL.LoadRace();
            gvRace.DataSource = list;
            gvRace.DataBind();
        }

        protected void gvRace_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvRace.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvRace.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvRace.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvRace.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvRace_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            RaceBL BL = new RaceBL();
            tbl_Race TBL = new tbl_Race();

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvRace.DataKeys[myRow.RowIndex].Value.ToString();

            if (commandName == "EditItem")
            {
                //Accessing BoundField Column
                string Source = gvRace.Rows[myRow.RowIndex].Cells[0].Text;

                ViewState["RaceID"] = Id;
                //int Id = Convert.ToInt32(e.CommandArgument);
                
                var temp = db.tbl_Race.ToList().FirstOrDefault(x => x.RaceID == Convert.ToInt16(Id));

                if (temp != null)
                {
                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "none");
                    divwarning.Style.Add("display", "none");
                    RemoveBorder();
                    Session["RaceID"] = Id;
                    txtRace.Text = temp.RaceDescription;
                    btnAdd.Text = "Update";

                }
            }

            //else if (commandName == "DeleteItem")
            //{
            //    string Source = gvRace.Rows[myRow.RowIndex].Cells[0].Text;

            //    tbl_Owner TBL2 = new tbl_Owner();
            //    OwnerBL BL2 = new OwnerBL();
            //    tbl_Driver TBL1 = new tbl_Driver();
            //    DriverBL BL1 = new DriverBL();
            //    tbl_User TBL3 = new tbl_User();
            //    UserBL BL3 = new UserBL();

            //    ViewState["RaceID"] = Id;

            //    var temp = db.tbl_Race.ToList().FirstOrDefault(x => x.RaceID == Convert.ToInt16(Id));
            //    db.tbl_Race.Remove(temp);

            //    db.SaveChanges();
            //    RemoveBorder();
            //    LoadData();
            //    Clear();
            //    btnAdd.Text = "Add";
            //}
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/Race.aspx");
        }
    }
}