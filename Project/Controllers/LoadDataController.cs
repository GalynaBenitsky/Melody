using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class LoadDataController : Controller
    {
        // Reference to the manager object
        Manager m = new Manager();

        // GET: LoadData
        public ActionResult Index()
        {
            if (m.LoadData())
            {
                return ViewBag.data("data has been loaded");
            }
            else
            {
                return Content("data exists already");
            }
        }

        public ActionResult Remove()
        {
            if(m.RemoveData())
            {
                return Content("thank you. Database has been removed");
            }
            else
            {
                return Content("could not remove data");
            }
        }

    

    }
}