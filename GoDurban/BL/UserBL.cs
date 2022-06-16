using GoDurban.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GoDurban.BL
{
    public class UserBL
    {
        private GoDurbanEntities db = new GoDurbanEntities();
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public void CreateUser(tbl_User model)
        {
            //string encryptedPassword = Encrypt(model.Password);
            string encryptedPassword = Encrypt("MojaCruise");

            tbl_User TBL = new tbl_User();
            TBL.AccessFailedCount = model.AccessFailedCount;
            TBL.CellNo = model.CellNo;
            TBL.DateCreated = DateTime.Now;
            TBL.Email = model.Email;
            TBL.EmailConfirmed = model.EmailConfirmed;
            TBL.FirstTimeLogin = true;
            TBL.GenderID = model.GenderID;
            TBL.IDNo = model.IDNo;
            TBL.IsActive = true;
            TBL.IsSupervisor = model.IsSupervisor;
            TBL.LockoutEnabled = model.LockoutEnabled;
            TBL.Name = model.Name;
            TBL.Password = encryptedPassword;
            TBL.PasswordConfirm = encryptedPassword;
            TBL.PasswordHash = encryptedPassword;
            TBL.RaceID = model.RaceID;
            TBL.RoleID = model.RoleID;
            TBL.SecurityStamp = model.SecurityStamp;
            TBL.Surname = model.Surname;
            TBL.TwoFactorEnabled = model.TwoFactorEnabled;
            TBL.UserID = TBL.UserID;
            TBL.UserName = model.UserName;
            TBL.UserRole = model.UserRole;
            //TBL.StatusID = model.StatusID;
            db.tbl_User.Add(TBL);
            db.SaveChanges();
        }

        public void UpdateUser(tbl_User model)
        {
            //string encryptedPassword = Encrypt(model.Password);
            //string encryptedPassword = Encrypt("MojaCruise");

            var TBL = db.tbl_User.ToList().FirstOrDefault(x=>x.UserID==model.UserID);
            TBL.AccessFailedCount = model.AccessFailedCount;
            TBL.CellNo = model.CellNo;
            TBL.Email = model.Email;
            //TBL.Password = encryptedPassword;
            //TBL.PasswordConfirm = encryptedPassword;
            TBL.GenderID = model.GenderID;
            TBL.IDNo = model.IDNo;
            TBL.Name = model.Name;
            TBL.RaceID = model.RaceID;
            //TBL.RoleID = model.RoleID;
            TBL.Surname = model.Surname;
            TBL.UserName = model.UserName;
            TBL.UserRole = model.UserRole;
            //TBL.IsActive = model.IsActive;
            TBL.IsSupervisor = model.IsSupervisor;
            db.SaveChanges();
        }

        public void DeleteUser(int userID)
        {     
            var TBL = db.tbl_User.ToList().FirstOrDefault(x => x.UserID == userID);
            TBL.IsActive = false;
            //TBL.IsDeleted = true;
            db.SaveChanges();
        }

        public List<tbl_User> LoadUser()
        {
             return db.tbl_User.Where(x => x.IsActive == true).ToList();
        }
    }
}