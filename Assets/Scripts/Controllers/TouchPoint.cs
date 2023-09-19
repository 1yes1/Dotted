using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotted
{
    public class TouchPoint : MonoBehaviour
    {
        private CircleCollider2D _circleCollider;
        private bool _isHiding = true;

        public bool IsHiding => _isHiding;

        private void Awake()
        {
            _circleCollider = GetComponent<CircleCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Circle"))
            {
                collision.GetComponent<Circle>().TrySelect();
            }
        }

        public void Hide()
        {
            _circleCollider.enabled = false;
            _isHiding = true;
        }

        public void Show()
        {
            _circleCollider.enabled = true;
            _isHiding = false;
        }
    }
}
