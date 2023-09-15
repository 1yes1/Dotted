using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class Circle : MonoBehaviour
    {
        private CircleController _circleController;
        private Animation _animation;

        private bool _canMove = false;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private float _count;
        private float _time;

        private const string SpawnAnimation = "CircleSpawnAnimation";
        private const string SelectedAnimation = "CircleSelectedAnimation";
        
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

        private void Update()
        {
            if(GameManager.Instance.IsGameRunning && _canMove)
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
            _time = 0;
            _canMove = true;
        }

        private void OnTargetReached()
        {
            _canMove = false;
            Invoke(nameof(SetTarget), GameManager.DefaultGameProperties.WaitTime);
        }

        private void OnMouseDown()
        {
            print("Down");
            _animation.Play(SelectedAnimation);
            _circleController.OnCircleDown(this);
        }

    }
}
