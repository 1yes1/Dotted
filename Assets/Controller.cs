using Dotted;
using Dotted.Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private Pointer _pointer;

    [SerializeField]
    private GameObject _startPoint;
    [SerializeField]
    private GameObject _endPoint;
    [SerializeField]
    private GameObject _intersectionPoint;


    private List<Dot> _holdingDots;

    private LineRenderer _lineRenderer;

    private void OnEnable()
    {
        CircleController.OnCircleSelectedEvent += OnCircleSelected;
    }

    private void OnDisable()
    {
        CircleController.OnCircleSelectedEvent -= OnCircleSelected;
    }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _holdingDots = new List<Dot>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        SetPositions();

        if (GameManager.Instance.IsLevelFailed)
            return;

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
            int checkCount = Mathf.Max(_lineRenderer.positionCount - i - 3,0);
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
                    _intersectionPoint.transform.position = intersectionPoint;
                    GameManager.Instance.LevelFailed();
                }
            }
        }
    }


    private bool FindIntersection(ref Vector2 intersectionPoint,Vector2 position1, Vector2 position2, Vector2 otherPosition1, Vector2 otherPosition2)
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


    private void OnCircleSelected(Circle circle)
    {
        if ((_holdingDots.Contains(circle) && _holdingDots[0] != circle) || (_holdingDots.Contains(circle) && _holdingDots[0] == circle && _holdingDots.Count <= 2))
            return;

        Vector3 lastCirclePosition = (_holdingDots.Count > 0) ? _holdingDots[_holdingDots.Count - 1].transform.position : _pointer.transform.position;

        if (_holdingDots.Count == 0 || _holdingDots[_holdingDots.Count -1] != _pointer)
        {
            _holdingDots.Add(_pointer);
            _lineRenderer.positionCount = _holdingDots.Count;
        }

        SetPositions();

        bool isFirst = (_holdingDots.Count == 1) ? true : false;
        _pointer.OnCircleChoosed(lastCirclePosition, circle.transform, OnPointerReachedTarget, isFirst);

    }

    private void SetPositions()
    {
        for (int i = 0; i < _holdingDots.Count; i++)
        {
            _lineRenderer.SetPosition(i, _holdingDots[i].transform.position);
        }
    }

    private void ChainCompleted()
    {
        GameEventCaller.Instance.OnChainCompleted(_holdingDots);

        if (GameManager.Instance.IsLevelFailed)
            return;

        for (int i = 0; i < _holdingDots.Count; i++)
        {
            if(_holdingDots[i] != _pointer)
                Destroy(_holdingDots[i].gameObject);

        }

        _holdingDots.Clear();
        _lineRenderer.positionCount = 0;
    }

    private void OnPointerReachedTarget(Transform circleTransform, bool hasNewTarget)
    {
        Circle circle = circleTransform.GetComponent<Circle>();

        _holdingDots.Remove(_pointer);
        _holdingDots.Add(circle);

        if (hasNewTarget )
            _holdingDots.Add(_pointer);

        _lineRenderer.positionCount = _holdingDots.Count;
        SetPositions();

        circle.OnPointerReached();

        if (_holdingDots[0] ==  circle && _holdingDots.Count > 2)
            ChainCompleted();
    }

}
