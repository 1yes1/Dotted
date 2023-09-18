using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        [SerializeField]
        private DefaultGameProperties _gameProperties;

        [SerializeField]
        private ScoreManager _scoreManager;

        private bool _isLevelFailed = false;
        private float _circleMoveSpeed = 0;
        private float _circleMaxTravelTime = 0;
        private int _maxCircleCount = 0;
        
        public static GameManager Instance => _instance;
        public static DefaultGameProperties DefaultGameProperties => Instance._gameProperties;

        public bool IsLevelFailed => _isLevelFailed;

        public GameEventCaller GameEventCaller { get; private set; }
        public GameEventReceiver GameEventReceiver { get; private set; }


        private void OnEnable()
        {
            GameEventReceiver.OnScoreChangedEvent += OnScoreChanged;
            GameEventReceiver.OnPlayAnimationFinishedEvent += Play;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnScoreChangedEvent -= OnScoreChanged;
            GameEventReceiver.OnPlayAnimationFinishedEvent -= Play;
        }


        private void Awake()
        {
            if (_instance == null)
                _instance = this;

            Initialize();
        }

        private void Initialize()
        {
            this.GameEventReceiver = new GameEventReceiver();
            this.GameEventCaller = new GameEventCaller(GameEventReceiver);
        }

        void Start()
        {
            _circleMoveSpeed = DefaultGameProperties.MoveSpeed;
            _circleMaxTravelTime = DefaultGameProperties.MaxTravelTime;
            _maxCircleCount = DefaultGameProperties.MaxCircleCount;
        }

        public void Play()
        {
            ScoreMenuView scoreMenuView = UIManager.GetView<ScoreMenuView>();
            PauseMenuView pauseMenuView = UIManager.GetView<PauseMenuView>();
            scoreMenuView.Show();
            pauseMenuView.Show();
        }

        public void Retry()
        {

        }

        public void Resume()
        {

        }

        private void OnScoreChanged(int score)
        {
            float newSpeed = GetCircleMoveSpeed();
            //print("New Speed: " + newSpeed);
            if (newSpeed > _circleMoveSpeed)
            {
                GameEventCaller.OnCircleMoveSpeedChanged(newSpeed);
                _circleMoveSpeed = newSpeed;
            }

            int maxCircleCount = GetMaxCircleCount();
            if(maxCircleCount > _maxCircleCount)
            {
                GameEventCaller.OnMaxCircleCountChanged(maxCircleCount);
                _maxCircleCount = maxCircleCount;
            }
            //print("New Max Circle Count: " + maxCircleCount);

            float maxTravelTime = GetMaxTravelTime();
            if(maxTravelTime < _circleMaxTravelTime)
            {
                GameEventCaller.OnMaxTravelTimeChanged(maxTravelTime);
                _circleMaxTravelTime = maxTravelTime;
            }
            //print("New Max Circle Count: " + maxTravelTime);

        }

        public void LevelFailed(Vector3 intersectionPoint)
        {
            _isLevelFailed = true;
            
            GameEventCaller.OnFailed();
            GameEventCaller.OnFailed(intersectionPoint);
            GameEventCaller.OnFailed(_scoreManager.Score);
        }

        private float GetCircleMoveSpeed()
        {
            return DefaultGameProperties.MoveSpeed + Mathf.CeilToInt(_scoreManager.Score / DefaultGameProperties.AddMultiplierScore) * DefaultGameProperties.MoveSpeedMultiplier;
        }

        private int GetMaxCircleCount()
        {
            return Mathf.CeilToInt(DefaultGameProperties.MaxCircleCount + Mathf.CeilToInt(_scoreManager.Score / DefaultGameProperties.AddMultiplierScore) * DefaultGameProperties.MaxCircleMultiplier);
        }

        private float GetMaxTravelTime()
        {
            return DefaultGameProperties.MaxTravelTime - Mathf.CeilToInt(_scoreManager.Score / DefaultGameProperties.AddMultiplierScore) * DefaultGameProperties.MaxTravelTimeMultiplier;
        }
    }
}

