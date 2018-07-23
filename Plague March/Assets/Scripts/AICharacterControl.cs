using System;
using UnityEngine;
using UnityEngine.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform[] targets;                                   // target to aim for

        //Used to iterate through targets
        private int i = 0;

        private bool patrol = true;
        private bool alerted = false;
        private bool chase = false;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
            if (patrol)
            {
                Patrolling();
            }

            if(alerted)
            {
                character.Move(Vector3.zero, false, false);
                //float timer = 0;
                //timer += Time.deltaTime;

                //if(timer >= 3.0f)
                //{
                //    chase = true;
                //}
            }
        }

        public void SetTarget(Transform target)
        {
            targets[0] = target;
        }

        public void StopAgent()
        {
            character.Move(Vector3.zero, false, false);
        }

        public void Patrolling()
        {
            if (Vector3.Distance(targets[i].position, agent.transform.position) >= 1.0f)
            {
                if (targets[i] != null)
                {
                    agent.SetDestination(targets[i].position);
                }

                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    character.Move(agent.desiredVelocity, false, false);
                }

                else
                {
                    character.Move(Vector3.zero, false, false);
                }
            }
            else
            {
                if (i >= targets.Length - 1)
                {
                    i = 0;
                }
                else
                {
                    ++i;
                }
            }
        }

        public void Alert()
        {

        }

        public void SetPatrol()
        {
            patrol = true;
            alerted = false;
            chase = false;
        }

        public void SetAlert()
        {
            patrol = false;
            alerted = true;
            chase = false;
        }

        public void SetChase()
        {
            patrol = false;
            alerted = false;
            chase = true;
        }
    }
}
