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
public class HighScores : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private Button submitButton;
    private Transform inputField;

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
            }
        }
    }

    IEnumerator PostScore()
    {
        Debug.Log("starting post");
        Score newScore = new Score();
        newScore.name = inputField.Find("Text").GetComponent<Text>().text;
        newScore.score = 5007.1f;
        string jsonScore = JsonUtility.ToJson(newScore);
        // Debug.Log(jsonScore);

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
        }
        else
        {
            // Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            scoreInfo = JsonUtility.FromJson<ScoreInfo>("{\"scores\":" + webRequest.downloadHandler.text + "}");
            ListScores();
        }

    }

    private void Awake()
    {
        // Debug.Log(Environment.UserName);
        entryContainer = transform.Find("entryContainer");
        entryTemplate = entryContainer.Find("entryTemplate");

        entryTemplate.gameObject.SetActive(false);

        // Score newScore = new Score();
        // newScore.name = "Judie Hopps";
        // newScore.score = 0.1f;
        // StartCoroutine(PostScore(newScore));
        StartCoroutine(GetScores());

        inputField = transform.Find("Footer Background").Find("InputField");
        submitButton = inputField.Find("SubmitButton").GetComponent<Button>();

        submitButton.onClick.AddListener(() => StartCoroutine(PostScore()));
    }

    private void ListScores()
    {
        entryContainer = transform.Find("entryContainer");
        entryTemplate = entryContainer.Find("entryTemplate");

        // entryTemplate.gameObject.SetActive(false);

        // ScoreInfo highScores = await GetScores();

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
}
