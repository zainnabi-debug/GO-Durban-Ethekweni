using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class AssociationServiceArea : System.Web.UI.Page
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
            lblAssociation.Visible = false;
            lblCheck.Visible = false;
            lblCheckUnlink.Visible = false;

            if (!IsPostBack)
            {
                LoadAssociation();
                LoadData(Convert.ToInt16(ddlAssociation.SelectedValue));
                LoadDataUnlink(Convert.ToInt32(ddlAssociation.SelectedValue));
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

        //public void LoadAssociation()
        //{
        //    AssociationBL BL = new AssociationBL();
        //    List<tbl_Association> list = BL.LoadAssociation();
        //    ddlAssociation.DataSource = list;
        //    ddlAssociation.DataValueField = "AssociationID";
        //    ddlAssociation.DataTextField = "AssociationName";
        //    ddlAssociation.DataBind();
        //}

        public void LoadAssociation()
        {
            var data = from a in db.Associations
                       join b in db.tbl_Status on a.StatusID equals b.StatusID
                       where b.StatusDescription == "Approved"
                       select new
                       {
                           a.AssociationID,
                           a.AssociationName
                       };
            ddlAssociation.DataSource = data.ToList();
            ddlAssociation.DataValueField = "AssociationID";
            ddlAssociation.DataTextField = "AssociationName";
            ddlAssociation.DataBind();
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

        protected void ddlAssociation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAssociation.SelectedValue == "0")
            {
                accordion2.Style.Add("display", "none");
                accordion3.Style.Add("display", "none");
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
            }
            else
            {
                LoadData(Convert.ToInt16(ddlAssociation.SelectedValue));
                LoadDataUnlink(Convert.ToInt32(ddlAssociation.SelectedValue));
                accordion2.Style.Add("display", "inline");
                accordion3.Style.Add("display", "inline");
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
            }
        }

        private bool ValidateAssociation()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if ((ddlAssociation.SelectedValue == "0"))
            {
                ddlAssociation.Style.Add("border", "1px solid red");
                valid = false;
                lblAssociation.Visible = true;
                // ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Association!');", true);
            }
            else
            {
                ddlAssociation.Style.Add("border", "");
                lblAssociation.Visible = false;
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
                if (ValidateCheckBox() && ValidateAssociation())
                {
                    AssociationServiceAreaBL BL = new AssociationServiceAreaBL();
                    tbl_AssociationServiceArea TBL = new tbl_AssociationServiceArea();

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
                                var associationservicearea = db.tbl_AssociationServiceArea.ToList().FindAll(x => x.AssociationID == Convert.ToInt16(ddlAssociation.SelectedValue) && x.RegionSAreaID == Convert.ToInt32(gvServiceArea.DataKeys[row.RowIndex].Values[0]));

                                if (associationservicearea.Count > 0)
                                {
                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "inline");
                                }
                                else
                                {
                                    TBL.AssociationID = Convert.ToInt16(ddlAssociation.SelectedValue);
                                    TBL.RegionSAreaID = Convert.ToInt32(gvServiceArea.DataKeys[row.RowIndex].Values[0]);
                                    BL.CreateAssociationServiceArea(TBL);

                                    LoadDataUnlink(Convert.ToInt32(ddlAssociation.SelectedValue));
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


        private void LoadData(int? associationID = null)
        {
            if (associationID != null)
            {
                var data = from a in db.tbl_ServiceArea
                           join b in db.tbl_RegionServiceArea on a.ServiceAreaID equals b.ServiceAreaID
                           join c in db.tbl_AssociationRegion on b.RegionID equals c.RegionID
                           join d in db.Associations on c.AssociationID equals d.AssociationID
                           where d.AssociationID == associationID.Value
                           select new
                           {
                               a.ServiceAreaID,
                               a.ServiceAreaDescription,
                               b.RegionSAreaID
                           };
                gvServiceArea.DataSource = data.ToList();
                gvServiceArea.DataBind();
            }
        }


        private void GenerateUniqueData(int cellno = 1)
        {
            string initialnamevalue = gvServiceArea.Rows[1].Cells[cellno].Text;
            for (int i = 1; i < gvServiceArea.Rows.Count; i++)
            {
                if (gvServiceArea.Rows[i].Cells[cellno].Text == initialnamevalue)
                {
                    gvServiceArea.Rows[i].Cells[cellno].Text = string.Empty;
                }
                else
                {
                    initialnamevalue = gvServiceArea.Rows[i].Cells[cellno].Text;
                }
            }
        }

        private void AvoidDuplicates()
        {
            int i = gvServiceArea.Rows.Count - 2; //GridView row count
            while (i >= 0) //Iterates through a while loop to get row index
            {
                GridViewRow curRow = gvServiceArea.Rows[i]; //Gets the current row
                GridViewRow preRow = gvServiceArea.Rows[i + 1]; //Gets the previous row

                int j = 1;
                while (j < curRow.Cells.Count) //Inner loop to get the row values
                {
                    /****Condition to check if it has duplicate rows - Starts****/
                    if (curRow.Cells[j].Text == preRow.Cells[j].Text) //Matches the row values
                    {
                        if (preRow.Cells[j].RowSpan < 2)
                        {
                            curRow.Cells[j].RowSpan = 2;
                            preRow.Cells[j].Visible = false;
                        }
                        else
                        {
                            curRow.Cells[j].RowSpan = preRow.Cells[j].RowSpan + 1;
                            preRow.Cells[j].Visible = false;
                        }
                    }
                    /****Ccondition to check if it has duplicate rows - Ends****/
                    j++;
                }
                i--;
            }
        }


        protected void gvServiceArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCheckedValues();
            gvServiceArea.PageIndex = e.NewPageIndex;
            //gvServiceArea.DataBind();
            LoadData(Convert.ToInt16(ddlAssociation.SelectedValue));
            PopulateCheckedValues();
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
                            int associationServiceAreaID = Convert.ToInt32(gvServiceAreaUnlink.DataKeys[row.RowIndex].Value);
                            using (SqlConnection con = new SqlConnection(connStr))
                            {
                                con.Open();
                                cmd = new SqlCommand("DELETE FROM tbl_OwnerServiceArea WHERE AssociationServiceAreaID=" + associationServiceAreaID, con);
                                cmd.ExecuteNonQuery();
                                cmd = new SqlCommand("DELETE FROM tbl_AssociationServiceArea WHERE AssociationServiceAreaID=" + associationServiceAreaID, con);
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

                    gvServiceAreaUnlink.AllowPaging = true;
                    gvServiceAreaUnlink.DataBind();
                    LoadDataUnlink(Convert.ToInt32(ddlAssociation.SelectedValue));
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
            LoadDataUnlink(Convert.ToInt32(ddlAssociation.SelectedValue));
        }

        private void LoadDataUnlink(int? associationID = null)
        {
            if (associationID != null)
            {
                var data = from a in db.tbl_AssociationServiceArea
                           join b in db.Associations on a.AssociationID equals b.AssociationID
                           join c in db.tbl_RegionServiceArea on a.RegionSAreaID equals c.RegionSAreaID
                           join d in db.tbl_ServiceArea on c.ServiceAreaID equals d.ServiceAreaID
                           where a.AssociationID == associationID.Value
                           select new
                           {
                               a.AssociationServiceAreaID,
                               b.AssociationName,
                               d.ServiceAreaDescription
                           };
                gvServiceAreaUnlink.DataSource = data.ToList();
                gvServiceAreaUnlink.DataBind();
            }
        }

        protected void gvServiceAreaUnlink_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCheckedValuesUnlink();
            gvServiceAreaUnlink.PageIndex = e.NewPageIndex;
            //gvServiceAreaUnlink.DataBind();
            LoadDataUnlink(Convert.ToInt16(ddlAssociation.SelectedValue));
            PopulateCheckedValuesUnlink();
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