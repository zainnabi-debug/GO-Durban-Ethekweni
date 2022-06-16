using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace GoDurban.BL
{
    public class RegionBL1
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateRegion(Models.Region model)
        {
            Models.Region TBL = new Models.Region();
            TBL.RegionID = TBL.RegionID;
            TBL.RegionName = model.RegionName;
            //TBL.RegionNo = model.RegionNo;
            TBL.AccountID = model.AccountID;
            TBL.StatusID = model.StatusID;
            db.Regions.Add(TBL);
            db.SaveChanges();
        }
        public void UpdateRegion(Models.Region model)
        {
            var TBL = db.Regions.ToList().FirstOrDefault(x => x.RegionID == model.RegionID);
            TBL.RegionName = model.RegionName;
            //TBL.RegionNo = model.RegionNo;
            //TBL.AccountID = model.AccountID;
            TBL.StatusID = model.StatusID;
            db.SaveChanges();
        }

        public void DeleteRegion(int regionID)
        {
            var region = db.Regions.ToList().FirstOrDefault(x => x.RegionID == regionID);
            db.Regions.Remove(region);
            db.SaveChanges();
        }

        public List<Models.Region> LoadRegion()
        {
            return db.Regions.ToList();
        }
    }
}