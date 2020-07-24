using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Place this class at the root level of the NPC.
 * Set the healthBarUI to the root healthbar object.
 */
public class NpcHealth : MonoBehaviour
{
    private float health;
    private float maxHealth = 100;
    private Camera cam;

    public GameObject healthBarUI;
    public Slider healthSlider;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        health = maxHealth;
        healthSlider.value = CalculateHealth();
        cam = Camera.main;
    }

    private void Update()
    {
        healthSlider.value = CalculateHealth();

        if (health < maxHealth)
            healthBarUI.SetActive(true);
        else if (health >= maxHealth)
            health = maxHealth;

        healthBarUI.SetActive(health < maxHealth && health > 0);

        // Angle to camera
        healthBarUI.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.down);
    }

    private float CalculateHealth()
    {
        return health / maxHealth;
    }

    /** 
     * Removes the amount of damage from the npc's health.
     */
    public float takeDamage(float amount)
    {
        health -= amount;

        if (health < 0)
        {
            health = 0;
            //die();
        }

        return health;
    }

    public void heal(float amount)
    {
        health += amount;
    }

    private void die()
    {
        GetComponent<NpcNavigator>().isAlive = false;
        GetComponent<Animator>().SetBool("isDead", true);
    }
}
