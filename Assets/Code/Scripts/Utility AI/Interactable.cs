using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RivetedRunes.UtilityAI.Actions;

namespace RivetedRunes.UtilityAI
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description
        [SerializeField] private NPCAction[] _actions;
        public NPCAction[] GetActions() => _actions;
    }
}
