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
            gameObject.transform.position = startingLocation;
        }

    }
}
