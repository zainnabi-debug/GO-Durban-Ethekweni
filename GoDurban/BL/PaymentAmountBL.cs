using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoDurban.BL
{
    public class PaymentAmountBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreatePaymentAmount(tbl_PaymentAmount model)
        {
            tbl_PaymentAmount TBL = new tbl_PaymentAmount();
            //TBL.PaymentAmountID = TBL.PaymentAmountID;
            TBL.Amount = model.Amount;
            TBL.Datetime = model.Datetime;
            TBL.Recipients = model.Recipients;
                        
            db.tbl_PaymentAmount.Add(TBL);
            db.SaveChanges();
        }

        public void UpdatePaymentAAmount(tbl_PaymentAmount model)
        {
            var TBL = db.tbl_PaymentAmount.ToList().FirstOrDefault(x => x.PaymentAmountID == model.PaymentAmountID);
            TBL.Amount = model.Amount;
            TBL.Datetime = model.Datetime;
            TBL.Recipients = model.Recipients;;
          
            db.SaveChanges();
        }

        public List<tbl_PaymentAmount> LoadPaymentAmount()
        {
            return db.tbl_PaymentAmount.ToList();
        }
    }
}