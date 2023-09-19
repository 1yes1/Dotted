using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Dotted
{
    public class PlayButton : MonoBehaviour
    {
        private Button _button;
        private Animation _animation;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _animation = GetComponent<Animation>();
        }

        private void Start()
        {
            AddListener(OnPlayClicked);
        }

        public void AddListener(UnityAction unityAction)
        {
            _button.onClick.AddListener(unityAction);
        }

        private void OnPlayClicked()
        {
            _animation.Play("PlayAnimation");
        }

        private void OnPlayAnimationFinished()
        {
            GameEventCaller.Instance.OnPlayAnimationFinished();
        }
    }
}
