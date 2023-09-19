using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        [SerializeField] private DefaultGameProperties _gameProperties;

        [SerializeField] private ScoreManager _scoreManager;

        private bool _isGameFailed = false;

        private bool _isGameStarted = false;

        private bool _isGamePlaying = false;

        private float _circleMoveSpeed = 0;

        private float _circleMaxTravelTime = 0;

        private float _circleCreatingFrequency = 0;

        private int _maxCircleCount = 0;

        public static GameManager Instance => _instance;

        public static DefaultGameProperties DefaultGameProperties => Instance._gameProperties;

        public bool IsGameFailed => _isGameFailed;

        public bool IsGameStarted => _isGameStarted;

        public bool IsGamePlaying => _isGamePlaying;

        public GameEventCaller GameEventCaller { get; private set; }

        public GameEventReceiver GameEventReceiver { get; private set; }

        public int Score => _scoreManager.Score;

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
            ResetGameProperties();
        }

        public void ResetGameProperties()
        {
            _circleMoveSpeed = DefaultGameProperties.MoveSpeed;
            _circleMaxTravelTime = DefaultGameProperties.MaxTravelTime;
            _maxCircleCount = DefaultGameProperties.MaxCircleCount;
            _circleCreatingFrequency = DefaultGameProperties.CircleCreatingFrequency;
        }

        public void Play()
        {
            ScoreMenuView scoreMenuView = UIManager.GetView<ScoreMenuView>();
            PauseMenuView pauseMenuView = UIManager.GetView<PauseMenuView>();
            scoreMenuView.Show();
            pauseMenuView.Show();

            _isGameStarted = true;
            _isGamePlaying = true;

            GameEventCaller.OnGameStarted();
        }

        public void Restart()
        {
            _isGameFailed = false;
            ResetGameProperties();
            GameEventCaller.OnCircleMoveSpeedChanged(_circleMoveSpeed);
            GameEventCaller.OnMaxCircleCountChanged(_maxCircleCount);
            GameEventCaller.OnMaxTravelTimeChanged(_circleMaxTravelTime);
            GameEventCaller.OnCircleCreatingFrequencyChanged(_circleCreatingFrequency);

            FailedMenuView failedMenuView = UIManager.GetView<FailedMenuView>();
            failedMenuView.Hide();
            GameEventCaller.OnGameRestarted();
        }

        public void Pause()
        {
            GameEventCaller.OnGamePaused();
            _isGamePlaying = false;
        }

        public void Resume()
        {
            GameEventCaller.OnGameResumed();
            _isGamePlaying = true;
        }

        private void OnScoreChanged(int score, Vector3? lastCirclePosition,float multipliers)
        {
            CheckCircleMoveSpeed();
            CheckMaxCircleCount();
            CheckMaxTravelTime();
            CheckCircleCreatingFrequency();
        }

        private void CheckCircleMoveSpeed()
        {
            float newSpeed = GetCircleMoveSpeed();
            //print("New Speed: " + newSpeed);
            if (newSpeed > _circleMoveSpeed)
            {
                newSpeed = (newSpeed > DefaultGameProperties.LimMaxMoveSpeed) ? DefaultGameProperties.LimMaxMoveSpeed : newSpeed;

                _circleMoveSpeed = newSpeed + TestTools.Instance.addMoveSpeed;

                GameEventCaller.OnCircleMoveSpeedChanged(_circleMoveSpeed);
            }

        }

        private void CheckMaxCircleCount()
        {
            int maxCircleCount = GetMaxCircleCount();
            if (maxCircleCount > _maxCircleCount)
            {
                maxCircleCount = (maxCircleCount > DefaultGameProperties.LimMaxCircleCount) ? DefaultGameProperties.LimMaxCircleCount : maxCircleCount;

                GameEventCaller.OnMaxCircleCountChanged(maxCircleCount);
                _maxCircleCount = maxCircleCount;
            }
        }

        private void CheckMaxTravelTime()
        {
            float maxTravelTime = GetMaxTravelTime();
            if (maxTravelTime < _circleMaxTravelTime)
            {
                maxTravelTime = (maxTravelTime < DefaultGameProperties.LimMinTravelTime) ? DefaultGameProperties.LimMinTravelTime : maxTravelTime;

                GameEventCaller.OnMaxTravelTimeChanged(maxTravelTime);
                _circleMaxTravelTime = maxTravelTime;
            }
        }

        private void CheckCircleCreatingFrequency()
        {
            float circleCreatingFrequency = GetCircleCreatingFrequency();
            if (circleCreatingFrequency < _circleCreatingFrequency)
            {
                circleCreatingFrequency = (circleCreatingFrequency < DefaultGameProperties.LimMinCircleCreatingFrequency) ? DefaultGameProperties.LimMinCircleCreatingFrequency : circleCreatingFrequency;

                GameEventCaller.OnCircleCreatingFrequencyChanged(circleCreatingFrequency);
                _circleCreatingFrequency = circleCreatingFrequency;
            }
        }

        public void LevelFailed(Vector3 intersectionPoint)
        {
            _isGameFailed = true;
            
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

        private float GetCircleCreatingFrequency()
        {
            return DefaultGameProperties.CircleCreatingFrequency - Mathf.CeilToInt(_scoreManager.Score / DefaultGameProperties.AddMultiplierScore) * DefaultGameProperties.CircleCreatingFrequencyMultiplier;
        }


        public void TEST_AddMoveSpeed(float addMoveSpeed)
        {
            float newSpeed = _circleMoveSpeed + addMoveSpeed;

            newSpeed = (newSpeed > DefaultGameProperties.LimMaxMoveSpeed) ? DefaultGameProperties.LimMaxMoveSpeed : newSpeed;
            newSpeed = (newSpeed < 0) ? 0 : newSpeed;

            GameEventCaller.OnCircleMoveSpeedChanged(newSpeed);
            _circleMoveSpeed = newSpeed;
        }

        public void TEST_AddCircle(int circle)
        {
            int maxCircleCount = _maxCircleCount + circle;

            maxCircleCount = (maxCircleCount > DefaultGameProperties.LimMaxCircleCount) ? DefaultGameProperties.LimMaxCircleCount : maxCircleCount;

            GameEventCaller.OnMaxCircleCountChanged(maxCircleCount);
            _maxCircleCount = maxCircleCount;
        }

    }
}

