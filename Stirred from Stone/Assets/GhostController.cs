using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float fieldOfView = 120f;
    [SerializeField] private LayerMask detectionLayers;
    
    [Header("Movement")]
    [SerializeField] private float updatePathInterval = 0.1f;
    [SerializeField] private float patrolRadius = 15f;
    [SerializeField] private float searchRadius = 20f;
    [SerializeField] private float huntRadius = 30f;
    
    [Header("Behavior")]
    [SerializeField] private float timeToLosePlayer = 5f;
    [SerializeField] private float searchDuration = 10f;
    [SerializeField] private float huntSpeedMultiplier = 1.5f;
    [SerializeField] private float normalSpeedMultiplier = 1f;
    
    private NavMeshAgent agent;
    private float nextPathUpdate;
    private Vector3 lastPlayerPos;
    private Vector3 lastKnownPlayerPos;
    private float timeSinceLastSeenPlayer;
    private float searchTimer;
    private float baseSpeed;
    
    private enum GhostState
    {
        Hunting,
        Chasing,
        Searching
    }
    private GhostState currentState = GhostState.Hunting;
    
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

        baseSpeed = agent.speed;
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

        switch (currentState)
        {
            case GhostState.Hunting:
                UpdateHunting();
                break;
            case GhostState.Chasing:
                UpdateChasing();
                break;
            case GhostState.Searching:
                UpdateSearching();
                break;
        }
    }

    private void UpdateHunting()
    {
        // Check if player is detected
        if (CanSeePlayer())
        {
            currentState = GhostState.Chasing;
            timeSinceLastSeenPlayer = 0f;
            agent.speed = baseSpeed * huntSpeedMultiplier;
            Debug.Log("Player detected, switching to chasing");
            return;
        }

        // If reached destination or no path, set new hunting point
        if (agent.remainingDistance <= agent.stoppingDistance || !agent.hasPath)
        {
            SetHuntingDestination();
        }
    }

    private void UpdateChasing()
    {
        if (CanSeePlayer())
        {
            timeSinceLastSeenPlayer = 0f;
            lastKnownPlayerPos = player.position;
            
            if (Time.time >= nextPathUpdate)
            {
                agent.SetDestination(player.position);
                nextPathUpdate = Time.time + updatePathInterval;
            }
            Debug.Log("Chasing player");
        }
        else
        {
            timeSinceLastSeenPlayer += Time.deltaTime;
            
            // If lost player for too long, switch to searching
            if (timeSinceLastSeenPlayer >= timeToLosePlayer)
            {
                currentState = GhostState.Searching;
                searchTimer = 0f;
                agent.speed = baseSpeed * normalSpeedMultiplier;
            }
        }
    }

    private void UpdateSearching()
    {
        searchTimer += Time.deltaTime;
        
        if (CanSeePlayer())
        {
            currentState = GhostState.Chasing;
            timeSinceLastSeenPlayer = 0f;
            agent.speed = baseSpeed * huntSpeedMultiplier;
            Debug.Log("Player detected, switching to chasing");
            return;
        }

        // If search time is up, return to hunting
        if (searchTimer >= searchDuration)
        {
            currentState = GhostState.Hunting;
            SetHuntingDestination();
            Debug.Log("Search time up, returning to hunting");
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // Move to random point within search radius
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * searchRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, searchRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            Debug.Log("Moving to random point within search radius");
        }
    }

    private bool CanSeePlayer()
    {
        // Check if player is within detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > detectionRange) return false;

        // Check if player is within field of view
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        return angleToPlayer <= fieldOfView / 2;
    }

    private void SetHuntingDestination()
    {
        // Get direction to player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        
        // Calculate a point between the ghost and player, but offset by some randomness
        float randomOffset = Random.Range(-huntRadius * 0.5f, huntRadius * 0.5f);
        Vector3 perpendicular = Vector3.Cross(directionToPlayer, Vector3.up).normalized;
        Vector3 targetPoint = transform.position + directionToPlayer * huntRadius + perpendicular * randomOffset;
        
        // Ensure the point is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPoint, out hit, huntRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
