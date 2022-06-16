using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoDurban.BL
{
    public class NoOfClicksPerMonthBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        public void CreateNoOfClicksPerMonth(NoOfClicksPerMonth model)
        {
            NoOfClicksPerMonth TBL = new NoOfClicksPerMonth();
            TBL.MonthClicked = TBL.MonthClicked;
            TBL.IsPaymentSent = model.IsPaymentSent;
            TBL.ColumnName = model.ColumnName;
            TBL.RangeDate = model.RangeDate;
            db.NoOfClicksPerMonths.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateNoOfClicksPerMonth(NoOfClicksPerMonth model)
        {
            var TBL = db.NoOfClicksPerMonths.ToList().FirstOrDefault(x => x.NoOfClicksID == model.NoOfClicksID);
            TBL.MonthClicked = TBL.MonthClicked;
            TBL.IsPaymentSent = model.IsPaymentSent;
            TBL.ColumnName = model.ColumnName;
            TBL.RangeDate = model.RangeDate;
            db.SaveChanges();
        }
    }
}