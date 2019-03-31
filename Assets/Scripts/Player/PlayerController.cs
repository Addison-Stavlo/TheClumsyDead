using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float speed = 4;
    public float sprintSpeed = 8;
    float rot = 0f;
    float vertRot = 180f;
    public float rotSpeed = 80;
    public float gravity = 8;
    public float jumpSpeed = 8;
    public float sprintCost = 10f;
    public float enduranceRegen = 10f;
    float maxEndurance = 100f;
    float currentEndurance;
    bool isSprinting;

    private UnityEngine.UI.Image enduranceBar;
    Vector3 moveDir = Vector3.zero;
    CharacterController controller;
    Animator anim;
    Transform camera;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("CameraBoom").transform;
        enduranceBar = transform.Find("Canvas").Find("EnduranceBG").Find("Endurance").GetComponent<Image>();
        currentEndurance = maxEndurance;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Camera();
    }

    void Camera()
    {
        // right click mouse movement controlls
        if (Input.GetMouseButton(1))
        {
            // hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            if (Input.GetAxis("Mouse X") != 0)
            {
                // turn character
                rot += Input.GetAxis("Mouse X");
                transform.eulerAngles = new Vector3(0, rot, 0);
            }
            if (Input.GetAxis("Mouse Y") != 0)
            {
                // raise/lower camera
                vertRot -= Input.GetAxis("Mouse Y");
                camera.eulerAngles = new Vector3(vertRot, rot, 0);
            }
        }


        if (Input.GetMouseButtonUp(1))
        {
            //show cursor again after rightclick controlls end
            Cursor.lockState = CursorLockMode.None;
        }


        //mouse scroll for camera distance
        if (Input.mouseScrollDelta.magnitude != 0)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                if (camera.localScale.z > 0)
                {
                    camera.localScale -= new Vector3(0, 0, Input.mouseScrollDelta.y);
                }
            }
            else
            {
                camera.localScale -= new Vector3(0, 0, Input.mouseScrollDelta.y);
            }
        }
    }

    void Movement()
    {
        if (controller.isGrounded)
        {


            moveDir = new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
            // Q E for strafe
            if (Input.GetKey(KeyCode.Q))
            {
                moveDir.x = -1;
            }
            if (Input.GetKey(KeyCode.E))
            {
                moveDir.x = 1;
            }
            // hold right click to strafe with A and D
            if (Input.GetMouseButton(1))
            {
                moveDir.x = Input.GetAxis("Horizontal");
            }
            moveDir = Vector3.Normalize(moveDir);

            //sprinting (shift) control
            if (Input.GetButton("Fire3"))
            {
                if (currentEndurance > sprintCost * Time.deltaTime)
                {
                    isSprinting = Input.GetButton("Fire3");
                    currentEndurance -= sprintCost * Time.deltaTime;
                    enduranceBar.fillAmount = currentEndurance / maxEndurance;
                }
                else
                {
                    isSprinting = false;
                }
            }
            else
            {
                isSprinting = false;
                if (currentEndurance < maxEndurance)
                {
                    currentEndurance += enduranceRegen * Time.deltaTime;
                    enduranceBar.fillAmount = currentEndurance / maxEndurance;
                }
            }

            // determining speed to move at!
            if (isSprinting)
            {
                moveDir = moveDir * sprintSpeed;
            }
            else
            {
                moveDir = moveDir * speed;
            }

            moveDir = transform.TransformDirection(moveDir);
            float forward_speed = Vector3.Dot(moveDir, Vector3.forward);
            float lateral_speed = Vector3.Dot(moveDir, Vector3.right);

            // set animation state between idle, walk, sprint
            if (forward_speed != 0)
            {
                if (isSprinting)
                {
                    anim.SetInteger("condition", 2);
                }
                else
                {
                    anim.SetInteger("condition", 1);
                }
            }
            else
            {
                anim.SetInteger("condition", 0);
            }
            if (Input.GetButton("Jump"))
            {
                moveDir.y = jumpSpeed;
                // anim.SetInteger("condition", 2);
            }
        }

        //if not right clicking turn with A and D
        if (!Input.GetMouseButton(1))
        {
            rot += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.eulerAngles = new Vector3(0, rot, 0);
            }
        }

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);
    }

    public void AcceptStamina(float amount)
    {
        currentEndurance = Mathf.Min(maxEndurance, currentEndurance + amount);
        enduranceBar.fillAmount = currentEndurance / maxEndurance;
    }
}
