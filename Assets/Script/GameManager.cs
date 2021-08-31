using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    [SerializeField] StageManager stageManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] Player player;
    [SerializeField] MyCamera myCamera;
    [SerializeField] EffectManager effectManager;
    
    public int stageCount;
    int maxStageCount;
    int preStageCount;
    public bool gameStop;
    public StageManager StageManager
    {
        get { return stageManager; }
    }
    public UIManager UIManager
    {
        get { return uiManager; }
    }
    public Player Player
    {
        get { return player; }
    }
    public MyCamera MyCamera
    {
        get { return myCamera; }
    }
    public EffectManager EffectManager
    {
        get { return effectManager; }
    }
    private void Awake()
    {
        if (null != Instance)
        {
            Destroy(gameObject);
            return;
        }

        gameStop = false;
        stageCount = 1;
        preStageCount = 1;
        maxStageCount = 1;
        instance = this;
        SetResolution();
    }
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        CheckCount();
        EndGame();
    }
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
                return null;
            else
                return instance;
        }
    }
    public void EndGame()
    {
        if (player.Hp <= 0)
        {
            UIManager.EndGame();
            UIManager.ResultInfo(maxStageCount,stageCount);
            UIManager.AllButtonDisable(true);
        }
    }
    public void GameReset()
    {
        StageManager.Reset();
        UIManager.Reset();
        Player.Reset();
        stageCount = 1;
    }
    void CheckCount()
    {
        if (preStageCount < stageCount)
        {
            preStageCount = stageCount;
            if (stageCount >= maxStageCount)
                maxStageCount = stageCount;
            UIManager.StageInfo(stageCount);
        }
    }

    public void SetResolution()
    {
        int setWidth = 1440; 
        int setHeight = 2560;

        int deviceWidth = Screen.width;  
        int deviceHeight = Screen.height;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); 
        
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); 
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); 
        }
        else 
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); 
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); 
        }
    }
}


