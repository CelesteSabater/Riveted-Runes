using System;
using UnityEngine;

namespace RivetedRunes.UtilityAI.Stats
{
    public abstract class BaseStat : ScriptableObject
    {
        [SerializeField] private StatType _type;
        [SerializeField] private string _statName;
        [SerializeField] private string _statDescription;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        private float _currentValue;
        public float currentValue
        {
            get { return _currentValue; }

            set 
            {
                this._currentValue = Mathf.Clamp(value, _minValue, _maxValue);
                ProcessStatChange();
            }
        }

        public void SetStatType(StatType type) => _type = type;
        public StatType GetStatType() => _type;

        public abstract void ProcessStatChange();
    }
}