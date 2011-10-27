using Db4objects.Db4o;

namespace OpenPOS.Data.Storage.DB4O
{
    public class DB4OServer : INoSqlServer
    {
        IObjectServer _server;

        public DB4OServer(string dbPath)
        {
            if (_server == null)
            {
                Connect(dbPath);
            }
        }
        
        public void Connect(string dbPath)
        {
            _server = Db4oFactory.OpenServer(dbPath, 0);
            DB = _server.OpenClient();
        }

        public IObjectContainer DB { get; private set; }
    }
}
