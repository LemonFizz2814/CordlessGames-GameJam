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
        ShootingAI,
        ShootingPlayer,
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

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        properties = GetComponent<AIProperties>();

        active = false;
        //agent.isStopped = true;
        agent.enabled = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(active && decision == DecisionStates.Walking)
        {
            //print("for gang members");

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
                RaycastHit hit;
                string oppositeGangName = ((GangController.Gangs)gangController.GetOppositeGangFaction((int)properties.gang)).ToString();

                if (Physics.Linecast(transform.position, nearestGangMember.transform.position, out hit))
                {
                    if (hit.transform.CompareTag(oppositeGangName))
                    {
                        StartCoroutine(Attack(nearestGangMember.transform));
                    }
                }
            }
        }
    }

    public void SpawnedIn(GangController _gangController)
    {
        active = true;

        agent.enabled = true;
        gameObject.SetActive(true);

        health      = properties.maxHealth;
        agent.speed = properties.speed;

        gangController = _gangController;
        StartCoroutine(PickTargetPosition());
    }

    IEnumerator Attack(Transform _attackTarget)
    {
        decision = DecisionStates.ShootingAI;
        yield return new WaitForSeconds(Random.Range(properties.attackDelayMin, properties.attackDelayMax));

        agent.isStopped = true;
        attackTarget = _attackTarget;

        print("ATTACK!!!!");
    }

    IEnumerator PickTargetPosition()
    {
        yield return new WaitForSeconds(1);

        decision = DecisionStates.Walking;
        var oppositeGang = gangController.GetGangMembers(gangController.GetOppositeGangFaction((int)properties.gang));

        if (oppositeGang.Count > 0)
        {
            walkTarget = oppositeGang[Random.Range(0, oppositeGang.Count)].transform;

            agent.isStopped = false;
            agent.SetDestination(walkTarget.position);
            print("my pos " + transform.position + " target pos " + walkTarget.position);
        }
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
        //agent.isStopped = true;
        agent.enabled = false;
        gameObject.SetActive(false);
        gangController.RemoveAI((int)properties.gang, gameObject);
    }
}
