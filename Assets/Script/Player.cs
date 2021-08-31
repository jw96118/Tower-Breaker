using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PLAYER_WEAPON_TYPE
{
    STAB = 0,
    CUT,
    SCRATCH,
    END
}
public class Player : MonoBehaviour
{
    int hp;
    float speed;
    float eventSpeed;
    float skillCoolTime;
    float tempTimer;
    bool dashFlag;
    bool guardFlag;
    bool guardActiveFlag;
    bool hitFlag;
    public bool collisionCheck;
    bool nextStageEvent;
    bool nextStageEventFirst;
    PLAYER_WEAPON_TYPE weaponType;
    const float playerAttackScaleY = 1f;

    Vector3 guardStartPos;
    Vector3 eventStartPos;

    public Vector3 startPos;
    public Transform nextPos;
    [SerializeField] Transform DashLimitPos;
    [SerializeField] Transform attackPos;
    [SerializeField] Transform Weapon;
    [SerializeField] PlayerSkill playerSkill;
    List<Vector2> attackScales;
    List<int> attackDamage;
    public int Hp
    {
        get { return hp; }
    }
    public float SkillCoolTime
    {
        get { return skillCoolTime; }
    }
    public PLAYER_WEAPON_TYPE Weapon_Type
    {
        get { return weaponType; }
    }
    private void Awake()
    {
        hp = 3;
        speed = 15f;
        eventSpeed = 3f;
        tempTimer = 0f;
        skillCoolTime = 5f;
        dashFlag = false;
        collisionCheck = false;
        nextStageEvent = false;
        nextStageEventFirst = false;
        hitFlag = true;
        weaponType = PLAYER_WEAPON_TYPE.STAB;
        attackScales = new List<Vector2>();
        attackDamage = new List<int>();
        //Spear
        attackScales.Add(new Vector2(1.8f, playerAttackScaleY));
        attackDamage.Add(5);
        //Sword
        attackScales.Add(new Vector2(1.5f, playerAttackScaleY));
        attackDamage.Add(7);
        //Claw
        attackScales.Add(new Vector2(1.2f, playerAttackScaleY));
        attackDamage.Add(9);
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    private void FixedUpdate()
    {

    }
    // Update is called once per frame
    void LateUpdate()
    {

        if (nextStageEvent)
        {
            if (nextStageEventFirst)
            {
                transform.position = Vector3.Lerp(eventStartPos, nextPos.position, tempTimer / 0.5f);
                tempTimer += Time.deltaTime;
                if (tempTimer > 0.5f)
                {
                    tempTimer = 0f;
                    GameManager.Instance.StageManager.NextStage();
                    Vector3 tempPos = GameManager.Instance.StageManager.GetNowStageInfo().PlayerStartPos.position;
                    tempPos.x -= 3f;
                    transform.position = tempPos;
                    nextStageEventFirst = false;
                }
            }
            else
            {
                transform.position = new Vector3(transform.position.x, GameManager.Instance.StageManager.GetNowStageInfo().PlayerStartPos.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, GameManager.Instance.StageManager.GetNowStageInfo().PlayerStartPos.position, Time.deltaTime * eventSpeed);
                if (!GameManager.Instance.StageManager.DownFlag)
                {
                    transform.position = GameManager.Instance.StageManager.GetNowStageInfo().PlayerStartPos.position;
                    nextStageEvent = false;
                }
            }
        }
        else
        {
            if (transform.position.x <= -2.4f)
            {
                if (hitFlag && !GameManager.Instance.StageManager.DownFlag)
                {
                    Hit();
                }
            }
            if (guardFlag)
            {
                transform.position = Vector3.Lerp(guardStartPos, startPos, tempTimer / 0.5f);
                tempTimer += Time.deltaTime;
                if (tempTimer > 0.5f)
                {
                    guardFlag = false;
                    tempTimer = 0f;
                    GameManager.Instance.UIManager.PlayerGaurdState(true);
                    transform.position = startPos;
                    hitFlag = true;
                }
            }
            else if (dashFlag)
            {
                Vector3 tempPos = transform.position;
                tempPos.x += Time.deltaTime * speed;
                if (DashLimitPos.position.x <= tempPos.x)
                {
                    dashFlag = false;
                    tempPos.x = DashLimitPos.position.x;
                }
                transform.position = tempPos;
            }
            else if (collisionCheck)
            {
                if (!GameManager.Instance.gameStop)
                {
                    Vector3 tempPos = transform.position;
                    tempPos.x -= Time.deltaTime * 1.5f;
                    transform.position = tempPos;
                }
            }
        }
    }
    public void Dash()
    {
        if (!collisionCheck)
            dashFlag = true;
    }
    public void Attack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackScales[(int)weaponType].x, attackScales[(int)weaponType].y), 0);
        foreach (Collider2D collider2D in collider2Ds)
        {
            if (collider2D.tag == "Monster")
            {
                collider2D.GetComponent<Monster>().Hit(attackDamage[(int)weaponType]);
            }
        }
        GameManager.Instance.EffectManager.UseAttackEffect((int)weaponType, attackPos.position);
    }
    public void Guard()
    {
        if (guardActiveFlag)
        {
            guardFlag = true;
            guardStartPos = transform.position;
            GameManager.Instance.UIManager.PlayerGaurdState(false);
            GameManager.Instance.StageManager.PushMonsters();
            GameManager.Instance.EffectManager.UseAttackEffect((int)EFFECT_TYPE.GAURD, attackPos.position);
        }
    }
    public void NextStageEvent()
    {
        nextStageEvent = true;
        nextStageEventFirst = true;
        eventStartPos = transform.position;
    }
    void Hit()
    {
        hp--;
        transform.position = startPos;
        GameManager.Instance.UIManager.ChangeHpUI(hp,false);
        GameManager.Instance.UIManager.PlayerGaurdState(false);

        GameManager.Instance.StageManager.ActiveStage(false);
        GameManager.Instance.gameStop = true;

        hitFlag = false;
        StartCoroutine(GameManager.Instance.MyCamera.CameraShake());
    }
    public void ChangeWeapon()
    {
        Weapon.transform.GetChild((int)weaponType).gameObject.SetActive(false);
        weaponType++;
        if (weaponType == PLAYER_WEAPON_TYPE.END)
            weaponType = PLAYER_WEAPON_TYPE.STAB;
        Weapon.transform.GetChild((int)weaponType).gameObject.SetActive(true);
    }
    public void UseSkill()
    {
        if (!playerSkill.gameObject.activeSelf)
        {
            playerSkill.gameObject.SetActive(true);
            playerSkill.SetSkill(attackPos.position, 5f, 0.7f);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            collisionCheck = true;
            dashFlag = false;
            guardActiveFlag = true;
        }
        if (collision.gameObject.tag == "PlayerHitBox")
        {
            if (hitFlag && !GameManager.Instance.StageManager.DownFlag)
            {
                //Hit();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            collisionCheck = false;
            guardActiveFlag = false;
        }
    }
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireCube(attackPos.position, new Vector2(attackScales[(int)weaponType].x, attackScales[(int)weaponType].y));
    }
    public void Reset()
    {

        hp = 3;
        speed = 15f;
        eventSpeed = 3f;
        tempTimer = 0f;
        dashFlag = false;
        collisionCheck = false;
        nextStageEvent = false;
        nextStageEventFirst = false;
        hitFlag = true;
        weaponType = PLAYER_WEAPON_TYPE.STAB;

        startPos = GameManager.Instance.StageManager.GetStageInfo(1).PlayerStartPos.position;
        transform.position = startPos;
    }
}
