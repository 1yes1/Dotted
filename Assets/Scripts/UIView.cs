using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Dotted
{
    public abstract class UIView : MonoBehaviour
    {
        [SerializeField] private bool _isPopup;

        public bool IsPopup => _isPopup;

        public void SetPopup()
        {
            _isPopup = true;
        }

        public abstract void Initialize();

        public virtual void Show() => gameObject.SetActive(true);

        public virtual void Hide() => gameObject.SetActive(false);
    }
}
