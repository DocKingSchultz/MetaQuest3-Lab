using UnityEngine;
using TMPro;

public class VRPositionTracker : MonoBehaviour
{

    [SerializeField]
    private TMP_Text txtComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Getting the position of the camera (headset)
        Vector3 headsetPosition = Camera.main.transform.position;
        Debug.Log("Headset Position: " + headsetPosition);
        txtComponent.text = headsetPosition.ToString();
    }
}
