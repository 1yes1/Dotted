using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dotted
{
    public class ScoreMenuView : UIView
    {

        [SerializeField] private TextMeshProUGUI _scoreText;

        [SerializeField] private TextMeshProUGUI _scoreDynamicText;

        [SerializeField] private Vector3 _dynamicScoreOffset;

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

        private void OnScoreChanged(int score,Vector3? lastCirclePosition)
        {
            int oldScore = int.Parse(_scoreText.text);
            int addedScore = score - oldScore;
            PlayAddedScoreAnimation(addedScore,lastCirclePosition);

            UpdateScore(score);
        }

        private void UpdateScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void PlayAddedScoreAnimation(int addedScore,Vector3? lastCirclePosition)
        {
            if (lastCirclePosition == null)
                return;

            _scoreDynamicText.text = "+"+addedScore.ToString();
            _scoreDynamicText.GetComponent<Animation>().Play();
            _scoreDynamicText.transform.position = Camera.main.WorldToScreenPoint((Vector3)lastCirclePosition) + _dynamicScoreOffset;
        }

    }
}
