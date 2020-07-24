using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * This class will navigate the npc towards the player.
 */
public class NpcNavigator : MonoBehaviour
{
    private readonly static float CHASE_RANGE = 13;
    private readonly static float SIGHT_RANGE = CHASE_RANGE * 1.5f;
    private readonly static float ATTACK_RANGE = 2;

    private GameObject hero;
    private GameObject target;
    private NavMeshAgent navAgent;
    private Animator animator;
    private bool hasSeenPlayer;
    public bool isAlive = true;

    public List<AudioClip> audioClips;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        hero = GameObject.FindGameObjectWithTag("Hero");
        target = hero;
        hasSeenPlayer = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject fakeNorris = GameObject.Find("Fake Norris(Clone)");
        target = fakeNorris == null ? hero : fakeNorris;

        if (canMove())
        {
            float targetDistance = Vector3.Distance(target.transform.position, transform.position);
            if (targetDistance <= SIGHT_RANGE)
            {
                var lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 100);
            }

            if (targetDistance > CHASE_RANGE)
            {
                hasSeenPlayer = false;
                navAgent.ResetPath();
            }
            else if (targetDistance > ATTACK_RANGE)
            {
                if (hasSeenPlayer) navAgent.SetDestination(target.transform.position);
                else
                {
                    Vector3 direction = transform.TransformDirection(Vector3.forward);
                    RaycastHit hit;
                    // if the enemy spots the target
                    if (Physics.Raycast(transform.position, direction, out hit, CHASE_RANGE))
                    {
                        if (hit.collider.gameObject.Equals(target))
                        {
                            navAgent.SetDestination(hit.point);
                            hasSeenPlayer = true;

                            if (target == hero)
                            {
                                AudioSource audio = GetComponent<AudioSource>();
                                audio.clip = audioClips.Find(x => x.name.Equals("seen_player"));
                                audio.Play();
                            }
                        }
                    }
                }
            }
            else // distance <= ATTACK_RANGE - Can attack player
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    navAgent.ResetPath();
                    GetComponent<NpcCombatController>().attackHero();
                }
            }

            animator.SetBool("isWalking", navAgent.hasPath);
            animator.SetBool("playerIsClose", targetDistance <= SIGHT_RANGE);

            // Don't start moving while attacking or receiving damage.
            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage")) && navAgent.hasPath)
                navAgent.ResetPath();
        }

        //TODO: Remove
        if (Input.GetKeyDown(KeyCode.P) && Vector3.Distance(hero.transform.position, transform.position) < SIGHT_RANGE)
        {
            GetComponent<NpcCombatController>().takeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.O) && target == fakeNorris)
            Destroy(fakeNorris);
    }

    private bool canMove()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage")
            || (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")
            || !isAlive))
            return false;

        return true;
    }
}
