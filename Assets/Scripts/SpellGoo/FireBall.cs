using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 30f;
    public float explosionRadius = 10f;

    Vector3 moveDir = Vector3.zero;
    CharacterController controller;
    Transform camera;


    public Transform[] particles;
    Rigidbody rigidBody;
    bool hitTarget = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        camera = GameObject.FindWithTag("MainCamera").transform;
        moveDir = camera.forward;
        // moveDir = transform.TransformDirection(moveDir);
        rigidBody.velocity = moveDir * speed;
    }

    // Update is called once per frame
    void Update()
    {
        // controller.Move(moveDir * speed * Time.deltaTime);

        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 2))
        // {
        //     float height = transform.position.y + 1 - hit.distance;
        //     transform.position = new Vector3(transform.position.x, height, transform.position.z);
        // }
    }

    void FixedUpdate()
    {
        // 'attach' to ground if it gets low enough
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 2f))
        {
            if (hit.transform.root.tag != "Player")
            {
                float height = transform.position.y + 1f - hit.distance;
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
                moveDir = new Vector3(moveDir.x, 0, moveDir.z);
                moveDir = Vector3.Normalize(moveDir);
                // moveDir = transform.TransformDirection(moveDir);
                rigidBody.velocity = moveDir * speed;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.transform.root.tag != "Player" && other.tag != "FireBall" && other.tag != "WarningZone" && other.tag != "PickUp")
        {
            if (!hitTarget)
            {
                hitTarget = true;
                Instantiate(particles[0], this.transform.position, this.transform.rotation);
                Destroy(gameObject, 0f);
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
                int i = 0;
                while (i < hitColliders.Length)
                {

                    SkeleController skeleController = hitColliders[i].GetComponent<SkeleController>();
                    if (skeleController != null)
                    {
                        skeleController.TakeDamage(damage);
                    }
                    i++;
                }
            }
        }


        // SkeleController skeleController = other.GetComponent<SkeleController>();
        // if (skeleController != null)
        // {
        //     skeleController.TakeDamage(damage);

        // }
    }
}
