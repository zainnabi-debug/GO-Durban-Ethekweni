using GoDurban.Models;
using System.Linq;

namespace GoDurban.BL
{
    public class PaymentBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreatePayment(Payment model)
        {
            Payment TBL = new  Payment();
            //TBL.PaymentID = TBL.PaymentID;
            TBL.AccountName = model.AccountName;
            TBL.AccountNo = model.AccountNo;
            TBL.AddressLine1 = model.AddressLine1;
            TBL.AddressLine2 = model.AddressLine2;
            TBL.AddressLine3 = model.AddressLine3;
            TBL.AddressLine4 = model.AddressLine4;
            TBL.AddressNo = model.AddressNo;
            TBL.Amount = model.Amount;
            TBL.BankAccNo = model.BankAccNo;
            TBL.BankName = model.BankName;
            TBL.BankTransit = model.BankTransit;
            //TBL.BatchNo = model.BatchNo;
            TBL.City = model.City;
            TBL.Date = model.Date;
            //TBL.DocNo = model.DocNo;
            TBL.DocTy = model.DocTy;
            TBL.ID1Code = model.ID1Code;
            TBL.IDIssuer = model.IDIssuer;
            TBL.IsPaymentSent = model.IsPaymentSent;
            //TBL.LineNo = model.LineNo;
            TBL.LongAddress = model.LongAddress;
            TBL.PaymentMethod = model.PaymentMethod;
            TBL.PhoneNo = model.PhoneNo;
            TBL.PostalCode = model.PostalCode;
            TBL.SP = model.SP;
            TBL.STSCD = model.STSCD;
            TBL.TaxID = model.TaxID;
            TBL.TransNo = model.TransNo;
            TBL.UserID = model.UserID;
            TBL.OwnerID = model.OwnerID;
            TBL.RegionID = model.RegionID;
            TBL.AssociationID = model.AssociationID;
            TBL.VoidedPayment = model.VoidedPayment;
            TBL.EntityType = model.EntityType;
            db.Payments.Add(TBL);
            db.SaveChanges();
        }

        public void UpdatePayment(Payment model)
        {
            var TBL = db.Payments.ToList().FirstOrDefault(x => x.PaymentID == model.PaymentID);
            //TBL.PaymentID = TBL.PaymentID;
            TBL.AccountName = model.AccountName;
            TBL.AccountNo = model.AccountNo;
            TBL.AddressLine1 = model.AddressLine1;
            TBL.AddressLine2 = model.AddressLine2;
            TBL.AddressLine3 = model.AddressLine3;
            TBL.AddressLine4 = model.AddressLine4;
            TBL.AddressNo = model.AddressNo;
            TBL.Amount = model.Amount;
            TBL.BankAccNo = model.BankAccNo;
            TBL.BankName = model.BankName;
            TBL.BankTransit = model.BankTransit;
            //TBL.BatchNo = model.BatchNo;
            TBL.City = model.City;
            TBL.Date = model.Date;
            //TBL.DocNo = model.DocNo;
            TBL.DocTy = model.DocTy;
            TBL.ID1Code = model.ID1Code;
            TBL.IDIssuer = model.IDIssuer;
            TBL.IsPaymentSent = model.IsPaymentSent;
            //TBL.LineNo = model.LineNo;
            TBL.LongAddress = model.LongAddress;
            TBL.OwnerID = model.OwnerID;
            TBL.RegionID = model.RegionID;
            TBL.AssociationID = model.AssociationID;
            TBL.PaymentMethod = model.PaymentMethod;
            TBL.PhoneNo = model.PhoneNo;
            TBL.PostalCode = model.PostalCode;
            TBL.SP = model.SP;
            TBL.STSCD = model.STSCD;
            TBL.TaxID = model.TaxID;
            TBL.TransNo = model.TransNo;
            TBL.VoidedPayment = model.VoidedPayment;
            TBL.UserID = model.UserID;
            TBL.EntityType = model.EntityType;
            db.SaveChanges();
        }

        public void UpdatePaymentTrue(Payment model)
        {
            var TBL = db.Payments.ToList().FirstOrDefault(x => x.PaymentID == model.PaymentID);
            TBL.IsPaymentSent = model.IsPaymentSent;
            db.SaveChanges();
        }
    }
}