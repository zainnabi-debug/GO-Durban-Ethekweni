using GoDurban.Models;
using System.Linq;

namespace GoDurban.BL
{
    public class DriverBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateDriver(tbl_Driver model)
        {
            tbl_Driver TBL = new tbl_Driver();
            TBL.DriverID = TBL.DriverID;
            TBL.OwnerID = model.OwnerID;
            TBL.Name = model.Name;
            TBL.Surname = model.Surname;
            TBL.IDNo = model.IDNo;
            //TBL.CalcBirthDate = model.CalcBirthDate;
            TBL.CellNo = model.CellNo;
            TBL.OfficeNo = model.OfficeNo;
            TBL.Email = model.Email;
            TBL.RaceID = model.RaceID;
            TBL.GenderID = model.GenderID;
            TBL.AddressStreet = model.AddressStreet;
            TBL.AddressSuburb = model.AddressSuburb;
            TBL.AddressCity = model.AddressCity;
            TBL.AddressCode = model.AddressCode;
            //TBL.DriverLicenseScanID = model.DriverLicenseScanID;
            TBL.LicenseCodeID = model.LicenseCodeID;
            TBL.LicenseExpiry = model.LicenseExpiry;
            //TBL.PRDPScanID = model.PRDPScanID;
            TBL.PRDPCode = model.PRDPCode;
            TBL.PRDPExpiry = model.PRDPExpiry;
            TBL.EmploymentContract = model.EmploymentContract;
            TBL.EmploymentContractExpiry = model.EmploymentContractExpiry;
            //TBL.EmploymentContractScanID = model.EmploymentContractScanID;
            //TBL.RecommendationLetterScanID = model.RecommendationLetterScanID;
            //TBL.CalcDriverNO = model.CalcDriverNO;
            TBL.ReviewContract = model.ReviewContract;
            TBL.StatusID = model.StatusID;
            TBL.TermsAndConditions = model.TermsAndConditions;
            TBL.ReasonID = model.ReasonID;
            db.tbl_Driver.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateDriver(tbl_Driver model)
        {
            var TBL = db.tbl_Driver.ToList().FirstOrDefault(x=>x.DriverID==model.DriverID);
            TBL.OwnerID = model.OwnerID;
            TBL.Name = model.Name;
            TBL.Surname = model.Surname;
            TBL.IDNo = model.IDNo;
            //TBL.CalcBirthDate = model.CalcBirthDate;
            TBL.CellNo = model.CellNo;
            TBL.OfficeNo = model.OfficeNo;
            TBL.Email = model.Email;
            TBL.RaceID = model.RaceID;
            TBL.GenderID = model.GenderID;
            TBL.AddressStreet = model.AddressStreet;
            TBL.AddressSuburb = model.AddressSuburb;
            TBL.AddressCity = model.AddressCity;
            TBL.AddressCode = model.AddressCode;
            TBL.DriverLicenseScanID = model.DriverLicenseScanID;
            TBL.LicenseCodeID = model.LicenseCodeID;
            TBL.LicenseExpiry = model.LicenseExpiry;
            TBL.PRDPScanID = model.PRDPScanID;
            TBL.PRDPCode = model.PRDPCode;
            TBL.PRDPExpiry = model.PRDPExpiry;
            TBL.EmploymentContract = model.EmploymentContract;
            TBL.EmploymentContractExpiry = model.EmploymentContractExpiry;
            TBL.EmploymentContractScanID = model.EmploymentContractScanID;
            TBL.RecommendationLetterScanID = model.RecommendationLetterScanID;
            //TBL.CalcDriverNO = model.CalcDriverNO;
            TBL.ReviewContract = model.ReviewContract;
            TBL.StatusID = model.StatusID;
            TBL.TermsAndConditions = model.TermsAndConditions;
            TBL.ReasonID = model.ReasonID;
            db.SaveChanges();
        }
    }
}