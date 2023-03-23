using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMovementController : MonoBehaviour
{
    // Start is called before the first frame update

    NavMeshAgent agent;

    Vector3[] availablePosition = {  new Vector3(0, 1, 0), new Vector3(43, 1, 0), new Vector3(22, 1, -43) };
    int positionIndex = 0;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = availablePosition[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(agent.transform.position, agent.destination) < 2.0f)
        {
            positionIndex++;
            if(positionIndex >= availablePosition.Length)
            {
                positionIndex = 0;
            }
            agent.destination = availablePosition[positionIndex];
        }   
    }
}
