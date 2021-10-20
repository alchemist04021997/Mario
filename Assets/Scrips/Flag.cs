using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    MainCharacter mainCharacter;
    Canvas canvas;
    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }
    public void ShowScore(int scoreFlag)
    {
        Text showScore = (Text)Instantiate(Resources.Load("Prefabs/ShowScore", typeof(Text)));
        showScore.transform.SetParent(canvas.transform);
        showScore.transform.localScale = new Vector3(1, 1);
        showScore.text = "+" + scoreFlag;
        showScore.rectTransform.position = new Vector2(transform.position.x + 0.6f, transform.position.y + 0.8f);
        StartCoroutine(CoroutineShowScore(showScore));
    }

    IEnumerator CoroutineShowScore(Text showScore)
    {
        while (true)
        {
            showScore.rectTransform.position = new Vector2(showScore.rectTransform.position.x, showScore.rectTransform.position.y + 2 * Time.deltaTime);
            if (showScore.rectTransform.position.y >= transform.position.y + 8)
            {
                Destroy(showScore.gameObject);
                break;
            }
            yield return null;
        }
    }
}
