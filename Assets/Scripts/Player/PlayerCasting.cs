using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCasting : MonoBehaviour
{
    // class Spell
    // {
    //     string Name { get; set; } = string.Empty;
    //     float manaCost { get; set; } = 0f;
    // }
    public Transform[] spells;
    public float maxMana = 100f;

    // public List<Spell> Spells = new List<Spell>();
    float currentMana;
    public float manaRegen = 5f;
    public float castingGCD = 1f;
    bool casting = false;
    float castingTimer;
    // Start is called before the first frame update
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
            if (currentMana >= 70)
            {
                castingTimer = 0f;
                currentMana -= 70;
                playerHealth.TakeHealing(20f);
                healingParticles.Play();
                manaBar.fillAmount = currentMana / maxMana;
            }
        }

        // fireball spell
        if (Input.GetKey(KeyCode.Alpha2) && castingTimer > castingGCD)
        {
            if (currentMana >= 30)
            {
                castingTimer = 0f;
                currentMana -= 30;
                manaBar.fillAmount = currentMana / maxMana;
                Vector3 dropLocation = this.transform.position;
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
}
