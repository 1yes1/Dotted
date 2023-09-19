using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dotted
{
    public class GameEventReceiver : IGameEvents
    {
        public static event Action<List<Dot>> OnChainCompletedEvent;

        public static event Action<List<Dot>> BeforeOnChainCompletedEvent;

        public static event Action OnFailedEvent;

        public static event Action<Vector3> OnFailedIntersectionEvent;

        public static event Action<int> OnFailedScoreEvent;

        public static event Action<Circle> OnCircleSelectedEvent;

        public static event Action<int, Vector3?,float> OnScoreChangedEvent;

        public static event Action<float> OnCircleMoveSpeedChangedEvent;

        public static event Action<int> OnMaxCircleCountChangedEvent;

        public static event Action<float> OnMaxTravelTimeChangedEvent;

        public static event Action<float> OnCircleCreatingFrequencyChangedEvent;

        public static event Action OnPlayAnimationFinishedEvent;

        public static event Action OnGameStartedEvent;

        public static event Action OnGameRestartedEvent;

        public static event Action OnGamePausedEvent;

        public static event Action OnGameResumedEvent;

        public static event Action<int> OnHighestScoreEvent;


        public void OnChainCompleted(List<Dot> circles)
        {
            OnChainCompletedEvent?.Invoke(circles);
        }

        public void OnCircleSelected(Circle circle)
        {
            OnCircleSelectedEvent.Invoke(circle);
        }

        public void OnFailed()
        {
            OnFailedEvent?.Invoke();
        }

        public void OnScoreChanged(int score, Vector3? lastCirclePosition = null, float multipliers = 0)
        {
            OnScoreChangedEvent?.Invoke(score, lastCirclePosition, multipliers);
        }
        public void OnCircleMoveSpeedChanged(float newSpeed)
        {
            OnCircleMoveSpeedChangedEvent?.Invoke(newSpeed);
        }

        public void OnMaxCircleCountChanged(int newCircleCount)
        {
            OnMaxCircleCountChangedEvent?.Invoke(newCircleCount);
        }

        public void OnMaxTravelTimeChanged(float maxTravelTime)
        {
            OnMaxTravelTimeChangedEvent?.Invoke(maxTravelTime);
        }

        public void OnPlayAnimationFinished()
        {
            OnPlayAnimationFinishedEvent?.Invoke();
        }

        public void OnFailed(Vector3 intersectionPoint)
        {
            OnFailedIntersectionEvent?.Invoke(intersectionPoint);
        }

        public void OnFailed(int score)
        {
            OnFailedScoreEvent?.Invoke(score);
        }

        public void OnGameStarted()
        {
            OnGameStartedEvent?.Invoke();
        }

        public void OnGameRestarted()
        {
            OnGameRestartedEvent?.Invoke();
        }

        public void OnGamePaused()
        {
            OnGamePausedEvent?.Invoke();
        }

        public void OnGameResumed()
        {
            OnGameResumedEvent?.Invoke();
        }

        public void OnCircleCreatingFrequencyChanged(float circleCreatingFrequency)
        {
            OnCircleCreatingFrequencyChangedEvent?.Invoke(circleCreatingFrequency);
        }

        public void OnHighestScore(int highestScore)
        {
            OnHighestScoreEvent?.Invoke(highestScore);
        }

        public void BeforeOnChainCompleted(List<Dot> circles)
        {
            BeforeOnChainCompletedEvent?.Invoke(circles);
        }
    }
}
