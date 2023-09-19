using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dotted
{
    public interface IGameEvents
    {
        public void OnGameStarted();

        public void OnFailed();

        public void OnFailed(Vector3 intersectionPoint);
        
        public void OnFailed(int score);

        public void OnGameRestarted();

        public void OnGamePaused();

        public void OnGameResumed();

        public void BeforeOnChainCompleted(List<Dot> circles);

        public void OnChainCompleted(List<Dot> circles);

        public void OnCircleSelected(Circle circle);

        public void OnScoreChanged(int score,Vector3? lastCirclePosition);

        public void OnCircleMoveSpeedChanged(float newSpeed);

        public void OnMaxCircleCountChanged(int newCircleCount);

        public void OnMaxTravelTimeChanged(float maxTravelTime);

        public void OnCircleCreatingFrequencyChanged(float circleCreatingFrequency);

        public void OnPlayAnimationFinished();

        public void OnHighestScore(int highestScore);

    }
}
