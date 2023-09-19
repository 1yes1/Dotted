using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dotted
{
    public class TestTools : MonoBehaviour
    {
        private static TestTools _instance;

        [SerializeField] private Button _decreaseMoveSpeedButton;
        [SerializeField] private Button _increaseMoveSpeedButton;

        [SerializeField] private Button _addCirclesButton;
        [SerializeField] private Button _autoCreateChainButton;

        private CircleController _circleController;

        public int addCircleCount = 0;
        public float addMoveSpeed = 0;
        private float _noIntersectionTime = 0;

        private int _tapCount = 0;
        private float _tapTimer = 0;
        public bool CanIntersect => _noIntersectionTime < 0;

        public static TestTools Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;

            _circleController = FindObjectOfType<CircleController>();

            _decreaseMoveSpeedButton.onClick.AddListener(DecreaseMoveSpeed);
            _increaseMoveSpeedButton.onClick.AddListener(IncreaseMoveSpeed);

            _addCirclesButton.onClick.AddListener(AddCircle);
            _autoCreateChainButton.onClick.AddListener(MakeAChain);

        }

        private void Update()
        {
            if(_noIntersectionTime > 0 && GameManager.Instance.IsGamePlaying)
                _noIntersectionTime -= Time.deltaTime;

            if(InputManager.TouchDown)
            {
                if (InputManager.TouchPosition.y <= Screen.height * 0.25f)
                    _tapCount++;

                if(_tapCount == 3)
                    ToggleTestTools();

                _tapTimer = 0.5f;
            }

            if(_tapTimer > 0)
                _tapTimer -= Time.deltaTime;
            else
                _tapCount = 0;
        }

        private void ToggleTestTools()
        {
            bool isOpen = _addCirclesButton.isActiveAndEnabled;

            _addCirclesButton.gameObject.SetActive(!isOpen);
            _autoCreateChainButton.gameObject.SetActive(!isOpen);
            _decreaseMoveSpeedButton.gameObject.SetActive(!isOpen);
            _increaseMoveSpeedButton.gameObject.SetActive(!isOpen);
            
            _tapCount = 0;
        }

        private void IncreaseMoveSpeed()
        {
            addMoveSpeed += 0.1f;
            SetMoveSpeed(0.1f);
        }

        private void DecreaseMoveSpeed()
        {
            addMoveSpeed -= 0.1f;
            addMoveSpeed = (addMoveSpeed < 0) ? 0 : addMoveSpeed;
            SetMoveSpeed(-0.1f);
        }

        private void SetMoveSpeed(float addMoveSpeed)
        {
            GameManager.Instance.TEST_AddMoveSpeed(addMoveSpeed);
        }

        private void AddCircle()
        {
            GameManager.Instance.TEST_AddCircle(5);
            //addCircleCount += 5;
            //addCircleCount = Mathf.Abs(addCircleCount);
        }

        private void MakeAChain()
        {
            _circleController.TEST_StartMakeAChain();
            _noIntersectionTime = 4;
        }

    }
}
