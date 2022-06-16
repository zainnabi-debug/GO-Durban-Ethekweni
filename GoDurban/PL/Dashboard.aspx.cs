using GoDurban.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace GoDurban.PL
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                Response.Redirect("~/Index.aspx?ShowDialog=yes");
            }
                        
            //if ((LoadServiceAreas()) != 0)
            //{
            //    lblServiceAreas.Text = Convert.ToString(LoadServiceAreas());
            //}
            //else
            //{
            //    lblServiceAreas.Text = "0";
            //}
                       
            //if ((LoadRegions()) != 0)
            //{
            //    lblRegions.Text = Convert.ToString(LoadRegions());
            //}
            //else
            //{
            //    lblRegions.Text = "0";
            //}
            
            //if ((LoadAssociations()) != 0)
            //{
            //    lblAssociations.Text = Convert.ToString(LoadAssociations());
            //}
            //else
            //{
            //    lblAssociations.Text = "0";
            //}

            //// load offences
            //if ((LoadOffenses()) != 0)
            //{
            //    lblOffenses.Text = Convert.ToString(LoadOffenses());
            //}
            //else
            //{
            //    lblOffenses.Text = "0";
            //}
        }

        public int LoadServiceAreas()
        {
            int servicearea = db.tbl_ServiceArea.ToList().Count();
            return servicearea;
        }
        public int LoadRegions()
        {
            int region = db.Regions.ToList().Count();
            return region;
        }

        public int LoadAssociations()
        {
            int status = db.Associations.ToList().Count();
            return status;
        }

        //public int LoadOffenses()
        //{
        //    int status = db.tbl_Driver.ToList().Where(x => x.OffenseListID != null).Count();
        //    return status;
        //}


    }
}