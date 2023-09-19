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

        [SerializeField] private DynamicScoreText _dynamicScoreTextPrefab;

        [SerializeField] private Vector3 _dynamicScoreOffset;

        private ObjectPool<DynamicScoreText> _dynamicTextPool;

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
            _dynamicTextPool = PoolManager.CreateObjectPool<DynamicScoreText>(_dynamicScoreTextPrefab);
        }

        private void OnScoreChanged(int score,Vector3? lastCirclePosition, float multipliers)
        {
            int oldScore = int.Parse(_scoreText.text);
            int addedScore = score - oldScore;
            PlayAddedScoreAnimation(addedScore,lastCirclePosition, multipliers);

            UpdateScore(score);
        }

        private void UpdateScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void PlayAddedScoreAnimation(int addedScore,Vector3? lastCirclePosition, float multipliers)
        {
            if (lastCirclePosition == null)
                return;

            if(multipliers > 1)
            {
                CreateDynamicScoreText((addedScore / multipliers).ToString(), (Vector3)lastCirclePosition,true);
                StartCoroutine(CreateDynamicScoreTextDelay("x"+(multipliers).ToString(), (Vector3)lastCirclePosition, true, 0.5f));
                StartCoroutine(CreateDynamicScoreTextDelay("+"+(addedScore).ToString(), (Vector3)lastCirclePosition,false, 1f));
            }
            else
            {
                CreateDynamicScoreText("+"+(addedScore).ToString(), (Vector3)lastCirclePosition);
            }

        }

        private IEnumerator CreateDynamicScoreTextDelay(string text, Vector3 position,bool playQuickFadeAnimation = false, float delay=0)
        {
            yield return new WaitForSeconds(delay);

            CreateDynamicScoreText(text,position, playQuickFadeAnimation);
        }


        private DynamicScoreText CreateDynamicScoreText(string text,Vector3 position,bool playQuickFadeAnimation = false)
        {
            DynamicScoreText textMeshPro = _dynamicTextPool.Pop();
            textMeshPro.transform.SetParent(transform);
            textMeshPro.SetText(text);
            textMeshPro.transform.position = Camera.main.WorldToScreenPoint((Vector3)position) + _dynamicScoreOffset;

            if (playQuickFadeAnimation)
                textMeshPro.PlayQuickFadeAnimation();
            else
                textMeshPro.PlaySlowFadeAnimation();

            return textMeshPro;

        }


    }
}
