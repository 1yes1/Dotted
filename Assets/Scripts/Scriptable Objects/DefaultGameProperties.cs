using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    [CreateAssetMenu(fileName = "DefaultGameProperties", menuName = "Dotted/DefaultGameProperties", order = 1)]
    public class DefaultGameProperties : ScriptableObject
    {
        [Header("Beginning")]
        
        [SerializeField]
        private int _startCircleCount = 5;

        [SerializeField]
        private int _maxCircleCount = 20;


        [Header("Game")]
        
        [SerializeField]
        private float _circleCreatingFrequencyAtStart = 1.5f;

        [SerializeField]
        private float _circleCreatingFrequency = 0.25f;


        [Header("Circle Movement")]

        [SerializeField]
        private float _moveSpeed = 5;

        [SerializeField]
        private float _waitTime = 1.25f;


        public int StartCircleCount => _startCircleCount;
        public int MaxCircleCount => _maxCircleCount;
        public float CircleCreatingFrequencyAtStart => _circleCreatingFrequencyAtStart;
        public float CircleCreatingFrequency => _circleCreatingFrequency;
        public float MoveSpeed => _moveSpeed;
        public float WaitTime => Random.Range(_waitTime, _waitTime + _waitTime * 0.2f);

    }
}
