using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum BUTTONTYPE
{
    ATTACK = 0,
    DASH,
    GAURD,
    WEAPON_CHANGE,
    PLAYER_SKILL
}
public class UIManager : MonoBehaviour
{
    //CoolTime
    float dashCoolTime;
    float gaurdCoolTime;
    //Button
    [SerializeField] GameObject attackButton;
    [SerializeField] GameObject dashButton;
    [SerializeField] GameObject gaurdButton;
    [SerializeField] GameObject nextStageUI;
    [SerializeField] GameObject hpUI;
    [SerializeField] GameObject monsterUI;
    [SerializeField] GameObject result;
    [SerializeField] GameObject PlayerWeaponButton;
    [SerializeField] GameObject PlayerWeapon;
    [SerializeField] GameObject PlayerSkillButton;
    [SerializeField] Text       stageInfo;
    //FiiledButton
    ButtonCoolTime dashFilledButton;
    ButtonCoolTime gaurdFilledButton;
    ButtonCoolTime skillFilledButton;
    int monsterCount;
    const int maxMonsterCount = 10;

    void Awake()
    {
        dashCoolTime = 1f;
        gaurdCoolTime = 2f;
        dashFilledButton = dashButton.GetComponent<ButtonCoolTime>();
        gaurdFilledButton = gaurdButton.GetComponent<ButtonCoolTime>();
        skillFilledButton = PlayerSkillButton.GetComponent<ButtonCoolTime>();
        monsterCount = maxMonsterCount;
        stageInfo = stageInfo.GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ActiveResultUI();
    }
    //Button Click Action
    public void AttackButtonClick()
    {
        GameManager.Instance.Player.Attack();
        GameManager.Instance.StageManager.ActiveStage(true);
        GameManager.Instance.gameStop = false;
    }
    public void DashButtonClick()
    {
        dashFilledButton.StartCool(dashCoolTime);
        GameManager.Instance.Player.Dash();
        GameManager.Instance.StageManager.ActiveStage(true);
        GameManager.Instance.gameStop = false;
    }
    public void GuardButtonClick()
    {
        gaurdFilledButton.StartCool(gaurdCoolTime);
        GameManager.Instance.Player.Guard();
        GameManager.Instance.StageManager.ActiveStage(true);
        GameManager.Instance.gameStop = false;
    }
    public void NextStageButtonClick()
    {
        GameManager.Instance.Player.NextStageEvent();
        StartCoroutine(ResetMonsterUI());
        ActiveNextStage(false);
    }
    public void ResetButtonClick()
    {
        GameManager.Instance.GameReset();
    }
    public void WeaponChangeButtonClick()
    {
        PlayerWeaponButton.transform.GetChild(0).GetChild(2).GetChild((int)GameManager.Instance.Player.Weapon_Type).gameObject.SetActive(false);
        PlayerWeapon.transform.GetChild((int)GameManager.Instance.Player.Weapon_Type).gameObject.SetActive(false);
        GameManager.Instance.Player.ChangeWeapon();
        PlayerWeapon.transform.GetChild((int)GameManager.Instance.Player.Weapon_Type).gameObject.SetActive(true);
        PlayerWeaponButton.transform.GetChild(0).GetChild(2).GetChild((int)GameManager.Instance.Player.Weapon_Type).gameObject.SetActive(true);

    }
    public void PlayerSkillClick()
    {
        GameManager.Instance.Player.UseSkill();
        skillFilledButton.StartCool(GameManager.Instance.Player.SkillCoolTime);
        GameManager.Instance.StageManager.ActiveStage(true);
        GameManager.Instance.gameStop = false;
    }
    ///////////////
    public void PlayerGaurdState(bool falg)
    {
        ButtonDisable(BUTTONTYPE.DASH, !falg);
        ButtonDisable(BUTTONTYPE.ATTACK, !falg);
        ButtonDisable(BUTTONTYPE.WEAPON_CHANGE, !falg);
        ButtonDisable(BUTTONTYPE.PLAYER_SKILL, !falg);
    }
    public void ButtonDisable(BUTTONTYPE type, bool flag)
    {
        switch (type)
        {
            case BUTTONTYPE.ATTACK:
                attackButton.transform.GetChild(0).gameObject.SetActive(flag);
                attackButton.transform.GetChild(1).gameObject.SetActive(!flag);
                break;
            case BUTTONTYPE.DASH:
                dashButton.transform.GetChild(0).gameObject.SetActive(flag);
                dashButton.transform.GetChild(1).gameObject.SetActive(false);
                dashButton.transform.GetChild(2).gameObject.SetActive(!flag);
                break;
            case BUTTONTYPE.GAURD:
                gaurdButton.transform.GetChild(0).gameObject.SetActive(flag);
                gaurdButton.transform.GetChild(1).gameObject.SetActive(false);
                gaurdButton.transform.GetChild(2).gameObject.SetActive(!flag);
                break;
            case BUTTONTYPE.WEAPON_CHANGE:
                PlayerWeaponButton.transform.GetChild(0).gameObject.SetActive(flag);
                PlayerWeaponButton.transform.GetChild(1).gameObject.SetActive(!flag);
                break;
            case BUTTONTYPE.PLAYER_SKILL:
                if (!skillFilledButton.CoolStartFlag)
                {
                    PlayerSkillButton.transform.GetChild(0).gameObject.SetActive(flag);
                    PlayerSkillButton.transform.GetChild(2).gameObject.SetActive(!flag);
                }
                break;
        }
    }
    public void ActiveNextStage(bool flag)
    {
        nextStageUI.transform.GetChild(0).gameObject.SetActive(flag);
        nextStageUI.transform.GetChild(1).gameObject.SetActive(flag);
    }
    public void ChangeHpUI(int hp, bool resetFlag)
    {
        if (resetFlag)
        {
            for (int i = 0; i < 3; i++)
                hpUI.transform.GetChild(i).gameObject.SetActive(true);
        }
        else
            hpUI.transform.GetChild(hp).gameObject.SetActive(false);
    }
    public void ChangeMonsterUI()
    {
        if (monsterCount > 0)
            monsterCount--;
        monsterUI.transform.GetChild(monsterCount).gameObject.SetActive(false);
    }

