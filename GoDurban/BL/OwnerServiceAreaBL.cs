using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class OwnerServiceAreaBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateOwnerServiceArea(tbl_OwnerServiceArea model)
        {
            tbl_OwnerServiceArea TBL = new tbl_OwnerServiceArea();
            TBL.OwnerServiceAreaID = TBL.OwnerServiceAreaID;
            TBL.OwnerID = model.OwnerID;
            TBL.AssociationServiceAreaID = model.AssociationServiceAreaID;
            db.tbl_OwnerServiceArea.Add(TBL);
            db.SaveChanges();
        }

        public void UpdateOwnerServiceArea(tbl_OwnerServiceArea model)
        {
            var TBL = db.tbl_OwnerServiceArea.ToList().FirstOrDefault(x => x.OwnerServiceAreaID == model.OwnerServiceAreaID);
            TBL.OwnerID = model.OwnerID;
            TBL.AssociationServiceAreaID = model.AssociationServiceAreaID;
            db.SaveChanges();
        }


        public void DeleteOwnerServiceArea(int Id)
        {
            using (GoDurbanEntities db = new GoDurbanEntities())
            {
                var osa = db.tbl_OwnerServiceArea.ToList().FirstOrDefault(x => x.OwnerServiceAreaID == Id);
                db.tbl_OwnerServiceArea.Remove(osa);
                db.SaveChanges();
            }
        }


        public List<tbl_OwnerServiceArea> LoadOwneralServiceArea()
        {
            return db.tbl_OwnerServiceArea.ToList();
        }
    }
}