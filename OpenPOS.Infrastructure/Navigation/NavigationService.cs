using System.Collections.Generic;
using System.Windows;
using System.Windows.Shell;
using System.Linq;
using OpenPOS.Infrastructure.Interfaces;
using System.IO;

namespace OpenPOS.Infrastructure.Navigation
{
    public class NavigationService : INavigationService
    {
        readonly List<NavigationPoint> _navigationPoints = new List<NavigationPoint>();

        public void RegisterNavigationPoint(NavigationPoint point)
        {
            _navigationPoints.Add(point);
        }

        public void CreateJumpList()
        {
            var jl = JumpList.GetJumpList(Application.Current);
            jl.JumpItems.Clear();
            for (int i = _navigationPoints.Count-1; i >= 0; i--)
            {
                var point = _navigationPoints[i];
                if (point.ShowInJumpList)
                {
                    jl.JumpItems.Add(new JumpTask
                                            {
                                                Title = point.Name,
                                                CustomCategory = point.Category,
                                                IconResourceIndex = 0,
                                                IconResourcePath =
                                                    Path.Combine(System.Environment.CurrentDirectory, "OpenPOS.exe"),
                                                Arguments = point.CreateArg(),
                                                ApplicationPath =
                                                    Path.Combine(System.Environment.CurrentDirectory, "OpenPOS.exe")
                                            });
                }
            }
            jl.Apply();
        }

        public void ClearJumpList()
        {
            var jl = JumpList.GetJumpList(Application.Current);
            jl.JumpItems.Clear();
            jl.Apply();
        }

        public NavigationPoint[] GetNavigationPoints()
        {
            return _navigationPoints.ToArray();
        }

        public NavigationPoint GetNavigationPointFromArg(string arg)
        {
            return _navigationPoints.SingleOrDefault(p => arg == p.CreateArg());
        }
    }
}
