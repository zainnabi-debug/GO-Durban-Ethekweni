using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class RaceBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
       
        public void CreateRace(tbl_Race model)
        {
            tbl_Race TBL = new tbl_Race();
            TBL.RaceID = TBL.RaceID;
            TBL.RaceDescription = model.RaceDescription;
            db.tbl_Race.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateRace(tbl_Race model)
        {
            var TBL = db.tbl_Race.ToList().FirstOrDefault(x=>x.RaceID==model.RaceID);
            TBL.RaceDescription = model.RaceDescription;
            db.SaveChanges();
        }

        public List<tbl_Race> LoadRace()
        {
            return db.tbl_Race.ToList();
        }
    }
}