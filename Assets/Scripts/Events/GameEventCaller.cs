using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotted.Assets.Scripts.Interfaces
{
    public class GameEventCaller : IGameEvents
    {
        private GameEventReceiver _gameEventReceiver;

        public static GameEventCaller Instance
        {
            get
            {
                return GameManager.Instance.GameEventCaller;
            }
        }

        public GameEventCaller(GameEventReceiver gameEventReceiver)
        {
            _gameEventReceiver = gameEventReceiver;
        }

        public void OnChainCompleted(List<Dot> circles)
        {
            _gameEventReceiver.OnChainCompleted(circles);
        }

        public void OnLevelFailed()
        {
            _gameEventReceiver.OnLevelFailed();
        }

        public void OnCircleSelected(Circle circle)
        {
            _gameEventReceiver.OnCircleSelected(circle);
        }
    }
}
