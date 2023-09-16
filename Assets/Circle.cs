using Dotted.Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class Circle : Dot
    {
        private CircleController _circleController;
        private Animation _animation;

        private bool _canMove = false;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private bool _isAddedToChain = false;
        private bool _isFirstCircle = false;
        private bool _isPointerComing = false;

        private const string SpawnAnimation = "CircleSpawnAnimation";
        
        private void Awake()
        {
            _animation = GetComponent<Animation>();
        }

        public void Initialize(CircleController circleController)
        {
            _circleController = circleController;
            _animation.Play(SpawnAnimation);
        }

        private void OnSpawnAnimationCompleted()
        {
            SetTarget();
        }

        private void Start()
        {
            
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private void Update()
        {
            if(!GameManager.Instance.IsLevelFailed && _canMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, _targetPosition, Time.deltaTime * GameManager.DefaultGameProperties.MoveSpeed);

                if (Vector3.Distance(transform.position,_targetPosition) <= 0.05f)
                    OnTargetReached();
            }
        }

        private void SetTarget()
        {
            _startPosition = transform.position;
            _targetPosition = _circleController.GetRandomPosition();
            _canMove = true;
        }

        private void OnTargetReached()
        {
            _canMove = false;
            Invoke(nameof(SetTarget), GameManager.DefaultGameProperties.WaitTime);
        }

        private void OnMouseEnter()
        {
            if (!GameManager.Instance.IsLevelFailed)
                CheckSelect();
        }

        private void OnMouseDown()
        {
            if (!GameManager.Instance.IsLevelFailed)
                CheckSelect();
        }

        private void CheckSelect()
        {
            if (!InputManager.IsSwiping || _isPointerComing)
                return;

            _isAddedToChain = true;
            _isPointerComing = true;

            _circleController.OnCircleSelected(this);

        }

        public void OnPointerReached()
        {
            _isPointerComing = false;
        }
    }
}
