using UnityEngine;
using RivetedRunes.Utils.Singleton;
using Cysharp.Threading.Tasks;
using RivetedRunes.Controllers;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace RivetedRunes.UtilityAI
{
    public class ActionableTarget
    {
        public Interactable interactableObject;
        public NPCAction action;

        public ActionableTarget(Interactable interactable, NPCAction action)
        {
            this.interactableObject = interactable;
            this.action = action;
        }
    }

    public class AIBrain : Singleton<AIBrain>
    {
        [SerializeField] private NPCAction[] _baseActionsAvailable;

        public async UniTask DecideBestAction(NPCController npc)
        {
            List<ActionableTarget> actionsAvailable = new();
            UpdateActionList(ref actionsAvailable);
            SetBestAction(npc, actionsAvailable);
            await UniTask.Yield();
        }

        private void UpdateActionList(ref List<ActionableTarget> actionsAvailable)
        {
            actionsAvailable.Clear();
            for (int i = 0; i < _baseActionsAvailable.Length; i++)
                actionsAvailable.Add(new ActionableTarget(null, _baseActionsAvailable[i]));

            Interactable[] interactables = FindObjectsOfType(typeof(Interactable)) as Interactable[];
            for (int i = 0; i < interactables.Length; i++)
            {
                NPCAction[] actions = interactables[i].GetActions();
                for (int j = 0; j < _baseActionsAvailable.Length; j++)
                    actionsAvailable.Add(new ActionableTarget(interactables[i], _baseActionsAvailable[j]));
            }
        }

        private void SetBestAction(NPCController npc, List<ActionableTarget> actionsAvailable)
        {
            if (actionsAvailable.Count == 0)
                return;

            int bestAction = 0;
            float bestScore = 0f;

            for (int i = 0; i < actionsAvailable.Count; i++)
            {
                NPCAction action = actionsAvailable[i].action;
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