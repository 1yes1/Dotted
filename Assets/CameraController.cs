using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 _failedFocusOffset;

        private bool _focusFailed = false;

        private Vector3 _intersectionPoint;

        private void OnEnable()
        {
            GameEventReceiver.OnFailedIntersectionEvent += OnFailed;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnFailedIntersectionEvent -= OnFailed;
        }

        private void OnFailed(Vector3 intersectionPoint)
        {
            _intersectionPoint = intersectionPoint;
            _intersectionPoint.z = transform.position.z;
            _focusFailed = true;
        }

        private void Update()
        {
            if( _focusFailed )
                transform.position = _intersectionPoint + _failedFocusOffset;
        }
    }
}
