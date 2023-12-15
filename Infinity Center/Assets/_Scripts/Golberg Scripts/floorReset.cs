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
        if (other.GameObject.tag == "Respawn")
        {
            gameObject.transform(startingLocation);
        }

    }
}
