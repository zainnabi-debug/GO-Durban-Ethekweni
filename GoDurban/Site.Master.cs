using GoDurban.Models;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace GoDurban
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            lblName.Text = "Welcome" + " " + Session["Name"] + " " + Session["Surname"];

            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session["UserRole"].ToString() == "Supervisor"))
            {
                lstPayment1.Visible = true;
                lstSupervisor1.Visible = true;
                lstAdmin1.Visible = false;
                lstRegion1.Visible = false;
                lstAssociation1.Visible = false;
                lstOwner1.Visible = false;
                lstDriver1.Visible = false;
                ddlReports.Visible = true;

            }
            else if ((Session["UserRole"].ToString() == "Admin"))
            {
                ddlReports.Visible = true;
                lstSupervisor1.Visible = false;
                lstPayment1.Visible = true;
                lstAdmin1.Visible = true;
                lstRegion1.Visible = true;
                lstAssociation1.Visible = true;
                lstOwner1.Visible = true;
                lstDriver1.Visible = true;

                lstServiceArea.Visible = true;
                lstGender.Visible = true;
                lstLicenseCode.Visible = true;
                lstRace.Visible = true;
                lstRole.Visible = false;
                lstStatus.Visible = true;
                lstVehicleInfo.Visible = true;
                lstReason.Visible = true;
                lstUsers.Visible = true;
                lstAuditTrail.Visible = true;

                lstRegion.Visible = true;
                lstRegionL.Visible = true;
                lstRegionSA.Visible = true;

                lstAssociation.Visible = true;
                lstAssociationL.Visible = true;
                lstAssociationR.Visible = true;
                lstAssociationSA.Visible = true;

                lstOwner.Visible = true;
                lstOwnerA.Visible = true;
                lstOwnerSA.Visible = true;

                lstVehicle.Visible = true;

                lstDriver.Visible = true;
            }
            else if (Session["UserRole"].ToString() == "User")
            {
                lstSupervisor1.Visible = false;

                using (GoDurbanEntities db = new GoDurbanEntities())
                {
                    tbl_UserPage up = new tbl_UserPage();

                    up.Association = Convert.ToBoolean(Session["Association"]);
                    up.AssociationL = Convert.ToBoolean(Session["AssociationL"]);
                    up.AssociationR = Convert.ToBoolean(Session["AssociationR"]);
                    up.AssociationSA = Convert.ToBoolean(Session["AssociationSA"]);

                    up.Owner = Convert.ToBoolean(Session["Owner"]);
                    up.OwnerA = Convert.ToBoolean(Session["OwnerA"]);
                    up.OwnerSA = Convert.ToBoolean(Session["OwnerSA"]);
                    up.Vehicle = Convert.ToBoolean(Session["Vehicle"]);

                    up.Region = Convert.ToBoolean(Session["Region"]);
                    up.RegionL = Convert.ToBoolean(Session["RegionL"]);
                    up.RegionSA = Convert.ToBoolean(Session["RegionSA"]);

                    up.Driver = Convert.ToBoolean(Session["Driver"]);
                   
                    if (up.Association == true || up.AssociationL == true || up.AssociationR == true || up.AssociationSA == true)
                    {
                        lstAssociation1.Visible = true;

                         if(up.Association == true)
                        {
                            lstAssociation.Visible = true;
                        }
                         else
                        {
                            lstAssociation.Visible = false;
                        }

                        if (up.AssociationL == true)
                        {
                            lstAssociationL.Visible = true;
                        }
                        else
                        {
                            lstAssociationL.Visible = false;
                        }

                        if (up.AssociationR == true)
                        {
                            lstAssociationR.Visible = true;
                        }
                        else
                        {
                            lstAssociationR.Visible = false;
                        }

                        if (up.AssociationSA == true)
                        {
                            lstAssociationSA.Visible = true;
                        }
                        else
                        {
                            lstAssociationSA.Visible = false;
                        }
                    }
                    else
                    {
                        lstAssociation1.Visible = false;
                    }

                    if (up.Owner == true || up.OwnerA == true || up.OwnerSA == true || up.Vehicle == true)
                    {
                        lstOwner1.Visible = true;

                        if (up.Owner == true)
                        {
                            lstOwner.Visible = true;
                        }
                        else
                        {
                            lstOwner.Visible = false;
                        }

                        if (up.OwnerA == true)
                        {
                            lstOwnerA.Visible = true;
                        }
                        else
                        {
                            lstOwnerA.Visible = false;
                        }

                        if (up.OwnerSA == true)
                        {
                            lstOwnerSA.Visible = true;
                        }
                        else
                        {
                            lstOwnerSA.Visible = false;
                        }

                        if (up.Vehicle == true)
                        {
                            lstVehicle.Visible = true;
                        }
                        else
                        {
                            lstVehicle.Visible = false;
                        }
                    }
                    else
                    {
                        lstOwner1.Visible = false;
                    }
                    
                    if (up.Region == true || up.RegionL == true || up.RegionSA == true)
                    {
                        lstRegion1.Visible = true;

                        if (up.Region == true)
                        {
                            lstRegion.Visible = true;
                        }
                        else
                        {
                            lstRegion.Visible = false;
                        }

                        if (up.RegionL == true)
                        {
                            lstRegionL.Visible = true;
                        }
                        else
                        {
                            lstRegionL.Visible = false;
                        }

                        if (up.RegionSA == true)
                        {
                            lstRegionSA.Visible = true;
                        }
                        else
                        {
                            lstRegionSA.Visible = false;
                        }
                    }
                    else
                    {
                        lstRegion1.Visible = false;
                    }

                    if (up.Driver == true)
                    {
                        lstDriver1.Visible = true;

                        if (up.Driver == true)
                        {
                            lstDriver.Visible = true;
                        }
                        else
                        {
                            lstDriver.Visible = false;
                        }
                    }
                    else
                    {
                        lstDriver1.Visible = false;
                    }                    
                }
            }
            else
            {
                lstPayment1.Visible = false;
                lstSupervisor1.Visible = false;
                lstAdmin1.Visible = false;
                lstRegion1.Visible = false;
                lstAssociation1.Visible = false;
                lstOwner1.Visible = false;
                lstDriver1.Visible = false;
            }
        }

        protected void Index_ServerClick(object sender, System.EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Index.aspx");
        }
    }
}