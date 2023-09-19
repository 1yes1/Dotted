using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dotted
{
    public class FailedMenuView : UIView
    {
        [SerializeField] private Button _retryButton;

        [SerializeField] private TextMeshProUGUI _scoreText;

        [SerializeField] private TextMeshProUGUI _highestScoreText;


        public override void Initialize()
        {
            _highestScoreText.text = "0";
            GameEventReceiver.OnHighestScoreEvent += OnHighestScore;
        }

        private void OnEnable()
        {
            _retryButton.onClick.RemoveAllListeners();
            _retryButton.onClick.AddListener(GameManager.Instance.Restart);
        }

        private void OnHighestScore(int highestScore)
        {
            _highestScoreText.text = highestScore.ToString();
            _highestScoreText.color = Color.yellow;
        }

        public void SetScore(int score)
        {
            _scoreText.text = score.ToString();
            if (PlayerPrefsManager.Instance.MaxScore != score)
                _highestScoreText.color = Color.white;

            _highestScoreText.text = PlayerPrefsManager.Instance.MaxScore.ToString();
        }

    }
}
