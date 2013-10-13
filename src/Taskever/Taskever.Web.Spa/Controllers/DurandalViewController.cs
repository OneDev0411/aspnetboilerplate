﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Taskever.Web.Controllers
{
    public class DurandalViewController : Controller
    {
        //
        // GET: /DurandalViewController/

        //public ActionResult GetAppView(string module, string view)
        //{
        //    return View("/App/" + module + "/views/" + view + ".cshtml");
        //}

        public ActionResult GetAppView(string viewUrl)
        {
            if (viewUrl.StartsWith("/"))
            {
                return View(viewUrl);
            }

            return View("/App/Main/" + viewUrl);
        }
    }
}
