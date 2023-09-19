using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dotted
{
    public class DynamicScoreText : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;
        private Animation _animation;

        private const string QuickFadeAnimation = "QuickFade";
        private const string SlowFadeAnimation = "SlowFade";

        private void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
            _animation = GetComponent<Animation>();
        }

        public void PlayQuickFadeAnimation()
        {
            _animation.Play(QuickFadeAnimation);
            Invoke(nameof(OnAnimationEnded), 1f);
        }

        public void PlaySlowFadeAnimation()
        {
            _animation.Play(SlowFadeAnimation);
            Invoke(nameof(OnAnimationEnded), 1f);
        }

        public void OnAnimationEnded()
        {
            PoolManager.GetPool<DynamicScoreText>().Push(this);
        }

        public void SetText(string text)
        {
            _textMesh.text = text;
        }

    }
}
