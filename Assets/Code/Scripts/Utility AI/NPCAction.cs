using System;
using UnityEngine;
using RivetedRunes.Controllers;

namespace RivetedRunes.UtilityAI
{
    public abstract class NPCAction : ScriptableObject
    {
        [SerializeField] private string _actionName;
        [SerializeField] private string _actionDescription;
        private float _score;
        [Tooltip("Time required in seconds.")]
        [SerializeField] private float _workRequired;
        private float _workPerformed = 0;
        public Consideration[] _considerations;

        public float score
        {
            get { return _score; }

            set 
            {
                this._score = Mathf.Clamp01(value);
            }
        }

        public float workPerformed
        {
            get
            {
                if (float.IsNaN(_workPerformed)) return 0;
                return _workPerformed;
            }

            set 
            {
                this._workPerformed = Mathf.Clamp(value, 0, _workRequired);
            }
        }
        
        public float GetWorkRequired() => _workRequired;
        public float GetPercentage()
        {
            if (_workRequired == 0) return 0;
            return _workPerformed / _workRequired;
        } 
        private void Awake()
        {
            score = 0;
        }

        public abstract void ExecuteAction(NPCController npc);

        public void CheckIsComplete(NPCController npc)
        {
            if (_workPerformed >= _workRequired)
            {
                CompleteAction(npc);
                workPerformed = 0;
            }
        }

        public abstract void CompleteAction(NPCController npc);
    }
}

