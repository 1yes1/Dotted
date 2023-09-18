using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dotted
{
    public class MainMenuView : UIView
    {
        [SerializeField] private PlayButton _playButton;

        private void OnEnable()
        {
            GameEventReceiver.OnPlayAnimationFinishedEvent += OnPlayAnimationFinished;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnPlayAnimationFinishedEvent -= OnPlayAnimationFinished;
        }


        public override void Initialize()
        {

        }

        private void OnPlayAnimationFinished()
        {
            Hide();
        }

    }
}
