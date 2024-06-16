using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button topButton;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text highScoreText;

    private void Awake()
    {
        this.gameObject.SetActive(false);
        restartButton.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
        topButton.onClick.AddListener(() => SceneManager.LoadScene("TopScene"));

        // ハイスコアデータ消したい時
        //PlayerPrefs.DeleteAll();
    }

    public void ShowGameOver(float score)
    {
        // ハイスコア処理
        float highScore = PlayerPrefs.GetFloat("highScore", 0f);
        if (highScore < score) {
            PlayerPrefs.SetFloat("highScore", score);
            highScoreText.text = score.ToString("N2") + "m";
        } else {
            highScoreText.text = highScore.ToString("N2") + "m";
        }

        scoreText.text = score.ToString("N2") + "m";
        this.gameObject.SetActive(true);
    }
}