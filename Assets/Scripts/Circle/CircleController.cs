using Dotted.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dotted
{
    public class CircleController : MonoBehaviour
    {
        [SerializeField] private Circle _circlePrefab;

        [SerializeField] private MultiplierCircle _multiplierCirclePrefab;

        [SerializeField] private Vector2 _creatingPadding;

        private ObjectPool<Circle> _circlePool;

        private ObjectPool<MultiplierCircle> _multiplierCirclePool;

        private List<Circle> _circles;

        private List<Circle> _selectedCircles;

        private float _timer;

        private float _moveSpeed;

        private float _maxCircleCount;

        private float _maxTravelTime;

        private float _circleCreatingFrequency;

        private bool _isInitialized= false;

        private bool _canCreateCircles = false;

        private Coroutine TEST_coroutine;

        private void OnEnable()
        {
            GameEventReceiver.OnChainCompletedEvent += OnChainCompleted;
            GameEventReceiver.OnCircleSelectedEvent += OnCircleSelected;
            GameEventReceiver.OnFailedEvent += OnFailed;
            GameEventReceiver.OnCircleMoveSpeedChangedEvent += OnCircleMoveSpeedChanged;
            GameEventReceiver.OnMaxCircleCountChangedEvent += OnMaxCircleCountChanged;
            GameEventReceiver.OnMaxTravelTimeChangedEvent += OnMaxTravelTimeChanged;
            GameEventReceiver.OnCircleCreatingFrequencyChangedEvent += OnCircleCreatingFrequencyChanged;
            GameEventReceiver.OnGameStartedEvent += OnGameStarted;
            GameEventReceiver.OnGameRestartedEvent += OnGameRestarted;
            GameEventReceiver.OnGamePausedEvent += OnGamePaused;
            GameEventReceiver.OnGameResumedEvent += OnGameResumed;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnChainCompletedEvent -= OnChainCompleted;
            GameEventReceiver.OnFailedEvent -= OnFailed;
            GameEventReceiver.OnCircleMoveSpeedChangedEvent -= OnCircleMoveSpeedChanged;
            GameEventReceiver.OnMaxCircleCountChangedEvent -= OnMaxCircleCountChanged;
            GameEventReceiver.OnMaxTravelTimeChangedEvent -= OnMaxTravelTimeChanged;
            GameEventReceiver.OnCircleCreatingFrequencyChangedEvent -= OnCircleCreatingFrequencyChanged;
            GameEventReceiver.OnGameStartedEvent -= OnGameStarted;
            GameEventReceiver.OnGameRestartedEvent -= OnGameRestarted;
        }

        void Awake()
        {
            _circles = new List<Circle>();
            _selectedCircles = new List<Circle>();
        }

        private void Start()
        {
            _circlePool = PoolManager.CreateObjectPool<Circle>(_circlePrefab);
            _multiplierCirclePool = PoolManager.CreateObjectPool<MultiplierCircle>(_multiplierCirclePrefab);
            _circlePool.CreateObjects(GameManager.DefaultGameProperties.MaxCircleCount);

            _maxCircleCount = GameManager.DefaultGameProperties.MaxCircleCount;
            _maxTravelTime = GameManager.DefaultGameProperties.MaxTravelTime;
            _moveSpeed = GameManager.DefaultGameProperties.MoveSpeed;
            _circleCreatingFrequency = GameManager.DefaultGameProperties.CircleCreatingFrequency;
            _timer = _circleCreatingFrequency;
        }

        void Update()
        {
            if (GameManager.Instance.IsGameFailed || !GameManager.Instance.IsGameStarted)
                return;

            if (_isInitialized && _canCreateCircles)
                CheckCreating();
        }

        private void CheckCreating()
        {
            _timer -= Time.deltaTime;
            if(_circles.Count < _maxCircleCount && _timer <= 0)
            {
                CreateCircle();
                _timer = GameManager.DefaultGameProperties.CircleCreatingFrequency;
            }
        }

        private IEnumerator InitializeCircles(float delay = 0)
        {
            yield return new WaitForSeconds(delay);

            for (int i = 0; i < GameManager.DefaultGameProperties.StartCircleCount; i++)
            {
                CreateCircle();
                yield return new WaitForSeconds(GameManager.DefaultGameProperties.CircleCreatingFrequencyAtStart);
            }
            _isInitialized = true;
            _canCreateCircles = true;
        }

        private void CreateCircle()
        {
            RareMultiplier rareMultiplier;
            Circle circle;

            if (GetRareMultiplier(out rareMultiplier))
            {
                //print("Create Rare: "+ rareMultiplier.percentage);
                circle = _multiplierCirclePool.Pop();
                ((MultiplierCircle)(circle)).SetScoreMultiplier(rareMultiplier.multiplier);
            }
            else
                circle = _circlePool.Pop();

            circle.transform.position = GetRandomPosition();
            circle.name = "Circle (" + _circles.Count + ")";
            circle.Initialize(this, _moveSpeed,_maxTravelTime);
            _circles.Add(circle);
        }

        public Vector2 GetRandomPosition()
        {
            Vector2 pos = new Vector2();

            Vector3 bounds = Camera.main.ScreenToWorldPoint(Vector2.one);
            float maxX = -bounds.x;
            float minX = bounds.x;
            float maxY = -bounds.y;
            float minY = bounds.y;

            pos.x = Random.Range(_creatingPadding.x + minX, maxX - _creatingPadding.x);
            pos.y = Random.Range(_creatingPadding.x + minY, maxY- _creatingPadding.y);
            return pos;
        }

        private void OnCircleSelected(Circle obj)
        {
            _selectedCircles.Add(obj);
        }

        private void OnChainCompleted(List<Dot> obj)
        {
            for (int i = 0; i < obj.Count - 1; i++)
            {
                if (_circles.Contains((Circle)obj[i]))
                {
                    Circle circle = (Circle)obj[i];
                    circle.PlayParticle();
                    circle.PushSelf();
                }
            }

            _circles.RemoveAll(a => obj.Contains(a));
            _selectedCircles.Clear();
        }

        private void OnFailed()
        {
            _canCreateCircles = false;
        }

        private void OnCircleMoveSpeedChanged(float newSpeed)
        {
            for (int i = 0; i < _circles.Count; i++)
            {
                _circles[i].SetMoveSpeed(newSpeed);
            }
            _moveSpeed = newSpeed;
        }

        private void OnMaxCircleCountChanged(int newCircleCount)
        {
            _maxCircleCount = newCircleCount + TestTools.Instance.addCircleCount;
        }

        private void OnMaxTravelTimeChanged(float time)
        {
            for (int i = 0; i < _circles.Count; i++)
            {
                _circles[i].SetMaxTravelTime(time);
            }
            _maxTravelTime = time;
        }

        private void OnCircleCreatingFrequencyChanged(float circleCreatingFrequency)
        {
            _circleCreatingFrequency = circleCreatingFrequency;
        }


        private void OnGameRestarted()
        {
            for (int i = 0; i < _circles.Count; i++)
            {
                _circles[i].IsSelectable = false;
                _circles[i].PlayParticle();
                _circles[i].PushSelf();
            }
            _circles.Clear();
            StartCoroutine(nameof(InitializeCircles), 1);
        }

        private void OnGameStarted()
        {
            StartCoroutine(nameof(InitializeCircles), 0);
        }


        private void OnGameResumed()
        {
            for (int i = 0; i < _circles.Count; i++)
            {
                _circles[i].StartMoving();
            }
            _canCreateCircles = true;
        }

        private void OnGamePaused()
        {
            for (int i = 0; i < _circles.Count; i++)
            {
                _circles[i].StopMoving();
            }
            _canCreateCircles = false;
        }


        private bool GetRareMultiplier(out RareMultiplier rareMultiplier)
        {
            List<RareMultiplier> usables = new List<RareMultiplier>();

            for (int i = GameManager.DefaultGameProperties.RareMultipliers.Count - 1; i >= 0; i--)
            {
                if (GameManager.DefaultGameProperties.RareMultipliers[i].IsUsable(GameManager.Instance.Score))
                {
                    rareMultiplier = GameManager.DefaultGameProperties.RareMultipliers[i];
                    return true;
                }
            }
            rareMultiplier = null;
            return false;
        }

        public void TEST_StartMakeAChain()
        {
            if(TEST_coroutine != null)
            {
                StopCoroutine(TEST_coroutine);
                TEST_coroutine = null;
                _selectedCircles[0].TrySelect();
            }
            else
                TEST_coroutine = StartCoroutine(nameof(TEST_MakeAChain));
        }

        private IEnumerator TEST_MakeAChain()
        {
            for (int i = 0; i < Mathf.Min(_circles.Count, 8); i++)
            {
                _circles[i].TrySelect();
                yield return new WaitForSeconds(0.15f);
            }
            yield return new WaitForSeconds(0.3f);

            if (_selectedCircles.Count > 0)
                _selectedCircles[0].TrySelect();

            TEST_coroutine = null;
        }

    }
}
