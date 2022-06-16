using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class OwnerBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateOwner(tbl_Owner model)
        {
            tbl_Owner TBL = new tbl_Owner();
            TBL.AddressCity = model.AddressCity;
            TBL.AddressCode = model.AddressCode;
            TBL.AddressStreet = model.AddressStreet;
            TBL.AddressSuburb = model.AddressSuburb;
            //TBL.CalcBirthDate = model.CalcBirthDate;
            TBL.CellNo = model.CellNo;
            TBL.Email = model.Email;
            TBL.GenderID = model.GenderID;
            TBL.IDNo = model.IDNo;
            TBL.Name = model.Name;
            TBL.OfficeNo = model.OfficeNo;
            TBL.RaceID = model.RaceID;
            TBL.CompanyName = model.CompanyName;
            TBL.OwnerID = TBL.OwnerID;
            TBL.Surname = model.Surname;
            TBL.ReviewContract = model.ReviewContract;
            TBL.StatusID = model.StatusID;
            TBL.TermsAndConditions = model.TermsAndConditions;
            TBL.ReasonID = model.ReasonID;
            TBL.AccountID = model.AccountID;
            //TBL.AccountNo = model.AccountNo;
            //TBL.BankBranch = model.BankBranch;
            //TBL.BankName = model.BankName;
            db.tbl_Owner.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateOwner(tbl_Owner model)
        {
            var TBL = db.tbl_Owner.ToList().FirstOrDefault(x=>x.OwnerID==model.OwnerID);
            TBL.AddressCity = model.AddressCity;
            TBL.AddressCode = model.AddressCode;
            TBL.AddressStreet = model.AddressStreet;
            TBL.AddressSuburb = model.AddressSuburb;
            //TBL.CalcBirthDate = model.CalcBirthDate;
            TBL.CellNo = model.CellNo;
            TBL.Email = model.Email;
            TBL.GenderID = model.GenderID;
            TBL.IDNo = model.IDNo;
            TBL.Name = model.Name;
            TBL.OfficeNo = model.OfficeNo;
            TBL.RaceID = model.RaceID;
            TBL.CompanyName = model.CompanyName;
            TBL.Surname = model.Surname;
            TBL.ReviewContract = model.ReviewContract;
            TBL.StatusID = model.StatusID;
            TBL.TermsAndConditions = model.TermsAndConditions;
            TBL.ReasonID = model.ReasonID;
            //TBL.AccountID = model.AccountID;
            //TBL.AccountNo = model.AccountNo;
            //TBL.BankBranch = model.BankBranch;
            //TBL.BankName = model.BankName;
            db.SaveChanges();
        }


        public void UpdateOwnerPendingDriver(tbl_Owner model)
        {
            var TBL = db.tbl_Owner.ToList().FirstOrDefault(x => x.OwnerID == model.OwnerID);
            TBL.PendingDriver = model.PendingDriver;
            db.SaveChanges();
        }
        public List<tbl_Owner> LoadOwner()
        {
            return db.tbl_Owner.ToList();
        }

        public List<tbl_RegionalLeadership> LoadRegionalLeadership()
        {
            return db.tbl_RegionalLeadership.ToList();
        }
    }
}