using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{ 
    const int StageTipSize = 19;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private GameObject[] bgPrefabs;
    [SerializeField]
    private int preInstantiate;
    [SerializeField]
    private List<GameObject> generatedBgList = new List<GameObject>();

    private int currentIndex;
    private float playerStartPos;
    private bool isGameOver = false;

    void Awake()
    {
        // playerの座標が0からスタートしない可能性があるため初期位置を取っておく
        playerStartPos = player.position.x;
        currentIndex = generatedBgList.Count - 1;
    }

    void Start()
    {
        UpdateBg(preInstantiate);        
    }

    void Update()
    {
        if (isGameOver) return;

        int playerPositionIndex = (int)((player.position.x - playerStartPos) / StageTipSize);
        if (playerPositionIndex + preInstantiate > currentIndex)
            UpdateBg(playerPositionIndex + preInstantiate);
    }

    private void UpdateBg(int toIndex)
    {
        if (toIndex <= currentIndex) return;
        for (int i = currentIndex + 1; i <= toIndex; i++)
        {
            GameObject stageObject = GenerateBg(i);
            generatedBgList.Add(stageObject);
        }
        while (generatedBgList.Count > preInstantiate + 2) DestroyOldestBg();
        currentIndex = toIndex;
    }

    private GameObject GenerateBg(int tipIndex)
    {
        GameObject stageObject = (GameObject)Instantiate(
            bgPrefabs[Random.Range(0, bgPrefabs.Length)],
            new Vector3(tipIndex * StageTipSize, 0, 0),
            Quaternion.identity,
            this.gameObject.transform) as GameObject;
        return stageObject;
    }

    private void DestroyOldestBg()
    {
        GameObject oldStage = generatedBgList[0];
        generatedBgList.RemoveAt(0);
        Destroy(oldStage);
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}