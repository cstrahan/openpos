
using OpenPOS.Infrastructure.Navigation;

namespace OpenPOS.Infrastructure.Interfaces
{
    public interface INavigationService
    {
        void RegisterNavigationPoint(NavigationPoint point);
        NavigationPoint[] GetNavigationPoints();
        NavigationPoint GetNavigationPointFromArg(string arg);

        void CreateJumpList();
        void ClearJumpList();
    }
}
