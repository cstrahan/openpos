using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOS.Data.Models;

namespace OpenPOS.Modules.Stock.Services
{
    public interface IStockService
    {
        OpenPOS.Data.Models.Stock GetStockByProductId(Guid id);
        void UpdateStock(OpenPOS.Data.Models.Stock stock);

        void AddStockAction(StockAction action);
    }
}
