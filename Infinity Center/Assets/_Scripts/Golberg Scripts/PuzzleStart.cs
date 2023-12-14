using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core.ScriptableObjects;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Shared;
using UnityEngine;

public class PuzzleStart : MonoBehaviour
{
    [SerializeField] public Collider buttonCollider; // Assign the mesh collider of the button
    public Rigidbody ballRigidbody; // The Rigidbody of the ball, found by tag

    public void Start()
    {
        // Find the ball using the tag and get its Rigidbody
        GameObject ballObject = GameObject.FindGameObjectWithTag("TaskItem");
        if (ballObject != null)
        {
            ballRigidbody = ballObject.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("Ball with tag 'TaskItem' not found in the scene.");
        }

        // Ensure the Rigidbody is initially disabled
        if (ballRigidbody != null)
        {
            ballRigidbody.isKinematic = true;
        }
        else
        {
            Debug.LogError("Rigidbody not found on the ball object.");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the trigger is the button collider
        if (other == buttonCollider)
        {
            StartPuzzle();
        }
    }

    public void StartPuzzle()
    {
        // Enable the Rigidbody, allowing the ball to move
        if (ballRigidbody != null)
        {
            ballRigidbody.isKinematic = false;
        }
    }
}
