using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Katsu.Systems.Inputs;

namespace Katsu.Game
{
    public class Vehicle : MonoBehaviour
    {
        const int POWER_INCREASE_RATE = 100;
        const int POWER_DECAY_RATE = 10000;

        [SerializeField] bool shouldReverse = default;

        [SerializeField] WheelJoint2D[] m_wheelJoints = default;

        InputReciever m_inputReciever = default;

        float m_power = 0;

        void Start()
        {
            m_inputReciever = new InputReciever();
            m_inputReciever.InputReceived += OnInputReceived;
        }

        void Update()
        {
            m_inputReciever.Update();
        }

        /// <summary>
        /// Updates the motor speed for each wheel
        /// </summary>
        /// <param name="speed">Speed to apply</param>
        void UpdateMotorSpeed(float speed)
        {
            for (int i = 0; i < m_wheelJoints.Length; i++)
            {
                if (!m_wheelJoints[i].useMotor)
                    continue;

                JointMotor2D jointMotor = m_wheelJoints[i].motor;
                jointMotor.motorSpeed = (speed < 0.0f) ? (shouldReverse ? speed : 0.0f) : speed;
                m_wheelJoints[i].motor = jointMotor;
            }
        }

        /// <summary>
        /// Fired when the input system detects an ais input
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Args containing the input type</param>
        void OnInputReceived(object sender, InputReceivedArgs args)
        {
            float powerIncrement = 0;
            switch (args.InputDirection)
            {
                case eInputType.ACCELERATE:
                    powerIncrement = (m_power < 0.0f) ? POWER_DECAY_RATE * Time.deltaTime : POWER_INCREASE_RATE * Time.deltaTime;
                    for (int i = 0; i < m_wheelJoints.Length; i++)
                    {
                        m_wheelJoints[i].useMotor = true;
                    }
                    break;

                case eInputType.BRAKE:
                    powerIncrement = (m_power > 0.0f) ? -POWER_DECAY_RATE * Time.deltaTime : -POWER_INCREASE_RATE * Time.deltaTime;
                    for (int i = 0; i < m_wheelJoints.Length; i++)
                    {
                        m_wheelJoints[i].useMotor = true;
                    }
                    break;
                
                case eInputType.NONE:
                    m_power = 0.0f;
                    UpdateMotorSpeed(m_power);
                    for (int i = 0; i < m_wheelJoints.Length; i++)
                    {
                        m_wheelJoints[i].useMotor = false;
                    }
                    break;
            }

            m_power += powerIncrement;
            UpdateMotorSpeed(m_power);
        }

        void OnDestroy()
        {
            m_inputReciever.InputReceived -= OnInputReceived;
        }
    }
}