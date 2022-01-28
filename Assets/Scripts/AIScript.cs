using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    enum DescisionStates
    {
        Walking,
        Shooting,
        Fleeing,
        Dead,
    }

    DescisionStates descisions;

    AIProperties properties;

    Transform walkTarget;

    NavMeshAgent agent;
}
