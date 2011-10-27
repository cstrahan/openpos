
namespace OpenPOS.Data.Storage
{
    public interface ISessionFactory
    {
        ISession CreateSession(string provider);
    }
}
