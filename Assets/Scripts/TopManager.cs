using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public class TopManager : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
    }
}