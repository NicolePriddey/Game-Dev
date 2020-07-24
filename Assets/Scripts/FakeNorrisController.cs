using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FakeNorrisController : MonoBehaviour
{
    public List<AudioClip> audioClips;
    private NavMeshAgent navMeshAgent;
    private bool wander = true;
    private float timeToLive = 10;
    private float deathTime;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        deathTime = Time.time + timeToLive;

        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioClips.Find(x => x.name.Equals("spawn"));
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < deathTime)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;

                if (Physics.Raycast(ray, out raycastHit))
                    assignDestination(raycastHit.point);
            }
            else if (navMeshAgent.velocity.Equals(new Vector3(0, 0, 0)) && wander)
                setNewDestination();

            Animator animator = GetComponent<Animator>();
            if (navMeshAgent.desiredVelocity.magnitude > 0.7 && navMeshAgent.hasPath)
                animator.SetBool("isRunning", true);
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && Random.Range(0, 100) > 97)
                    animator.SetTrigger("bored");

                if (animator.GetBool("isRunning"))
                    animator.SetBool("isRunning", false);
            }
        }
        else
            die();
    }

    private void die()
    {
        AudioSource.PlayClipAtPoint(audioClips.Find(x => x.name.Equals("despawn")), GameObject.Find("Cluck Norris").transform.position);
        Destroy(gameObject);
    }

    /**
     * Gets a random location no more than 10 units away from it.
     */
    private void setNewDestination()
    {
        float distance = 10;

        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, LayerMask.NameToLayer("Terrain"));

        navMeshAgent.SetDestination(navHit.position);
    }

    public void assignDestination(Vector3 newDestination)
    {
        float maxDistance = 100;
        wander = false;

        NavMeshHit navHit;
        NavMesh.SamplePosition(newDestination, out navHit, maxDistance, LayerMask.NameToLayer("Terrain"));

        navMeshAgent.SetDestination(navHit.position);
    }
}
