using UnityEngine;
using RivetedRunes.Controllers;

namespace RivetedRunes.UtilityAI
{
    public abstract class Consideration : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        private float _score;
        public float score
        {
            get { return _score; }

            set 
            {
                this._score = Mathf.Clamp01(value);
            }
        }

        private void Awake() {
            score = 0;
        }

        public abstract float ScoreConsideration(NPCController npc, NPCAction action);
    }
}