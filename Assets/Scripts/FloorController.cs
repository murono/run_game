using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{ 
    //自動生成したいオブジェクトの端から端までの座標の大きさ
    const int StageTipSize = 14;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private GameObject[] stagePrefabs;
    //ステージ生成の先読み個数
    [SerializeField]
    private int preInstantiate;
    //作ったステージチップの保持リスト
    [SerializeField]
    private List<GameObject> generatedStageList = new List<GameObject>();

    [SerializeField]
    private GameObject obstaclePrefab;
    [SerializeField]
    private GameObject[] itemPrefabs;

    [SerializeField]
    private GaugeController gaugeController;

     private int currentTipIndex;
    private float playerStartPos;
    private bool isGameOver = false;

    void Awake()
    {
        // playerの座標が0からスタートしない可能性があるため初期位置を取っておく
        playerStartPos = player.position.x;
        currentTipIndex = generatedStageList.Count - 1;
    }

    void Start()
    {
        UpdateStage(preInstantiate);        
    }

    void Update()
    {
        if (isGameOver) return;

        //キャラクターの位置から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(((player.position.x - playerStartPos) + 9) / StageTipSize);
        //次のステージチップに入ったらステージの更新処理を行う
        if (charaPositionIndex + preInstantiate > currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    //指定のインデックスまでのステージチップを生成して、管理下におく
    private void UpdateStage(int toTipIndex)
    {
        if (toTipIndex <= currentTipIndex) return;
        //指定のステージチップまで生成
        for (int i = currentTipIndex + 1; i <= toTipIndex; i++)
        {
            int nextStageTip = Random.Range(0, stagePrefabs.Length);
            GameObject stageObject = GenerateStage(i, nextStageTip);
            generatedStageList.Add(stageObject);
            GenerateObstacle(stageObject, nextStageTip);
            GenerateItem(stageObject, nextStageTip);
        }
        while (generatedStageList.Count > preInstantiate + 3) DestroyOldestStage();
        currentTipIndex = toTipIndex;

    }

    //指定のインデックス位置にstageオブジェクトをランダムに生成
    private GameObject GenerateStage(int tipIndex, int nextStageTip)
    {
        GameObject stageObject = (GameObject)Instantiate(
            stagePrefabs[nextStageTip],
            new Vector3(tipIndex * StageTipSize, 0, 0), //今回はx軸方向に無限生成するのでこの書き方をしている
            Quaternion.identity,
            this.gameObject.transform) as GameObject;
        return stageObject;
    }

    //一番古いステージを削除
    private void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }


    // 障害物置ける座標
    private List<Vector2> obstacleVector2List = new List<Vector2>{
        // 1個目
        new Vector2(-11f, -2.8f),
        new Vector2(-7.7f, -2.8f),
        new Vector2(-2.7f, -2.8f),
        // 2個目
        new Vector2(-9.5f, -1.8f),
        new Vector2(-4.8f, -1.8f),
        new Vector2(-0.2f, -0.8f),
        // 3個目
        new Vector2(-10.3f, -1.8f),
        new Vector2(-3.8f, -0.8f),
        new Vector2(0.7f, -3.7f),
        // 4個目
        new Vector2(-9.7f, -2.8f),
        new Vector2(-6.3f, -1.8f),
        new Vector2(0.5f, -1.8f),
    };

    //障害物をランダムに生成
    private void GenerateObstacle(GameObject stageObject, int nextStageTip)
    {
        Transform parentTransform = stageObject.transform.Find("Obstacle").gameObject.transform;
        GameObject obj = Instantiate(
            obstaclePrefab,
            obstacleVector2List[Random.Range(0, 3) + (nextStageTip * 3)],
            Quaternion.identity);
        obj.transform.SetParent(parentTransform, false);
    }

    // アイテム置ける座標
    private List<Vector2> itemVector2List = new List<Vector2>{
        // 1個目
        new Vector2(-9f, -1.4f),
        new Vector2(-4f, -2f),
        new Vector2(-0f, -0.3f),
        // 2個目
        new Vector2(-2f, -0.3f),
        new Vector2(-8f, -0.9f),
        new Vector2(-5.5f, -0.4f),
        // 3個目
        new Vector2(-2.5f, 0.7f),
        new Vector2(-9f, -0.5f),
        new Vector2(-0.9f, -3f),
        // 4個目
        new Vector2(-3.9f, 1f),
        new Vector2(-0.5f, -0.9f),
        new Vector2(-11f, -2f),
    };

    //アイテムをランダムに生成
    private void GenerateItem(GameObject stageObject, int nextStageTip)
    {

        // ゲージが減ってきているとボーナスあり
        int bonus = 1;
        if (gaugeController.GetRemainingAmountPercent() < 0.4f)
            bonus = 4;

        // 確率でアイテムを生成しない[つくる, つくらない]
        if (MyUtil.GetRandomIndex(2 * bonus, 1, 0) == 1) return;

        // 確率でスペシャルアイテムにする[普通, スペシャル]
        int index = MyUtil.GetRandomIndex(10, 1 * bonus, 0);
        GameObject itemPrefab = itemPrefabs[index];
        
        Transform parentTransform = stageObject.transform.Find("Item").gameObject.transform;
        GameObject obj = Instantiate(
            itemPrefab,
            itemVector2List[Random.Range(0, 3) + (nextStageTip * 3)],
            Quaternion.identity);
        obj.transform.SetParent(parentTransform, false);
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}