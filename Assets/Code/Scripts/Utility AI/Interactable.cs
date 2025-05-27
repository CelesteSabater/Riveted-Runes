using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RivetedRunes.Config;
using RivetedRunes.UtilityAI.Actions;
using RivetedRunes.Controllers;

namespace RivetedRunes.UtilityAI
{
    [Serializable]
    public struct InteractSeat
    {
        public Transform seatLocation;
        public NPCController currentInteractor;
    }

    [Serializable]
    public struct InteractableAction
    {
        public NPCAction action;
        public InteractSeat[] workSeats;

        public readonly bool GetAvailableSeat()
        {
            for (int i = 0; i < workSeats.Length; i++)
                if (workSeats[i].currentInteractor == null) return true;
            return false;
        }

        public readonly Transform ClaimWorkSeat(NPCController npc)
        {
            for (int i = 0; i < workSeats.Length; i++)
                if (workSeats[i].currentInteractor == null)
                {
                    workSeats[i].currentInteractor = npc;
                    return workSeats[i].seatLocation;
                }

            return null;
        }

        public readonly void ReleaseWorkSeat(NPCController npc)
        {
            for (int i = 0; i < workSeats.Length; i++)
                if (workSeats[i].currentInteractor == npc) workSeats[i].currentInteractor = null;
        }
    }

    public class Interactable : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private InteractableAction[] _interactableActions;

        public InteractableAction[] GetActions() => _interactableActions;
    }
}
