using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class OwnerAssociationBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        public void CreateOwnerAssociation(tbl_OwnerAssociation model)
        {
            tbl_OwnerAssociation TBL = new tbl_OwnerAssociation();
            TBL.OwnerAssociationID = TBL.OwnerAssociationID;
            TBL.AssociationID = model.AssociationID;
            TBL.OwnerID = model.OwnerID;
            db.tbl_OwnerAssociation.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateOwnerAssociation(tbl_OwnerAssociation model)
        {
            var TBL = db.tbl_OwnerAssociation.ToList().FirstOrDefault(x=>x.OwnerAssociationID==model.OwnerAssociationID);
            TBL.AssociationID = model.AssociationID;
            TBL.OwnerID = model.OwnerID;
            db.SaveChanges();
        }

        public List<tbl_OwnerAssociation> LoadOwnerAssociation()
        {
            return db.tbl_OwnerAssociation.ToList();
        }
    }
}