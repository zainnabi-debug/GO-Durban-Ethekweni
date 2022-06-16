using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class RegionServiceAreaBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateRegionServiceArea(tbl_RegionServiceArea model)
        {
            tbl_RegionServiceArea TBL = new tbl_RegionServiceArea();
            TBL.RegionSAreaID = TBL.RegionSAreaID;
            TBL.RegionID = model.RegionID;
            TBL.ServiceAreaID = model.ServiceAreaID;
            db.tbl_RegionServiceArea.Add(TBL);
            db.SaveChanges();
        }

        public void UpdateRegionServiceArea(tbl_RegionServiceArea model)
        {
            var TBL = db.tbl_RegionServiceArea.ToList().FirstOrDefault(x=>x.RegionSAreaID==model.RegionSAreaID);
            TBL.RegionID = model.RegionID;
            TBL.ServiceAreaID = model.ServiceAreaID;
            db.SaveChanges();
        }
        
        public List<tbl_RegionServiceArea> LoadRegionalServiceArea()
        {
            return db.tbl_RegionServiceArea.ToList();
        }
    }
}