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

        private Animation _animation;


        public override void Initialize()
        {
            _animation = GetComponent<Animation>();
        }

        private void OnEnable()
        {
            _retryButton.onClick.RemoveAllListeners();
            _retryButton.onClick.AddListener(GameManager.Instance.Retry);
        }

        public void SetScore(int score)
        {
            _scoreText.text = score.ToString();
        }

    }
}
