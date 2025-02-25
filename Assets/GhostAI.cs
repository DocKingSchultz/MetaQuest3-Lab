using UnityEngine;
using System.Collections;

public class GhostAI : MonoBehaviour
{
    public Transform player; // Assign CenterEyeAnchor (OVR Camera Rig)
    public float moveSpeed = 3f; // Running speed
    public float rotationSpeed = 3f; // How fast the ghost turns
    public float idleTime = 2f; // How long the ghost idles before chasing again

    private Animator animator;
    private Vector3 targetPosition;
    private enum GhostState { Spawning, Chasing, Surprising, Attacking, Fleeing, Idling }
    private GhostState currentState;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(GhostLoop()); // Start ghost behavior loop
    }

    IEnumerator GhostLoop()
    {
        while (true)
        {
            // Step 1: Spawn Behind Player
            SpawnBehindPlayer();
            yield return new WaitForSeconds(1f);

            // Step 2: Run Towards Player
            ChangeState(GhostState.Chasing);
            SetTargetPosition(player.position + player.forward * 1f); // Pass in front
            yield return MoveToTarget();

            // Step 3: Surprise Animation in Front of Player
            ChangeState(GhostState.Surprising);
            animator.SetTrigger("isSurprised");
            yield return new WaitForSeconds(1.5f); // Play surprise animation

            // Step 4: Attack the Player
            ChangeState(GhostState.Attacking);
            animator.SetBool("isAttacking", true);
            yield return new WaitForSeconds(2f); // Attack duration
            animator.SetBool("isAttacking", false);

            // Step 5: Run Away from Player
            ChangeState(GhostState.Fleeing);
            SetTargetPosition(player.position - player.forward * 5f); // Run away
            yield return MoveToTarget();

            // Step 6: Idle for a While
            ChangeState(GhostState.Idling);
            yield return new WaitForSeconds(idleTime);

            // Loop back to Step 2 (Chasing)
        }
    }

    void SpawnBehindPlayer()
    {
        transform.position = player.position - player.forward * 3f; // Spawn 3m behind
        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z); // Keep floating
        ChangeState(GhostState.Spawning);
    }

    void SetTargetPosition(Vector3 newTarget)
    {
        targetPosition = newTarget;
    }

    IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z); // Keep at same height
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void ChangeState(GhostState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GhostState.Chasing:
                animator.SetBool("isRunning", true);
                break;
            case GhostState.Surprising:
            case GhostState.Attacking:
            case GhostState.Idling:
                animator.SetBool("isRunning", false);
                break;
            case GhostState.Fleeing:
                animator.SetBool("isRunning", true);
                break;
        }
    }
}