using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class PurchaseReceivalService : IPurchaseReceivalService
    {
        private IPurchaseReceivalRepository _repository;
        private IPurchaseReceivalValidator _validator;

        public PurchaseReceivalService(IPurchaseReceivalRepository _purchaseReceivalRepository, IPurchaseReceivalValidator _purchaseReceivalValidator)
        {
            _repository = _purchaseReceivalRepository;
            _validator = _purchaseReceivalValidator;
        }

        public IPurchaseReceivalValidator GetValidator()
        {
            return _validator;
        }

        public IList<PurchaseReceival> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseReceival GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseReceival> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }
        
        public PurchaseReceival CreateObject(PurchaseReceival purchaseReceival, IContactService _contactService)
        {
            purchaseReceival.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseReceival, _contactService) ? _repository.CreateObject(purchaseReceival) : purchaseReceival);
        }

        public PurchaseReceival CreateObject(int contactId, DateTime receivalDate, IContactService _contactService)
        {
            PurchaseReceival pr = new PurchaseReceival
            {
                ContactId = contactId,
                ReceivalDate = receivalDate
            };
            return this.CreateObject(pr, _contactService);
        }

        public PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival, IContactService _contactService)
        {
            return (_validator.ValidUpdateObject(purchaseReceival, _contactService) ? _repository.UpdateObject(purchaseReceival) : purchaseReceival);
        }

        public PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService)
        {
            return (_validator.ValidDeleteObject(purchaseReceival, _purchaseReceivalDetailService) ? _repository.SoftDeleteObject(purchaseReceival) : purchaseReceival);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                                IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidConfirmObject(purchaseReceival, _purchaseReceivalDetailService))
            {
                _repository.ConfirmObject(purchaseReceival);
                IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
                foreach (var detail in details)
                {
                    detail.ConfirmedAt = purchaseReceival.ConfirmedAt;
                    _purchaseReceivalDetailService.ConfirmObject(detail, _purchaseOrderDetailService, _stockMutationService, _itemService);
                    PurchaseOrderDetail pod = _purchaseOrderDetailService.GetObjectById(detail.PurchaseOrderDetailId);
                    _purchaseOrderDetailService.FulfilObject(pod);
                }
            }
            return purchaseReceival;
        }

        public PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                                IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidUnconfirmObject(purchaseReceival, _purchaseReceivalDetailService, _itemService))
            {
                _repository.UnconfirmObject(purchaseReceival);
                IList<PurchaseReceivalDetail> details = _purchaseReceivalDetailService.GetObjectsByPurchaseReceivalId(purchaseReceival.Id);
                foreach (var detail in details)
                {
                    _purchaseReceivalDetailService.UnconfirmObject(detail, _purchaseOrderDetailService, _stockMutationService, _itemService);
                }
            }
            return purchaseReceival;
        }
    }
}