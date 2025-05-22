using UnityEngine;
using RivetedRunes.Utils.Singleton;
using Cysharp.Threading.Tasks;
using RivetedRunes.Controllers;
using System.Collections.Generic;

namespace RivetedRunes.UtilityAI
{
    public class AIBrain : Singleton<AIBrain>
    {
        [SerializeField] private NPCAction[] _baseActionsAvailable;

        public async UniTask DecideBestAction(NPCController npc)
        {
            List<NPCAction> actionsAvailable = new List<NPCAction>();
            UpdateActionList(ref actionsAvailable);
            SetBestAction(npc, actionsAvailable);
            await UniTask.Yield();
        }

        private void UpdateActionList(ref List<NPCAction> actionsAvailable)
        {
            actionsAvailable.Clear();
            actionsAvailable.AddRange(_baseActionsAvailable);
            Interactable[] interactables = FindObjectsOfType(typeof(Interactable)) as Interactable[];
            for(int i = 0; i < interactables.Length; i++)
            {
                actionsAvailable.AddRange(interactables[i].GetActions());
            }
        }

        private void SetBestAction(NPCController npc, List<NPCAction> actionsAvailable)
        {             
            if (actionsAvailable.Count == 0)
                return;
            
            int bestAction = 0;
            float bestScore = 0f;

            for(int i = 0; i < actionsAvailable.Count; i++)
            {
                NPCAction action = actionsAvailable[i];
                if (bestScore >= ScoreAction(npc, ref action))
                    continue;

                bestAction = i;
                bestScore = action.score;
            }

            npc.SetBestAction(actionsAvailable[bestAction]);
        }

        private float ScoreAction(NPCController npc, ref NPCAction action)
        {
            float score = 1f;

            for (int i = 0; i < action._considerations.Length; i++)
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