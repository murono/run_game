using UnityEngine;
using UnityEngine.UI;

public class JumpButton : MonoBehaviour
{
    [SerializeField]
    private Button jumpButton;

    [SerializeField]
    private GameObject player;

    private void Awake()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
       jumpButton.onClick.AddListener(() => playerController.Jump());
    }
}