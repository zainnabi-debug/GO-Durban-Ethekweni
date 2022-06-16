using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class RegionalLeadershipBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        //public bool CreateRegionalLeadership(tbl_RegionalLeadership model)
        //{
        //    bool valid = false;
        //    tbl_RegionalLeadership TBL = new tbl_RegionalLeadership();
        //    TBL.AddressCity = model.AddressCity;
        //    TBL.AddressCode = model.AddressCode;
        //    TBL.AddressStreet = model.AddressStreet;
        //    TBL.AddressSuburb = model.AddressSuburb;
        //    TBL.CalcBirthDate = model.CalcBirthDate;
        //    TBL.CellNo = model.CellNo;
        //    TBL.Email = model.Email;
        //    TBL.GenderID = model.GenderID;
        //    TBL.IDNo = model.IDNo;
        //    TBL.Name = model.Name;
        //    TBL.OfficeNo = model.OfficeNo;
        //    TBL.RaceID = model.RaceID;
        //    TBL.RegionalLeaderID = TBL.RegionalLeaderID;
        //    TBL.RegionID = model.RegionID;
        //    TBL.Surname = model.Surname;
        //    TBL.ReviewContract = model.ReviewContract;
        //    TBL.StatusID = model.StatusID;
        //    TBL.TermsAndConditions = model.TermsAndConditions;
        //    TBL.ReasonID = model.ReasonID;
        //    db.tbl_RegionalLeadership.Add(TBL);
        //    db.SaveChanges();
        //    valid = true;

        //    return valid;
        //}

        public void CreateRegionalLeadership(tbl_RegionalLeadership model)
        {
            tbl_RegionalLeadership TBL = new tbl_RegionalLeadership();
            TBL.AddressCity = model.AddressCity;
            TBL.AddressCode = model.AddressCode;
            TBL.AddressStreet = model.AddressStreet;
            TBL.AddressSuburb = model.AddressSuburb;
            //TBL.CalcBirthDate = model.CalcBirthDate;
            TBL.CellNo = model.CellNo;
            TBL.Email = model.Email;
            TBL.GenderID = model.GenderID;
            TBL.IDNo = model.IDNo;
            TBL.Passport = model.Passport;
            TBL.IsPassport = model.IsPassport;
            TBL.Name = model.Name;
            TBL.OfficeNo = model.OfficeNo;
            TBL.RaceID = model.RaceID;
            TBL.RegionalLeaderID = TBL.RegionalLeaderID;
            TBL.RegionID = model.RegionID;
            TBL.Surname = model.Surname;
            TBL.ReviewContract = model.ReviewContract;
            TBL.StatusID = model.StatusID;
            TBL.TermsAndConditions = model.TermsAndConditions;
            TBL.ReasonID = model.ReasonID;
            db.tbl_RegionalLeadership.Add(TBL);
            db.SaveChanges();
        }

        public void UpdateRegionalLeadership(tbl_RegionalLeadership model)
        {
            var TBL = db.tbl_RegionalLeadership.ToList().FirstOrDefault(x => x.RegionalLeaderID == model.RegionalLeaderID);           
            TBL.AddressCity = model.AddressCity;
            TBL.AddressCode = model.AddressCode;
            TBL.AddressStreet = model.AddressStreet;
            TBL.AddressSuburb = model.AddressSuburb;
            //TBL.CalcBirthDate = model.CalcBirthDate;
            TBL.CellNo = model.CellNo;
            TBL.Email = model.Email;
            TBL.GenderID = model.GenderID;
            TBL.IDNo = model.IDNo;
            TBL.Passport = model.Passport;
            TBL.IsPassport = model.IsPassport;
            TBL.Name = model.Name;
            TBL.OfficeNo = model.OfficeNo;
            TBL.RaceID = model.RaceID;
            TBL.RegionID = model.RegionID;
            TBL.Surname = model.Surname;
            TBL.ReviewContract = model.ReviewContract;
            TBL.StatusID = model.StatusID;
            TBL.TermsAndConditions = model.TermsAndConditions;
            TBL.ReasonID = model.ReasonID;
            db.SaveChanges();
        }

        public List<tbl_RegionalLeadership> LoadRegionalLeadership()
        {
            return db.tbl_RegionalLeadership.ToList();
        }
    }
}