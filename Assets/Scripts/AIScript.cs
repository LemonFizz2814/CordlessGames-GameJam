using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    int layerMask = 1 << 8;

    enum DecisionStates
    {
        Walking,
        Shooting,
        Fleeing,
        Dead,
    }

    DecisionStates decision;

    AIProperties properties;

    Transform walkTarget;
    Transform attackTarget;

    NavMeshAgent agent;

    GangController gangController;

    bool active = false;
    float health;

    private void Update()
    {
        if(active && decision == DecisionStates.Walking)
        {
            GameObject nearestGangMember = null;
            float closest = Mathf.Infinity;

            var oppositeGang = gangController.GetGangMembers(gangController.GetOppositeGangFaction((int)properties.gang));

            for (int i = 0; i < oppositeGang.Count; i++)
            {
                if(Vector3.Distance(transform.position, oppositeGang[i].transform.position) < closest)
                {
                    nearestGangMember = oppositeGang[i];
                }
            }

            if (nearestGangMember != null)
            {
                if (Physics.Linecast(transform.position, nearestGangMember.transform.position, layerMask))
                {
                    Attack(nearestGangMember.transform);
                }
            }
        }
    }

    public void SpawnedIn(GangController _gangController)
    {
        active = true;
        agent.isStopped = false;
        health = properties.maxHealth;
        gangController = _gangController;
        PickTargetPosition();
    }

    void Attack(Transform _attackTarget)
    {
        decision = DecisionStates.Shooting;
        attackTarget = _attackTarget;
    }

    void PickTargetPosition()
    {
        decision = DecisionStates.Walking;
        var oppositeGang = gangController.GetGangMembers(gangController.GetOppositeGangFaction((int)properties.gang));
        walkTarget = oppositeGang[Random.Range(0, oppositeGang.Count)].transform;

        agent.SetDestination(walkTarget.position);
    }

    public void HitByBullet(float _damage)
    {
        health -= _damage;

        HealthCheck();
    }

    void HealthCheck()
    {
        if(health <= 0)
        {
            AIDied();
        }
    }

    void AIDied()
    {
        active = false;
        agent.isStopped = true;
        gangController.RemoveAI((int)properties.gang, gameObject);
    }
}
