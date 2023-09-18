using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dotted
{
    public class PauseMenuView : UIView
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private GameObject _pauseMenu;

        private const string ShowPauseMenuAnimation = "ShowPauseMenu";

        public override void Initialize()
        {

        }

        private void Start()
        {
            _pauseButton.onClick.AddListener(ShowPauseMenu);
            _resumeButton.onClick.AddListener(HidePauseMenu);
        }

        private void ShowPauseMenu()
        {
            _pauseMenu.SetActive(true);
            _pauseMenu.GetComponent<Animation>().Play(ShowPauseMenuAnimation);
        }

        private void HidePauseMenu()
        {
            _pauseMenu.SetActive(false);
        }

    }
}
