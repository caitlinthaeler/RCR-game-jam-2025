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
    [SerializeField] private float timeToLosePlayer = 3f;
    [SerializeField] private float searchDuration = 8f;
    [SerializeField] private float huntSpeedMultiplier = 1.5f;
    [SerializeField] private float normalSpeedMultiplier = 1f;
    [SerializeField] private float searchPointSpacing = 5f;
    
    private NavMeshAgent agent;
    private float nextPathUpdate;
    private Vector3 lastPlayerPos;
    private Vector3 lastKnownPlayerPos;
    private float timeSinceLastSeenPlayer;
    private float searchTimer;
    private float baseSpeed;
    private Vector3[] searchPoints;
    private int currentSearchPointIndex;
    private bool wasPlayerVisible;
    
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
        lastPlayerPos = player.position;
        
        // Configure NavMeshAgent for better path following
        agent.updateRotation = true;
        agent.updatePosition = true;
        agent.stoppingDistance = 0.1f;
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

        bool isPlayerVisible = CanSeePlayer();
        
        // Debug visibility changes
        if (isPlayerVisible != wasPlayerVisible)
        {
            Debug.Log($"Player visibility changed: {isPlayerVisible}");
            wasPlayerVisible = isPlayerVisible;
        }

        // Only update last known position if we can see the player
        if (isPlayerVisible && lastPlayerPos != player.position)
        {
            lastPlayerPos = player.position;
            Debug.Log($"Updated last known player position: {lastPlayerPos}");
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
            agent.SetDestination(player.position);
            Debug.Log($"Player detected! Distance: {Vector3.Distance(transform.position, player.position)}");
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
            agent.SetDestination(player.position);
            Debug.Log($"Chasing player at distance: {Vector3.Distance(transform.position, player.position)}");
        }
        else
        {
            timeSinceLastSeenPlayer += Time.deltaTime;
            Debug.Log($"Time since last seen player: {timeSinceLastSeenPlayer:F1}s");
            
            // Move to last known position when losing sight
            if (agent.destination != lastKnownPlayerPos)
            {
                agent.SetDestination(lastKnownPlayerPos);
                Debug.Log($"Lost sight of player - moving to last known position: {lastKnownPlayerPos}");
            }
            
            // If lost player for too long, switch to searching
            if (timeSinceLastSeenPlayer >= timeToLosePlayer)
            {
                currentState = GhostState.Searching;
                searchTimer = 0f;
                agent.speed = baseSpeed * normalSpeedMultiplier;
                GenerateSearchPoints();
                currentSearchPointIndex = 0;
                Debug.Log($"Lost player! Last known position: {lastKnownPlayerPos}");
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
            agent.SetDestination(player.position);
            Debug.Log("Found player during search!");
            return;
        }

        // If search time is up, return to hunting
        if (searchTimer >= searchDuration)
        {
            currentState = GhostState.Hunting;
            SetHuntingDestination();
            Debug.Log($"Search time up ({searchTimer:F1}s). Returning to hunting.");
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // Move to next search point
            if (currentSearchPointIndex < searchPoints.Length)
            {
                agent.SetDestination(searchPoints[currentSearchPointIndex]);
                Debug.Log($"Moving to search point {currentSearchPointIndex + 1}/{searchPoints.Length} at {searchPoints[currentSearchPointIndex]}");
                currentSearchPointIndex++;
            }
            else
            {
                // If we've checked all points, generate new ones
                Debug.Log("Completed search pattern, generating new points");
                GenerateSearchPoints();
                currentSearchPointIndex = 0;
            }
        }
    }

    private void GenerateSearchPoints()
    {
        // Generate search points in a pattern around the last known position
        int numPoints = Mathf.CeilToInt(searchRadius / searchPointSpacing);
        searchPoints = new Vector3[numPoints * 2]; // Two circles of points

        Debug.Log($"Generating {numPoints * 2} search points around {lastKnownPlayerPos}");

        for (int i = 0; i < numPoints; i++)
        {
            float angle = (2 * Mathf.PI * i) / numPoints;
            
            // Inner circle
            Vector3 innerPoint = lastKnownPlayerPos + new Vector3(
                Mathf.Cos(angle) * (searchRadius * 0.5f),
                0,
                Mathf.Sin(angle) * (searchRadius * 0.5f)
            );
            
            // Outer circle
            Vector3 outerPoint = lastKnownPlayerPos + new Vector3(
                Mathf.Cos(angle) * searchRadius,
                0,
                Mathf.Sin(angle) * searchRadius
            );

            // Ensure points are on NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(innerPoint, out hit, searchRadius, NavMesh.AllAreas))
            {
                searchPoints[i] = hit.position;
            }
            if (NavMesh.SamplePosition(outerPoint, out hit, searchRadius, NavMesh.AllAreas))
            {
                searchPoints[i + numPoints] = hit.position;
            }
        }

        // Shuffle the search points for more natural behavior
        for (int i = searchPoints.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector3 temp = searchPoints[i];
            searchPoints[i] = searchPoints[j];
            searchPoints[j] = temp;
        }
    }

    private bool CanSeePlayer()
    {
        // Check if player is within detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > detectionRange)
        {
            Debug.DrawLine(transform.position, player.position, Color.red);
            Debug.Log($"Player out of range: {distanceToPlayer:F1} > {detectionRange}");
            return false;
        }

        // Check if player is within field of view
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > fieldOfView / 2)
        {
            Debug.DrawLine(transform.position, player.position, Color.yellow);
            Debug.Log($"Player out of field of view: {angleToPlayer:F1}° > {fieldOfView/2}°");
            return false;
        }

        // Check for line of sight
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.5f; // Start slightly above ground
        Vector3 rayEnd = player.position + Vector3.up * 0.5f; // Target slightly above ground
        Vector3 rayDirection = (rayEnd - rayStart).normalized;
        
        // Use Physics.Raycast with all layers except the player layer
        int layerMask = ~LayerMask.GetMask("Player"); // Invert the player layer mask to exclude it
        if (Physics.Raycast(rayStart, rayDirection, out hit, detectionRange, layerMask))
        {
            // If we hit something before reaching the player, line of sight is blocked
            float distanceToHit = Vector3.Distance(rayStart, hit.point);
            float rayDistanceToPlayer = Vector3.Distance(rayStart, rayEnd);
            
            if (distanceToHit < rayDistanceToPlayer)
            {
                Debug.DrawLine(rayStart, hit.point, Color.red);
                Debug.Log($"Line of sight blocked by {hit.transform.name} at {hit.distance:F1} units. Distance to player: {rayDistanceToPlayer:F1}");
                return false;
            }
        }

        // If we didn't hit anything or hit the player, line of sight is clear
        Debug.DrawLine(rayStart, rayEnd, Color.green);
        Debug.Log($"Line of sight clear to player at {Vector3.Distance(rayStart, rayEnd):F1} units");
        return true;
    }

    private void SetHuntingDestination()
    {
        Vector3 targetPosition;
        Vector3 directionToTarget;
        
        if (CanSeePlayer())
        {
            // If we can see the player, hunt towards their position
            targetPosition = player.position;
            directionToTarget = (player.position - transform.position).normalized;
            Debug.Log("Hunting towards visible player");
        }
        else
        {
            // If we can't see the player, hunt based on last known position
            targetPosition = lastKnownPlayerPos;
            directionToTarget = (lastKnownPlayerPos - transform.position).normalized;
            Debug.Log("Hunting towards last known position");
        }
        
        // Calculate a point with random offset
        float randomOffset = Random.Range(-huntRadius * 0.5f, huntRadius * 0.5f);
        Vector3 perpendicular = Vector3.Cross(directionToTarget, Vector3.up).normalized;
        Vector3 targetPoint = transform.position + directionToTarget * huntRadius + perpendicular * randomOffset;
        
        // Ensure the point is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPoint, out hit, huntRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log($"Setting new hunting destination: {hit.position}");
        }
    }
}
