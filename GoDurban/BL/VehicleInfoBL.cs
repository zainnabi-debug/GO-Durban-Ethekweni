using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class VehicleInfoBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateVehicleInfo(tbl_VehicleInfo model)
        {
            tbl_VehicleInfo TBL = new tbl_VehicleInfo();
            TBL.VehicleInfoID = TBL.VehicleInfoID;
            TBL.Make = model.Make;
            TBL.Model = model.Model;
            TBL.Capacity = model.Capacity;
            db.tbl_VehicleInfo.Add(TBL);
            db.SaveChanges();
        }

        public void UpdateVehicleInfo(tbl_VehicleInfo model)
        {
            var TBL = db.tbl_VehicleInfo.ToList().FirstOrDefault(x => x.VehicleInfoID == model.VehicleInfoID);
            TBL.Capacity = model.Capacity;
            TBL.Model = model.Model;
            TBL.Make = model.Make;
            db.SaveChanges();
        }

        public List<tbl_VehicleInfo> LoadVehicleInfo()
        {
            return db.tbl_VehicleInfo.ToList();
        }
    }
}