using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    int layerMask = 1 << 8;

    enum DecisionStates
    {
        Idle,
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

    BulletPool bulletPool;

    PlayerScript player;

    bool active = false;
    float health;

    private Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        properties = GetComponent<AIProperties>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        active = false;
        //agent.isStopped = true;
        agent.enabled = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (active && (decision == DecisionStates.Walking || decision == DecisionStates.Idle))
        {
            if(!CheckForEnemies() && decision == DecisionStates.Idle)
            {
                //StopAllCoroutines();
                StartCoroutine(PickTargetPosition());
            }
        }

        //Walking Animation
        if (decision == DecisionStates.Walking && !anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            anim.Play("Run");
        }

        if (agent.remainingDistance < 1)
        {
            //StopAllCoroutines();
            StartCoroutine(PickTargetPosition());
        }
    }

    bool CheckForEnemies()
    {
        GameObject nearestGangMember = null;
        float closest = Mathf.Infinity;

        var oppositeGang = gangController.GetGangMembers(gangController.GetOppositeGangFaction((int)properties.gang));

        if(player.GetBone() || player.GetMilk())
        {
            oppositeGang.Add(player.gameObject);
        }

        for (int i = 0; i < oppositeGang.Count; i++)
        {
            if (Vector3.Distance(transform.position, oppositeGang[i].transform.position) < closest)
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
                    StopAllCoroutines();
                    StartCoroutine(Attack(nearestGangMember.transform));
                    return true;
                }
            }
        }
        return false;
    }

    public void SpawnedIn(GangController _gangController, BulletPool _bulletPool)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        active = true;

        agent.enabled = true;
        gameObject.SetActive(true);

        health      = properties.maxHealth;
        agent.speed = properties.speed;
        bulletPool  = _bulletPool;

        gangController = _gangController;
        StartCoroutine(PickTargetPosition());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            HitByBullet(properties.damage);
        }
        if(other.CompareTag("PlayerBullet"))
        {
            HitByBullet(10);
        }
    }

    IEnumerator Attack(Transform _attackTarget)
    {
        if(decision == DecisionStates.Walking)
        {
            decision = DecisionStates.ShootingAI;
            yield return new WaitForSeconds(Random.Range(properties.attackDelayMin, properties.attackDelayMax));
        }
        else
        {
            decision = DecisionStates.ShootingAI;
            yield return new WaitForSeconds(properties.shootDelay);
        }

        agent.isStopped = true;
        attackTarget = _attackTarget;

        var burst = Random.Range(properties.burstMin, properties.burstMax);

        for(int i = 0; i < burst; i++)
        {
            var _direction = (attackTarget.position - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), 1);

            Vector3 dir = (transform.position - attackTarget.position).normalized;
            Vector3 accuracy = new Vector3(dir.x + Random.Range(-properties.accuracy, properties.accuracy), dir.y, dir.z + Random.Range(-properties.accuracy, properties.accuracy));
            bulletPool.Shoot(transform.GetChild(1).gameObject, -accuracy, "Bullet");
            yield return new WaitForSeconds(properties.firerate);
        }

        decision = DecisionStates.Idle;
        if (!CheckForEnemies())
        {
            StartCoroutine(PickTargetPosition());
        }
    }

    IEnumerator PickTargetPosition()
    {
        decision = DecisionStates.Walking;
        yield return new WaitForSeconds(0.5f);

        var oppositeGang = gangController.GetGangMembers(gangController.GetOppositeGangFaction((int)properties.gang));

        if (oppositeGang.Count > 0)
        {
            walkTarget = oppositeGang[Random.Range(0, oppositeGang.Count)].transform;

            agent.isStopped = false;
            agent.SetDestination(walkTarget.position);
        }
        else
        {
            PickTargetPosition();
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
