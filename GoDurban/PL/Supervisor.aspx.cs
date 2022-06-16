using GoDurban.Models;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Supervisor : System.Web.UI.Page
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

                if (type != "Supervisor")
                {
                    Response.Redirect("~/Error.aspx");
                }
            }
            if (!IsPostBack)
            {
                LoadDataRegion();
                LoadDataAssociation();
                LoadDataOwner();
                LoadDataDriver();
                LoadDataVehicle();
                LoadRegionalleadership();
                LoadAssociationLeadership();
            }

            divRegion1.Visible = gvRegion.Rows.Count > 0;
            divAssociation1.Visible = gvAssociation.Rows.Count > 0;
            divOwner1.Visible = gvOwner.Rows.Count > 0;
            divDriver.Visible = gvDriver.Rows.Count > 0;
            divVehicle.Visible = gvVehicle.Rows.Count > 0;
            divRegionalLeader.Visible = gvRegionalLeader.Rows.Count > 0;
            divAssociationLeadership.Visible = gvAssociationLeadership.Rows.Count > 0;
        }

        private void LoadDataRegion()
        {
            var data = (from a in db.Regions
                        join b in db.tbl_Status on a.StatusID equals b.StatusID
                        join c in db.Accounts on a.AccountID equals c.AccountID
                        join d in db.Banks on c.BankID equals d.BankID
                        where b.StatusDescription == "Pending"
                        select new
                        {
                            a.RegionID,
                            a.RegionNo,
                            a.RegionName,
                            d.BankName,
                            d.BranchCode,
                            c.AccountNo,
                            b.StatusDescription
                        });
            gvRegion.DataSource = data.ToList().OrderByDescending(x => x.RegionID);
            gvRegion.DataBind();
        }

        private void LoadDataAssociation()
        {
            var data = (from a in db.Associations
                        join b in db.tbl_Status on a.StatusID equals b.StatusID
                        join c in db.Accounts on a.AccountID equals c.AccountID
                        join d in db.Banks on c.BankID equals d.BankID
                        where b.StatusDescription == "Pending"
                        select new
                        {
                            a.AssociationID,
                            a.AssociationNo,
                            a.AssociationName,
                            d.BankName,
                            d.BranchCode,
                            c.AccountNo,
                            b.StatusDescription
                        });
            gvAssociation.DataSource = data.ToList().OrderByDescending(x => x.AssociationID);
            gvAssociation.DataBind();
        }

        private void LoadDataOwner()
        {
            var list = (from a in db.tbl_Owner
                        join b in db.tbl_Gender on a.GenderID equals b.GenderID
                        join d in db.tbl_Race on a.RaceID equals d.RaceID
                        join e in db.tbl_Status on a.StatusID equals e.StatusID
                        join f in db.Accounts on a.AccountID equals f.AccountID
                        join c in db.Banks on f.BankID equals c.BankID
                        //join f in db.tbl_Reason on a.ReasonID equals f.ReasonID
                        where e.StatusDescription == "Pending"
                        select new
                        {
                            a.OwnerID,
                            a.AddressCity,
                            a.AddressCode,
                            a.AddressStreet,
                            a.AddressSuburb,
                            a.CalcBirthDate,
                            a.CellNo,
                            a.Email,
                            b.GenderDescription,
                            a.IDNo,
                            a.Name,
                            a.OfficeNo,
                            d.RaceDescription,
                            a.Surname,
                            a.CompanyName,
                            e.StatusDescription,
                            //f.ReasonDescription,
                            c.BranchCode,
                            c.BankName,
                            f.AccountID,
                            f.AccountNo
                        });
            gvOwner.DataSource = list.ToList().OrderByDescending(x => x.OwnerID);
            gvOwner.DataBind();
        }
        private void LoadDataDriver()
        {
            var list = (from a in db.tbl_Driver
                        join b in db.tbl_Gender on a.GenderID equals b.GenderID
                        join c in db.tbl_Owner on a.OwnerID equals c.OwnerID
                        join d in db.tbl_Race on a.RaceID equals d.RaceID
                        join e in db.tbl_Status on a.StatusID equals e.StatusID
                        //join f in db.tbl_Reason on a.ReasonID equals f.ReasonID
                        where (c.StatusID == 15 && e.StatusDescription == "Pending")
                        select new
                        {
                            a.DriverID,
                            a.OwnerID,
                            a.AddressCity,
                            a.AddressCode,
                            a.AddressStreet,
                            a.AddressSuburb,
                            a.CalcDriverNO,
                            a.CellNo,
                            a.Email,
                            b.GenderDescription,
                            a.IDNo,
                            a.Name,
                            a.OfficeNo,
                            d.RaceDescription,
                            a.Surname,
                            OwnerName = c.Name + " " + c.Surname,
                            e.StatusDescription,
                            //f.ReasonDescription
                        });
            gvDriver.DataSource = list.ToList().OrderByDescending(x => x.DriverID);
            gvDriver.DataBind();
        }

        public void LoadDataVehicle()
        {
            var list = (from a in db.tbl_Vehicle
                        join b in db.tbl_Owner on a.OwnerID equals b.OwnerID
                        join d in db.tbl_VehicleInfo on a.VehicleInfoID equals d.VehicleInfoID
                        join e in db.tbl_Status on a.StatusID equals e.StatusID
                        //join f in db.tbl_Reason on a.ReasonID equals f.ReasonID
                        where (b.StatusID == 15 && e.StatusDescription == "Pending")
                        select new
                        {
                            a.VehicleID,
                            a.CORExpiry,
                            a.CORScanID,
                            a.EngineNumber,
                            a.LastServiced,
                            a.NumberPlate,
                            a.OperatingLicenseExpiry,                           
                            b.IDNo,
                            a.VehicleAccreditation,
                            a.VINNumber,
                            a.OperatingLicense,
                            d.Make,
                            d.Model,
                            a.Capacity,
                            a.Year,
                            a.YearRegistered,
                            e.StatusDescription,
                            NameSurname = b.Name + " " + b.Surname
                        });
            gvVehicle.DataSource = list.ToList().OrderByDescending(x => x.VehicleID);
            gvVehicle.DataBind();
        }

        protected void gvRegion_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadDataRegion();

            if (gvRegion.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvRegion.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvRegion.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvRegion.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvAssociation_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadDataAssociation();

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

        protected void gvOwner_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadDataOwner();

            if (gvOwner.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvOwner.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvOwner.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvOwner.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvDriver_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadDataOwner();

            if (gvDriver.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvDriver.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvDriver.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvDriver.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvVehicle_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadDataVehicle();

            if (gvVehicle.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvVehicle.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvVehicle.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvVehicle.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvRegion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            if (commandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvRegion.Rows[index];
                Response.Redirect("~/PL/ApproveRegion.aspx?RegionID=" + row.Cells[0].Text);
            }
        }

        protected void gvAssociation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            if (commandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvAssociation.Rows[index];
                Response.Redirect("~/PL/ApproveAssociation.aspx?AssociationID=" + row.Cells[0].Text);
            }
        }


        protected void gvDriver_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            if (commandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvDriver.Rows[index];
                Response.Redirect("~/PL/ApproveDriver.aspx?DriverID=" + row.Cells[0].Text);
            }
        }

        protected void gvVehicle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            if (commandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvVehicle.Rows[index];
                Response.Redirect("~/PL/ApproveVehicle.aspx?VehicleID=" + row.Cells[0].Text);
            }
        }

        protected void gvOwner_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            if (commandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvOwner.Rows[index];
                Response.Redirect("~/PL/ApproveOwner.aspx?OwnerID=" + row.Cells[1].Text);
            }
        }

        protected void gvOwner_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gvBank = (GridView)e.Row.FindControl("gvBank");
                int id = Convert.ToInt32(e.Row.Cells[1].Text);

                var list = (from a in db.Accounts
                            join b in db.tbl_Owner on a.AccountID equals b.AccountID
                            join c in db.Banks on a.BankID equals c.BankID
                            where (b.OwnerID == id)
                            select new
                            {
                                a.AccountID,
                                a.AccountNo,
                                c.BankName,
                                c.BranchCode,
                                b.OwnerID
                            });
                gvBank.DataSource = list.ToList();
                gvBank.DataBind();
            }
        }

        private void LoadRegionalleadership()
        {
            var list = (from a in db.tbl_RegionalLeadership
                        join b in db.tbl_Gender on a.GenderID equals b.GenderID
                        join c in db.Regions on a.RegionID equals c.RegionID
                        join d in db.tbl_Race on a.RaceID equals d.RaceID
                        join e in db.tbl_Status on a.StatusID equals e.StatusID

                        where (e.StatusDescription == "Pending")

                        select new
                        {
                            a.RegionalLeaderID,
                            a.AddressCity,
                            a.AddressCode,
                            a.AddressStreet,
                            a.AddressSuburb,
                            a.CalcBirthDate,
                            a.CellNo,
                            a.Email,
                            b.GenderDescription,
                            a.IDNo,
                            a.Passport,
                            a.Name,
                            a.OfficeNo,
                            d.RaceDescription,
                            c.RegionName,
                            a.Surname,
                            e.StatusDescription,
                            //f.ReasonDescription

                        });
            gvRegionalLeader.DataSource = list.ToList().OrderByDescending(x => x.RegionalLeaderID);
            gvRegionalLeader.DataBind();
        }

        protected void gvRegionalLeader_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadRegionalleadership();

            if (gvRegionalLeader.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvRegionalLeader.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvRegionalLeader.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvRegionalLeader.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        protected void gvRegionalLeader_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            if (commandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvRegionalLeader.Rows[index];
                Response.Redirect("~/PL/ApproveRegionalLeadership.aspx?RegionalLeaderID=" + row.Cells[0].Text);
            }
        }

        private void LoadAssociationLeadership()
        {
            var list = (from a in db.tbl_AssociationLeadership
                        join b in db.tbl_Gender on a.GenderID equals b.GenderID
                        join c in db.Associations on a.AssociationID equals c.AssociationID
                        join d in db.tbl_Race on a.RaceID equals d.RaceID
                        join e in db.tbl_Status on a.StatusID equals e.StatusID

                        where (e.StatusDescription == "Pending")

                        select new
                        {
                            a.AssociationLeaderID,
                            a.AddressCity,
                            a.AddressCode,
                            a.AddressStreet,
                            a.AddressSuburb,
                            a.CalcBirthDate,
                            a.CellNo,
                            a.Email,
                            b.GenderDescription,
                            a.IDNo,
                            a.Passport,
                            a.Name,
                            a.OfficeNo,
                            d.RaceDescription,
                            a.Surname,
                            c.AssociationName,
                            e.StatusDescription,
                            //f.ReasonDescription
                        });
            gvAssociationLeadership.DataSource = list.ToList().OrderByDescending(x => x.AssociationLeaderID);
            gvAssociationLeadership.DataBind();
        }

        protected void gvAssociationLeadership_PreRender(object sender, EventArgs e)
        {
            //calling the Load method to populate the gridview 
            LoadAssociationLeadership();

            if (gvAssociationLeadership.Rows.Count > 0)
            {
                //Replace the <td> with <th> and adds the scope attribute
                gvAssociationLeadership.UseAccessibleHeader = true;

                //Adds the <thead> and <tbody> elements required for DataTables to work
                gvAssociationLeadership.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Adds the <tfoot> element required for DataTables to work
                gvAssociationLeadership.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvAssociationLeadership_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            if (commandName == "Select")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvAssociationLeadership.Rows[index];
                Response.Redirect("~/PL/ApproveAssociationLeadership.aspx?AssociationLeaderID=" + row.Cells[0].Text);
            }
        }
    }
}