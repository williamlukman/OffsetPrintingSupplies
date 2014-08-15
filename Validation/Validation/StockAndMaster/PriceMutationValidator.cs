using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class PriceMutationValidator : IPriceMutationValidator
    {
        public PriceMutation VDeactivateObject(PriceMutation priceMutation)
        {

            return priceMutation;
        }

        public PriceMutation VCreateObject(PriceMutation priceMutation, IPriceMutationService _priceMutationService)
        {

            return priceMutation;
        }

        public PriceMutation VUpdateObject(PriceMutation priceMutation, IPriceMutationService _priceMutationService)
        {
            VCreateObject(priceMutation, _priceMutationService);
            return priceMutation;
        }

        public PriceMutation VDeleteObject(PriceMutation priceMutation)
        {
            
            return priceMutation;
        }

        public bool ValidCreateObject(PriceMutation priceMutation, IPriceMutationService _priceMutationService)
        {
            VCreateObject(priceMutation, _priceMutationService);
            return isValid(priceMutation);
        }

        public bool ValidUpdateObject(PriceMutation priceMutation, IPriceMutationService _priceMutationService)
        {
            priceMutation.Errors.Clear();
            VUpdateObject(priceMutation, _priceMutationService);
            return isValid(priceMutation);
        }

        public bool ValidDeactivateObject(PriceMutation priceMutation, IPriceMutationService _priceMutationService)
        {
            priceMutation.Errors.Clear();
            VUpdateObject(priceMutation, _priceMutationService);
            return isValid(priceMutation);
        }

        public bool ValidDeleteObject(PriceMutation priceMutation)
        {
            priceMutation.Errors.Clear();
            VDeleteObject(priceMutation);
            return isValid(priceMutation);
        }

        public bool isValid(PriceMutation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PriceMutation obj)
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
