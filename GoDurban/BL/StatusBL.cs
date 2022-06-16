using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class StatusBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateStatus(tbl_Status model)
        {
            tbl_Status TBL = new tbl_Status();
            TBL.StatusID = TBL.StatusID;
            TBL.StatusDescription = model.StatusDescription;
            db.tbl_Status.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateStatus(tbl_Status model)
        {
            var TBL = db.tbl_Status.ToList().FirstOrDefault(x => x.StatusID == model.StatusID);
            TBL.StatusDescription = model.StatusDescription;
            db.SaveChanges();
        }

        public List<tbl_Status> LoadStatus()
        {
            return db.tbl_Status.ToList();
        }

        public List<tbl_Status> LoadStatusNotAll()
        {
            var sts = from s in db.tbl_Status
                          where s.StatusDescription != "Approved" 
                          select s;
            return sts.ToList();
        }
    }
}