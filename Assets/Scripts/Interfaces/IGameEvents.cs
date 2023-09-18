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
        public void OnFailed();

        public void OnFailed(Vector3 intersectionPoint);
        
        public void OnFailed(int score);

        public void OnChainCompleted(List<Dot> circles);

        public void OnCircleSelected(Circle circle);

        public void OnScoreChanged(int score);

        public void OnCircleMoveSpeedChanged(float newSpeed);

        public void OnMaxCircleCountChanged(int newCircleCount);

        public void OnMaxTravelTimeChanged(float maxTravelTime);

        public void OnPlayAnimationFinished();

    }
}
