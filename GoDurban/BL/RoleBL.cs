using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class RoleBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void Add(tbl_Role model)
        {
            tbl_Role TBL = new tbl_Role();
            TBL.RoleID = TBL.RoleID;
            TBL.RoleDescription = model.RoleDescription;
            db.tbl_Role.Add(TBL);
            db.SaveChanges();
        }
        public void Update(tbl_Role model)
        {
            var TBL = db.tbl_Role.ToList().FirstOrDefault(x=>x.RoleID==model.RoleID);
            TBL.RoleDescription = model.RoleDescription;
            db.SaveChanges();
        }

        public tbl_Role GetRoleByName(int roleID)
        {
            var TBL = db.tbl_Role.ToList().FirstOrDefault(x => x.RoleID == roleID);
            return TBL;
        }
        
        public List<tbl_Role> LoadRole()
        {
            // this is the load method, its returns everything from the Register table where IsDeleted = false
            return db.tbl_Role.ToList();
        }
    }
}