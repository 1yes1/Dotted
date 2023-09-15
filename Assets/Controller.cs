using Dotted;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private List<Circle> _holdingCircles;

    private LineRenderer _lineRenderer;

    private void OnEnable()
    {
        CircleController.OnCircleDownEvent += OnCircleDown;
    }

    private void OnDisable()
    {
        CircleController.OnCircleDownEvent -= OnCircleDown;
    }

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _holdingCircles = new List<Circle>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        SetPositions();
    }

    private void OnCircleDown(Circle circle)
    {
        _holdingCircles.Add(circle);
        _lineRenderer.positionCount = _holdingCircles.Count;
        SetPositions();
    }

    private void SetPositions()
    {
        for (int i = 0; i < _holdingCircles.Count; i++)
        {
            _lineRenderer.SetPosition(i, _holdingCircles[i].transform.position);
        }

    }

}
