using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dotted
{
    public class CircleController : MonoBehaviour
    {
        [SerializeField]
        private Circle _circlePrefab;

        [SerializeField]
        private Vector2 _padding;

        private List<Circle> _circles;

        private float _timer;

        private float _moveSpeed;

        private float _maxCircleCount;

        private float _maxTravelTime;

        private void OnEnable()
        {
            GameEventReceiver.OnChainCompletedEvent += OnChainCompleted;
            GameEventReceiver.OnFailedEvent += OnFailed;
            GameEventReceiver.OnCircleMoveSpeedChangedEvent += OnCircleMoveSpeedChanged;
            GameEventReceiver.OnMaxCircleCountChangedEvent += OnMaxCircleCountChanged;
            GameEventReceiver.OnMaxTravelTimeChangedEvent += OnMaxTravelTimeChanged;
        }


        private void OnDisable()
        {
            GameEventReceiver.OnChainCompletedEvent -= OnChainCompleted;
            GameEventReceiver.OnFailedEvent -= OnFailed;
            GameEventReceiver.OnCircleMoveSpeedChangedEvent -= OnCircleMoveSpeedChanged;
            GameEventReceiver.OnMaxCircleCountChangedEvent -= OnMaxCircleCountChanged;
        }

        private void OnChainCompleted(List<Dot> obj)
        {
            _circles.RemoveAll(a => obj.Contains(a));
        }

        void Awake()
        {
            _circles = new List<Circle>();
        }

        private void Start()
        {
            _maxCircleCount = GameManager.DefaultGameProperties.MaxCircleCount;
            _maxTravelTime = GameManager.DefaultGameProperties.MaxTravelTime;
            _moveSpeed = GameManager.DefaultGameProperties.MoveSpeed;

            StartCoroutine(nameof(InitializeCircles));
            _timer = GameManager.DefaultGameProperties.CircleCreatingFrequency;
        }

        void Update()
        {
            if (!GameManager.Instance.IsLevelFailed)
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

        private IEnumerator InitializeCircles()
        {
            for (int i = 0; i < GameManager.DefaultGameProperties.StartCircleCount; i++)
            {
                CreateCircle();
                yield return new WaitForSeconds(GameManager.DefaultGameProperties.CircleCreatingFrequencyAtStart);
            }
        }

        private void CreateCircle()
        {
            Circle circle = Instantiate(_circlePrefab, transform);
            circle.name = "Circle (" + _circles.Count + ")";
            circle.transform.position = GetRandomPosition();
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

            pos.x = Random.Range(_padding.x + minX, maxX - _padding.x);
            pos.y = Random.Range(_padding.x + minY, maxY- _padding.y);
            return pos;
        }

        private void OnFailed()
        {
            
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
            _maxCircleCount = newCircleCount;
        }

        private void OnMaxTravelTimeChanged(float time)
        {
            for (int i = 0; i < _circles.Count; i++)
            {
                _circles[i].SetMaxTravelTime(time);
            }
            _maxTravelTime = time;
        }

    }
}
