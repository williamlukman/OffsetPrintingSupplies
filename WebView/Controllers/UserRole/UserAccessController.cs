﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Service.Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace WebView.Controllers
{
    public class UserAccessController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("UserAccessController");
        private IUserAccountService _userAccountService;
        private IUserMenuService _userMenuService;
        private IUserAccessService _userAccessService;

        public UserAccessController()
        {
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            _userAccessService = new UserAccessService(new UserAccessRepository(), new UserAccessValidator());
            _userMenuService = new UserMenuService(new UserMenuRepository(), new UserMenuValidator());
        }

        public dynamic GetUserAccess(int userId,string groupname)
        {
          
            try
            {
                int totalmenu = _userMenuService.GetQueryable().Count();
               var  q = _userAccessService.GetQueryableObjectsByUserAccountId(userId).Include("UserMenu");

               if (q.Count() < totalmenu)
               {
                   _userAccessService.CreateDefaultAccess(userId, _userMenuService, _userAccountService);
               }

               var model = (from m in q
                            select new
                            {
                                m.Id,
                                m.UserMenu.Name,
                                m.UserMenu.GroupName,
                                m.AllowView,
                                m.AllowCreate,
                                m.AllowEdit,
                                m.AllowDelete,
                                m.AllowUndelete,
                                m.AllowPrint,
                                m.AllowPaid,
                                m.AllowConfirm,
                                m.AllowReconcile,
                                m.AllowUnconfirm,
                                m.AllowUnpaid,
                                m.AllowUnreconcile,
                                m.AllowSpecialPricing,
                            }).ToList();
               return Json(new
              {
                  model
              }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                LOG.Error("GetUserAccessAccountingList", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }
         
           
        }

        [HttpPost]
        public dynamic Update(UserAccess model, string colName, bool isAllow)
        {
            try
            {
                if (!AuthenticationModel.IsAllowed("Edit", Core.Constants.Constant.MenuName.UserAccessRight, Core.Constants.Constant.MenuGroupName.Setting))
                {
                    Dictionary<string, string> Errors = new Dictionary<string, string>();
                    Errors.Add("Generic", "You are Not Allowed to Edit record");

                    return Json(new
                    {
                        Errors
                    }, JsonRequestBehavior.AllowGet);
                }

                var data = _userAccessService.GetObjectById(model.Id);
                switch (colName)
                {
                    case "View": data.AllowView = isAllow; break;
                    case "Create": data.AllowCreate = isAllow; break;
                    case "Edit": data.AllowEdit = isAllow; break;
                    case "Delete": data.AllowDelete = isAllow; break;
                    case "UnDelete": data.AllowUndelete = isAllow; break;
                    case "Confirm": data.AllowConfirm = isAllow; break;
                    case "UnConfirm": data.AllowUnconfirm = isAllow; break;
                    case "Paid": data.AllowPaid = isAllow; break;
                    case "UnPaid": data.AllowUnpaid = isAllow; break;
                    case "Reconcile": data.AllowReconcile = isAllow; break;
                    case "UnReconcile": data.AllowUnreconcile = isAllow; break;
                    case "ManualPricing": data.AllowSpecialPricing = isAllow; break;
                    case "Print": data.AllowPrint = isAllow; break;
                }
                model = _userAccessService.UpdateObject(data, _userAccountService, _userMenuService);
            }
            catch (Exception ex)
            {
                LOG.Error("Update Failed", ex);
                Dictionary<string, string> Errors = new Dictionary<string, string>();
                Errors.Add("Generic", "Error " + ex);

                return Json(new
                {
                    Errors
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                model.Errors
            });
        }


        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.UserAccessRight, Core.Constants.Constant.MenuGroupName.Setting))
            {
                return Content(Core.Constants.Constant.ControllerOutput.PageViewNotAllowed);
            }

            return View();
        }

    }
}
