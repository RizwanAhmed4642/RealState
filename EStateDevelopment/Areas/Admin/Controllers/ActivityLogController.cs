using EStateDevelopment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EStateDevelopment.Areas.Admin.Controllers
{
    public class ActivityLogController : Controller
    {
        QIGIEntities db = new QIGIEntities();
        // GET: Admin/ActicityLog
        [Authorize(Roles = "QIGIAdmin, Admin,Administrator")]
        public ActionResult UserActivityLog()
        {
            try
            {
                var list = db.Activity_Log.OrderByDescending(x => x.id).ToList().Where(x => x.modified_by != "QIGIAdmin" && x.modified_by != "Admin" && x.modified_by != "Administrator").ToList();
                return View(list);
            }
            catch (Exception ex)
            {

                var massage = ex.Message;
            }
            return View();
        } 
        public ActionResult AdminActivityLog()
        {
            try
            {
                var list = db.Activity_Log.OrderByDescending(x => x.id).ToList().Where(x => x.modified_by == "QIGIAdmin" || x.modified_by == "Admin" || x.modified_by == "Administrator");
                return View(list);
            }
            catch (Exception ex)
            {

                var massage = ex.Message;
            }
            return View();
        }
    }
}