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
            GameEventReceiver.OnChainCompletedEvent += OnChainCompleted;    
        }

        private void OnDisable()
        {
            GameEventReceiver.OnChainCompletedEvent -= OnChainCompleted;
        }

        private void OnChainCompleted(List<Dot> obj)
        {
            //Pointer ı çıkartıyoruz
            int circleCount = obj.Count - 1;

            //print(circleCount);

            int addScore = CalculateScore(circleCount);
            _score += addScore;

            GameEventCaller.Instance.OnScoreChanged(_score);

            print("Add Score: " + addScore);
        }

        private int CalculateScore(int circleCount)
        {
            return Mathf.CeilToInt(Mathf.Pow(GameManager.DefaultGameProperties.StartScore * circleCount, GameManager.DefaultGameProperties.ScoreMultiplier));
        }

    }
}
