using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotted
{
    public class GameEventReceiver : IGameEvents
    {
        public static event Action<List<Dot>> OnChainCompletedEvent;
        public static event Action OnLevelFailedEvent;
        public static event Action<Circle> OnCircleSelectedEvent;

        public void OnChainCompleted(List<Dot> circles)
        {
            OnChainCompletedEvent?.Invoke(circles);
        }

        public void OnCircleSelected(Circle circle)
        {
            OnCircleSelectedEvent.Invoke(circle);
        }

        public void OnLevelFailed()
        {
            OnLevelFailedEvent?.Invoke();
        }
    }
}
