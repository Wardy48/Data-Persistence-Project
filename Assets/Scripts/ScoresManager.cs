using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresManager : MonoBehaviour
{
    [SerializeField] Text highScoreText;    

    // Start is called before the first frame update
    void Start()
    {
        string playerName = PlayerPrefs.GetString("NameChosenByUser");
        highScoreText.text = playerName + "'s high score: ";        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
