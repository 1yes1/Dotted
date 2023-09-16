using Dotted.Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class GameManager : MonoBehaviour
    {
        public static event Action OnLevelFailedEvent;

        private static GameManager _instance;

        [SerializeField]
        private DefaultGameProperties _gameProperties;

        private bool _isLevelFailed = false;
        
        public static GameManager Instance => _instance;
        public static DefaultGameProperties DefaultGameProperties => Instance._gameProperties;

        public bool IsLevelFailed => _isLevelFailed;

        public GameEventCaller GameEventCaller { get; private set; }
        public GameEventReceiver GameEventReceiver { get; private set; }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;

            Initialize();
        }

        private void Initialize()
        {
            this.GameEventReceiver = new GameEventReceiver();
            this.GameEventCaller = new GameEventCaller(GameEventReceiver);
        }

        void Start()
        {
        }

        private void Update()
        {
        }

        public void LevelFailed()
        {
            _isLevelFailed = true;
            OnLevelFailedEvent?.Invoke();
        }

    }
}

