﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dotted
{
    public class Dot:MonoBehaviour
    {
        protected Animation _animation;

        protected void Awake()
        {
            _animation = GetComponent<Animation>();
        }
    }
}
