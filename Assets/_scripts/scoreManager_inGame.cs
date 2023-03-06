using UnityEngine;

public class scoreManager_inGame : MonoBehaviour
{
    public static float sliderScore;

    void Start()
    {
        sliderScore = PlayerPrefs.GetFloat("sliderScore", 0);
    }

    void Update()
    {
        
    }

    public static void addScore(float amount)
    {
        sliderScore += amount;
        gameController.gainedScoreInLevel += amount;

        Debug.Log("score added || Score: " + sliderScore);
    }
}
