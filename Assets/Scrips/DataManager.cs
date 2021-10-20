using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [SerializeField] Canvas canvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        canvas = FindObjectOfType<Canvas>();
    }
    public Text coinText, levelText, timeText, scoreText;
    int totalScoreText = 0, totalCoin = 0;
    [HideInInspector] public int turnNumber = 3;
    public void UpdateScore(int score)
    {
        totalScoreText += score;
        if (totalScoreText < 1000)
        {
            scoreText.text = "MARIO\n000" + totalScoreText;
        }
        else if (totalScoreText < 10000)
        {
            scoreText.text = "MARIO\n00" + totalScoreText;
        }
        else if (totalScoreText < 100000)
        {
            scoreText.text = "MARIO\n0" + totalScoreText;
        }
        else if (totalScoreText < 1000000)
        {
            scoreText.text = "MARIO\n" + totalScoreText;
        }
    }
    public void UpdateCoin()
    {
        totalCoin += 1;
        coinText.text = " x " + totalCoin;
    }
    public void CDTime(int timeRemaining)
    {
        timeText.text = "TIME\n" + timeRemaining;
    }
    public void ShowScore(int score, float x, float y)
    {
        Text showScore = (Text)Instantiate(Resources.Load("Prefabs/ShowScore", typeof(Text)));
        showScore.transform.SetParent(canvas.transform);
        showScore.transform.localScale = new Vector3(1, 1);
        showScore.text = "+" + score;
        showScore.rectTransform.position = new Vector2(x, y);
        StartCoroutine(CoroutineShowScore(showScore));
    }

    IEnumerator CoroutineShowScore(Text showScore)
    {
        float a = 1;
        while (true)
        {
            showScore.rectTransform.position = new Vector2(showScore.rectTransform.position.x, showScore.rectTransform.position.y + 0.5f * Time.deltaTime);
            showScore.color = new Color(1, 1, 1, a);
            a -= 0.01f;
            if (a <= 0)
            {
                Destroy(showScore.gameObject);
                break;
            }
            yield return null;
        }
    }
}
