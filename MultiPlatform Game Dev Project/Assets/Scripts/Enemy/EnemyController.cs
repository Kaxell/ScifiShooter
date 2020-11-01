using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


//Path Finding Algorithm is referenced from this video: https://www.youtube.com/watch?v=UjkSFoLxesw


public class EnemyController : MonoBehaviour
{
    private GameObject gameController;

    //Variables
    public Canvas UI;
    public Text NameText;
    public Slider healthBar;

    
    [Range(1, 9)]
    public int Level = 1;

    public int Health;
    public float Armor;
    public int EnemyDamagePower;
    private NavMeshAgent enemyNavMesh;

    public LayerMask GroundLayerMask;
    public LayerMask PlayerLayerMask;
    private GameObject player;

    private Vector3 walkingPoint;
    public float WalkingPointRange = 5f;
    private bool isWalkingPointSet;

    public float AttackInterval = 1f;
    private bool alreadyAttacked;

    public float AiSightRange = 20;
    public float AiAttackRange = 3;

    private bool isPlayerInSightRange;
    private bool isPlayerInAttackRange;

    private AudioSource hitPlayerSound;

    public enum AIBehavior { Idle, Patrol, Follow, Attack };
    public AIBehavior CurrentAI;
    public bool allowEnemyPatrol;


    //Debug
    //[Header("DEBUG")]
    //public bool ShowAttackRange = true;
    //public bool ShowAlertRange = true;
   

    public void Init(int level)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        enemyNavMesh = GetComponent<NavMeshAgent>();
        CurrentAI = AIBehavior.Idle;
        allowEnemyPatrol = true;
        hitPlayerSound = GetComponent<AudioSource>();
        SetEnemyLevel(level);
    }


    public void SetEnemyLevel(int level)
    {
        Level = level;
        Health *= Level;
        Armor = Mathf.Clamp(Armor * Level, 0.0f, 1f);
        EnemyDamagePower = EnemyDamagePower * Level;
        healthBar.maxValue = Health;
        healthBar.minValue = 0;
        healthBar.value = Health;
        enemyNavMesh.speed = 5 + ((float)Level / 2);
        enemyNavMesh.acceleration = 2 + ((float)Level / 5);
        NameText.text = "Level " + Level + " Monster";
        
        if (Level == 1)
        {
            NameText.color = Color.white;
        }
        else if (Level == 2)
        {
            NameText.color = Color.blue;
        }
        else if (Level == 3)
        {
            NameText.color = Color.red;
        }

    }
    

    // Update is called once per frame
    void Update()
    {
        isPlayerInSightRange = Physics.CheckSphere(transform.position, AiSightRange, PlayerLayerMask);
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, AiAttackRange, PlayerLayerMask);
        CurrentAI = AIBehavior.Attack;
        
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
                Debug.DrawLine(transform.position, player.transform.position, Color.yellow);
                break;
            
            case AIBehavior.Attack:

                enemyNavMesh.SetDestination(transform.position);
                transform.LookAt(player.transform);
                Debug.DrawLine(transform.position, player.transform.position, Color.red);

                if (!alreadyAttacked)
                {
                    //////Attack Code///////                    
                    player.transform.gameObject.GetComponent<PlayerStatController>().TakeDamage(EnemyDamagePower);
                    Debug.Log("Player is getting attacked!");
                    alreadyAttacked = true;
                    hitPlayerSound.Play();
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
        alreadyAttacked = false;
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
        
        healthBar.value = Health;
        
        if (Health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);            
        }
    }

    private void DestroyEnemy()
    {
        if (UnityEngine.Random.value < 0.1f)
        {
            var healthPickUp = GameObject.FindGameObjectWithTag("PickUpHealth");
            Instantiate(healthPickUp, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);
        }
        else if (UnityEngine.Random.value < 0.2f)
        {
            var ammoPickUp = GameObject.FindGameObjectWithTag("PickUpAmmo");
            Instantiate(ammoPickUp, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);
        }

        gameController.GetComponent<GameController>().OnEnemyDestroyed(gameObject); 
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AiSightRange);                        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AiAttackRange);        
    }
}
