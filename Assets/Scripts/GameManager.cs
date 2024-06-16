using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GaugeController gaugeController;
    [SerializeField]
    private BackGroundController backGroundController;
    [SerializeField]
    private FloorController floorController;
    [SerializeField]
    private GameOverManager gameOverManager;

    public void ShowGameOver(PlayerController.MotionType motionType = PlayerController.MotionType.GameOver)
    {
        gaugeController.GameOver();
        backGroundController.GameOver();
        floorController.GameOver();
        float score = playerController.GameOver(motionType);

        gameOverManager.ShowGameOver(score);
    }
}