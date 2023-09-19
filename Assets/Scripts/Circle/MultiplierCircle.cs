using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dotted
{
    public class MultiplierCircle : Circle
    {
        [SerializeField] private TextMeshPro _scoreMultiplierText;

        private int _scoreMultiplier = 2;

        public int ScoreMultiplier => _scoreMultiplier;

        public void SetScoreMultiplier(int multiply)
        {
            _scoreMultiplierText.text = "x" + multiply.ToString();
            _scoreMultiplier = multiply;
        }
        public override void PushSelf()
        {
            PoolManager.GetPool<MultiplierCircle>().Push(this);
        }

    }

}
