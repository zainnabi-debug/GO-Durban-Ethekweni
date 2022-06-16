using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoDurban.BL
{
    public class AssociationBL1
    {
        private GoDurbanEntities db = new GoDurbanEntities();

        public void CreateAssociation(Association model)
        {
            Association TBL = new Association();
            TBL.AssociationID = TBL.AssociationID;
            TBL.AssociationName = model.AssociationName;
            //TBL.AssociationNo = model.AssociationNo;
            TBL.StatusID = model.StatusID;
            TBL.AccountID = model.AccountID;
            db.Associations.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateAssociation(Association model)
        {
            var TBL = db.Associations.ToList().FirstOrDefault(x => x.AssociationID == model.AssociationID);
            TBL.AssociationName = model.AssociationName;
            //TBL.AssociationNo = model.AssociationNo;
            TBL.StatusID = model.StatusID;
            //TBL.AccountID = model.AccountID;
            db.SaveChanges();
        }
        public List<Association> LoadAssociation()
        {
            return db.Associations.ToList();
        }

    }
}