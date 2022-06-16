using GoDurban.BL;
using GoDurban.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class AssociationRegion : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                LoadRegion();
                LoadAssociation();
                //RemoveDuplicateItems(ddlAssociation);
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
        //public void LoadRegion()
        //{
        //    RegionBL BL = new RegionBL();
        //    List<tbl_Region> list = BL.LoadRegion();
        //    ddlRegion.DataSource = list.ToList();
        //    ddlRegion.DataValueField = "RegionID";
        //    ddlRegion.DataTextField = "RegionName";
        //    ddlRegion.DataBind();
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
            ddlRegion.DataSource = data.ToList();
            ddlRegion.DataValueField = "RegionID";
            ddlRegion.DataTextField = "RegionName";
            ddlRegion.DataBind();
        }

        //private void LoadAssociation()
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

        private bool ValidateAssociationRegion()
        {
            bool valid = true;

            divsuccess.Style.Add("display", "none");
            divdanger.Style.Add("display", "none");
            divinfo.Style.Add("display", "none");
            divwarning.Style.Add("display", "none");

            if ((ddlAssociation.SelectedValue == "0") && (ddlRegion.SelectedValue == "0"))
            {
                ddlAssociation.Style.Add("border", "1px solid red");
                ddlRegion.Style.Add("border", "1px solid red");
                valid = false;

                lblAssociation.Text = "Select Association and Region";
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Association and Region!');", true);
                divwarning.Style.Add("display", "none");
            }
            else if ((ddlAssociation.SelectedValue == "0") && (ddlRegion.SelectedValue != "0"))
            {
                ddlAssociation.Style.Add("border", "1px solid red");
                ddlRegion.Style.Add("border", "1px solid red");
                valid = false;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Association and Region!');", true);
            }
            else if ((ddlAssociation.SelectedValue != "0") && (ddlRegion.SelectedValue == "0"))
            {
                ddlAssociation.Style.Add("border", "1px solid red");
                ddlRegion.Style.Add("border", "1px solid red");
                valid = false;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Please Select Association and Region!');", true);
                divwarning.Style.Add("display", "none");
            }
            else
            {
                ddlAssociation.Style.Add("border", "");
                ddlRegion.Style.Add("border", "");
            }
            return valid;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateAssociationRegion())
                {
                    // creating an object for business logic

                    AssociationRegionBL BL = new AssociationRegionBL();
                    tbl_AssociationRegion TBL = new tbl_AssociationRegion();

                    //Check button text to add or update
                    string temp = btnAdd.Text;

                    if (temp.Contains("Link"))
                    {
                        var association = db.tbl_AssociationRegion.ToList().FindAll(x => String.Compare(x.AssociationID.ToString(), (string)ddlAssociation.SelectedValue, StringComparison.OrdinalIgnoreCase) == 0);
                        var region = db.tbl_AssociationRegion.ToList().FindAll(x => String.Compare(x.RegionID.ToString(), (string)ddlRegion.SelectedValue, StringComparison.OrdinalIgnoreCase) == 0);

                        if (association.Count > 0  || region.Count < 0)
                        {
                            divsuccess.Style.Add("display", "none");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "inline");
                        }
                        else
                        {
                            TBL.RegionID = Convert.ToInt16(ddlRegion.SelectedValue);
                            TBL.AssociationID = Convert.ToInt16(ddlAssociation.SelectedValue);
                            BL.CreateAssociationRegion(TBL);

                            divsuccess.Style.Add("display", "inline");
                            divdanger.Style.Add("display", "none");
                            divinfo.Style.Add("display", "none");
                            divwarning.Style.Add("display", "none");
                        }
                    }
                    else if (temp.Contains("Update"))
                    {
                        int Id = int.Parse(ViewState["AssociationRegionID"].ToString());
                        TBL.AssociationRegionID = Id;
                        TBL.RegionID = Convert.ToInt16(ddlRegion.SelectedValue);
                        TBL.AssociationID = Convert.ToInt16(ddlAssociation.SelectedValue);
                        BL.UpdateAssociationRegion(TBL);

                        divsuccess.Style.Add("display", "none");
                        divdanger.Style.Add("display", "none");
                        divinfo.Style.Add("display", "inline");
                        divwarning.Style.Add("display", "none");

                        btnAdd.Text = "Link";
                    }

                    //Refresh Grid
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
        public void Clear()
        {
            ddlAssociation.SelectedValue = "0";
            ddlRegion.SelectedValue = "0";
        }
        private void LoadData()
        {
            var list = (from a in db.tbl_AssociationRegion
                        join b in db.Associations on a.AssociationID equals b.AssociationID
                        join c in db.Regions on a.RegionID equals c.RegionID
                        join d in db.tbl_Status on b.StatusID equals d.StatusID 
                        where (((a.AssociationID==b.AssociationID) && b.StatusID == 15)&& ((a.RegionID == c.RegionID) && c.StatusID == 15))
                        select new
                        {
                            a.AssociationRegionID,
                            b.AssociationName,
                            c.RegionName
                        });
            gvAssociationRegion.DataSource = list.ToList().OrderByDescending(x => x.AssociationRegionID);
            gvAssociationRegion.DataBind();
        }

        protected void gvAssociationRegion_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadData();

            if (gvAssociationRegion.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvAssociationRegion.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvAssociationRegion.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvAssociationRegion.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvAssociationRegion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AssociationRegionBL BL = new AssociationRegionBL();
            tbl_AssociationRegion TBL = new tbl_AssociationRegion();

            string commandName = e.CommandName;

            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button 
            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row 
            GridView myGrid = (GridView)sender; // the gridview 
            string Id = gvAssociationRegion.DataKeys[myRow.RowIndex].Value.ToString();

            //if (commandName == "EditItem")
            //{
            //    //Accessing BoundField Column
            //    string Source = gvAssociationRegion.Rows[myRow.RowIndex].Cells[0].Text;

            //    ViewState["AssociationRegionID"] = Id;
            //    //int Id = Convert.ToInt32(e.CommandArgument);

            //    var temp = db.tbl_AssociationRegion.ToList().FirstOrDefault(x => x.AssociationRegionID == Convert.ToInt16(Id));

            //    if (temp != null)
            //    {
            //        RemoveBorder();
            //        Session["AssociationRegionID"] = Id;
            //        ddlRegion.SelectedValue = temp.RegionID.ToString();
            //        ddlAssociation.SelectedValue = temp.AssociationID.ToString();
            //        btnAdd.Text = "Update";

            //    }
            //}

            if (commandName == "DeleteItem")
            {
                try
                {
                    string Source = gvAssociationRegion.Rows[myRow.RowIndex].Cells[0].Text;

                    ViewState["AssociationRegionID"] = Id;

                    var temp = db.tbl_AssociationRegion.ToList().FirstOrDefault(x => x.AssociationRegionID == Convert.ToInt16(Id));

                    db.tbl_AssociationRegion.Remove(temp);
                    db.SaveChanges();
                    RemoveBorder();
                    LoadData();
                    btnAdd.Text = "Link";

                    divsuccess.Style.Add("display", "none");
                    divdanger.Style.Add("display", "none");
                    divinfo.Style.Add("display", "inline");
                    divwarning.Style.Add("display", "none");
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
        protected void RemoveBorder()
        {
            ddlAssociation.Style.Add("border", "");
            ddlRegion.Style.Add("border", "");
        }
        
    }
}