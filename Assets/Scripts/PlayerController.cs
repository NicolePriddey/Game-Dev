using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public GameObject fakeNorris;
    private NavMeshAgent navMeshAgent;
    private Camera cam;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftShift))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                navMeshAgent.SetDestination(raycastHit.point);
            }
        }

        if (canTransition())
        {
            if (Input.GetKeyDown(KeyCode.Z))
                anim.SetBool("attack", true);

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("jump", true);
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 0), ForceMode.Impulse);
            }
            else if (Input.GetKeyDown(KeyCode.X))
                anim.SetBool("hit", true);
        }

        if (Input.GetKeyDown(KeyCode.C))
            anim.SetBool("die", true);
        if (Input.GetKeyDown(KeyCode.L) && GameObject.Find("Fake Norris(Clone)") == null)
            Instantiate(fakeNorris, transform.position + Vector3.forward, transform.rotation);

        anim.SetBool("isWalking", navMeshAgent.hasPath ? true : false);
    }

    /**
     * Checks to see if the player can transition into another animation like jumping or attacking - or whether
     * they are busy.
     */
    private bool canTransition()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            return true;
        return false;
    }
}