using UnityEngine;
using RivetedRunes.Controllers;

namespace RivetedRunes.UtilityAI.Stats.Needs
{
    [CreateAssetMenu(fileName = "FlatConsideration", menuName = "Utility AI/Considerations/Flat")]
    public class Flat : Consideration
    {
        [Range(0, 1f)]
        [SerializeField] private float _value;

        public override float ScoreConsideration(NPCController npc, NPCAction action)
        {
            return _value;
        }
    }
}
