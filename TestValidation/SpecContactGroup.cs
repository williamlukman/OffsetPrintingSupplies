using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using NSpec;
using Service.Service;
using Core.Interface.Service;
using Data.Context;
using System.Data.Entity;
using Data.Repository;
using Validation.Validation;

namespace TestValidation
{

    public class SpecContactGroup: nspec
    {

        DataBuilder d;
        void before_each()
        {
            var db = new OffsetPrintingSuppliesEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();

                d.baseGroup = d._contactGroupService.CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);

                
                d.contactGroup1 = d._contactGroupService.CreateObject("Admin", "Administrators Group");
            }
        }

        void contactgroup_validation()
        {
        
            it["validates_contactgroup"] = () =>
            {
                d.contactGroup1.Errors.Count().should_be(0);
            };

            it["contactgroup_with_no_name"] = () =>
            {
                ContactGroup noname = new ContactGroup()
                {
                    Name = "     ",
                    Description = "Keterangan"
                };
                noname = d._contactGroupService.CreateObject(noname);
                noname.Errors.Count().should_not_be(0);
            };

            it["contactgroup_with_same_name"] = () =>
            {
                ContactGroup samename = new ContactGroup()
                {
                    Name = Core.Constants.Constant.GroupType.Base,
                    Description = "Keterangan"
                };
                samename = d._contactGroupService.CreateObject(samename);
                samename.Errors.Count().should_not_be(0);
            };

            it["update_with_noname"] = () =>
            {
                d.contactGroup1.Name = "   ";
                d.contactGroup1 = d._contactGroupService.UpdateObject(d.contactGroup1);
                d.contactGroup1.Errors.Count().should_not_be(0);
            };

            it["update_with_samename"] = () =>
            {
                d.contactGroup1.Name = Core.Constants.Constant.GroupType.Base;
                d.contactGroup1 = d._contactGroupService.UpdateObject(d.contactGroup1);
                d.contactGroup1.Errors.Count().should_not_be(0);
            };

            it["delete_contactgroup"] = () =>
            {
                d.contactGroup1 = d._contactGroupService.SoftDeleteObject(d.contactGroup1);
                d.contactGroup1.Errors.Count().should_be(0);
            };
        }
    }
}