using GoDurban.Models;
using System.Linq;

namespace GoDurban.BL
{
    public class UserPageBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateUserPage(tbl_UserPage model)
        {
            tbl_UserPage TBL = new tbl_UserPage();
            TBL.UserPageID = TBL.UserPageID;
            TBL.Association = model.Association;
            TBL.AssociationL = model.AssociationL;
            TBL.AssociationR = model.AssociationR;
            TBL.AssociationSA = model.AssociationSA;
            TBL.Owner = model.Owner;
            TBL.OwnerA = model.OwnerA;
            TBL.OwnerSA = model.OwnerSA;
            TBL.Region = model.Region;
            TBL.RegionL = model.RegionL;
            TBL.RegionSA = model.RegionSA;
            TBL.Driver = model.Driver;
            TBL.Vehicle = model.Vehicle;
            TBL.UserID = model.UserID;
            db.tbl_UserPage.Add(TBL);
            db.SaveChanges();
        }

        public void UpdateUserPage(tbl_UserPage model)
        {
            var TBL = db.tbl_UserPage.ToList().FirstOrDefault(x => x.UserPageID == model.UserPageID);
            TBL.Association = model.Association;
            TBL.AssociationL = model.AssociationL;
            TBL.AssociationR = model.AssociationR;
            TBL.AssociationSA = model.AssociationSA;
            TBL.Owner = model.Owner;
            TBL.OwnerA = model.OwnerA;
            TBL.OwnerSA = model.OwnerSA;
            TBL.Region = model.Region;
            TBL.RegionL = model.RegionL;
            TBL.RegionSA = model.RegionSA;
            TBL.Driver = model.Driver;
            TBL.Vehicle = model.Vehicle;
            TBL.UserID = model.UserID;
            db.SaveChanges();
        }
    }
}