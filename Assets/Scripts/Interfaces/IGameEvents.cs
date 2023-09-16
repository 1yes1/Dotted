using Dotted.Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotted
{
    public interface IGameEvents
    {
        public void OnLevelFailed();
        public void OnChainCompleted(List<Dot> circles);

        public void OnCircleSelected(Circle circle);
    }
}
