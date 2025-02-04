using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomTroopMovement : MonoBehaviour
{
    public GameObject hero;
    public NavMeshAgent agent;
    public float followDistance = 5;
    public float speed;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.destination = hero.transform.position;
        agent.speed = speed;
        agent.autoRepath = true;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(this.transform.position, hero.transform.position);
        if (dist <= followDistance)
        {
            agent.speed = 0;

            animator.SetBool("isWalking", false);
        }
        else
        {
            agent.speed = speed;
            agent.destination = hero.transform.position;
            animator.SetBool("isWalking", true);
        }
    }
}
