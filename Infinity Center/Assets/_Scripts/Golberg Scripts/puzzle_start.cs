using UnityEngine;
using HVRPhysicsButton;
using HurricaneVR;
public class EnableRigidbodyOnButtonPress : MonoBehaviour
{
    // Reference to the HVRPhysicsButton component.
    [SerializeField]public HVRPhysicsButton button_top;

    // Reference to the Rigidbody you want to enable.
    [SerializeField]public Rigidbody targetRigidbody;

    private void Awake()
    {
        // Ensure the Rigidbody is not null.
        if (targetRigidbody == null)
        {
            targetRigidbody = GetComponent<Rigidbody>();
        }

        // Register the button down event.
        hvrPhysicsButton.ButtonDown.AddListener(HandleButtonDown);
    }

    private void OnDestroy()
    {
        // Unregister the button down event to avoid memory leaks.
        hvrPhysicsButton.ButtonDown.RemoveListener(HandleButtonDown);
    }

    private void HandleButtonDown(HVRPhysicsButton pressedButton)
    {
        // Enable the Rigidbody when the button is pressed.
        if (targetRigidbody != null)
        {
            targetRigidbody.isKinematic = false; // if you want to enable physics interactions
            targetRigidbody.useGravity = true; // if you want gravity to affect the Rigidbody
        }
    }
}
