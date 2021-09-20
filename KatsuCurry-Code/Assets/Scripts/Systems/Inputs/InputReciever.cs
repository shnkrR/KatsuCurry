using System;
using UnityEngine;

namespace Katsu.Systems.Inputs
{
    public class InputReciever
    {
        public const string HORIZONTAL_AXIS = "Horizontal";
        public const string VERTICAL_AXIS = "Vertical";

        public EventHandler<InputReceivedArgs> InputReceived;

        eInputType m_previousInputType = eInputType.NONE;

        public void Update()
        {
            eInputType inputType = eInputType.NONE;
            if (Input.GetAxis(HORIZONTAL_AXIS) > 0f)
            {
                inputType = eInputType.ACCELERATE;
            }

            if (Input.GetAxis(HORIZONTAL_AXIS) < 0f)
            {
                inputType = eInputType.BRAKE;
            }

            if (Input.GetAxis(VERTICAL_AXIS) > 0f)
            {
                inputType = eInputType.ROTATE_ANTI_CLOCKWISE;
            }

            if (Input.GetAxis(VERTICAL_AXIS) < 0f)
            {
                inputType = eInputType.ROTATE_CLOCKWISE;
            }

            if ((inputType != eInputType.NONE) || (m_previousInputType != eInputType.NONE))
            {
                InputReceivedArgs args = new InputReceivedArgs(inputType);
                InputReceived.Invoke(this, args);
            }

            m_previousInputType = inputType;
        }
    }
}