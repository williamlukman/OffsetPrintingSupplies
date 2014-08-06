using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ICoreBuilderValidator
    {
        CoreBuilder VHasUniqueBaseSku(CoreBuilder coreBuilder, ICoreBuilderService _coreBuilderService);
        CoreBuilder VNameNotEmpty(CoreBuilder coreBuilder);
        CoreBuilder VHasUsedCoreItem(CoreBuilder coreBuilder, IItemService _itemService);
        CoreBuilder VHasNewCoreItem(CoreBuilder coreBuilder, IItemService _itemService);
        CoreBuilder VHasUoM(CoreBuilder coreBuidler, IUoMService _uomService);
        CoreBuilder VIsInCoreIdentificationDetail(CoreBuilder coreBuilder, ICoreIdentificationDetailService _coreIdentificationDetailService);
        CoreBuilder VIsInRollerBuilder(CoreBuilder coreBuilder, IRollerBuilderService _rollerBuilderService);
        CoreBuilder VCreateObject(CoreBuilder coreBuilder, ICoreBuilderService _coreBuilderService, IUoMService _uomService, IItemService _itemService);
        CoreBuilder VUpdateObject(CoreBuilder coreBuilder, ICoreBuilderService _coreBuilderService, IUoMService _uomService, IItemService _itemService);
        CoreBuilder VDeleteObject(CoreBuilder coreBuilder,  ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService);
        bool ValidCreateObject(CoreBuilder coreBuilder, ICoreBuilderService _coreBuilderService, IUoMService _uomService, IItemService _itemService);
        bool ValidUpdateObject(CoreBuilder coreBuilder, ICoreBuilderService _coreBuilderService, IUoMService _uomService, IItemService _itemService);
        bool ValidDeleteObject(CoreBuilder coreBuilder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService);
        bool isValid(CoreBuilder coreBuilder);
        string PrintError(CoreBuilder coreBuilder);
    }
}