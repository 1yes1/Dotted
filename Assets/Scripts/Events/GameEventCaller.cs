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

        public void OnScoreChanged(int score, Vector3? lastCirclePosition = null)
        {
            _gameEventReceiver.OnScoreChanged(score, lastCirclePosition);
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

        public void OnGameStarted()
        {
            _gameEventReceiver.OnGameStarted();
        }

        public void OnGameRestarted()
        {
            _gameEventReceiver.OnGameRestarted();
        }

        public void OnGamePaused()
        {
            _gameEventReceiver.OnGamePaused();
        }

        public void OnGameResumed()
        {
            _gameEventReceiver.OnGameResumed();
        }

        public void OnCircleCreatingFrequencyChanged(float circleCreatingFrequency)
        {
            _gameEventReceiver.OnCircleCreatingFrequencyChanged(circleCreatingFrequency);
        }

        public void OnHighestScore(int highestScore)
        {
            _gameEventReceiver.OnHighestScore(highestScore);
        }

        public void BeforeOnChainCompleted(List<Dot> circles)
        {
            _gameEventReceiver.BeforeOnChainCompleted(circles);
        }
    }
}
