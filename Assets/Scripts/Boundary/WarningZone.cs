using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningZone : MonoBehaviour
{
    Canvas boundaryWarning;
    // Start is called before the first frame update
    void Start()
    {
        boundaryWarning = GameObject.FindWithTag("BoundaryWarning").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER WARNING ZONE");
        if (other.transform.root.tag == "Player")
        {
            boundaryWarning.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            boundaryWarning.enabled = false;
        }
    }
}
