using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class AssociationServiceAreaBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateAssociationServiceArea(tbl_AssociationServiceArea model)
        {
            tbl_AssociationServiceArea TBL = new tbl_AssociationServiceArea();
            TBL.AssociationServiceAreaID = TBL.AssociationServiceAreaID;
            TBL.AssociationID = model.AssociationID;
            TBL.RegionSAreaID = model.RegionSAreaID;
            db.tbl_AssociationServiceArea.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateAssociationServiceArea(tbl_AssociationServiceArea model)
        {
            var TBL = db.tbl_AssociationServiceArea.ToList().FirstOrDefault(x => x.AssociationServiceAreaID == model.AssociationServiceAreaID);
            TBL.AssociationID = model.AssociationID;
            TBL.RegionSAreaID = model.RegionSAreaID;
            db.SaveChanges();
        }


        public List<tbl_AssociationServiceArea> LoadAssociationServiceArea()
        {
            return db.tbl_AssociationServiceArea.ToList();
        }
    }
}