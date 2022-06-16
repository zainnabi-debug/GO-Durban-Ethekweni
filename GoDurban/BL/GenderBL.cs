using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class GenderBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        public void CreateGender(tbl_Gender model)
        {
            tbl_Gender TBL = new tbl_Gender();
            TBL.GenderID = TBL.GenderID;
            TBL.GenderDescription = model.GenderDescription;
            db.tbl_Gender.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateGender(tbl_Gender model)
        {
            var TBL = db.tbl_Gender.ToList().FirstOrDefault(x=>x.GenderID==model.GenderID);
            TBL.GenderDescription = model.GenderDescription;
            db.SaveChanges();
        }
        public List<tbl_Gender> LoadGender()
        {
            return db.tbl_Gender.ToList();
        }
    }
}