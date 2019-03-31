using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPickup : MonoBehaviour
{
    public float staminaAmount = 50f;
    public int dropWeight = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            other.GetComponent<PlayerController>().AcceptStamina(staminaAmount);
            Destroy(gameObject);
        }
    }
}
