using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        [SerializeField]
        private DefaultGameProperties _gameProperties;

        private bool _isGameRunning = false;
        

        public static GameManager Instance => _instance;
        public static DefaultGameProperties DefaultGameProperties => Instance._gameProperties;
        public bool IsGameRunning => _isGameRunning;


        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }


        void Start()
        {
            _isGameRunning = true;
        }

        private void Update()
        {
        }

    }
}

