using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public bool isDead = false;
    AudioSource playerAudio;
    public AudioClip deathClip;

    PlayerController playerController;
    PlayerCasting playerCasting;
    PlayerMelee playerMelee;
    Transform camera;

    Transform highScoresTable;
    Text gameOver;

    float restartDelay = 5f;
    float restartTimer;
    // Start is called before the first frame update
    void Start()
    {
        hitPoints = maxHP;
        playerAudio = GetComponent<AudioSource>();
        // healthBar = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<Image>();
        healthBar = transform.Find("Canvas").Find("HealthBG").Find("CurrentHealth").GetComponent<Image>();
        anim = GetComponent<Animator>();
        damageImage = transform.Find("Canvas").Find("DamageImage").GetComponent<Image>();
        playerController = GetComponent<PlayerController>();
        playerMelee = GetComponent<PlayerMelee>();
        playerCasting = GetComponent<PlayerCasting>();
        camera = GameObject.FindGameObjectWithTag("CameraBoom").transform;
        gameOver = GameObject.FindGameObjectWithTag("GameOver").GetComponent<Text>();
        highScoresTable = transform.Find("Canvas").Find("HighScoresTable");
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
        if (!isDead)
        {
            damaged = false;
        }
        else
        {
            //dead so restart game/scene
            // restartTimer += Time.deltaTime;
            // if (restartTimer >= restartDelay)
            // {
            //     SceneManager.LoadScene(0);
            // }
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            damaged = true;
            playerAudio.Play();
            hitPoints -= damage;
            healthBar.fillAmount = hitPoints / maxHP;
            if (hitPoints <= 0f)
            {
                Death();
            }
        }

    }

    public void TakeHealing(float healAmount)
    {
        hitPoints += healAmount;
        hitPoints = Mathf.Min(maxHP, hitPoints);
        healthBar.fillAmount = hitPoints / maxHP;
    }

    void Death()
    {
        isDead = true;
        anim.Play("DeathAnim");
        gameOver.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        highScoresTable.gameObject.SetActive(true);
        transform.eulerAngles = new Vector3(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z);
        camera.eulerAngles = new Vector3(-90, transform.rotation.y, 0);
        playerAudio.clip = deathClip;
        playerAudio.Play();
        playerMelee.enabled = false;
        playerCasting.enabled = false;
        playerController.enabled = false;
    }
}
