using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCasting : MonoBehaviour
{

    public float healManaCost = 70f;
    public float healAmount = 50f;
    public float fireManaCost = 70f;
    public Transform[] spells;
    public float maxMana = 100f;

    float currentMana;
    public float manaRegen = 5f;
    public float castingGCD = 1f;
    bool casting = false;
    float castingTimer;

    private UnityEngine.UI.Image manaBar;
    ParticleSystem healingParticles;
    PlayerHealth playerHealth;
    void Start()
    {
        currentMana = maxMana;
        manaBar = transform.Find("Canvas").Find("ManaBG").Find("CurrentMana").GetComponent<Image>();
        healingParticles = transform.Find("healing_particles").GetComponent<ParticleSystem>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        castingTimer += Time.deltaTime;
        handleInput();

    }
    void handleInput()
    {
        // healing spell
        if (Input.GetKey(KeyCode.Alpha1) && castingTimer > castingGCD)
        {
            // casting = true;
            if (currentMana >= healManaCost)
            {
                castingTimer = 0f;
                currentMana -= healManaCost;
                playerHealth.TakeHealing(healAmount);
                healingParticles.Play();
                manaBar.fillAmount = currentMana / maxMana;
            }
        }

        // fireball spell
        if (Input.GetKey(KeyCode.Alpha2) && castingTimer > castingGCD)
        {
            if (currentMana >= fireManaCost)
            {
                castingTimer = 0f;
                currentMana -= fireManaCost;
                manaBar.fillAmount = currentMana / maxMana;
                Vector3 dropLocation = transform.position;
                dropLocation.y += 1f;
                Instantiate(spells[0], dropLocation, spells[0].transform.rotation);
            }

        }

        if (currentMana < maxMana)
        {
            currentMana += manaRegen * Time.deltaTime;
            manaBar.fillAmount = currentMana / maxMana;
        }
    }

    public void AcceptMana(float amount)
    {
        currentMana = Mathf.Min(maxMana, currentMana + amount);
        manaBar.fillAmount = currentMana / maxMana;
    }
}
