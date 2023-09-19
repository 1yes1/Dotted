using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 _focusOffset;
        [SerializeField] private float _focusTime = 1;

        private Vector3 _intersectionPoint;
        private Vector3 _startPoint;

        private void OnEnable()
        {
            GameEventReceiver.OnFailedIntersectionEvent += OnFailed;
            GameEventReceiver.OnGameRestartedEvent += OnGameRestarted;
        }

        private void OnDisable()
        {
            GameEventReceiver.OnFailedIntersectionEvent -= OnFailed;
            GameEventReceiver.OnGameRestartedEvent -= OnGameRestarted;
        }

        private void OnFailed(Vector3 intersectionPoint)
        {
            _intersectionPoint = intersectionPoint;
            _intersectionPoint.z = transform.position.z;
            _startPoint = transform.position;
            StartCoroutine(FocusTarget());
        }

        IEnumerator FocusTarget()
        {
            float timer = 0;
            while(_focusTime > timer)
            {
                transform.position = Vector3.Lerp(_startPoint, _intersectionPoint + _focusOffset, timer / _focusTime);

                timer += Time.deltaTime;
                yield return null;
            }
        }


        private void OnGameRestarted()
        {
            transform.position = new Vector3(0, 0, transform.position.z);
        }

    }
}
