using EStateDevelopment.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EStateDevelopment.Areas.Admin
{
    public class AdminActivity_Logs
    {

        protected Activity_Log log = null;
        public AdminActivity_Logs()
        {
            log = new Activity_Log();
        }

        public void AddLog(string operation_name = "", string message = "")
        {
            QIGIEntities db = new QIGIEntities();
            var RoleName = "QIGIAdmin";
            var userNameis = HttpContext.Current.User.Identity.Name.ToString();
            if (userNameis != null)
            {
                var currentuser = db.AspNetUsers.ToList().Where(x => x.Email == userNameis).FirstOrDefault();
                if (currentuser != null)
                {
                    var currentrole = db.AspNetRoles.Find(currentuser.role_id);
                    RoleName = currentrole.Name;
                }
            }
            var count = db.Activity_Log.ToList().Count();
            //var userNameis = HttpContext.Current.User.Identity.Name;
            var completemessage = message;
            //log.created_by = userNameis;
            log.created_by = userNameis;
            log.modified_by = RoleName;
            log.created_date = DateTime.Now;
            log.new_details = completemessage;
            log.operation_name = operation_name;
            log.log_time = (count + 1).ToString();
            db.Activity_Log.Add(log);
            db.SaveChanges();
        }

        public void AddLoginLog(string username = "", string operation_name = "", string message = "")
        {
            QIGIEntities db = new QIGIEntities();
            var RoleName = "QIGIAdmin";
            var userNameis = username;
            if (userNameis != null)
            {
                var currentuser = db.AspNetUsers.ToList().Where(x => x.Email == userNameis).FirstOrDefault();
                if (currentuser != null)
                {
                    var currentrole = db.AspNetRoles.Find(currentuser.role_id);
                    RoleName = currentrole.Name;
                }
            }
            var count = db.Activity_Log.ToList().Count();

           // var userNameis = username;
            var completemessage = message;

            log.created_by = userNameis;
            log.created_date = DateTime.Now;
            log.new_details = completemessage;
            log.operation_name = operation_name;
            log.log_time = (count + 1).ToString();
            db.Activity_Log.Add(log);
            db.SaveChanges();
        }

        public void AddLog(string operation_name = "", string message = "", string Email = "")
        {
            QIGIEntities db = new QIGIEntities();

            var count = db.Activity_Log.ToList().Count();
            //var userNameis = HttpContext.Current.User.Identity.Name;
            var completemessage = message;
            //log.created_by = userNameis;
            log.created_by = Email;
            log.modified_by = Email;
            log.created_date = DateTime.Now;
            log.new_details = completemessage;
            log.operation_name = operation_name;
            log.log_time = (count + 1).ToString();
            db.Activity_Log.Add(log);
            db.SaveChanges();
        }
    }

}