using System;
using Unity.VisualScripting;
using UnityEngine;
using RivetedRunes.Utils.Singleton;

namespace RivetedRunes.Managers.TimeManager
{
    public class TimeManager : RivetedRunes.Utils.Singleton.Singleton<TimeManager>
    {
        [Header("Config")]
        [SerializeField] private TimeSpeed[] _timeSpeeds;
        private int _currentSpeed;

        void Start()
        {
            if (_timeSpeeds == null || _timeSpeeds.Length == 0)
            {
                _timeSpeeds = new TimeSpeed[] 
                { 
                    new TimeSpeed(1, "x1")
                };
            }

            _currentSpeed = 0;
        }

        public void UpSpeed()
        {
            if (_currentSpeed == _timeSpeeds.Length - 1)
                return;

            _currentSpeed++;
            GameEvents.current.ChangeGameSpeed();
        }

        public void DownSpeed()
        {
            if (_currentSpeed == 0)
                return;

            _currentSpeed--;
            GameEvents.current.ChangeGameSpeed();
        }

        public float GetTime(bool ignoreCurrentSpeed = false)
        {
            if (ignoreCurrentSpeed)
                return Time.deltaTime;
            else
                return _timeSpeeds[_currentSpeed].timeSpeed * Time.deltaTime;
        }
    }
}