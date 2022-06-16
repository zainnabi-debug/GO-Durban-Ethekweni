using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class ReasonBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateReason(tbl_Reason model)
        {
            tbl_Reason TBL = new tbl_Reason();
            TBL.ReasonID = TBL.ReasonID;
            TBL.ReasonDescription = model.ReasonDescription;
            db.tbl_Reason.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateReason(tbl_Reason model)
        {
            var TBL = db.tbl_Reason.ToList().FirstOrDefault(x => x.ReasonID == model.ReasonID);
            TBL.ReasonDescription = model.ReasonDescription;
            db.SaveChanges();
        }

        public List<tbl_Reason> LoadReason()
        {
            return db.tbl_Reason.ToList();
        }
    }
}