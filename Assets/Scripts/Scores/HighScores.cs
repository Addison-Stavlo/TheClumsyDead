using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using System.Threading.Tasks;
// using System.Net;
using System;
using System.Text;
// using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class HighScores : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    public ScoreManager scoreManager;
    private Button submitButton;
    private InputField inputField;

    [Serializable]
    public class Score
    {
        public string name;
        public float score;
    }

    [Serializable]
    public class ScoreInfo
    {
        public List<Score> scores;
    }

    public ScoreInfo scoreInfo;

    IEnumerator GetScores()
    {
        string uri = "https://arcade-scores.herokuapp.com/arcadeScores/clumsyScore";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                scoreInfo = JsonUtility.FromJson<ScoreInfo>("{\"scores\":" + webRequest.downloadHandler.text + "}");
                ListScores();
                inputField.Select();
            }
        }
    }

    IEnumerator PostScore()
    {
        // InputField inputText = inputField as InputField;
        Score newScore = new Score();
        newScore.name = inputField.text;
        newScore.score = scoreManager.score;
        string jsonScore = JsonUtility.ToJson(newScore);

        // clear input field
        inputField.text = "";

        string uri = "https://arcade-scores.herokuapp.com/arcadeScores/clumsyScore";

        UnityWebRequest webRequest = new UnityWebRequest(uri, "POST");


        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonScore);
        webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        webRequest.SetRequestHeader("Content-Type", "application/json");

        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log("Error: " + webRequest.error + webRequest.downloadHandler.text);
            ChangeFooter();
        }
        else
        {
            scoreInfo = JsonUtility.FromJson<ScoreInfo>("{\"scores\":" + webRequest.downloadHandler.text + "}");
            ListScores();
            ChangeFooter();
        }
    }

    private void Awake()
    {
        StartCoroutine(GetScores());
        scoreManager = GameObject.FindWithTag("ScoreBoard").GetComponent<ScoreManager>();
        entryContainer = transform.Find("entryContainer");
        entryTemplate = entryContainer.Find("entryTemplate");

        entryTemplate.gameObject.SetActive(false);

        transform.Find("Footer Background").Find("playerScore").GetComponent<Text>().text = "Your Score: " + scoreManager.score;
        inputField = transform.Find("Footer Background").Find("InputField").GetComponent<InputField>();

        submitButton = transform.Find("Footer Background").Find("InputField").Find("SubmitButton").GetComponent<Button>();
        submitButton.onClick.AddListener(() => StartCoroutine(PostScore()));


    }

    private void ListScores()
    {
        entryContainer = transform.Find("entryContainer");
        entryTemplate = entryContainer.Find("entryTemplate");
        //turn off all previous entries / placeholder entry
        foreach (Transform child in entryContainer)
        {
            child.gameObject.SetActive(false);
        }

        float entryHeight = 30f;

        for (int i = 0; i < 10; i++)
        {
            Transform newEntry = Instantiate(entryTemplate, entryContainer);
            RectTransform newEntryPos = newEntry.GetComponent<RectTransform>();
            newEntryPos.anchoredPosition = new Vector2(0, -entryHeight * i);
            newEntry.gameObject.SetActive(true);

            int rank = i + 1;
            string rankStr = rankString(rank);
            // set background for even rows
            if (rank % 2 == 0)
            {
                newEntry.Find("background").gameObject.SetActive(false);
            }

            newEntry.Find("position").GetComponent<Text>().text = rankStr;
            newEntry.Find("name").GetComponent<Text>().text = scoreInfo.scores[i].name;
            newEntry.Find("score").GetComponent<Text>().text = scoreInfo.scores[i].score.ToString();
        }
    }
    private string rankString(int rank)
    {
        string rankStr;

        switch (rank)
        {
            case 1:
                rankStr = "1st";
                break;
            case 2:
                rankStr = "2nd";
                break;
            case 3:
                rankStr = "3rd";
                break;
            default:
                rankStr = rank + "th";
                break;
        }

        return rankStr;
    }

    private void ChangeFooter()
    {
        transform.Find("Footer Background").gameObject.SetActive(false);
        Transform restartFooter = transform.Find("Footer Background Restart");
        restartFooter.gameObject.SetActive(true);

        Button restartButton = restartFooter.Find("Button").GetComponent<Button>();
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        restartButton.Select();
    }

    private void OnGUI()
    {
        if (inputField.isFocused && Input.GetKey(KeyCode.Return))
        {
            StartCoroutine(PostScore());
        }
    }
}
