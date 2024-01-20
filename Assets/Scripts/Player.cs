using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Player reference
    /// </summary>
    public static Player instance;

    /// <summary>
    /// Player movement speed
    /// </summary>
    public float moveSpeed = 2f;

    /// <summary>
    /// Player animator component
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Pounch time - default 500ms
    /// </summary>
    float phaseInterval = 0.5f;

    /// <summary>
    /// Random chance 
    /// (using Punch ID)
    /// </summary>
    int randomPunchCriteria = 0;

    /// <summary>
    /// Check if double punch is posible
    /// </summary>
    bool doublePunchPhaseAvailable = false;

    /// <summary>
    /// Trigger charge attack
    /// </summary>
    bool chargeAttack = false;

    /// <summary>
    /// Punches sound FX
    /// </summary>
    public AudioSource[] punchFX;

    /// <summary>
    /// Collider sensitive punches
    /// (reference by default - hands)
    /// </summary>
    public HitBox[] hitBoxes;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Attack state
    /// </summary>
    /// <param name="doublePunch"></param>
    public void AttackState()
    {
        for (int i = 0; i < hitBoxes.Length; i++)
        {
            hitBoxes[i]._thisCollider.enabled = true;
        }

        // get random punch 
        randomPunchCriteria = Random.Range(0, 2);
        animator.SetInteger("PunchID", randomPunchCriteria);

        // P3/Charge Initialized
        if (chargeAttack)
        {
            animator.SetBool("chargeAttack", true);

            for (int i = 0; i < hitBoxes.Length; i++)
            {
                hitBoxes[i]._thisCollider.enabled = true;
            }

            doublePunchPhaseAvailable = false;
            Bot.instance.RecieveDamage(100, punchFX[2]);
            chargeAttack = false;

        }

        // P2 Initialized
        if (doublePunchPhaseAvailable)
        {
            animator.SetBool("doubleAttack", true);
            phaseInterval = 0;
            chargeAttack = true;

            Bot.instance.RecieveDamage(10, punchFX[1]);
        }

        // P1 Initialized
        if (!doublePunchPhaseAvailable && !chargeAttack)
        {
            animator.SetBool("attack", true);
            phaseInterval = 0;
            doublePunchPhaseAvailable = true;

            Bot.instance.RecieveDamage(10, punchFX[0]);
        }

    }

    /// <summary>
    /// Control character movements
    /// </summary>
    public void Movement()
    {
        animator.SetFloat("StrafeX", -Input.GetAxis("Vertical"));
        animator.SetFloat("StrafeZ", Input.GetAxis("Horizontal"));

        transform.LookAt(Bot.instance.transform);
    }

    /// <summary>
    /// Handling Animator reset
    /// </summary>
    void Reset_AttackAnimatorPreferences()
    {
        animator.SetBool("attack", false);
        animator.SetBool("doubleAttack", false);

        for (int i = 0; i < hitBoxes.Length; i++)
        {
            hitBoxes[i]._thisCollider.enabled = false;
        }

    }

    void Update()
    {
        phaseInterval += Time.deltaTime;

        if (phaseInterval >= 0.5f)
        {
            print("500ms depleted,P2 punch not available");
            phaseInterval = 0;
            Reset_AttackAnimatorPreferences();
            doublePunchPhaseAvailable = false;
        } 

        Movement();

    }
}

