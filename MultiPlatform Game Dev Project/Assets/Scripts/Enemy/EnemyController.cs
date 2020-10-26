using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

//Algorithm referenced from this video: https://www.youtube.com/watch?v=UjkSFoLxesw


public class EnemyController : MonoBehaviour
{
    //Variables
    protected int Health = 100;
    protected float Armor = 0.2f;

    public LayerMask GroundLayerMask;
    public LayerMask PlayerLayerMask;
    private NavMeshAgent enemyNavMesh;
    private GameObject player;

    private Vector3 walkingPoint;
    public float WalkingPointRange = 5f;
    private bool isWalkingPointSet;

    public float AttackInterval = 0.5f;
    private bool alreadyAttacked;

    public float AiSightRange = 20;
    public float AiAttackRange = 12;

    private bool isPlayerInSightRange;
    private bool isPlayerInAttackRange;

    public enum AIBehavior { Idle, Patrol, Follow, Attack };
    public AIBehavior CurrentAI;
    public bool allowEnemyPatrol;


    //Debug
    [Header("DEBUG")]
    public bool ShowAttackRange = true;
    public bool ShowAlertRange = true;
   

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyNavMesh = GetComponent<NavMeshAgent>();
        CurrentAI = AIBehavior.Idle;
        allowEnemyPatrol = true;
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerInSightRange = Physics.CheckSphere(transform.position, AiSightRange, PlayerLayerMask);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, AiAttackRange, PlayerLayerMask);

        if (isPlayerInAttackRange && isPlayerInSightRange)
        {
            CurrentAI = AIBehavior.Attack;
        }
        else if (!isPlayerInAttackRange && isPlayerInSightRange)
        {
            CurrentAI = AIBehavior.Follow;
        }
        else
        {
            CurrentAI = allowEnemyPatrol ? AIBehavior.Patrol : AIBehavior.Idle;
        }


        switch (CurrentAI)
        {
            case AIBehavior.Patrol:
                Debug.Log("AI Patrolling");
                if (!isWalkingPointSet)
                {
                    FindNewWalkPoint();
                }

                if (isWalkingPointSet)
                {
                    enemyNavMesh.SetDestination(walkingPoint);
                }

                Vector3 distanceToWalkPoint = transform.position - walkingPoint;

                if (distanceToWalkPoint.magnitude < 1)
                {
                    isWalkingPointSet = false;
                }
                break;

            case AIBehavior.Follow:
                enemyNavMesh.SetDestination(player.transform.position);
                break;
            
            case AIBehavior.Attack:

                enemyNavMesh.SetDestination(transform.position);
                transform.LookAt(player.transform);

                if (!alreadyAttacked)
                {
                    //////Attack Code Here///////
                             
                    alreadyAttacked = true;
                    Invoke(nameof(ResetAttack), AttackInterval);

                }
                break;

            default:
                break;

        }
    }

    private void FindNewWalkPoint()
    {
        float randX = UnityEngine.Random.Range(-WalkingPointRange, WalkingPointRange);
        float randZ = UnityEngine.Random.Range(-WalkingPointRange, WalkingPointRange);

        walkingPoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

        if (Physics.Raycast(walkingPoint, -transform.up, 2f, GroundLayerMask))
        {
            isWalkingPointSet = true;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = true;
    }

    public void TakeDamage(int damage)
    {
        if (Armor > 0 )
        {
            Health -= Mathf.RoundToInt((float)damage * (1 - Armor));
        }
        else
        {
            Health -= damage;
        }

        if (Health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);            
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);        
    }

    private void OnDrawGizmosSelected()
    {
        if (ShowAlertRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, AiSightRange);
        }

        if (ShowAttackRange)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AiAttackRange);
        }
    }
}
