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
    [SerializeField] private float memoryDuration = 2f; // How long the ghost can "remember" player movement after losing sight
    [SerializeField] private float searchDuration = 8f;
    [SerializeField] private float huntSpeedMultiplier = 1.5f;
    [SerializeField] private float normalSpeedMultiplier = 1f;
    [SerializeField] private float searchPointSpacing = 5f;
    [SerializeField] private float searchExpansionRate = 1.5f; // How much to expand search radius over time
    [SerializeField] private float playerDirectionInfluence = 0.3f; // How much to bias hunting towards player (0-1)
    
    [Header("Audio")]
    [SerializeField] private AudioClip walkFootstep;
    [SerializeField] private AudioClip runFootstep;
    [SerializeField] private float walkStepInterval = 0.5f; // Time between footsteps when walking
    [SerializeField] private float runStepInterval = 0.3f;  // Time between footsteps when running
    
    private NavMeshAgent agent;
    private float nextPathUpdate;
    private Vector3 lastPlayerPos;
    private Vector3 lastKnownPlayerPos;
    private Vector3 playerMoveDirection;
    private float timeSinceLastSeenPlayer;
    private float searchTimer;
    private float currentSearchRadius;
    private float baseSpeed;
    private Vector3[] searchPoints;
    private int currentSearchPointIndex;
    private bool wasPlayerVisible;
    private float timeSinceLastActualSight;
    private bool hasLineOfSight;
    
    private AudioSource audioSource;
    private float nextStepTime;
    private float currentStepInterval;
    private bool isRunning;
    
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

        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; // 3D sound
            audioSource.loop = false;
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
        nextStepTime = 0f;
        currentStepInterval = walkStepInterval;
        
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

        // Update footstep timing based on current speed
        float currentSpeed = agent.velocity.magnitude;
        if (currentSpeed > 0.1f) // Only play footsteps if moving
        {
            // Determine if we're running based on speed
            bool shouldBeRunning = currentSpeed > baseSpeed * 1.2f;
            
            // Update step interval and footstep type if running state changed
            if (shouldBeRunning != isRunning)
            {
                isRunning = shouldBeRunning;
                currentStepInterval = isRunning ? runStepInterval : walkStepInterval;
                Debug.Log($"Footstep type changed to: {(isRunning ? "running" : "walking")}");
            }
            
            // Play footstep if it's time
            if (Time.time >= nextStepTime)
            {
                PlayFootstep();
                nextStepTime = Time.time + currentStepInterval;
            }
        }
        else
        {
            isRunning = false;
            currentStepInterval = walkStepInterval;
        }

        bool actualLineOfSight = CheckLineOfSight();
        bool isPlayerVisible = actualLineOfSight || (timeSinceLastActualSight < memoryDuration);
        
        // Update timing for actual line of sight
        if (actualLineOfSight)
        {
            timeSinceLastActualSight = 0f;
            hasLineOfSight = true;
        }
        else
        {
            timeSinceLastActualSight += Time.deltaTime;
            if (timeSinceLastActualSight >= memoryDuration && hasLineOfSight)
            {
                hasLineOfSight = false;
                Debug.Log("Ghost's memory of player movement has faded");
            }
        }
        
        // Debug visibility changes
        if (isPlayerVisible != wasPlayerVisible)
        {
            Debug.Log($"Player visibility changed: {isPlayerVisible}");
            wasPlayerVisible = isPlayerVisible;
        }

        // Track player movement direction when we have actual line of sight
        if (actualLineOfSight && lastPlayerPos != player.position)
        {
            playerMoveDirection = (player.position - lastPlayerPos).normalized;
            lastPlayerPos = player.position;
            Debug.Log($"Updated player movement direction: {playerMoveDirection}");
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
            agent.speed = baseSpeed * huntSpeedMultiplier;
            Debug.Log($"Chasing player at distance: {Vector3.Distance(transform.position, player.position)}");
        }
        else
        {
            timeSinceLastSeenPlayer += Time.deltaTime;
            Debug.Log($"Time since last seen player: {timeSinceLastSeenPlayer:F1}s");
            
            // Project where the player might have gone based on their last movement direction
            Vector3 predictedPosition = lastKnownPlayerPos + playerMoveDirection * 5f; // Look 5 units ahead
            NavMeshHit hit;
            if (NavMesh.SamplePosition(predictedPosition, out hit, 5f, NavMesh.AllAreas))
            {
                lastKnownPlayerPos = hit.position; // Update last known position to the predicted position
                agent.SetDestination(hit.position);
                Debug.Log($"Lost sight of player - moving to predicted position: {hit.position}");
            }
            else
            {
                // If can't find valid position ahead, just go to last known position
                agent.SetDestination(lastKnownPlayerPos);
                Debug.Log($"Lost sight of player - moving to last known position: {lastKnownPlayerPos}");
            }
            
            // If lost player for too long, switch to searching
            if (timeSinceLastSeenPlayer >= timeToLosePlayer)
            {
                currentState = GhostState.Searching;
                searchTimer = 0f;
                currentSearchRadius = searchRadius * 0.5f; // Start with a smaller radius since we have a good idea where they went
                agent.speed = baseSpeed * normalSpeedMultiplier;
                GenerateSearchPoints();
                currentSearchPointIndex = 0;
                Debug.Log($"Lost player! Starting focused search around predicted position");
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
            // Expand search radius over time
            currentSearchRadius = searchRadius * (1 + (searchTimer / searchDuration) * searchExpansionRate);
            
            // Move to next search point
            if (currentSearchPointIndex < searchPoints.Length)
            {
                agent.SetDestination(searchPoints[currentSearchPointIndex]);
                Debug.Log($"Moving to search point {currentSearchPointIndex + 1}/{searchPoints.Length} at {searchPoints[currentSearchPointIndex]}. Current search radius: {currentSearchRadius:F1}");
                currentSearchPointIndex++;
            }
            else
            {
                // If we've checked all points, generate new ones with expanded radius
                Debug.Log($"Completed search pattern, generating new points with radius {currentSearchRadius:F1}");
                GenerateSearchPoints();
                currentSearchPointIndex = 0;
            }
        }
    }

    private void GenerateSearchPoints()
    {
        // Use the current (possibly expanded) search radius
        int numPoints = Mathf.CeilToInt(currentSearchRadius / searchPointSpacing);
        searchPoints = new Vector3[numPoints * 2]; // Two circles of points

        // Weight the search points towards the direction the player was moving
        Vector3 searchCenter = lastKnownPlayerPos + playerMoveDirection * (currentSearchRadius * 0.3f);
        Debug.Log($"Generating {numPoints * 2} search points around {searchCenter} with radius {currentSearchRadius:F1}");

        for (int i = 0; i < numPoints; i++)
        {
            float angle = (2 * Mathf.PI * i) / numPoints;
            
            // Inner circle at half the current radius
            Vector3 innerPoint = searchCenter + new Vector3(
                Mathf.Cos(angle) * (currentSearchRadius * 0.5f),
                0,
                Mathf.Sin(angle) * (currentSearchRadius * 0.5f)
            );
            
            // Outer circle at full current radius
            Vector3 outerPoint = searchCenter + new Vector3(
                Mathf.Cos(angle) * currentSearchRadius,
                0,
                Mathf.Sin(angle) * currentSearchRadius
            );

            // Ensure points are on NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(innerPoint, out hit, currentSearchRadius, NavMesh.AllAreas))
            {
                searchPoints[i] = hit.position;
            }
            if (NavMesh.SamplePosition(outerPoint, out hit, currentSearchRadius, NavMesh.AllAreas))
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

    private bool CheckLineOfSight()
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
        Vector3 rayStart = transform.position + Vector3.up * 0.5f;
        Vector3 rayEnd = player.position + Vector3.up * 0.5f;
        Vector3 rayDirection = (rayEnd - rayStart).normalized;
        
        int layerMask = ~LayerMask.GetMask("Player");
        if (Physics.Raycast(rayStart, rayDirection, out hit, detectionRange, layerMask))
        {
            float distanceToHit = Vector3.Distance(rayStart, hit.point);
            float rayDistanceToPlayer = Vector3.Distance(rayStart, rayEnd);
            
            if (distanceToHit < rayDistanceToPlayer)
            {
                Debug.DrawLine(rayStart, hit.point, Color.red);
                Debug.Log($"Line of sight blocked by {hit.transform.name} at {hit.distance:F1} units. Distance to player: {rayDistanceToPlayer:F1}");
                return false;
            }
        }

        Debug.DrawLine(rayStart, rayEnd, Color.green);
        Debug.Log($"Line of sight clear to player at {Vector3.Distance(rayStart, rayEnd):F1} units");
        return true;
    }

    private bool CanSeePlayer()
    {
        return hasLineOfSight || timeSinceLastActualSight < memoryDuration;
    }

    private void SetHuntingDestination()
    {
        Vector3 targetPosition;
        Vector3 directionToTarget;
        
        // Blend between random direction and player direction based on playerDirectionInfluence
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0;
        randomDirection.Normalize();
        
        Vector3 towardsPlayer = (player.position - transform.position).normalized;
        
        // Blend the directions
        directionToTarget = Vector3.Lerp(randomDirection, towardsPlayer, playerDirectionInfluence);
        directionToTarget.Normalize();
        
        // Calculate target point with random offset
        float randomOffset = Random.Range(-huntRadius * 0.5f, huntRadius * 0.5f);
        Vector3 perpendicular = Vector3.Cross(directionToTarget, Vector3.up).normalized;
        Vector3 targetPoint = transform.position + directionToTarget * huntRadius + perpendicular * randomOffset;
        
        // Ensure the point is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPoint, out hit, huntRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log($"Setting new hunting destination with {playerDirectionInfluence * 100:F0}% player influence: {hit.position}");
        }
    }

    private void PlayFootstep()
    {
        if (audioSource == null || (walkFootstep == null && runFootstep == null)) return;

        // Choose appropriate footstep sound based on running state
        AudioClip footstepSound = isRunning ? runFootstep : walkFootstep;
        
        if (footstepSound != null)
        {
            audioSource.clip = footstepSound;
            audioSource.pitch = 1f; // Keep pitch constant
            audioSource.Play();
        }
    }
}
