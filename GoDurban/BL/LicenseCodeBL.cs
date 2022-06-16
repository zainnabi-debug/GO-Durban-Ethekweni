using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class LicenseCodeBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        public void CreateLicenseCode(tbl_LicenseCode model)
        {
            tbl_LicenseCode TBL = new tbl_LicenseCode();
            TBL.LicenseCode = model.LicenseCode;
            TBL.LicenseCodeDescription = model.LicenseCodeDescription;
            TBL.LicenseCodeID = TBL.LicenseCodeID;
            db.tbl_LicenseCode.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateLicenseCode(tbl_LicenseCode model)
        {
            var TBL = db.tbl_LicenseCode.ToList().FirstOrDefault(x=>x.LicenseCodeID==model.LicenseCodeID);
            TBL.LicenseCode = model.LicenseCode;
            TBL.LicenseCodeDescription = model.LicenseCodeDescription;
            db.SaveChanges();
        }
        public List<tbl_LicenseCode> LoadLicenseCode()
        {
            return db.tbl_LicenseCode.ToList();
        }
    }
}