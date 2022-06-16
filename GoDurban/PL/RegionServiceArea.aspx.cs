using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class RegionServiceArea : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        private static int PageSize = 10;

        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        SqlCommand cmd;

        int select;

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
            lblRegion.Visible = false;
            lblCheck.Visible = false;
            lblCheckUnlink.Visible = false;

            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divsuccess.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if (!IsPostBack)
            {
                LoadRegion();
                LoadData();
                LoadDataUnlink(Convert.ToInt32(ddlRegionName.SelectedValue));
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

        //public void LoadRegion()
        //{
        //    RegionBL BL = new RegionBL();
        //    List<tbl_Region> list = BL.LoadRegion();
        //    ddlRegionName.DataSource = list;
        //    ddlRegionName.DataValueField = "RegionID";
        //    ddlRegionName.DataTextField = "RegionName";
        //    ddlRegionName.DataBind();
        //}
        public void LoadRegion()
        {
            var data = from a in db.Regions
                       join b in db.tbl_Status on a.StatusID equals b.StatusID
                       where b.StatusDescription == "Approved"
                       select new
                       {
                           a.RegionID,
                           a.RegionName
                       };
            ddlRegionName.DataSource = data.ToList();
            ddlRegionName.DataValueField = "RegionID";
            ddlRegionName.DataTextField = "RegionName";
            ddlRegionName.DataBind();
        }

        private bool ValidateRegion()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if ((ddlRegionName.SelectedValue == "0"))
            {
                ddlRegionName.Style.Add("border", "1px solid red");
                valid = false;
                lblRegion.Visible = true;
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Region!');", true);
            }
            else
            {
                ddlRegionName.Style.Add("border", "");
                lblRegion.Visible = false;
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

        protected void ddlRegionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRegionName.SelectedValue == "0")
            {
                accordion2.Style.Add("display", "inline");
                accordion3.Style.Add("display", "none");
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
                //lblRegion.Visible = true;
            }
            else
            {
                LoadData();
                LoadDataUnlink(Convert.ToInt32(ddlRegionName.SelectedValue));
                accordion2.Style.Add("display", "inline");
                accordion3.Style.Add("display", "inline");
                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
                //lblRegion.Visible = false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateCheckBox() && ValidateRegion())
                {
                    RegionServiceAreaBL BL = new RegionServiceAreaBL();
                    tbl_RegionServiceArea TBL = new tbl_RegionServiceArea();

                    CheckBox chkAll = (CheckBox)gvServiceArea.HeaderRow.Cells[1].FindControl("chkAll");

                    int a = gvServiceArea.PageIndex;
                    for (int i = 0; i < gvServiceArea.PageCount; i++)
                    {
                        gvServiceArea.SetPageIndex(i);
                        foreach (GridViewRow row in gvServiceArea.Rows)
                        {
                            CheckBox Chbox = (CheckBox)row.FindControl("chkRow");
                            if (Chbox.Checked == true)
                            {
                                var regionservicearea = db.tbl_RegionServiceArea.ToList().FindAll(x => x.RegionID == Convert.ToInt16(ddlRegionName.SelectedValue) && x.ServiceAreaID == Convert.ToInt32(gvServiceArea.DataKeys[row.RowIndex].Values[0]));

                                if (regionservicearea.Count > 0)
                                {
                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "inline");
                                }
                                else
                                {
                                    TBL.RegionID = Convert.ToInt16(ddlRegionName.SelectedValue);
                                    TBL.ServiceAreaID = Convert.ToInt32(gvServiceArea.DataKeys[row.RowIndex].Values[0]);
                                    BL.CreateRegionServiceArea(TBL);

                                    LoadDataUnlink(Convert.ToInt32(ddlRegionName.SelectedValue));
                                    accordion2.Style.Add("display", "inline");
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

        private void LoadData()
        {
            ServiceAreaBL BL = new ServiceAreaBL();
            List<tbl_ServiceArea> list = BL.LoadServiceArea();
            gvServiceArea.DataSource = list;
            gvServiceArea.DataBind();
        }

        protected void gvServiceArea_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCheckedValues();
            gvServiceArea.PageIndex = e.NewPageIndex;
            //gvServiceArea.DataBind();
            LoadData();
            PopulateCheckedValues();
        }

        //This method is used to populate the saved checkbox values
        private void PopulateCheckedValues()
        {
            ArrayList serviceareas = (ArrayList)Session["CHECKED_ITEMS"];
            if (serviceareas != null && serviceareas.Count > 0)
            {
                foreach (GridViewRow gvrow in gvServiceArea.Rows)
                {
                    int index = (int)gvServiceArea.DataKeys[gvrow.RowIndex].Value;
                    if (serviceareas.Contains(index))
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
            ArrayList serviceareas = new ArrayList();
            int index = -1;
            foreach (GridViewRow gvrow in gvServiceArea.Rows)
            {
                index = (int)gvServiceArea.DataKeys[gvrow.RowIndex].Value;
                bool result = ((CheckBox)gvrow.FindControl("chkRow")).Checked;

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    serviceareas = (ArrayList)Session["CHECKED_ITEMS"];
                if (result)
                {
                    if (!serviceareas.Contains(index))
                        serviceareas.Add(index);
                }
                else
                    serviceareas.Remove(index);
            }
            if (serviceareas != null && serviceareas.Count > 0)
                Session["CHECKED_ITEMS"] = serviceareas;
        }


        private void LoadDataUnlink(int? regionID = null)
        {
            if (regionID != null)
            {
                var data = from rsa in db.tbl_RegionServiceArea
                           join r in db.Regions on rsa.RegionID equals r.RegionID
                           join sa in db.tbl_ServiceArea on rsa.ServiceAreaID equals sa.ServiceAreaID
                           where rsa.RegionID == regionID.Value
                           select new
                           {
                               rsa.RegionSAreaID,
                               r.RegionName,
                               sa.ServiceAreaDescription
                           };
                gvServiceAreaUnlink.DataSource = data.ToList();
                gvServiceAreaUnlink.DataBind();
            }
        }

        protected void btnUnlink_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateCheckBoxUnlink())
                {
                    gvServiceAreaUnlink.AllowPaging = true;
                    foreach (GridViewRow row in gvServiceAreaUnlink.Rows)
                    {
                        if ((row.FindControl("chkRowUnlink") as CheckBox).Checked)
                        {
                            int regionSAreaID = Convert.ToInt32(gvServiceAreaUnlink.DataKeys[row.RowIndex].Value);

                            var regionservicearea = db.tbl_RegionServiceArea.FirstOrDefault(x => x.RegionSAreaID == regionSAreaID);

                            if (regionservicearea != null)
                            {
                                var associationserviceare = db.tbl_AssociationServiceArea.FirstOrDefault(x => x.RegionSAreaID == regionSAreaID);
                                if (associationserviceare != null)
                                {
                                    var ownerservicearea = db.tbl_OwnerServiceArea.FirstOrDefault(x => x.AssociationServiceAreaID == associationserviceare.AssociationServiceAreaID);
                                    if (ownerservicearea != null)
                                    {
                                        db.tbl_OwnerServiceArea.Remove(ownerservicearea);
                                        db.SaveChanges();
                                    }

                                    db.tbl_AssociationServiceArea.Remove(associationserviceare);
                                    db.SaveChanges();
                                }

                                db.tbl_RegionServiceArea.Remove(regionservicearea);
                                db.SaveChanges();

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
                    LoadDataUnlink(Convert.ToInt32(ddlRegionName.SelectedValue));
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
            LoadDataUnlink(Convert.ToInt32(ddlRegionName.SelectedValue));
        }

        protected void gvServiceAreaUnlink_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCheckedValuesUnlink();
            gvServiceAreaUnlink.PageIndex = e.NewPageIndex;
            //gvServiceAreaUnlink.DataBind();
            LoadDataUnlink(Convert.ToInt32(ddlRegionName.SelectedValue));
            PopulateCheckedValuesUnlink();
        }

        //This method is used to populate the saved checkbox values
        private void PopulateCheckedValuesUnlink()
        {
            ArrayList regionserviceareas = (ArrayList)Session["CHECKED_ITEMS"];
            if (regionserviceareas != null && regionserviceareas.Count > 0)
            {
                foreach (GridViewRow gvrow in gvServiceAreaUnlink.Rows)
                {
                    int index = (int)gvServiceAreaUnlink.DataKeys[gvrow.RowIndex].Value;
                    if (regionserviceareas.Contains(index))
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
            ArrayList regionserviceareas = new ArrayList();
            int index = -1;
            foreach (GridViewRow gvrow in gvServiceAreaUnlink.Rows)
            {
                index = (int)gvServiceAreaUnlink.DataKeys[gvrow.RowIndex].Value;
                bool result = ((CheckBox)gvrow.FindControl("chkRowUnlink")).Checked;

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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PL/RegionServiceArea.aspx");
        }


        protected void gvServiceArea_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            //if (gvServiceArea.Rows.Count > 0)
            //{
            //    //Replace the <td> with <th> and adds the scope attribute
            //    gvServiceArea.UseAccessibleHeader = true;

            //    //Adds the <thead> and <tbody> elements required for DataTables to work
            //    gvServiceArea.HeaderRow.TableSection = TableRowSection.TableHeader;

            //    //Adds the <tfoot> element required for DataTables to work
            //    gvServiceArea.FooterRow.TableSection = TableRowSection.TableFooter;
            //}

            if (ViewState["SelectAllLink"] != null)
            {
                foreach (GridViewRow row in gvServiceArea.Rows)
                {
                    CheckBox Chbox = (CheckBox)row.FindControl("chkRow");
                    Chbox.Checked = true;

                }
                ViewState["SelectAllLink"] = null;
            }
        }

        protected void gvServiceAreaUnlink_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview
            if (ddlRegionName.SelectedIndex == 0)
                LoadDataUnlink();
            else
                LoadDataUnlink(Convert.ToInt32(ddlRegionName.SelectedValue));

            if (ViewState["SelectAllUnlink"] != null)
            {
                foreach (GridViewRow row in gvServiceAreaUnlink.Rows)
                {
                    CheckBox Chbox = (CheckBox)row.FindControl("chkRowUnlink");
                    Chbox.Checked = true;

                }
                ViewState["SelectAllUnlink"] = null;
            }
            //if (gvServiceAreaUnlink.Rows.Count > 0)
            //{
            //    //Replace the <td> with <th> and adds the scope attribute
            //    gvServiceAreaUnlink.UseAccessibleHeader = true;

            //    //Adds the <thead> and <tbody> elements required for DataTables to work
            //    gvServiceAreaUnlink.HeaderRow.TableSection = TableRowSection.TableHeader;

            //    //Adds the <tfoot> element required for DataTables to work
            //    gvServiceAreaUnlink.FooterRow.TableSection = TableRowSection.TableFooter;
            //}
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            ViewState["SelectAllLink"] = true;
        }

        protected void btnSelectAllUnlink_Click(object sender, EventArgs e)
        {
            ViewState["SelectAllUnlink"] = true;
        }
    }
}