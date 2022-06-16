using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class AccountBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        int AccID = 0;
        public int CreateAccount(Models.Account model)
        {
            Models.Account TBL = new Models.Account();
            TBL.AccountID = TBL.AccountID;
            TBL.AccountNo = model.AccountNo;
            TBL.BankID = model.BankID;
            db.Accounts.Add(TBL);
            db.SaveChanges();
            
            //AccID = TBL.AccountID;
            return AccID;
        }
        public void UpdateAccount(Account model)
        {
            var TBL = db.Accounts.ToList().FirstOrDefault(x => x.AccountID == model.AccountID);
            TBL.AccountNo = model.AccountNo;
            TBL.BankID = model.BankID;
            db.SaveChanges();
        }

        public List<Account> LoadAccount()
        {
            return db.Accounts.ToList();
        }
    }
}