using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class StartMenuUIManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInputField; // Reference to the InputField component

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

    }

    public void StartNew()
    {
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
