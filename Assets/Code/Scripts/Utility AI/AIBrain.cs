using UnityEngine;
using RivetedRunes.Utils.Singleton;
using RivetedRunes.Controllers;

namespace RivetedRunes.UtilityAI
{
    public class AIBrain : Singleton<AIBrain>
    {
        [SerializeField] private NPCAction[] _actionsAvailable;
        public ref NPCAction[] GetActionsAvailable() => ref _actionsAvailable;

        public void DecideBestAction(NPCController npc)
        {
            if (_actionsAvailable == null)
                return;
            
            if (_actionsAvailable.Length == 0)
                return;
            
            int bestActionIndex = 0;
            float bestScore = 0f;

            for(int i = 0; i < _actionsAvailable.Length; i++)
            {
                if (bestScore >= ScoreAction(npc, ref _actionsAvailable[i]))
                    continue;
                
                bestActionIndex = i;    
                bestScore = _actionsAvailable[i].score;        
            }

            npc.SetBestAction(_actionsAvailable[bestActionIndex]);
        }

        public float ScoreAction(NPCController npc, ref NPCAction action)
        {
            float score = 1f;

            for(int i = 0; i < action._considerations.Length; i++)
            {
                float considerationScore = action._considerations[i].ScoreConsideration(npc, action);
                score *= considerationScore;  
                if (score != 0)
                {
                    action.score = score;
                    return action.score;
                }
            }

            // MAGIC TRICK
            float originalScore = score;
            float modFactor = 1 - (1 / action._considerations.Length);
            float makeupValue = (1 - originalScore) * modFactor;
            action.score = originalScore + (makeupValue * originalScore);

            return action.score;
        }
    }
}