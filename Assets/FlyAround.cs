using UnityEngine;

public class FlyAround : MonoBehaviour
{
    public Transform target;  // The target to fly around (can be AR Camera or player's position)
    public float speed = 20f; // Speed at which the ghost flies around
    public float movementRange = 5f; // Range in which the ghost can move in the room
    public float floatSpeed = 0.5f; // Speed of floating up/down
    private float angle = 0f; // Angle to move the object
    private Vector3 randomTarget;  // Random target position for the ghost
    private float movementInterval = 2f; // Time interval to change the target
    private float movementTimer = 0f; // Timer for random target switching
    private float floatAmount = 0f;  // How much it floats up/down

    void Start()
    {
        SetRandomTarget();  // Set initial random target
    }

    void Update()
    {
        Debug.Log("Ghost Script Running");
        // Move the ghost towards the current random target
        transform.position = Vector3.MoveTowards(transform.position, randomTarget, speed * Time.deltaTime);

        // Make the ghost float up and down
        floatAmount += floatSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, Mathf.Sin(floatAmount) * 0.5f + target.position.y, transform.position.z);

        // If the ghost reaches its target, set a new random target
        if (Vector3.Distance(transform.position, randomTarget) < 0.1f)
        {
            SetRandomTarget();
        }

        // Optionally, you can make it float around in a randomized direction by adjusting the interval
        movementTimer += Time.deltaTime;
        if (movementTimer >= movementInterval)
        {
            SetRandomTarget();
            movementTimer = 0f; // Reset timer
        }
    }

    // Set a new random target within a specified range
    void SetRandomTarget()
    {
        float randomX = target.position.x + Random.Range(-movementRange, movementRange);
        float randomY = target.position.y + Random.Range(-movementRange, movementRange);
        float randomZ = target.position.z + Random.Range(-movementRange, movementRange);

        randomTarget = new Vector3(randomX, randomY, randomZ);
    }
}
