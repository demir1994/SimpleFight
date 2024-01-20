using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class Bot : MonoBehaviour
{
    /// <summary>
    /// Bot reference
    /// </summary>
    public static Bot instance;

    /// <summary>
    /// Bot movement speed
    /// </summary>
    public float moveSpeed = 1.5f;

    /// <summary>
    /// Bot health
    /// </summary>
    public float baseHealth = 100;

    /// <summary>
    /// 
    /// </summary>
    public Slider baseHealthSlider;

    /// <summary>
    /// Punch FX listener
    /// </summary>
    private static AudioSource punch_FX;

    /// <summary>
    /// Recieved damage value
    /// </summary>
    [SerializeField]
    public float damageValue;

    /// <summary>
    /// AI Animator
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Distance to avoid player attacks
    /// </summary>
    public float avoidanceDistance = 2f;

    /// <summary>
    /// Player referene
    /// </summary>
    public Transform player;

    /// <summary>
    /// Random movement interval
    /// </summary>
    public float randomMoveInterval = 2f;

    /// <summary>
    /// Time since last chosen movement position
    /// </summary>
    private float timeSinceLastRandomMove;

    /// <summary>
    /// Random movement destination
    /// </summary>
    private Vector3 randomDestination;

    /// <summary>
    /// Walkable areas reference
    /// </summary>
    public WalkableZone walkableArea;

    /// <summary>
    /// Move position on draw gizmos-debug(color)
    /// </summary>
    public Color drawColor;

    /// <summary>
    /// Desired position for movement
    /// </summary>
    private Vector3 moveHere;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Generate random movement at chosen area
    /// </summary>
    /// <returns></returns>
    public Vector3 RandomMovement()
    {
        // determine posible bot area movements
        float minPosX = walkableArea.min_posX.position.x;
        float maxPosX = walkableArea.max_posX.position.x;

        float minPosZ = walkableArea.min_posZ.position.z;
        float maxPosZ = walkableArea.max_posZ.position.z;

        timeSinceLastRandomMove += Time.deltaTime;

        if (timeSinceLastRandomMove > randomMoveInterval) 
        {
            // gather random position
            float moveToX = Random.Range(minPosX, maxPosX);
            float moveToZ = Random.Range(minPosZ, maxPosZ);

            moveHere = new Vector3(moveToX, 0, moveToZ);

            timeSinceLastRandomMove = 0;
        }

        return moveHere;
    }

    /// <summary>
    /// Control AI movements
    /// </summary>
    public void Movement()
    {
        Vector3 moveToRandom = RandomMovement();

        Vector3 direction = (moveToRandom - transform.position).normalized;
        direction.Normalize();

        animator.SetFloat("StrafeX", direction.x);
        animator.SetFloat("StrafeZ", direction.z);

        //transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

        // Rotate towards the target
        Vector3 directionToTarget = moveToRandom - transform.position;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);

        // Apply the rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, Time.deltaTime * 3f);
    }

    private void Update()
    {
        Movement();
    }

    private void OnDrawGizmosSelected()
    {
        //moving here
        Gizmos.color = new Color(drawColor.r, drawColor.g, drawColor.b, drawColor.a);
        Gizmos.DrawCube(moveHere, new Vector3(0.2f, 1.5f, 0.2f));
    }

    /// <summary>
    /// Avoid Player attacks
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="punchFX"></param>
    //void AvoidMeleeAttack()
    //{
    //    if (player == null)
    //    {
    //        Debug.LogError("Player not assigned to the AI controller.");
    //        return;
    //    }

    //    // Check the distance between AI and player
    //    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    //    // If the player is within the avoidance distance, move away from the player
    //    if (distanceToPlayer < avoidanceDistance)
    //    {
    //        Vector3 directionToPlayer = player.position - transform.position;
    //        Vector3 newPosition = transform.position - directionToPlayer.normalized * moveSpeed * Time.deltaTime;

    //        // Update the AI position
    //        transform.position = newPosition;
    //    }
    //}

    /// <summary>
    /// Recieve damage from opponent
    /// </summary>
    public void RecieveDamage(float damage, AudioSource punchFX)
    {
        damageValue = damage;
        punch_FX = punchFX;
        print(damageValue);

        if (baseHealth <= 0)
        {
            GameManager.gameManager.WinGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HitBox" && baseHealth >= 0)
        {
            baseHealth -= damageValue;
            baseHealthSlider.value = baseHealth;

            if (punch_FX != null)
            punch_FX.Play();
        }
    }
}

