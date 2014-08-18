using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;

namespace WebView.Controllers
{
    public class RollerWarehouseMutationController : Controller
    {
        //
        // GET: /RollerWarehouseMutation/

        public ActionResult Index()
        {
            return View();
        }

    }
}
