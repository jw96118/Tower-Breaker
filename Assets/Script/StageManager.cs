using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo
{
    public int downYIndex;
    public bool downFlag;
    Transform playerStartPos;
    Transform monsterStartPos;
    public List<Monster> monsterList;
    //Reset Info
    Vector3 resetPos;
    int resetIndex;

    public Transform PlayerStartPos
    {
        get { return playerStartPos; }
    }
    public Transform MonsterStartPos
    {
        get { return monsterStartPos; }
    }
    public Vector3 ResetPos
    {
        get { return resetPos; }
    }
    public StageInfo(int index, Vector3 pos)
    {
        resetIndex = downYIndex = index;
        resetPos = pos;
        downFlag = false;
    }
    public StageInfo(int index, Transform playerPos, Transform monsterPos, Vector3 pos)
    {
        resetIndex = downYIndex = index;
        downFlag = false;
        playerStartPos = playerPos;
        monsterStartPos = monsterPos;
        monsterList = new List<Monster>();
        resetPos = pos;
    }
    ~StageInfo() { }
    public void Reset()
    {
        downFlag = false;
        downYIndex = resetIndex;
        if (monsterList != null)
        {
            foreach (Monster monster in monsterList)
                ObjPool.Instance.PushObject(monster.gameObject, OBJ_TYPE.MONSTER);
        }
    }
}
public class StageManager : MonoBehaviour
{
    [SerializeField] List<GameObject> stageList;
    List<StageInfo> stageInfoList;
    List<float> downYList;
    float speed;
    Vector3 maxPos;
    int maxCount;
    bool downFlag;
    int startIndex;
    int nowIndex;
    bool nextStageUIFlag;
    public bool DownFlag
    {
        get { return downFlag; }
    }
    private void Awake()
    {
        speed = 2f;
        downFlag = false;
        nextStageUIFlag = false;
        maxCount = 1;
        startIndex = 0;
        float startY = -8.018f;
        float plusY = 3.458f;
        stageInfoList = new List<StageInfo>();
        downYList = new List<float>();
        nowIndex = 1;
        for (int i = 0; i < stageList.Count; i++)
        {

            downYList.Add(startY);
            startY += plusY;
            stageList[i].transform.position = new Vector3(stageList[i].transform.position.x, startY, stageList[i].transform.position.z);
            if (i == stageList.Count - 2)
                maxPos = stageList[i].transform.position;

            if (i > 0)
            {
                stageInfoList.Add(new StageInfo(i, stageList[i].transform.GetChild(2).transform, stageList[i].transform.GetChild(3).transform, stageList[i].transform.position));
                SpawnMonster(i);
            }
            else
                stageInfoList.Add(new StageInfo(i, stageList[i].transform.position));

        }
    }
    // Start is called before the first frame update
    void Start()
    {

        GameManager.Instance.Player.startPos = stageInfoList[nowIndex].PlayerStartPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (MonsterListEmpty(nowIndex))
        {
            if (!nextStageUIFlag)
            {
                GameManager.Instance.UIManager.ActiveNextStage(true);
                GameManager.Instance.UIManager.AllButtonDisable(true);
                nextStageUIFlag = true;
                GameManager.Instance.gameStop = false;
            }
        }
        StageDown();
    }
    public void NextStage()
    {
        if (downFlag)
            return;
        for (int i = startIndex; i < stageList.Count; i++)
        {
            stageInfoList[i].downFlag = true;
        }
        downFlag = true;

        nowIndex++;
        if (nowIndex >= stageList.Count)
            nowIndex = 1;
        GameManager.Instance.stageCount++;

    }
    void StageDown()
    {
        if (downFlag)
        {
            for (int i = startIndex; i < stageList.Count; i++)
            {
                Vector3 tempPos = stageList[i].transform.position;
                tempPos.y -= Time.deltaTime * speed;
                if (downYList[stageInfoList[i].downYIndex] > tempPos.y)
                {
                    tempPos.y = downYList[stageInfoList[i].downYIndex];
                    if (stageInfoList[i].downFlag)
                    {
                        stageInfoList[i].downYIndex--;
                        if (stageInfoList[i].downYIndex < 0)
                            stageInfoList[i].downYIndex = stageList.Count - (1 + startIndex);
                    }
                    stageInfoList[i].downFlag = false;
                }
                StageMonsterDown(i);
                stageList[i].transform.position = tempPos;
            }
            if (!EndCheck())
            {
                downFlag = false;
                if (startIndex > 0)
                {
                    stageList[maxCount].transform.position = maxPos;
                    SpawnMonster(maxCount);
                    maxCount++;
                    if (maxCount >= stageList.Count)
                        maxCount = 1;
                }

                startIndex = 1;
                nextStageUIFlag = false;

                GameManager.Instance.UIManager.AllButtonDisable(false);
                GameManager.Instance.Player.startPos = stageInfoList[nowIndex].PlayerStartPos.position;
            }
        }
    }
    bool EndCheck()
    {
        for (int i = startIndex; i < stageList.Count; i++)
        {
            if (stageInfoList[i].downFlag)
                return true;
        }
        return false;
    }
    void SpawnMonster(int index)
    {
        const float plusX = 0.5f;
        stageInfoList[index].monsterList.Clear();
        for (int i = 0; i < 10; i++)
        {
            Monster monster = ObjPool.Instance.PullObject(OBJ_TYPE.MONSTER).GetComponent<Monster>();
            Vector3 spawnPos = stageInfoList[index].MonsterStartPos.position;
            spawnPos.x += plusX * i;
            monster.transform.position = spawnPos;
            stageInfoList[index].monsterList.Add(monster);
        }
    }
    bool MonsterListEmpty(int index)
    {
        if (stageInfoList[index].monsterList == null || stageInfoList[index].monsterList.Count == 0)
            return true;

        List<Monster> tmepList = new List<Monster>();

        foreach (Monster monster in stageInfoList[index].monsterList)
        {
            if (!monster.gameObject.activeSelf)
            {
                tmepList.Add(monster);
            }
        }
        stageInfoList[index].monsterList.RemoveAll(tmepList.Contains);

        if (stageInfoList[index].monsterList.Count == 0)
            return true;
        else
            return false;
    }
    void StageMonsterDown(int index)
    {
        if (!MonsterListEmpty(index))
        {
            foreach (Monster monster in stageInfoList[index].monsterList)
            {
                Vector3 tempPos = monster.transform.position;
                tempPos.y -= Time.deltaTime * speed;
                monster.transform.position = tempPos;
            }
            //for (int i = 0; i < stageInfoList[index].monsterList.Count; i++)
            //{
            //    Vector3 tempPos = stageInfoList[index].monsterList[i].transform.position;
            //    tempPos.y -= Time.deltaTime * speed;
            //    stageInfoList[index].monsterList[i].transform.position = tempPos;
            //}
        }
    }
    public void ActiveStage(bool flag)
    {
        foreach (Monster monster in stageInfoList[nowIndex].monsterList)
        {
            monster.ActiveFlag = flag;
        }
    }
    public StageInfo GetStageInfo(int index)
    {
        if (index >= stageInfoList.Count)
            return null;
        return stageInfoList[index];
    }
    public StageInfo GetNowStageInfo()
    {
        return stageInfoList[nowIndex];
    }
    public void PushMonsters()
    {
        foreach (Monster monster in stageInfoList[nowIndex].monsterList)
        {
            monster.PushFlag = true;
            Vector3 tempPos = monster.transform.position;
            tempPos.x += monster.PushLange;
            monster.PushPos = tempPos;
        }
    }
    public Vector3 FrontMonsterPos()
    {
        return stageInfoList[nowIndex].monsterList[0].transform.position;
    }
    public void Reset()
    {
        speed = 2f;
        downFlag = false;
        nextStageUIFlag = false;
        maxCount = 1;
        startIndex = 0;
        float startY = -8.018f;
        float plusY = 3.458f;
        nowIndex = 1;
        for (int i = 0; i < stageList.Count; i++)
        {
            downYList[i] = startY;
            startY += plusY;
            stageInfoList[i].Reset();
            stageList[i].transform.position = stageInfoList[i].ResetPos;
            if (i == stageList.Count - 2)
                maxPos = stageList[i].transform.position;
            if (i > 0)
                SpawnMonster(i);
        }
        GameManager.Instance.Player.startPos = stageInfoList[nowIndex].PlayerStartPos.position;
    }
}
