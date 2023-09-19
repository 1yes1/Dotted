using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Dotted
{
    public class ScoreManager:MonoBehaviour
    {
        private int _score = 0;

        public int Score => _score;

        private void OnEnable()
        {
            GameEventReceiver.BeforeOnChainCompletedEvent += OnChainCompleted;
            GameEventReceiver.OnGameRestartedEvent += OnGameRestarted;
        }

        private void OnDisable()
        {
            GameEventReceiver.BeforeOnChainCompletedEvent -= OnChainCompleted;
            GameEventReceiver.OnGameRestartedEvent -= OnGameRestarted;
        }

        private void OnChainCompleted(List<Dot> obj)
        {
            int circleCount = obj.Count - 1;
            int multipliers = 1;

            for (int i = 0; i < obj.Count - 1; i++)
            {
                Circle circle = ((Circle)obj[i]);
                if (circle is MultiplierCircle)
                    multipliers *= ((MultiplierCircle)circle).ScoreMultiplier;
            }
            int addScore = CalculateScore(circleCount, multipliers);
            _score += addScore;
            GameEventCaller.Instance.OnScoreChanged(_score, obj[obj.Count-1].transform.position,multipliers);

            //print("Add Score: " + addScore);
        }

        private int CalculateScore(int circleCount,int multipliers)
        {
            return Mathf.CeilToInt(Mathf.Pow(GameManager.DefaultGameProperties.StartScore * circleCount, GameManager.DefaultGameProperties.ScoreMultiplier)) * multipliers;
        }

        private void OnGameRestarted()
        {
            _score = 0;
            GameEventCaller.Instance.OnScoreChanged(_score);
        }


    }
}
