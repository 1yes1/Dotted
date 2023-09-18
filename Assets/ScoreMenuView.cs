using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dotted
{
    public class ScoreMenuView : UIView
    {

        [SerializeField]
        private TextMeshProUGUI _scoreText;

        private void OnEnable()
        {
            GameEventReceiver.OnScoreChangedEvent += OnScoreChanged;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnScoreChangedEvent -= OnScoreChanged;
        }

        public override void Initialize()
        {
            _scoreText.text = "0";
        }

        private void UpdateScore(int score)
        {
            _scoreText.text = score.ToString();
        }


        private void OnScoreChanged(int score)
        {
            UpdateScore(score);
        }

    }
}
