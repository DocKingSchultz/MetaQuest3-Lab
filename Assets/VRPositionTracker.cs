using UnityEngine;
using TMPro;
using System.IO;

public class VRPositionTracker : MonoBehaviour
{

    [SerializeField]
    private TMP_Text txtComponent;

    private Vector3 previousPosition;
    private float movementSpeed;
    private float distance;
    private Vector3 startingPosition;
    string path;
    bool shouldRecord;



    void Start()
    {
        previousPosition = Camera.main.transform.position;
        startingPosition = Camera.main.transform.position;
        path = Path.Combine(Application.persistentDataPath, "output.csv");
        ClearFile();
       
    }

    void Update()
    {
        // Getting the position of the camera (headset)
        Vector3 headsetPosition = Camera.main.transform.position;

        // Get the rotation of the camera (headset)
        Quaternion headsetRotation = Camera.main.transform.rotation;

        // Calculate movement speed (distance traveled per frame)
        movementSpeed = Vector3.Distance(headsetPosition, previousPosition) / Time.deltaTime;

        // Calculate distance
        distance = Vector3.Distance(new Vector3(headsetPosition.x, 0f, headsetPosition.z), startingPosition);

        // Update the previous position for the next frame
        previousPosition = headsetPosition;

        // Format the output for readability
        string positionText = string.Format("Position: {0}\nRotation: {1}\nSpeed: {2:F2} units/s\n Distance: {3:F3}\n Recording: {4}\n",
                                            headsetPosition.ToString("F3"),
                                            headsetRotation.eulerAngles.ToString("F3"),
                                            movementSpeed,
                                            distance,
                                            shouldRecord);
        if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            shouldRecord = true;
        }

        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            shouldRecord = false;
        }

        txtComponent.text = positionText;

        // Debugging (optional)
        Debug.Log("Position: " + headsetPosition);
        Debug.Log("Rotation: " + headsetRotation.eulerAngles);
        Debug.Log("Speed: " + movementSpeed);
        
        if (shouldRecord)
        {
            string saveCsvFormat = string.Format("{0:F3}, {1:F3}, {2:F3}, {3:F3}, {4:F2}\n", headsetPosition.x, headsetPosition.y, headsetPosition.z, distance, movementSpeed);
            SaveFile(saveCsvFormat);
        }
    }

    public void SaveFile(string content)
    {
        try
        {
            
            // Write content to the file
            File.AppendAllText(path, content);
            Debug.Log($"File saved at {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save file: {e.Message}");
        }
    }

    void ClearFile()
    {
        File.WriteAllText(path, string.Empty);
    }

}
