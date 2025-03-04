using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    [SerializeField] private Transform player;    // Reference to the player's transform
    [SerializeField] private float updatePathInterval = 0.1f;    // How often to update the path
    
    private NavMeshAgent agent;
    private float nextPathUpdate;
    private Vector3 lastPlayerPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component missing from ghost!");
            return;
        }

        // Check if agent is on NavMesh
        if (!agent.isOnNavMesh)
        {
            Debug.LogError("Ghost is not on NavMesh!");
        }

        // Debug info
        Debug.Log($"Ghost NavMeshAgent initialized with speed: {agent.speed}, acceleration: {agent.acceleration}");
        if (player == null)
        {
            Debug.LogError("Player reference not set in Ghost!");
        }
        else
        {
            Debug.Log("Player reference found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || agent == null) return;
        if (!agent.isOnNavMesh)
        {
            Debug.LogError("Ghost is not on NavMesh!");
            return;
        }

        // Only log when player actually moves
        if (lastPlayerPos != player.position)
        {
            Debug.Log($"Player moved to: {player.position}");
            lastPlayerPos = player.position;
        }

        // Update the path periodically rather than every frame for better performance
        if (Time.time >= nextPathUpdate)
        {
            Vector3 targetPos = player.position;
            agent.SetDestination(targetPos);
            
            // More detailed movement debugging
            Debug.Log($"Ghost stats: Position={transform.position}, " +
                     $"Velocity={agent.velocity.magnitude}, " +
                     $"Remaining Distance={agent.remainingDistance}, " +
                     $"Stopping Distance={agent.stoppingDistance}");
            
            // Check if path is valid
            if (agent.hasPath)
            {
                Debug.Log("Path found to player!");
            }
            else
            {
                Debug.LogWarning("No valid path to player!");
            }
            
            nextPathUpdate = Time.time + updatePathInterval;
        }
    }
}
