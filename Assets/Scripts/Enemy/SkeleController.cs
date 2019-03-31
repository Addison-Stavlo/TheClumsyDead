using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeleController : MonoBehaviour
{
    public float agroRange = 15.0f;
    public float meleeRange = 3.0f;
    public float attackDelay = 1.0f;
    public float damage = 10f;
    public float maxHP = 50f;
    public float hitPoints;
    float attackTimer;
    public bool isDead = false;
    float distanceToPlayer;

    public float attackSwingTime = 1.2f;
    public float swingLoadUp = 0.8f;
    float attackCollisionTimer;
    private UnityEngine.UI.Image healthBar;
    private UnityEngine.UI.Image healthBarBG;
    Vector3 spawnPoint;
    Vector3 moveDir = Vector3.zero;
    // CharacterController controller;
    Animator anim;
    Transform player;
    UnityEngine.AI.NavMeshAgent nav;

    CapsuleCollider collision;

    Transform skelePosition;

    ScoreManager scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        // controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        scoreManager = GameObject.FindGameObjectWithTag("Player").transform.Find("Canvas").Find("ScoreBoard").GetComponent<ScoreManager>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        collision = GetComponent<CapsuleCollider>();
        skelePosition = GetComponent<Transform>();
        spawnPoint = skelePosition.position;
        healthBarBG = transform.Find("EnemyCanvas").Find("HealthBG").GetComponent<Image>();
        healthBar = transform.Find("EnemyCanvas").Find("HealthBG").Find("Health").GetComponent<Image>();
        hitPoints = maxHP;

    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = (skelePosition.position - player.position).magnitude;
        Movement();
        attackTimer += Time.deltaTime;
        attackCollisionTimer += Time.deltaTime;
        Attack();
    }

    void Movement()
    {

        if (hitPoints > 0)
        {

            // float distanceToSpawnPoint = (skelePosition.position - spawnPoint).magnitude;

            if (distanceToPlayer >= meleeRange - 1.5)
            {
                nav.SetDestination(player.position);
                // anim.SetBool("isMoving", true);
            }
            else if (distanceToPlayer < meleeRange - 1.5)
            {
                nav.SetDestination(skelePosition.position);
                transform.LookAt(player);
                // anim.SetBool("isMoving", false);
            }
            // else if (distanceToSpawnPoint > 2)
            // {
            //     nav.SetDestination(spawnPoint);
            // }

            // nav.SetDestination(player.position);

            if (nav.hasPath)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            nav.enabled = false;
        }
    }

    void Attack()
    {
        if (hitPoints > 0)
        {
            if (attackTimer >= attackDelay && distanceToPlayer < meleeRange)
            {
                anim.Play("Attack");
                attackTimer = 0f;
                attackCollisionTimer = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (attackCollisionTimer <= attackSwingTime && attackCollisionTimer > swingLoadUp)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (hitPoints > 0)
        {
            hitPoints -= damage;
            healthBar.fillAmount = hitPoints / maxHP;
            anim.Play("Damage");

            if (hitPoints <= 0)
            {
                Die();
            }

        }
    }

    void Die()
    {
        anim.Play("Death");
        scoreManager.AddPoints();
        isDead = true;
        collision.enabled = false;
        healthBar.enabled = false;
        healthBarBG.enabled = false;
    }
}
