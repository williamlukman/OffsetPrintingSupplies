using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Repository;
using Service.Service;
using Validation.Validation;

namespace WebView.Controllers
{
    public class HomeController : Controller
    {
        private IContactGroupService _contactGroupService;
        private IContactService _contactService;
        private ContactGroup baseContactGroup;
        private Contact baseContact;

        public HomeController()
        {
            _contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            _contactService = new ContactService(new ContactRepository(), new ContactValidator());

            if (baseContactGroup == null) 
            {
                baseContactGroup = _contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);
            }

            if (baseContact == null)
            {
                baseContact = _contactService.CreateObject(Core.Constants.Constant.BaseContact, "BaseAddr", "123456", "PIC", "123", "Base@email.com", _contactGroupService);
            }
        }

        //
        // GET: /Home/
   
        public ActionResult Index()
        {
            return View();
        }



    
       
    }
}
