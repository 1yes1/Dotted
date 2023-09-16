using System.Collections.Generic;
using UnityEngine;
using UnityExtra;

namespace Dotted
{
    public class InputManager : MonoBehaviour
    {
        public static bool TouchDown => Input.GetMouseButtonDown(0);

        public static bool TouchUp => Input.GetMouseButtonUp(0);

        public static Vector3 TouchPosition => Input.mousePosition;

        public static bool IsSwiping => Input.GetMouseButton(0); 

    }
}

