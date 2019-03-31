using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 100f;
    public float hitPoints;
    private UnityEngine.UI.Image healthBar;
    Animator anim;

    Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    bool damaged;
    // Start is called before the first frame update
    void Start()
    {
        hitPoints = maxHP;
        // healthBar = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<Image>();
        healthBar = transform.Find("Canvas").Find("HealthBG").Find("CurrentHealth").GetComponent<Image>();
        anim = GetComponent<Animator>();
        damageImage = transform.Find("Canvas").Find("DamageImage").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged)
        {
            damageImage.color = flashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void TakeDamage(float damage)
    {
        damaged = true;
        hitPoints -= damage;
        healthBar.fillAmount = hitPoints / maxHP;
    }

    public void TakeHealing(float healAmount)
    {
        hitPoints += healAmount;
        hitPoints = Mathf.Min(maxHP, hitPoints);
        healthBar.fillAmount = hitPoints / maxHP;
    }
}
