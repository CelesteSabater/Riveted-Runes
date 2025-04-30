using System;
using UnityEngine;
using UnityEngine.AI;

namespace RivetedRunes.Managers
{
    public class MoveManager : MonoBehaviour
    {
        private NavMeshAgent _agent;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public void MoveTo(UnityEngine.Vector3 pos)
        {
            if (_agent == null)
                return;
            
            _agent.destination = pos;
        }
    }
}
