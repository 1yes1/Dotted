using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class Pointer : Dot
    {
        [SerializeField] private float _moveTime = 0.5f;

        [SerializeField] private Color _moveColor;

        [SerializeField] private Vector3 _moveScale;


        private Controller _controller;

        private Queue<Transform> _targetCircleTransforms;

        private Action<Transform,bool> _onPointerReachedTargetAction;

        private SpriteRenderer _spriteRenderer;

        private float _timer;

        private Vector3 _startPosition;

        private Transform _targetTransform;

        private bool _isAnimationPlaying = false;

        private bool _canGoTarget = false;

        private const string CircleChoosedAnimation = "PointerCircleChoosed";

        public int QueueCount => _targetCircleTransforms.Count;

        private void OnEnable()
        {
            GameEventReceiver.OnChainCompletedEvent += OnChainCompleted;
            GameEventReceiver.OnFailedEvent += OnGameFailed;
            GameEventReceiver.OnGamePausedEvent += OnGamePaused;
            GameEventReceiver.OnGameResumedEvent += OnGameResumed;
            GameEventReceiver.OnGameRestartedEvent += OnGameRestarted;
        }


        private void OnDisable()
        {
            GameEventReceiver.OnChainCompletedEvent -= OnChainCompleted;
            GameEventReceiver.OnFailedEvent -= OnGameFailed;
            GameEventReceiver.OnGamePausedEvent -= OnGamePaused;
            GameEventReceiver.OnGameResumedEvent -= OnGameResumed;
            GameEventReceiver.OnGameRestartedEvent -= OnGameRestarted;
        }

        private void Awake()
        {
            base.Awake();

            _targetCircleTransforms = new Queue<Transform>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Hide();
        }

        public void Initialize(Controller controller)
        {
            _controller = controller;
        }

        public void OnCircleChoosed(Vector3 startPosition, Transform targetCircleTransform,Action<Transform,bool> OnPointerReachedTargetAction,bool isFirst)
        {
            transform.position = startPosition;

            Show();

            AddNewTarget(targetCircleTransform);

            _onPointerReachedTargetAction = OnPointerReachedTargetAction;

            StartMovingTarget(isFirst);

            if (isFirst)
                OnPointerReachedTarget(targetCircleTransform);
        }

        private void FixedUpdate()
        {
            if(_isAnimationPlaying)
            {
                if(_targetTransform == null)
                    Hide();
                else
                    transform.position = _targetTransform.position;
            }

            //D�zeltilecek !GameManager.Instance.IsGameFailed buras�
            if (_canGoTarget && GameManager.Instance.IsGamePlaying && !GameManager.Instance.IsGameFailed)
            {
                transform.position = Vector3.Lerp(_startPosition, _targetTransform.position, _timer / _moveTime);
                _timer += Time.fixedDeltaTime;

                if (Vector3.Distance(transform.position, _targetTransform.position) <= 0.01f)
                    OnPointerReachedTarget(_targetTransform);
            }
        }


        private void Hide()
        {
            _spriteRenderer.enabled = false;
            _isAnimationPlaying = false;
        }

        private void Show()
        {
            _spriteRenderer.enabled = true;
        }

        private void StopMoving()
        {
            _canGoTarget = false;
        }

        private void OnPointerReachedTarget(Transform circle)
        {
            transform.position = circle.position;

            _canGoTarget = false;

            _animation.Play(CircleChoosedAnimation);

            _isAnimationPlaying = true;

            bool hasNewTarget = (_targetCircleTransforms.Count > 0) ? true : false;
            _onPointerReachedTargetAction?.Invoke(circle,hasNewTarget);

            TryToMove();
        }

        private void TryToMove()
        {
            if(GameManager.Instance.IsGameFailed)
            {
                _targetCircleTransforms.Clear();
                StopMoving();
                StopAnimation();
            }

            if (_targetCircleTransforms.Count > 0)
                StartMovingTarget();
        }

        private void StartMovingTarget(bool isFirst = false)
        {
            if (_canGoTarget)
                return;

            _startPosition = transform.position;
            _targetTransform = GetTarget();
            transform.localScale = _moveScale;
            SetColor(_moveColor);
            _timer = 0;

            //S�raya alm�� bu circle � fakat bundan �nceki circle zinciri tamamlam��
            //O y�zden bunun ilk halka oldu�unu varsay�yoruz
            if (_controller.HoldingDotCount <= 0)
            {
                OnPointerReachedTarget(_targetTransform);
                return;
            }

            if (_targetTransform != null && !isFirst)
                _canGoTarget = true;
            else
                return;


            StopAnimation();
        }

        private void AddNewTarget(Transform circleTransform)
        {
            _targetCircleTransforms.Enqueue(circleTransform);
        }

        private Transform GetTarget()
        {
            Transform result;
            if (_targetCircleTransforms.TryDequeue(out result))
                return result;
            else
                return null;
        }

        private void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        private void StopAnimation()
        {
            if (_animation.isPlaying)
            {
                _animation.Stop();
                _isAnimationPlaying = false;
            }
        }

        private void OnChainCompleted(List<Dot> obj)
        {
            Hide();
            _canGoTarget = false;
            _targetTransform = transform;
            StopAnimation();
        }

        private void OnGameFailed()
        {
            StopAnimation();
            Hide();
        }

        private void OnGameResumed()
        {
            if(_targetTransform != null)
                _isAnimationPlaying = true;
        }

        private void OnGamePaused()
        {
            StopMoving();
            StopAnimation();
        }

        private void OnGameRestarted()
        {
            _targetTransform = null;
            StopAnimation();
            StopMoving();
            Hide();
        }

    }
}
