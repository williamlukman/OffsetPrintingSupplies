using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IStockMutationValidator
    {
        StockMutation VItemCase(StockMutation sm);
        StockMutation VStatus(StockMutation sm);
        StockMutation VSourceDocumentType(StockMutation sm);
        StockMutation VSourceDocumentDetailType(StockMutation sm);
        StockMutation VQuantity(StockMutation sm);
        StockMutation VCreateObject(StockMutation sm);
        StockMutation VDeleteObject(StockMutation sm);
        bool ValidCreateObject(StockMutation sm);
        bool ValidDeleteObject(StockMutation sm);
        bool isValid(StockMutation sm);
        string PrintError(StockMutation sm);
    }
}
