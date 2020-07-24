using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCombatController : MonoBehaviour
{
    public List<AudioClip> combatAudioClips;
    private GameObject hero;
    public bool hasHitHero = false; // used to make sure npc only hits hero once per attack.

    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
    }

    public void attackHero()
    {
        hasHitHero = false;
        GetComponent<Animator>().SetTrigger("attack");

        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = combatAudioClips.Find(x => x.name.Equals("attack"));
        audio.Play();
    }

    public void takeDamage(int amount)
    {
        float health = GetComponent<NpcHealth>().takeDamage(amount);

        AudioSource audio = GetComponent<AudioSource>();
        if (health > 0)
        {
            GetComponent<Animator>().SetTrigger("hit");
            audio.clip = combatAudioClips.Find(x => x.name.Equals("hit"));
        }
        else
        {
            GetComponent<Animator>().SetBool("isDead", true);
            GetComponent<NpcNavigator>().isAlive = false;
            audio.clip = combatAudioClips.Find(x => x.name.Equals("die"));
        }
        audio.Play();
    }
}