    IEnumerator ResetMonsterUI()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (monsterCount < maxMonsterCount)
            {
                monsterUI.transform.GetChild(monsterCount).gameObject.SetActive(true);
                monsterCount++;
            }
            else
                break;
        }
    }
    void ActiveResultUI()
    {
        if (result.activeSelf)
        {
            if (result.transform.GetChild(0).GetComponent<Image>().sprite.name == "result_banner_10")
            {
                result.transform.GetChild(1).gameObject.SetActive(true);
                result.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }
    public void ResultInfo(int max, int count)
    {
        string tempString;
        tempString = "결과\n" + "최고 기록 : " + max + "\n현재 기록 : " + count;
        result.transform.GetChild(1).gameObject.GetComponent<Text>().text = tempString;
    }


  
    public void EndGame()
    {
        AllButtonDisable(false);
        result.SetActive(true);
        monsterUI.SetActive(false);
    }
    public void StageInfo(int count)
    {
        string tempString;
        tempString = count + " / ∞";
        stageInfo.text = tempString;
    }

    public void Reset()
    {
        AllButtonDisable(false);
        monsterUI.SetActive(true);
        StartCoroutine(ResetMonsterUI());
        for (int i = 0; i < 3; i++)
            hpUI.transform.GetChild(i).gameObject.SetActive(true);

        result.transform.GetChild(1).gameObject.SetActive(false);
        result.transform.GetChild(2).gameObject.SetActive(false);
        result.SetActive(false);

        PlayerWeapon.transform.GetChild(0).gameObject.SetActive(true);
        PlayerWeapon.transform.GetChild(1).gameObject.SetActive(false);
        PlayerWeapon.transform.GetChild(2).gameObject.SetActive(false);

        skillFilledButton.Reset();
    }
    public void AllButtonDisable(bool flag)
    {
        ButtonDisable(BUTTONTYPE.ATTACK, flag);
        ButtonDisable(BUTTONTYPE.DASH, flag);
        ButtonDisable(BUTTONTYPE.GAURD, flag);
        ButtonDisable(BUTTONTYPE.WEAPON_CHANGE, flag);
        ButtonDisable(BUTTONTYPE.PLAYER_SKILL, flag);
    }
}

