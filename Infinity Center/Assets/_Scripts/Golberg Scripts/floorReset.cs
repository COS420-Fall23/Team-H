using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorReset : MonoBehaviour
{
    Vector3 startingLocation;
    // Start is called before the first frame update
    void Start()
    {
        startingLocation = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            gameObject.transform.position = startingLocation;
            rb.velocity = new Vector3(0, 0, 0);
        }

    }
}
