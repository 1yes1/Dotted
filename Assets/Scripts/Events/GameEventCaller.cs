using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dotted
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

        public void OnFailed()
        {
            _gameEventReceiver.OnFailed();
        }

        public void OnCircleSelected(Circle circle)
        {
            _gameEventReceiver.OnCircleSelected(circle);
        }

        public void OnScoreChanged(int score)
        {
            _gameEventReceiver.OnScoreChanged(score);
        }

        public void OnCircleMoveSpeedChanged(float newSpeed)
        {
            _gameEventReceiver.OnCircleMoveSpeedChanged(newSpeed);
        }

        public void OnMaxCircleCountChanged(int newCircleCount)
        {
            _gameEventReceiver.OnMaxCircleCountChanged(newCircleCount);
        }

        public void OnMaxTravelTimeChanged(float maxTravelTime)
        {
            _gameEventReceiver.OnMaxTravelTimeChanged(maxTravelTime);
        }

        public void OnPlayAnimationFinished()
        {
            _gameEventReceiver.OnPlayAnimationFinished();
        }

        public void OnFailed(Vector3 intersectionPoint)
        {
            _gameEventReceiver.OnFailed(intersectionPoint);
        }

        public void OnFailed(int score)
        {
            _gameEventReceiver.OnFailed(score);
        }
    }
}
