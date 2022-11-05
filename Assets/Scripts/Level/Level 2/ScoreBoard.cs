using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    int playerScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Listener();
        scoreText.text = playerScore.ToString();
    }

    private void Listener()
    {
        Level_Manager.instance.OnPlayerGetScore += Event_OnPlayerGetScore;
    }

    private void Event_OnPlayerGetScore(object sender, System.EventArgs e)
    {
        playerScore++;
        scoreText.text = playerScore.ToString();
    }
}
