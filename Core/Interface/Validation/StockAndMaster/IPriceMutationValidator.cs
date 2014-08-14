using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPriceMutationValidator
    {
        PriceMutation VDeactivateObject(PriceMutation priceMutation);
        PriceMutation VCreateObject(PriceMutation priceMutation, IPriceMutationService _priceMutationService);
        PriceMutation VUpdateObject(PriceMutation priceMutation, IPriceMutationService _priceMutationService);
        PriceMutation VDeleteObject(PriceMutation priceMutation);
        bool ValidDeactivateObject(PriceMutation priceMutation, IPriceMutationService _priceMutationService);
        bool ValidCreateObject(PriceMutation priceMutation, IPriceMutationService _priceMutationService);
        bool ValidUpdateObject(PriceMutation priceMutation, IPriceMutationService _priceMutationService);
        bool ValidDeleteObject(PriceMutation priceMutation);
        bool isValid(PriceMutation priceMutation);
        string PrintError(PriceMutation priceMutation);
    }
}
