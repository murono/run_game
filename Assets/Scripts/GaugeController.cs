using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GaugeController : MonoBehaviour
{
    [SerializeField]
    private GameObject gaugeInsideUI;

    [SerializeField]
    private float gaugeMax = 1000.0f;
    [SerializeField]
    private float gaugeRemain = 1000.0f;
    [SerializeField]
    private GameManager gameManager;

    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        DecreaseHp();
    }

    private void DecreaseHp()
    {
        if (gaugeRemain <= 0f)
            gameManager.ShowGameOver(PlayerController.MotionType.OutOfBattery);
        if (gaugeRemain > 0f)
            gaugeRemain -= 0.01f;

        ChangeGauge();
    }

    private void ChangeGauge()
    {
        float remaining = gaugeRemain / gaugeMax;
        gaugeInsideUI.GetComponent<Image>().fillAmount = remaining;
    }

    public void IncreaseHp(float amount)
    {
        if (gaugeRemain < gaugeMax)
            gaugeRemain += amount;
        ChangeGauge();
    }

    public float GetRemainingAmountPercent()
    {
        return gaugeRemain / gaugeMax;
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}