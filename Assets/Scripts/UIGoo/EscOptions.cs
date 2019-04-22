using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EscOptions : MonoBehaviour
{
    private Transform optionWindow;
    private PlayerController playerController;
    private Button returnButton;
    private Button quitButton;

    void Awake()
    {
        returnButton = transform.Find("ReturnButton").GetComponent<Button>();
        quitButton = transform.Find("QuitButton").GetComponent<Button>();

        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        returnButton.onClick.AddListener(() => CloseWindow());
        quitButton.onClick.AddListener(() => Application.Quit());
    }

    void CloseWindow()
    {
        playerController.optionsOpen = false;
        transform.gameObject.SetActive(false);
    }
}
