using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Bip001 Prop1"))
        {
            GameObject npc = other.transform.root.gameObject;
            if (npc.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack") && !npc.GetComponent<NpcCombatController>().hasHitHero)
            {
                npc.GetComponent<NpcCombatController>().hasHitHero = true;
                Debug.Log("Hit By NPC");
            }
            else if (npc.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                Debug.Log("Hit but already hit once in this attack animation.");
            }
            else
                Debug.Log("Hit but not attacking - Do Nothing.");
        }
    }
}
