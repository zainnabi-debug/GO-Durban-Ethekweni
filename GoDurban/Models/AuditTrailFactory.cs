using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;

namespace GoDurban.Models
{
    public class AuditTrailFactory
    {        
        private DbContext context;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public AuditTrailFactory(DbContext context)
        {
            this.context = context;
        }
       
        public enum AuditActions
        {
            // Insert
            Inserted,

            //Update
            Updated,
           
            //Delete
            Deleted
        }

        public string GetCurrentPageName()
        {
            string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo sPage = new System.IO.FileInfo(sPath);
            string sRet = sPage.Name;
            return sRet;
        }

        public tbl_AuditTrail GetAudit(DbEntityEntry entry)
        {
            tbl_AuditTrail audit = new tbl_AuditTrail();
            audit.Name = HttpContext.Current.Session["Name"].ToString(); //You can pass the current user as a parameter
            audit.Surname = HttpContext.Current.Session["Surname"].ToString();
            audit.TableName = GetCurrentPageName();
            audit.TableIdValue = GetKeyValue(entry);
            audit.Date = DateTime.Now;

            //entry is Added 
            if (entry.State == EntityState.Added)
            {
                var newValues = new StringBuilder();
                SetAddedProperties(entry, newValues);
                audit.NewData = newValues.ToString();

                if(audit.NewData!=null)
                audit.Actions = AuditActions.Inserted.ToString();
            }
            //entry in deleted
            else if (entry.State == EntityState.Deleted)
            {
                var oldValues = new StringBuilder();
                SetDeletedProperties(entry, oldValues);
                audit.OldData = oldValues.ToString();
                audit.Actions = AuditActions.Deleted.ToString();
            }
            //entry is modified
            else if (entry.State == EntityState.Modified)
            {
                var oldValues = new StringBuilder();
                var newValues = new StringBuilder();
                SetModifiedProperties(entry, oldValues, newValues);
                audit.OldData = oldValues.ToString();
                audit.NewData = newValues.ToString();
                audit.Actions = AuditActions.Updated.ToString();

                var modifiedProperties = entry.CurrentValues.PropertyNames.Where(propertyName => entry.Property(propertyName).IsModified).ToList();
                var properties = string.Join(" , ", modifiedProperties.ToList());
                audit.ChangedColums = properties;
            }
            return audit;
        }

        private void SetAddedProperties(DbEntityEntry entry, StringBuilder newData)
        {
            foreach (var propertyName in entry.CurrentValues.PropertyNames)
            {
                var newVal = entry.CurrentValues[propertyName];
                if (newVal != null)
                {
                    newData.AppendFormat("{0}={1} , ", propertyName, newVal);
                }
            }
            if (newData.Length > 0)
            {
                newData = newData.Remove(newData.Length - 3, 3);
            }
        }

        private void SetDeletedProperties(DbEntityEntry entry, StringBuilder oldData)
        {
            DbPropertyValues dbValues = entry.GetDatabaseValues();
            foreach (var propertyName in dbValues.PropertyNames)
            {
                var oldVal = dbValues[propertyName];
                if (oldVal != null)
                {
                    oldData.AppendFormat("{0}={1} , ", propertyName, oldVal);
                }
            }
            if (oldData.Length > 0)
            {
                oldData = oldData.Remove(oldData.Length - 3, 3);
            }
        }

        private void SetModifiedProperties(DbEntityEntry entry, StringBuilder oldData, StringBuilder newData)
        {
            DbPropertyValues dbValues = entry.GetDatabaseValues();
            foreach (var propertyName in entry.OriginalValues.PropertyNames)
            {
                var oldVal = dbValues[propertyName];
                var newVal = entry.CurrentValues[propertyName];
                if (oldVal != null && newVal != null && !Equals(oldVal, newVal))
                {
                    newData.AppendFormat("{0}={1} , ", propertyName, newVal);
                    oldData.AppendFormat("{0}={1} , ", propertyName, oldVal);
                }
            }
            if (oldData.Length > 0)
            {
                oldData = oldData.Remove(oldData.Length - 3, 3);
            }
            if (newData.Length > 0)
            {
                newData = newData.Remove(newData.Length - 3, 3);
            }
        }

        private string GetKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            string id = "0";
            if (objectStateEntry.EntityKey.EntityKeyValues != null)
            {
                id = objectStateEntry.EntityKey.EntityKeyValues[0].Value.ToString();
            }
            return id;
        }
       
        //private string GetTableName(DbEntityEntry dbEntry)
        //{
        //    TableAttribute tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
        //    string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;
        //    return tableName;
        //}
        
        private EntityObject CloneEntity(EntityObject obj)
        {
            DataContractSerializer dcSer = new DataContractSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();

            dcSer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;

            EntityObject newObject = (EntityObject)dcSer.ReadObject(memoryStream);
            return newObject;
        }
    }
}