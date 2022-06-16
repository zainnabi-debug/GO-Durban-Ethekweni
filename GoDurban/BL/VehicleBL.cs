using GoDurban.Models;
using System.Collections.Generic;
using System.Linq;

namespace GoDurban.BL
{
    public class VehicleBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        public void CreateVehicle(tbl_Vehicle model)
        {
            tbl_Vehicle TBL = new tbl_Vehicle();
            TBL.CORExpiry = model.CORExpiry;
            TBL.CORScanID = model.CORScanID;
            TBL.EngineNumber = model.EngineNumber;
            TBL.LastServiced = model.LastServiced;
            TBL.NumberPlate = model.NumberPlate;
            TBL.OperatingLicenseExpiry = model.OperatingLicenseExpiry;
            TBL.OperatingLicense = model.OperatingLicense;
            TBL.OwnerID = model.OwnerID;
            TBL.VehicleID = TBL.VehicleID;
            TBL.VehicleInfoID = model.VehicleInfoID;
            TBL.VINNumber = model.VINNumber;
            TBL.YearRegistered = model.YearRegistered;
            TBL.Capacity = model.Capacity;
            TBL.Year = model.Year;
            TBL.StatusID = model.StatusID;
            TBL.ReasonID = model.ReasonID;
            TBL.PaymentAdded = model.PaymentAdded;
            db.tbl_Vehicle.Add(TBL);
            db.SaveChanges();
        }

        public void UpdateVehicle(tbl_Vehicle model)
        {
            var TBL = db.tbl_Vehicle.ToList().FirstOrDefault(x => x.VehicleID == model.VehicleID);
            TBL.VehicleID = TBL.VehicleID;
            TBL.CORExpiry = model.CORExpiry;
            TBL.CORScanID = model.CORScanID;
            TBL.EngineNumber = model.EngineNumber;
            TBL.LastServiced = model.LastServiced;
            TBL.NumberPlate = model.NumberPlate;
            TBL.OperatingLicenseExpiry = model.OperatingLicenseExpiry;
            TBL.OperatingLicense = model.OperatingLicense;
            TBL.OwnerID = model.OwnerID;
            TBL.VehicleAccreditation = model.VehicleAccreditation;
            TBL.VehicleInfoID = model.VehicleInfoID;
            TBL.VINNumber = model.VINNumber;
            TBL.YearRegistered = model.YearRegistered;
            TBL.Capacity = model.Capacity;
            TBL.Year = model.Year;
            TBL.StatusID = model.StatusID;
            TBL.ReasonID = model.ReasonID;
            TBL.PaymentAdded = model.PaymentAdded;
            db.SaveChanges();
        }

        public List<tbl_Vehicle> LoadVehicle()
        {
            return db.tbl_Vehicle.ToList();
        }
    }
}