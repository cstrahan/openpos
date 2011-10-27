using System;
using System.IO;
using OpenPOS.Data.Models;
using System.Data.Entity;

namespace OpenPOS.Data.Storage
{
    public class OpenPOSContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<ClosedCash> ClosedCash { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<StockAction> StockActions { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketLine> TicketLines { get; set; }
        public DbSet<User> Users { get; set; }
    }

    public class SessionFactory : ISessionFactory
    {
        ISession _session;

        #region Default DB

        private void CreateDefaultDatabase()
        {
            _session.Add<Terminal>(new Terminal
                                       {
                Id = Guid.NewGuid()
            });
            _session.CommitChanges();

            _session.Add<Category>(new Category
                                       {
                Id = Guid.NewGuid(),
                Name = "Category Standard"
            });
            _session.CommitChanges();

            _session.Add<Tax>(new Tax
                                  {
                Id = Guid.NewGuid(),
                Name = "Tax Standard",
                Rate = 0.14
            });
            _session.Add<Tax>(new Tax
            {
                Id = Guid.NewGuid(),
                Name = "Tax Exempt",
                Rate = 0.00
            });
            _session.CommitChanges();

            _session.Add<Role>(new Role
            {
                Id = Guid.NewGuid(),
                Name = "Administrator Role"
            });
            _session.Add<Role>(new Role
            {
                Id = Guid.NewGuid(),
                Name = "Manager Role"
            });
            _session.Add<Role>(new Role
            {
                Id = Guid.NewGuid(),
                Name = "Employee Role"
            });
            _session.Add<Role>(new Role
            {
                Id = Guid.NewGuid(),
                Name = "Guest Role"
            });
            _session.CommitChanges();

            _session.Add<User>(new User
            {
                Id = Guid.NewGuid(),
                Name = "Administrator",
                RoleId = _session.Single<Role>(r => r.Name == "Administrator Role").Id
            });
            _session.Add<User>(new User
            {
                Id = Guid.NewGuid(),
                Name = "Manager",
                RoleId = _session.Single<Role>(r => r.Name == "Manager Role").Id
            });
            _session.Add<User>(new User
            {
                Id = Guid.NewGuid(),
                Name = "Employee",
                RoleId = _session.Single<Role>(r => r.Name == "Employee Role").Id
            });
            _session.Add<User>(new User
            {
                Id = Guid.NewGuid(),
                Name = "Guest",
                RoleId = _session.Single<Role>(r => r.Name == "Guest Role").Id
            });
            _session.CommitChanges();       
        }

        #endregion

        public ISession CreateSession(string provider)
        {
            switch (provider)
            {
                case "MSSQL":
                    {
                        //const string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=OpenPOS_MSSQL;Integrated Security=True;";
                        if (!BuildEFSession())
                            CreateDefaultDatabase();
                    }
                    break;
                case "MongoDB":
                    if (!BuildMongoDBSession("OpenPOS"))
                        CreateDefaultDatabase();
                    break;
                case "DB4O":
                    if (!BuildDB4OSession("OpenPOS.db4o"))
                        CreateDefaultDatabase();
                    break;
                default:
                    {
                        //const string connectionString = "Data Source=OpenPOS.sdf";
                        if (!BuildEFSession())
                        {
                            CreateDefaultDatabase();
                        //    //ImportProductsCSV("D:\\JVLStockSheet.csv");
                        }
                    }
                    break;
            }

            return _session;
        }

        #region EF

        private bool BuildEFSession()
        {
            OpenPOSContext context = new OpenPOSContext();
            bool dbExists = context.Database.Exists();
            _session = new EF.EFSession(context);
            return dbExists;
        }

        #endregion
        
        #region LinqToSql

        //private bool BuildLinqToSqlSession(string connectionString)
        //{
        //    var dataContext = new Models.OpenPOS(connectionString);
        //    var dbExists = dataContext.DatabaseExists();

        //    if (!dbExists)
        //    {
        //        dataContext.CreateDatabase();
        //    }

        //    _session = new LinqToSql.LinqToSqlSession(dataContext);

        //    return dbExists;
        //}

        #endregion 
        
        #region MongoDB

        private bool BuildMongoDBSession(string db)
        {
            var dbExists = File.Exists("C:\\Data\\db\\" + db + ".ns");
            _session = new MongoDB.MongoSession(db);

            return dbExists;
        }

        #endregion

        #region DB4O

        private bool BuildDB4OSession(string db)
        {
            var dbExists = File.Exists(db);
            _session = new DB4O.Db4oSession(new DB4O.DB4OServer(db));

            return dbExists;
        }

        #endregion
    }
}
