using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public int InputX { get; private set; } // Get the normalized horizontal axis
        public int InputY { get; private set; } // Get the normalized vertical axis

        public bool AnyKeyPressed { get; private set; }

        public void OnMoveInput(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();
            InputX = Mathf.RoundToInt(input.x);
            InputY = Mathf.RoundToInt(input.y);
        }

        public void OnAnyKey(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                AnyKeyPressed = true;
            }
            if (context.canceled)
            {
                AnyKeyPressed = false;
            }
        }
    }
}