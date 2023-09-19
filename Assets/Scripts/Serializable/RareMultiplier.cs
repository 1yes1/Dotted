using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dotted
{
    [Serializable]
    public class RareMultiplier
    {
        [Range(2, 10)]
        public int multiplier = 2;

        public int minScore = 5000;

        public float percentage = 0.01f;

        public bool IsUsable(int score)
        {
            float num = Random.Range(0f, 100f);
            //Debug.Log("Num: " + num);

            if (score > minScore && num <= percentage)
            {
                return true;
            }

            return false;
        }

    }
}
