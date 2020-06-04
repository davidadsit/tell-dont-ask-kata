using TellDontAskKata.Entities;

namespace TellDontAskKata.Repository
{
    public interface IProductCatalog
    {
        Product GetByName(string name);
    }
}
