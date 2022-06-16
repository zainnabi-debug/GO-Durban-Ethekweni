using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class AssociationLeadershipBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateAssociationLeadership(tbl_AssociationLeadership model)
        {
            tbl_AssociationLeadership TBL = new tbl_AssociationLeadership();
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
            TBL.AssociationLeaderID = TBL.AssociationLeaderID;
            TBL.AssociationID = model.AssociationID;
            TBL.Surname = model.Surname;
            TBL.ReviewContract = model.ReviewContract;
            TBL.StatusID = model.StatusID;
            TBL.TermsAndConditions = model.TermsAndConditions;
            TBL.ReasonID = model.ReasonID;
            db.tbl_AssociationLeadership.Add(TBL);
            db.SaveChanges();
        }

        public void UpdateAssociationLeadership(tbl_AssociationLeadership model)
        {
            var TBL = db.tbl_AssociationLeadership.ToList().FirstOrDefault(x=>x.AssociationLeaderID==model.AssociationLeaderID);
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
            TBL.AssociationID = model.AssociationID;
            TBL.Surname = model.Surname;
            TBL.ReviewContract = model.ReviewContract;
            TBL.StatusID = model.StatusID;
            TBL.TermsAndConditions = model.TermsAndConditions;
            TBL.ReasonID = model.ReasonID;
            db.SaveChanges();
        }

        public List<tbl_AssociationLeadership> LoadAssociationLeadership()
        {
            return db.tbl_AssociationLeadership.ToList();
        }
    }
}