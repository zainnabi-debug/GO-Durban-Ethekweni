using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class OperatingLicenseBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateOperatingLicense(tbl_OperatingLicense model)
        {
            tbl_OperatingLicense TBL = new tbl_OperatingLicense();
            TBL.OperatingLicenseID = TBL.OperatingLicenseID;
            TBL.Description = model.Description;
            TBL.OLCode = model.OLCode;
            TBL.RouteCode = model.RouteCode;            
            db.tbl_OperatingLicense.Add(TBL);
            db.SaveChanges();
        }

        public void UpdateOperatingLicense(tbl_OperatingLicense model)
        {
            var TBL = db.tbl_OperatingLicense.ToList().FirstOrDefault(x=>x.OperatingLicenseID==model.OperatingLicenseID);
            TBL.Description = model.Description;
            TBL.OLCode = model.OLCode;
            TBL.RouteCode = model.RouteCode;
            db.SaveChanges();
        }

        public List<tbl_OperatingLicense> LoadOperatingLicense()
        {
            return db.tbl_OperatingLicense.ToList();
        }

    }
}