using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

namespace Dotted
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Pointer _pointer;

        [SerializeField] private TouchPoint _touchPoint;

        [SerializeField] private SpriteRenderer _intersectionPoint;

        private List<Dot> _holdingDots;

        private LineRenderer _lineRenderer;

        public int HoldingDotCount => _holdingDots.Count;

        private void OnEnable()
        {
            GameEventReceiver.OnCircleSelectedEvent += OnCircleSelected;
            GameEventReceiver.OnGameRestartedEvent += OnGameRestarted;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnCircleSelectedEvent -= OnCircleSelected;
            GameEventReceiver.OnGameRestartedEvent -= OnGameRestarted;
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _holdingDots = new List<Dot>();
        }

        private void Start()
        {
            _pointer.Initialize(this);
        }

        private void LateUpdate()
        {
            //if (GameManager.Instance.IsGameFailed || !GameManager.Instance.IsGameStarted)
            //    return;
            
            SetPositions();
        }

        private void Update()
        {
            if (GameManager.Instance.IsGameFailed || !GameManager.Instance.IsGameStarted || !GameManager.Instance.IsGamePlaying)
                return;

            if (InputManager.IsSwiping)
            {
                if (_touchPoint.IsHiding)
                    _touchPoint.Show();

                _touchPoint.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(InputManager.TouchPosition);

            }
            else if(!_touchPoint.IsHiding)
                _touchPoint.Hide();
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.IsGameFailed || !GameManager.Instance.IsGameStarted)
                return;

            if(TestTools.Instance.CanIntersect)
                CheckIntersections();
        }

        private void CheckIntersections()
        {
            int length = Mathf.Max(_lineRenderer.positionCount - 2, 0);
            for (int i = 0; i < length; i++)
            {
                //i max = 5
                //i = 0 için
                //2 kontrol
                //i = 1 için
                //1 kontrol
                //i = 3 için
                //1 kontrol ama geride olduðu için yoksayýlýyor
                //Kendi noktasýný ve öndeki 2 noktayý çýkarýyoruz => -3
                int checkCount = Mathf.Max(_lineRenderer.positionCount - i - 3, 0);
                int startIndex = i + 2;

                Vector2 currentLine1 = _lineRenderer.GetPosition(i);
                Vector2 currentLine2 = _lineRenderer.GetPosition(i + 1);
                for (int j = 0; j < checkCount; j++)
                {
                    Vector2 checkPosition1 = _lineRenderer.GetPosition(startIndex + j);
                    Vector2 checkPosition2 = _lineRenderer.GetPosition((startIndex + j + 1));
                    Vector2 intersectionPoint = Vector2.zero;

                    if (FindIntersection(ref intersectionPoint, currentLine1, currentLine2, checkPosition1, checkPosition2))
                    {
                        print("Line1Index: " + i);
                        print("Line2Index: " + (i + 1));
                        print("Line3Index: " + (startIndex + j));
                        print("Line4Index: " + (startIndex + j + 1));
                        OnIntersection(intersectionPoint);
                    }
                }
            }
        }


        private bool FindIntersection(ref Vector2 intersectionPoint, Vector2 position1, Vector2 position2, Vector2 otherPosition1, Vector2 otherPosition2)
        {
            float denom = (position1.x - position2.x) * (otherPosition1.y - otherPosition2.y) - (position1.y - position2.y) * (otherPosition1.x - otherPosition2.x);

            if (denom == 0)
            {
                return false;
            }

            float t = ((position1.x - otherPosition1.x) * (otherPosition1.y - otherPosition2.y) - (position1.y - otherPosition1.y) * (otherPosition1.x - otherPosition2.x)) / denom;
            float u = -((position1.x - position2.x) * (position1.y - otherPosition1.y) - (position1.y - position2.y) * (position1.x - otherPosition1.x)) / denom;

            if (0 <= t && t <= 1 && 0 <= u && u <= 1)
            {
                float intersectionX = position1.x + t * (position2.x - position1.x);
                float intersectionY = position1.y + t * (position2.y - position1.y);
                intersectionPoint = new Vector2(intersectionX, intersectionY);
                return true;
            }
            else
            {
                return false;
            }
        }


        private void OnIntersection(Vector3 intersectionPoint)
        {
            _intersectionPoint.transform.position = intersectionPoint;
            _intersectionPoint.enabled = true;
            GameManager.Instance.LevelFailed(intersectionPoint);
        }


        private void OnCircleSelected(Circle circle)
        {
            //print(circle.name+" bOOL: "+ (_holdingDots.Contains(circle) && _holdingDots[0] == circle && _holdingDots.Count + _pointer.QueueCount <= 2));
            if ((_holdingDots.Contains(circle) && _holdingDots[0] != circle) || (_holdingDots.Contains(circle) && _holdingDots[0] == circle && _holdingDots.Count + _pointer.QueueCount <= 2))
                return;

            Vector3 lastCirclePosition = (_holdingDots.Count > 0) ? _holdingDots[_holdingDots.Count - 1].transform.position : _pointer.transform.position;

            //Ýlki olduðu için bir süre seçilemez
            if (_holdingDots.Count == 0)
                circle.IsSelectable = false;

            if (_holdingDots.Count == 0 || _holdingDots[_holdingDots.Count - 1] != _pointer)
                AddDot(_pointer);

            circle.CircleSelected();

            SetPositions();

            bool isFirst = (_holdingDots.Count == 1) ? true : false;
            _pointer.OnCircleChoosed(lastCirclePosition, circle.transform, OnPointerReachedTarget, isFirst);
        }

        private void SetPositions()
        {
            for (int i = 0; i < _holdingDots.Count; i++)
            {
                if (_holdingDots[i] != null)
                    _lineRenderer.SetPosition(i, _holdingDots[i].transform.position);
                else
                    _holdingDots.RemoveAt(i);
            }
        }

        private void OnPointerReachedTarget(Transform circleTransform, bool hasNewTarget)
        {
            Circle circle = circleTransform.GetComponent<Circle>();

            _holdingDots.Remove(_pointer);

            AddDot(circle);

            if (hasNewTarget)
                AddDot(_pointer);

            //circle.OnPointerReached();

            if (_holdingDots[0] == circle && _holdingDots.Count > 2)
                ChainCompleted();
        }

        private void ChainCompleted()
        {
            if (GameManager.Instance.IsGameFailed)
                return;
            GameEventCaller.Instance.BeforeOnChainCompleted(_holdingDots);

            GameEventCaller.Instance.OnChainCompleted(_holdingDots);

            _holdingDots.Clear();
            _lineRenderer.positionCount = 0;
        }


        private void AddDot(Dot dot)
        {
            _holdingDots.Add(dot);
            _lineRenderer.positionCount = _holdingDots.Count;

            //Eðer seçili circle sayýsý 2 yi geçtiyse ilki seçilebilir olacak
            if (_holdingDots.Count > 2)
                _holdingDots[0].GetComponent<Circle>().IsSelectable = true;

        }

        private void OnGameRestarted()
        {
            _holdingDots.Clear();
            _lineRenderer.positionCount = 0;
            _intersectionPoint.enabled = false;
        }

    }

}