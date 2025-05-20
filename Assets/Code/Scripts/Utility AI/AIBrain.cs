using UnityEngine;
using RivetedRunes.Utils.Singleton;
using RivetedRunes.Controllers;

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
            
            NPCAction bestAction = null;
            float bestScore = 0f;

            foreach (NPCAction action in actionsAvailable)
            { 
                if (bestScore >= ScoreAction(npc, ref action))
                    continue;

                bestAction = action;
                bestScore = action.score;
            }

            npc.SetBestAction(bestAction);
        }

        private void ScoreAction(NPCController npc, ref NPCAction action)
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