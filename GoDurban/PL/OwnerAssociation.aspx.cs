using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class OwnerAssociation : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        string connStr = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;


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
                LoadData();
                LoadDataUnlink(Convert.ToInt32(ddlOwner.SelectedValue));
                //RemoveDuplicateItems(ddlOwner);
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



        //private void LoadData()
        //{
        //    AssociationBL BL = new AssociationBL();
        //    List<tbl_Association> list = BL.LoadAssociation();
        //    gvAssociation.DataSource = list;
        //    gvAssociation.DataBind();
        //}

        private void LoadData()
        {
            var data = from a in db.Associations
                       join b in db.tbl_Status on a.StatusID equals b.StatusID
                       where b.StatusDescription == "Approved"
                       select new
                       {
                           a.AssociationID,
                           a.AssociationName
                       };
            gvAssociation.DataSource = data.ToList();
            gvAssociation.DataBind();
        }

        private bool ValidateOwnerAssociation()
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
                // ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Owner!');", true);
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

            foreach (GridViewRow row in gvAssociation.Rows)
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

            foreach (GridViewRow row in gvAssociationUnlink.Rows)
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
                if (ValidateCheckBox() && ValidateOwnerAssociation())
                {
                    OwnerAssociationBL BL = new OwnerAssociationBL();
                    tbl_OwnerAssociation TBL = new tbl_OwnerAssociation();

                    CheckBox chkAll = (CheckBox)gvAssociation.HeaderRow.Cells[0].FindControl("chkAll");

                    int a = gvAssociation.PageIndex;
                    for (int i = 0; i < gvAssociation.PageCount; i++)
                    {
                        gvAssociation.SetPageIndex(i);

                        foreach (GridViewRow row in gvAssociation.Rows)
                        {
                            CheckBox Chbox = (CheckBox)row.FindControl("chkRow");
                            if (Chbox.Checked == true)
                            {
                                var ownerassociation = db.tbl_OwnerAssociation.ToList().FindAll(x => x.OwnerID == Convert.ToInt16(ddlOwner.SelectedValue) && x.AssociationID == Convert.ToInt32(gvAssociation.DataKeys[row.RowIndex].Values[0]));

                                if (ownerassociation.Count > 0)
                                {
                                    divsuccess.Style.Add("display", "none");
                                    divdanger.Style.Add("display", "none");
                                    divinfo.Style.Add("display", "none");
                                    divwarning.Style.Add("display", "inline");

                                    lblCheck.Visible = false;
                                    lblCheckUnlink.Visible = false;
                                }
                                else
                                {
                                    TBL.OwnerID = Convert.ToInt16(ddlOwner.SelectedValue);
                                    TBL.AssociationID = Convert.ToInt32(gvAssociation.DataKeys[row.RowIndex].Values[0]);
                                    BL.CreateOwnerAssociation(TBL);

                                    LoadDataUnlink(Convert.ToInt32(ddlOwner.SelectedValue));

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
                    gvAssociation.SetPageIndex(a);
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


        public void Clear()
        {
            ddlOwner.SelectedValue = "0";
        }


        private void LoadDataUnlink(int? ownerID = null)
        {
            if (ownerID != null)
            {
                var list = (from a in db.tbl_OwnerAssociation
                            join b in db.Associations on a.AssociationID equals b.AssociationID
                            join c in db.tbl_Owner on a.OwnerID equals c.OwnerID
                            where a.OwnerID == ownerID.Value
                            select new
                            {
                                a.OwnerAssociationID,
                                b.AssociationName,
                                Name = c.Name + " " + c.Surname + " - " + c.IDNo,
                            });
                gvAssociationUnlink.DataSource = list.ToList();
                gvAssociationUnlink.DataBind();
            }
        }

        protected void btnUnlink_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateCheckBoxUnlink())
                {
                    foreach (GridViewRow row in gvAssociationUnlink.Rows)
                    {
                        if ((row.FindControl("chkRowUnlink") as CheckBox).Checked)
                        {
                            int ownerAssociationID = Convert.ToInt32(gvAssociationUnlink.DataKeys[row.RowIndex].Value);

                            ViewState["OwnerAssociationID"] = ownerAssociationID;

                            var temp = db.tbl_OwnerAssociation.ToList().FirstOrDefault(x => x.OwnerAssociationID == Convert.ToInt16(ownerAssociationID));

                            db.tbl_OwnerAssociation.Remove(temp);
                            db.SaveChanges();

                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "inline");
                            divwarning.Style.Add("display", "none");
                            lblCheckUnlink.Visible = false;
                        }
                    }

                    CheckBox chkAll = (CheckBox)gvAssociation.HeaderRow.Cells[0].FindControl("chkAll");

                    foreach (GridViewRow row in gvAssociation.Rows)
                    {
                        CheckBox Chbox = (CheckBox)row.FindControl("chkRow");
                        Chbox.Checked = false;
                        chkAll.Checked = false;
                    }

                    gvAssociationUnlink.AllowPaging = true;
                    gvAssociationUnlink.DataBind();
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


        protected void RemoveBorder()
        {
            ddlOwner.Style.Add("border", "");
        }

        protected void gvAssociation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCheckedValues();
            gvAssociation.PageIndex = e.NewPageIndex;
            //gvAssociation.DataBind();
            LoadData();
            PopulateCheckedValues();
        }

        private void PopulateCheckedValues()
        {
            ArrayList association = (ArrayList)Session["CHECKED_ITEMS"];
            if (association != null && association.Count > 0)
            {
                foreach (GridViewRow gvrow in gvAssociation.Rows)
                {
                    int index = (int)gvAssociation.DataKeys[gvrow.RowIndex].Value;
                    if (association.Contains(index))
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
            ArrayList association = new ArrayList();
            int index = -1;
            foreach (GridViewRow gvrow in gvAssociation.Rows)
            {
                index = (int)gvAssociation.DataKeys[gvrow.RowIndex].Value;
                bool result = ((CheckBox)gvrow.FindControl("chkRow")).Checked;

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    association = (ArrayList)Session["CHECKED_ITEMS"];
                if (result)
                {
                    if (!association.Contains(index))
                        association.Add(index);
                }
                else
                    association.Remove(index);
            }
            if (association != null && association.Count > 0)
                Session["CHECKED_ITEMS"] = association;
        }


        protected void gvAssociationUnlink_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCheckedValuesUnlink();
            gvAssociationUnlink.PageIndex = e.NewPageIndex;
            //gvAssociationUnlink.DataBind();
            LoadDataUnlink(Convert.ToInt16(ddlOwner.SelectedValue));
            PopulateCheckedValuesUnlink();
        }


        private void PopulateCheckedValuesUnlink()
        {
            ArrayList ownerassociation = (ArrayList)Session["CHECKED_ITEMS"];
            if (ownerassociation != null && ownerassociation.Count > 0)
            {
                foreach (GridViewRow gvrow in gvAssociationUnlink.Rows)
                {
                    int index = (int)gvAssociationUnlink.DataKeys[gvrow.RowIndex].Value;
                    if (ownerassociation.Contains(index))
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
            ArrayList ownerassociation = new ArrayList();
            int index = -1;
            foreach (GridViewRow gvrow in gvAssociationUnlink.Rows)
            {
                index = (int)gvAssociationUnlink.DataKeys[gvrow.RowIndex].Value;
                bool result = ((CheckBox)gvrow.FindControl("chkRowUnlink")).Checked;

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    ownerassociation = (ArrayList)Session["CHECKED_ITEMS"];
                if (result)
                {
                    if (!ownerassociation.Contains(index))
                        ownerassociation.Add(index);
                }
                else
                    ownerassociation.Remove(index);
            }
            if (ownerassociation != null && ownerassociation.Count > 0)
                Session["CHECKED_ITEMS"] = ownerassociation;
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
                //lblOwner.Text = "Required";
            }
            else
            {
                LoadData();
                LoadDataUnlink(Convert.ToInt32(ddlOwner.SelectedValue));
                accordion2.Style.Add("display", "inline");
                accordion3.Style.Add("display", "inline");

                divsuccess.Style.Add("display", "none");
                divdanger.Style.Add("display", "none");
                divinfo.Style.Add("display", "none");
                divwarning.Style.Add("display", "none");
                //lblOwner.Text = "";
            }
        }
        protected void gvAssociation_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            //LoadData();


            if (gvAssociation.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvAssociation.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvAssociation.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvAssociation.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvAssociationUnlink_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            //LoadDataUnlink();

            if (gvAssociationUnlink.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvAssociationUnlink.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvAssociationUnlink.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvAssociationUnlink.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}