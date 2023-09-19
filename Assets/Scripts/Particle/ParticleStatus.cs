using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dotted.Assets.Scripts
{
    public class ParticleStatus:MonoBehaviour
    {
        public event Action<ParticleSystem> OnDisableEvent;


        private void OnDisable()
        {
            OnDisableEvent?.Invoke(GetComponent<ParticleSystem>());
        }
    }
}
