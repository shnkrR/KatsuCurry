using System;

namespace Katsu.Systems.Inputs
{
    public enum eInputType
    {
        NONE = -1,
        ACCELERATE = 0,
        BRAKE,
        ROTATE_ANTI_CLOCKWISE,
        ROTATE_CLOCKWISE,
    }

    public class InputReceivedArgs : EventArgs
    {
        public eInputType InputDirection { private set; get; }

        public InputReceivedArgs(eInputType inputDirection)
        {
            InputDirection = inputDirection;
        }
    }
}