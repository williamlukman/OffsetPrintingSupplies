using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Validation.Validation
{
    public class CoreBuilderValidator : ICoreBuilderValidator
    {

        public CoreBuilder VHasUniqueBaseSku(CoreBuilder coreBuilder, ICoreBuilderService _coreBuilderService)
        {
            if (String.IsNullOrEmpty(coreBuilder.BaseSku) || coreBuilder.BaseSku.Trim() == "")
            {
                coreBuilder.Errors.Add("BaseSku", "Tidak boleh kosong");
            }
            if (_coreBuilderService.IsBaseSkuDuplicated(coreBuilder))
            {
                coreBuilder.Errors.Add("BaseSku", "Tidak boleh diduplikasi");
            }
            return coreBuilder;
        }

        public CoreBuilder VNameNotEmpty(CoreBuilder coreBuilder)
        {
            if (String.IsNullOrEmpty(coreBuilder.Name) || coreBuilder.Name.Trim() == "")
            {
                coreBuilder.Errors.Add("Name", "Tidak boleh kosong");
            }
            return coreBuilder;
        }

        public CoreBuilder VHasUsedCoreItem(CoreBuilder coreBuilder, IItemService _itemService)
        {
            Item UsedCoreItem = _itemService.GetObjectById(coreBuilder.UsedCoreItemId);
            if (UsedCoreItem == null)
            {
                coreBuilder.Errors.Add("UsedCoreItemId", "Tidak terasosiasi dengan Item");
            }
            return coreBuilder;
        }

        public CoreBuilder VHasNewCoreItem(CoreBuilder coreBuilder, IItemService _itemService)
        {
            Item NewCoreItem = _itemService.GetObjectById(coreBuilder.NewCoreItemId);
            if (NewCoreItem == null)
            {
                coreBuilder.Errors.Add("NewCoreItemId", "Tidak terasosiasi dengan Item");
            }
            return coreBuilder;
        }

        public CoreBuilder VIsInCoreIdentificationDetail(CoreBuilder coreBuilder, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreBuilderId(coreBuilder.Id);
            if (details.Any())
            {
                coreBuilder.Errors.Add("Generic", "Tidak boleh terasosiasi dengan Core Identification Detail");
            }
            return coreBuilder;
        }

        public CoreBuilder VIsInRollerBuilder(CoreBuilder coreBuilder, IRollerBuilderService _rollerBuilderService)
        {
            IList<RollerBuilder> rollerBuilders = _rollerBuilderService.GetObjectsByCoreBuilderId(coreBuilder.Id);
            if (rollerBuilders.Any())
            {
                coreBuilder.Errors.Add("Generic", "Tidak boleh terasosiasi dengan Roller Builder");
            }
            return coreBuilder;
        }


        public CoreBuilder VCreateObject(CoreBuilder coreBuilder, ICoreBuilderService _coreBuilderService, IItemService _itemService)
        {
            VHasUniqueBaseSku(coreBuilder, _coreBuilderService);
            if (!isValid(coreBuilder)) { return coreBuilder; }
            VHasUsedCoreItem(coreBuilder, _itemService);
            if (!isValid(coreBuilder)) { return coreBuilder; }
            VHasNewCoreItem(coreBuilder, _itemService);
            if (!isValid(coreBuilder)) { return coreBuilder; }
            VNameNotEmpty(coreBuilder);
            return coreBuilder;
        }

        public CoreBuilder VUpdateObject(CoreBuilder coreBuilder, ICoreBuilderService _coreBuilderService, IItemService _itemService)
        {
            VCreateObject(coreBuilder, _coreBuilderService, _itemService);
            return coreBuilder;
        }

        public CoreBuilder VDeleteObject(CoreBuilder coreBuilder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService)
        {
            VIsInCoreIdentificationDetail(coreBuilder, _coreIdentificationDetailService);
            if (!isValid(coreBuilder)) { return coreBuilder; }
            VIsInRollerBuilder(coreBuilder, _rollerBuilderService);
            return coreBuilder;
        }

        public bool ValidCreateObject(CoreBuilder coreBuilder, ICoreBuilderService _coreBuilderService, IItemService _itemService)
        {
            VCreateObject(coreBuilder, _coreBuilderService, _itemService);
            return isValid(coreBuilder);
        }

        public bool ValidUpdateObject(CoreBuilder coreBuilder, ICoreBuilderService _coreBuilderService, IItemService _itemService)
        {
            coreBuilder.Errors.Clear();
            VUpdateObject(coreBuilder, _coreBuilderService, _itemService);
            return isValid(coreBuilder);
        }

        public bool ValidDeleteObject(CoreBuilder coreBuilder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService)
        {
            coreBuilder.Errors.Clear();
            VDeleteObject(coreBuilder, _coreIdentificationDetailService, _rollerBuilderService);
            return isValid(coreBuilder);
        }

        public bool isValid(CoreBuilder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CoreBuilder obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
