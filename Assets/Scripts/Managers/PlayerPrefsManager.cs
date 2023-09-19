using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dotted
{
    public class PlayerPrefsManager:MonoBehaviour
    {
        private static PlayerPrefsManager _instance;

        private int _maxScore;

        public static PlayerPrefsManager Instance => _instance;

        public int MaxScore
        {
            get { return _maxScore; }
            set { PlayerPrefs.SetInt(nameof(MaxScore), _maxScore = value); }
        }

        private void OnEnable()
        {
            GameEventReceiver.OnFailedScoreEvent += OnFailed;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnFailedScoreEvent -= OnFailed;
        }

        private void Awake()
        {
            if(_instance == null)
                _instance = this;

            GetPrefs();
        }


        private void OnFailed(int score)
        {
            if (score > _maxScore)
            {
                MaxScore = score;
                GameEventCaller.Instance.OnHighestScore(MaxScore);
            }
        }

        private void GetPrefs()
        {
            _maxScore = PlayerPrefs.GetInt("MaxScore");
        }


    }
}
