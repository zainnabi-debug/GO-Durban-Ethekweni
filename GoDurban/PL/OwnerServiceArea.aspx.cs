using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class OwnerServiceArea : System.Web.UI.Page
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

            lblError.Visible = false;
            lblOwner.Visible = false;
            lblCheck.Visible = false;
            lblCheckUnlink.Visible = false;

            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divsuccess.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if (!IsPostBack)
            {
                LoadOwner();
                LoadData(Convert.ToInt16(ddlOwner.SelectedValue));
                LoadDataUnlink(Convert.ToInt32(ddlOwner.SelectedValue));
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

        public void LoadOwner()
        {
            var data = from o in db.tbl_Owner
                       join s in db.tbl_Status on o.StatusID equals s.StatusID
                       where s.StatusDescription == "Approved"
                       select new
                       {
                           OwnerID = o.OwnerID,
                           Name = o.Name + " " + o.Surname + " - " + o.IDNo
                       };
            ddlOwner.DataSource = data.ToList();
            ddlOwner.DataValueField = "OwnerID";
            ddlOwner.DataTextField = "Name";
            ddlOwner.DataBind();
        }

        public void RemoveDuplicateItems(DropDownList ddl)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                ddl.SelectedIndex = i;
                string str = ddl.SelectedItem.ToString();
                for (int counter = i + 1; counter < ddl.Items.Count; counter++)
                {
                    ddl.SelectedIndex = counter;
                    string compareStr = ddl.SelectedItem.ToString();
                    if (str == compareStr)
                    {
                        ddl.Items.RemoveAt(counter);
                        counter = counter - 1;
                    }
                }
            }
        }

        private void LoadData(int? ownerID = null)
        {
            if (ownerID != null)
            {
                var data = from a in db.tbl_ServiceArea
                           join b in db.tbl_RegionServiceArea on a.ServiceAreaID equals b.ServiceAreaID
                           join c in db.tbl_AssociationServiceArea on b.RegionSAreaID equals c.RegionSAreaID
                           join d in db.tbl_OwnerAssociation on c.AssociationID equals d.AssociationID
                           join e in db.Associations on d.AssociationID equals e.AssociationID
                           where d.OwnerID == ownerID.Value
                           select new
                           {
                               a.ServiceAreaID,
                               a.ServiceAreaDescription,
                               c.AssociationServiceAreaID,
                               e.AssociationName,
                           };
                gvServiceArea.DataSource = data.ToList();
                gvServiceArea.DataBind();
            }
        }


        private void LoadDataUnlink(int? ownerID = null)
        {
            if (ownerID != null)
            {
                var data = from a in db.tbl_OwnerServiceArea
                           join b in db.tbl_Owner on a.OwnerID equals b.OwnerID
                           join c in db.tbl_AssociationServiceArea on a.AssociationServiceAreaID equals c.AssociationServiceAreaID
                           join d in db.tbl_RegionServiceArea on c.RegionSAreaID equals d.RegionSAreaID
                           join e in db.tbl_ServiceArea on d.ServiceAreaID equals e.ServiceAreaID
                           join f in db.Associations on c.AssociationID equals f.AssociationID
                           where a.OwnerID == ownerID.Value
                           select new
                           {
                               a.OwnerServiceAreaID,
                               Name = (b.Name + " " + b.Surname + " " + b.IDNo),
                               e.ServiceAreaDescription,
                               e.ServiceAreaID,
                               f.AssociationName
                           };
                gvServiceAreaUnlink.DataSource = data.ToList();
                gvServiceAreaUnlink.DataBind();
            }
        }

        private bool ValidateOwner()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if ((ddlOwner.SelectedValue == "0"))
            {
                ddlOwner.Style.Add("border", "1px solid red");
                valid = false;
                lblOwner.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Owner!');", true);
            }
            else
            {
                ddlOwner.Style.Add("border", "");
                lblOwner.Visible = false;
            }

            return valid;
        }
        private bool ValidateCheckBox()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            foreach (GridViewRow row in gvServiceArea.Rows)
            {
                CheckBox Chbox = (CheckBox)row.FindControl("chkRow");
                if (Chbox.Checked == false)
                {
                    lblCheck.Visible = true;
                    //Chbox.Checked = true;
                }
                else
                {
                    lblCheck.Visible = false;
                }
            }

            return valid;
        }
        private bool ValidateCheckBoxUnlink()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            foreach (GridViewRow row in gvServiceAreaUnlink.Rows)
            {
                CheckBox Chbox = (CheckBox)row.FindControl("chkRowUnlink");
                if (Chbox.Checked == false)
                {
                    lblCheckUnlink.Visible = true;
                    //Chbox.Checked = true;
                }
                else
                {
                    lblCheckUnlink.Visible = false;
                }
            }

            return valid;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateCheckBox() && ValidateOwner())
                {
                    OwnerServiceAreaBL BL = new OwnerServiceAreaBL();
                    tbl_OwnerServiceArea TBL = new tbl_OwnerServiceArea();

                    CheckBox chkAll = (CheckBox)gvServiceArea.HeaderRow.Cells[0].FindControl("chkAll");

                    int a = gvServiceArea.PageIndex;
                    for (int i = 0; i < gvServiceArea.PageCount; i++)
                    {
                        gvServiceArea.SetPageIndex(i);
                        foreach (GridViewRow row in gvServiceArea.Rows)
                        {
                            CheckBox Chbox = (CheckBox)row.FindControl("chkRow");
                            if (Chbox.Checked == true)
                            {
                                var ownerservicearea = db.tbl_OwnerServiceArea.ToList().FindAll(x => x.OwnerID == Convert.ToInt16(ddlOwner.SelectedValue) && x.AssociationServiceAreaID == Convert.ToInt32(gvServiceArea.DataKeys[row.RowIndex].Values[0]));

                                if (ownerservicearea.Count > 0)
                                {
                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "inline");
                                }
                                else
                                {
                                    TBL.OwnerID = Convert.ToInt16(ddlOwner.SelectedValue);
                                    TBL.AssociationServiceAreaID = Convert.ToInt32(gvServiceArea.DataKeys[row.RowIndex].Values[0]);
                                    BL.CreateOwnerServiceArea(TBL);

                                    LoadDataUnlink(Convert.ToInt32(ddlOwner.SelectedValue));
                                    //accordion2.Style.Add("display", "inline");
                                    accordion3.Style.Add("display", "inline");

                                    divsuccess.Style.Add("display", "inline");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "none");
                                    Chbox.Checked = false;
                                    chkAll.Checked = false;
                                    lblCheck.Visible = false;
                                }
                            }
                        }
                    }
                    gvServiceArea.SetPageIndex(a);
                }
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblCheck.Visible = false;
                lblCheckUnlink.Visible = false;

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Network problem or services are currently down, please contact support team.";
            }
        }


        protected void btnUnlink_Click(object sender, EventArgs e)
        {

            try
            {
                if (ValidateCheckBoxUnlink())
                {
                    foreach (GridViewRow row in gvServiceAreaUnlink.Rows)
                    {
                        if ((row.FindControl("chkRowUnlink") as CheckBox).Checked)
                        {
                            int ownerServiceAreaID = Convert.ToInt32(gvServiceAreaUnlink.DataKeys[row.RowIndex].Value);
                            using (SqlConnection con = new SqlConnection(connStr))
                            {
                                con.Open();
                                cmd = new SqlCommand("DELETE FROM tbl_OwnerServiceArea WHERE OwnerServiceAreaID=" + ownerServiceAreaID, con);
                                cmd.ExecuteNonQuery();
                                con.Close();

                                divsuccess.Style.Add("display", "none");
                                divdanger.Style.Add("display", "none");
                                divinfo.Style.Add("display", "inline");
                                divwarning.Style.Add("display", "none");
                                lblCheckUnlink.Visible = false;
                            }
                        }
                    }

                    CheckBox chkAll = (CheckBox)gvServiceArea.HeaderRow.Cells[0].FindControl("chkAll");

                    foreach (GridViewRow row in gvServiceArea.Rows)
                    {
                        CheckBox Chbox = (CheckBox)row.FindControl("chkRow");
                        Chbox.Checked = false;
                        chkAll.Checked = false;
                    }

                    gvServiceAreaUnlink.AllowPaging = true;
                    gvServiceAreaUnlink.DataBind();
                    LoadDataUnlink(Convert.ToInt32(ddlOwner.SelectedValue));
                    PopulateCheckedValuesUnlink();
                }
            }
            catch (Exception ex)
            {
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");

                lblCheck.Visible = false;
                lblCheckUnlink.Visible = false;

                lblError.Visible = true;
                //lblError.Text = "Connection Problem: " + ex.Message.ToString();
                lblError.Text = "Network problem or services are currently down, please contact support team.";
            }
            LoadDataUnlink(Convert.ToInt32(ddlOwner.SelectedValue));
        }

        protected void ddlOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOwner.SelectedValue == "0")
            {
                accordion2.Style.Add("display", "inline");
                accordion3.Style.Add("display", "none");
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
            }
            else
            {
                LoadData(Convert.ToInt16(ddlOwner.SelectedValue));
                LoadDataUnlink(Convert.ToInt16(ddlOwner.SelectedValue));
                accordion2.Style.Add("display", "inline");
                accordion3.Style.Add("display", "inline");
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
            }
        }

        protected void gvServiceArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCheckedValues();
            gvServiceArea.PageIndex = e.NewPageIndex;
            //gvServiceArea.DataBind();
            LoadData(Convert.ToInt16(ddlOwner.SelectedValue));
            PopulateCheckedValues();
        }

        protected void gvServiceAreaUnlink_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCheckedValuesUnlink();
            gvServiceAreaUnlink.PageIndex = e.NewPageIndex;
            //gvServiceAreaUnlink.DataBind();
            LoadDataUnlink(Convert.ToInt16(ddlOwner.SelectedValue));
            PopulateCheckedValuesUnlink();
        }

        private void PopulateCheckedValues()
        {
            ArrayList regionserviceareas = (ArrayList)Session["CHECKED_ITEMS"];
            if (regionserviceareas != null && regionserviceareas.Count > 0)
            {
                foreach (GridViewRow gvrow in gvServiceArea.Rows)
                {
                    int index = (int)gvServiceArea.DataKeys[gvrow.RowIndex].Value;
                    if (regionserviceareas.Contains(index))
                    {
                        CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkRow");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }

        //This method is used to save the checkedstate of values
        private void SaveCheckedValues()
        {
            ArrayList regionserviceareas = new ArrayList();
            int index = -1;
            foreach (GridViewRow gvrow in gvServiceArea.Rows)
            {
                index = (int)gvServiceArea.DataKeys[gvrow.RowIndex].Value;
                bool result = ((CheckBox)gvrow.FindControl("chkRow")).Checked;

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    regionserviceareas = (ArrayList)Session["CHECKED_ITEMS"];
                if (result)
                {
                    if (!regionserviceareas.Contains(index))
                        regionserviceareas.Add(index);
                }
                else
                    regionserviceareas.Remove(index);
            }
            if (regionserviceareas != null && regionserviceareas.Count > 0)
                Session["CHECKED_ITEMS"] = regionserviceareas;
        }

        private void PopulateCheckedValuesUnlink()
        {
            ArrayList associationserviceareas = (ArrayList)Session["CHECKED_ITEMS"];
            if (associationserviceareas != null && associationserviceareas.Count > 0)
            {
                foreach (GridViewRow gvrow in gvServiceAreaUnlink.Rows)
                {
                    int index = (int)gvServiceAreaUnlink.DataKeys[gvrow.RowIndex].Value;
                    if (associationserviceareas.Contains(index))
                    {
                        CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkRowUnlink");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }

        //This method is used to save the checkedstate of values
        private void SaveCheckedValuesUnlink()
        {
            ArrayList associationserviceareas = new ArrayList();
            int index = -1;
            foreach (GridViewRow gvrow in gvServiceAreaUnlink.Rows)
            {
                index = (int)gvServiceAreaUnlink.DataKeys[gvrow.RowIndex].Value;
                bool result = ((CheckBox)gvrow.FindControl("chkRowUnlink")).Checked;

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    associationserviceareas = (ArrayList)Session["CHECKED_ITEMS"];
                if (result)
                {
                    if (!associationserviceareas.Contains(index))
                        associationserviceareas.Add(index);
                }
                else
                    associationserviceareas.Remove(index);
            }
            if (associationserviceareas != null && associationserviceareas.Count > 0)
                Session["CHECKED_ITEMS"] = associationserviceareas;
        }

    }
}