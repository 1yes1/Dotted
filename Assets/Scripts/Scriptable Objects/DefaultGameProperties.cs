using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    [CreateAssetMenu(fileName = "DefaultGameProperties", menuName = "Dotted/DefaultGameProperties", order = 1)]
    public class DefaultGameProperties : ScriptableObject
    {
        [Header("Circle")]
        
        [SerializeField]
        private int _startCircleCount = 5;

        [SerializeField]
        private int _maxCircleCount = 20;

        [SerializeField]
        private float _maxCircleMultiplier = 0.5f;


        [Header("Circle Creating")]
        
        [SerializeField]
        private float _circleCreatingFrequencyAtStart = 1.5f;

        [SerializeField]
        private float _circleCreatingFrequency = 0.25f;


        [Header("Circle Movement")]

        [SerializeField]
        private float _moveSpeed = 5;

        [SerializeField]
        private float _maxTravelTime = 3;

        [SerializeField]
        private float _maxTravelTimeMultiplier = 0.5f;

        [SerializeField]
        private float _waitTime = 1.25f;

        [SerializeField]
        private int _addMultiplierScore = 1000;

        [SerializeField]
        private float _moveSpeedMultiplier = 1.05f;


        [Header("Scoring")]

        [SerializeField]
        private int _startScore = 5;
        
        [SerializeField]
        private float _scoreMultiplier = 1.8f;


        public int StartCircleCount => _startCircleCount;
        public int MaxCircleCount => _maxCircleCount;
        public float CircleCreatingFrequencyAtStart => _circleCreatingFrequencyAtStart;
        public float CircleCreatingFrequency => _circleCreatingFrequency;
        public float MoveSpeed => _moveSpeed;
        public float WaitTime => Random.Range(_waitTime, _waitTime + _waitTime * 0.2f);
        public int StartScore => _startScore;
        public float ScoreMultiplier => _scoreMultiplier;
        public int AddMultiplierScore => _addMultiplierScore;
        public float MoveSpeedMultiplier => _moveSpeedMultiplier;
        public float MaxTravelTime => _maxTravelTime;
        public float MaxTravelTimeMultiplier => _maxTravelTimeMultiplier;
        public float MaxCircleMultiplier => _maxCircleMultiplier;


    }
}
