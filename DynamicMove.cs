using UnityEngine;

public class DynamicMove : MonoBehaviour
{

public Transform target; // The target object to orbit around
    public float moveSpeed = 5f; // The movement speed of the orbital pattern
    public float orbitRadius = 5f; // The radius of the orbital pattern

    private Vector3 lastKnownPosition; // The last known position of the target
    public LayerMask mask;
    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target object is not assigned!");
            enabled = false;
            return;
        }

        lastKnownPosition = target.position;
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 directionToTarget = target.position - transform.position;

        // Check for obstacles along the line of sight to the target
        if (Physics.Raycast(transform.position, directionToTarget, out hit,mask))
        {
            // Something is blocking the line of sight, move in an orbital pattern towards the last known position
            Vector3 orbitalPosition = lastKnownPosition + Quaternion.Euler(0, Time.time * moveSpeed, 0) * (directionToTarget.normalized * orbitRadius);
            transform.position = Vector3.MoveTowards(transform.position, orbitalPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            // No obstacles, move towards the target
            transform.position = Vector3.MoveTowards(transform.position, lastKnownPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the orbit radius in the Unity editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(lastKnownPosition, orbitRadius);
        //Draw a line between the transform and last known position
        Debug.DrawLine(transform.position, lastKnownPosition);
    }
    
}