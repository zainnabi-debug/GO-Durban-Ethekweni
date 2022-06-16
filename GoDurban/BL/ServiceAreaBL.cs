using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class ServiceAreaBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateServiceArea(tbl_ServiceArea model)
        {
            tbl_ServiceArea TBL = new tbl_ServiceArea();
            TBL.ServiceAreaID = TBL.ServiceAreaID;
            TBL.ServiceAreaDescription = model.ServiceAreaDescription;
            db.tbl_ServiceArea.Add(TBL);
            db.SaveChanges();
        }


        public void UpdateServiceArea(tbl_ServiceArea model)
        {
            var TBL = db.tbl_ServiceArea.ToList().FirstOrDefault(x => x.ServiceAreaID == model.ServiceAreaID);
            TBL.ServiceAreaDescription = model.ServiceAreaDescription;
            db.SaveChanges();
        }

        public void DeleteServiceArea(int Id)
        {
            var data = db.tbl_ServiceArea.ToList().FirstOrDefault(x => x.ServiceAreaID == Id);
            //data.IsDeleted = true;
            db.tbl_ServiceArea.Remove(data);
            db.SaveChanges();
        }


        public List<tbl_ServiceArea> LoadServiceArea()
        {
            return db.tbl_ServiceArea.ToList();
        }
    }
}