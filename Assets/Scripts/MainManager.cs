using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Microsoft.SqlServer.Server;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public int HighScore;
    public string HighScorer;
    public GameObject GameOverText;
    public Text HighScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private int sceneIndex;

    void Awake()
    {
        LoadHighScoreData();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the current active scene
        Scene sceneLoaded = SceneManager.GetActiveScene();

        // Update the sceneIndex
        sceneIndex = sceneLoaded.buildIndex;
        if(sceneIndex == 1)
        {
            const float step = 0.6f;
            int perLine = Mathf.FloorToInt(4.0f / step);

            string playerName = PlayerPrefs.GetString("NameChosenByUser");
            
            int[] pointCountArray = new [] {1,1,2,2,5,5};
            for (int i = 0; i < LineCount; ++i)
            {
                for (int x = 0; x < perLine; ++x)
                {
                    Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                    var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                    brick.PointValue = pointCountArray[i];
                    brick.onDestroyed.AddListener(AddPoint);
                }
            }        
            
            ScoreText.text = $"{playerName}'s score: {m_Points}";
            
            if (HighScore != 0 && HighScore != 1)
            {
                HighScoreText.text = "High score: " + HighScore + " points, by " + HighScorer;
            }
            else if (HighScore == 1)
            {
                HighScoreText.text = "High score: " + HighScore + " point, by " + HighScorer;
            }
            else
            {
                HighScoreText.text = "No high score to show. Make this count, " + playerName + "!";
            }
        }
    }

    private void Update()
    {
        if(sceneIndex == 1)
        {
            if (!m_Started)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Started = true;
                    float randomDirection = Random.Range(-1.0f, 1.0f);
                    Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                    forceDir.Normalize();

                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                }
            }
            else if (m_GameOver)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            
            if (m_Points > HighScore)
            {
                HighScore = m_Points;
                string playerName = PlayerPrefs.GetString("NameChosenByUser");
                HighScorer = playerName;
            }
        }
    }

    void AddPoint(int point)
    {
        string playerName = PlayerPrefs.GetString("NameChosenByUser");
        m_Points += point;
        ScoreText.text = $"{playerName}'s score: {m_Points}";
    }

    public void GameOver()
    {
        SaveHighScoreData();
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (HighScore != 1)
        {
            HighScoreText.text = "High score: " + HighScore + " points, by " + HighScorer;
        }
        else if (HighScore == 1)
        {
            HighScoreText.text = "High score: " + HighScore + " point, by " + HighScorer;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int HighScore;
        public string HighScorer;
    }

    public void SaveHighScoreData()
    {
        SaveData data = new SaveData();
        data.HighScore = HighScore;
        data.HighScorer = HighScorer;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScoreData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScorer = data.HighScorer;
            HighScore = data.HighScore;
        }        
    }

    public void DeleteHighScoreData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}