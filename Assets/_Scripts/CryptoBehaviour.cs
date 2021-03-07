using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

public enum CryptoState
{
    IDLE,
    RUN,
    JUMP,
    KICK
}


public class CryptoBehaviour : MonoBehaviour
{
    [Header("Line of Sight")] 
    public bool HasLOS;

    public GameObject player;

    private NavMeshAgent agent;
    private Animator animator;

    [Header("Attack")]
    public float distance;
    public PlayerBehaviour playerBehaviour;
    public int damageDelay = 30;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (HasLOS)
        {
            agent.SetDestination(player.transform.position);
            playerBehaviour = FindObjectOfType<PlayerBehaviour>();
        }


        if(HasLOS && Vector3.Distance(transform.position, player.transform.position) < distance)
        {
                // could be an attack
            animator.SetInteger("AnimState", (int)CryptoState.KICK);
            transform.LookAt(transform.position - player.transform.forward);

            if (Time.frameCount % damageDelay == 0)
            {
                DoKickDamage();
            }

            if (agent.isOnOffMeshLink)
            {
                animator.SetInteger("AnimState", (int)CryptoState.JUMP);
            }
        }
        else if (HasLOS)
        {
            animator.SetInteger("AnimState", (int)CryptoState.RUN);
        }
        else
        {
            animator.SetInteger("AnimState", (int)CryptoState.IDLE);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HasLOS = true;
            player = other.transform.gameObject;
        }
    }

    private void DoKickDamage()
    {
        playerBehaviour.TakeDamage(20);
    }

}

