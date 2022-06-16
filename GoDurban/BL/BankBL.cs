using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class BankBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public List<Bank> LoadBanks()
        {
            return db.Banks.ToList();
        }
        public List<Bank> LoadBranchs()
        {
            return db.Banks.ToList();
        }
    }
}