using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dotted
{
    public class CircleController : MonoBehaviour
    {
        public static event Action<Circle> OnCircleDownEvent;

        [SerializeField]
        private Circle _circlePrefab;

        [SerializeField]
        private Vector2 _padding;

        private List<Circle> _circles;

        private float _timer;

        void Awake()
        {
            _circles = new List<Circle>();
        }

        private void Start()
        {
            StartCoroutine(nameof(InitializeCircles));
            _timer = GameManager.DefaultGameProperties.CircleCreatingFrequency;
        }

        void Update()
        {
            if (GameManager.Instance.IsGameRunning)
                CheckCreating();
        }


        private void CheckCreating()
        {
            _timer -= Time.deltaTime;
            if(_circles.Count < GameManager.DefaultGameProperties.MaxCircleCount && _timer <= 0)
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
            circle.transform.position = GetRandomPosition();
            circle.Initialize(this);
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


        public void OnCircleDown(Circle circle)
        {
            OnCircleDownEvent?.Invoke(circle);
        }

    }
}
