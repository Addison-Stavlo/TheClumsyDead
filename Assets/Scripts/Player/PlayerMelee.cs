using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    // Start is called before the first frame update
    public float attackDelay = 1.0f;
    public float damage = 25f;
    public float attackSwingTime = 0.4f;
    float attackCollisionTimer;
    float attackTimer;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        attackTimer += Time.deltaTime;
        attackCollisionTimer += Time.deltaTime;
    }

    void GetInput()
    {
        if (Input.GetButton("Fire1"))
        {
            if (attackTimer > attackDelay)
            {
                Attacking();
            }
        }
    }

    void Attacking()
    {
        attackTimer = 0f;
        anim.Play("WK_heavy_infantry_08_attack_B", -1, 0.0f);
        attackCollisionTimer = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (attackCollisionTimer <= attackSwingTime)
        {
            SkeleController skeleController = other.GetComponent<SkeleController>();
            if (skeleController != null)
            {
                skeleController.TakeDamage(damage);
            }
        }
    }
}
