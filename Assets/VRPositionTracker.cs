using UnityEngine;
using TMPro;

public class VRPositionTracker : MonoBehaviour
{

    [SerializeField]
    private TMP_Text txtComponent;

    private Vector3 previousPosition;
    private float movementSpeed;

    void Start()
    {
        previousPosition = Camera.main.transform.position;
    }

    void Update()
    {
        // Getting the position of the camera (headset)
        Vector3 headsetPosition = Camera.main.transform.position;

        // Get the rotation of the camera (headset)
        Quaternion headsetRotation = Camera.main.transform.rotation;

        // Calculate movement speed (distance traveled per frame)
        movementSpeed = Vector3.Distance(headsetPosition, previousPosition) / Time.deltaTime;

        // Update the previous position for the next frame
        previousPosition = headsetPosition;

        // Format the output for readability
        string positionText = string.Format("Position: {0}\nRotation: {1}\nSpeed: {2:F2} units/s",
                                            headsetPosition.ToString("F3"),
                                            headsetRotation.eulerAngles.ToString("F3"),
                                            movementSpeed);
        txtComponent.text = positionText;

        // Debugging (optional)
        Debug.Log("Position: " + headsetPosition);
        Debug.Log("Rotation: " + headsetRotation.eulerAngles);
        Debug.Log("Speed: " + movementSpeed);
    }
}
