using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.PlasticSCM.Editor.WebApi;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class StartMenuUIManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInputField; // Reference to the InputField component
    [SerializeField] TMP_Text highScoreInfo;

    // This method is called when the player finishes editing the text field
    public void OnValueChanged()
    {
        // Save the player's name in PlayerPrefs
        PlayerPrefs.SetString("NameChosenByUser", nameInputField.text);
        // Save changes to disk
        PlayerPrefs.Save();
    }

    // Start is called before the first frame update
    void Start()
    {
        MainManager mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        int highScore = mainManager.HighScore;
        string highScorer = mainManager.HighScorer;
        if (highScore != 0 && highScore != 1)
        {
            highScoreInfo.text = "Current high scorer: " + highScorer + ", with " + highScore + " points.";
        }
        else if (highScore == 1)
        {
            highScoreInfo.text = "Current high scorer: " + highScorer + ", with " + highScore + " point.";
        }
        else
        {
            highScoreInfo.text = "No high score to show just yet!";
        }
    }

    public void ShowHighScoreWasReset()
    {
        highScoreInfo.text = "No high score to show just yet!";
    }

    public void StartNew()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            PlayerPrefs.SetString("NameChosenByUser", "Anon");
        }
        SceneManager.LoadScene(1);
    }

        public void ExitApplication()
        {
    #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
    #else
            Application.Quit(); // original code to quit Unity player
    #endif
        }
}
