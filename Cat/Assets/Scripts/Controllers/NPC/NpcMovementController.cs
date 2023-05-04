using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovementController : MonoBehaviour
{
    // Start is called before the first frame update

    NavMeshAgent agent;

    PathPointModel[] availablePosition = {};

    private float standsAt = 0;
    int positionIndex = 0;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (availablePosition.Length > 0) {
            agent.destination = availablePosition[0].position;
        }
    }

    public void SetPositions(PathPointModel[] positions) {
        availablePosition = positions;
    }

    // Update is called once per frame
    void Update()
    {
        if (availablePosition.Length == 0) {
            return;
        }
        if (standsAt > 0) {
            standsAt -= Time.deltaTime;

            return;
        }

        if (Vector3.Distance(agent.transform.position, agent.destination) < 2.0f)
        {
            if (availablePosition[positionIndex].interactive) {
                if(standsAt < 0)
                {
                    positionIndex++;
                    standsAt = 0;
                }
                else
                {
                    InteractivePathPointController interactive = availablePosition[positionIndex].interactive;
                    interactive.objectToInteract.Interact();
                    standsAt = interactive.secondsToAwait;
                    return;
                }
            } else {
                positionIndex++;
            }

            if(positionIndex >= availablePosition.Length)
            {
                positionIndex = 0;
            }
            agent.destination = availablePosition[positionIndex].position;
        }   
    }
}
