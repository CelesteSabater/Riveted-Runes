using UnityEngine;
using RivetedRunes.Utils.Singleton;
using RivetedRunes.Controllers;

namespace RivetedRunes.UtilityAI
{
    public class AIBrain : Singleton<AIBrain>
    {
        [SerializeField] private NPCAction[] _baseActionsAvailable;
        private List<NPCAction> _actionsAvailable = new List<NPCAction>();

        public void DecideBestAction(NPCController npc)
        {
            UpdateActionList();
            SetBestAction(npc);
        }

        private void UpdateActionList()
        {
            _actionsAvailable.Clear();
            _actionsAvailable.AddRange(_baseActionsAvailable);
            Interactable[] interactables = FindObjectsOfType(typeof(Interactable)) as Interactable[];
            for(int i = 0; i < interactables.Length; i++)
            {
                _actionsAvailable.AddRange(interactables[i].GetActions());
            }
        }

        private void SetBestAction(NPCController npc)
        {             
            if (_actionsAvailable.Count == 0)
                return;
            
            NPCAction bestAction = null;
            float bestScore = 0f;

            foreach (NPCAction action in _actionsAvailable)
            { 
                if (bestScore >= ScoreAction(npc, ref action))
                    continue;

                bestAction = action;
                bestScore = action.score;
            }

            npc.SetBestAction(bestAction);
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