using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class AssociationRegionBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateAssociationRegion(tbl_AssociationRegion model)
        {
            tbl_AssociationRegion TBL = new tbl_AssociationRegion();
            TBL.AssociationRegionID = TBL.AssociationRegionID;
            TBL.AssociationID = model.AssociationID;
            TBL.RegionID = model.RegionID;
            db.tbl_AssociationRegion.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateAssociationRegion(tbl_AssociationRegion model)
        {
            var TBL = db.tbl_AssociationRegion.ToList().FirstOrDefault(x => x.AssociationRegionID == model.AssociationRegionID);
            TBL.AssociationID = model.AssociationID;
            TBL.RegionID = model.RegionID;
            db.SaveChanges();
        }
        public void DeleteAssociationRegion(int assregionID)
        {
            var assregion = db.tbl_AssociationRegion.ToList().FirstOrDefault(x => x.AssociationRegionID == assregionID);
            db.tbl_AssociationRegion.Remove(assregion);
            db.SaveChanges();
        }

        public List<tbl_AssociationRegion> LoadAssociationRegion()
        {
            return db.tbl_AssociationRegion.ToList();
        }
    }
}