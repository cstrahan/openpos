using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOS.Data.Storage;

namespace OpenPOS.Modules.Stock.Services
{
    public class StockService : IStockService
    {
        private readonly ISession _session;

        public StockService(ISession session)
        {
            _session = session;
        }

        public Data.Models.Stock GetStockByProductId(Guid id)
        {
            return _session.Single<Data.Models.Stock>(s => s.ProductId == id);
        }

        public void UpdateStock(Data.Models.Stock stock)
        {
            _session.Update<Data.Models.Stock>(stock);
            _session.CommitChanges();
        }

        public void AddStockAction(Data.Models.StockAction action)
        {
            _session.Add<Data.Models.StockAction>(action);
            _session.CommitChanges();
        }
    }
}
