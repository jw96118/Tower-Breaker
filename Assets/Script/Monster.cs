using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MONSTER_TYPE
{
    SWORDMAN=0,
    KNIGHT,
    END
}
public class Monster : MonoBehaviour
{
    protected int hp;
    protected int maxHp;
    protected float speed;
    protected float maxSpeed;
    protected float pushSpeed;
    protected float tempTimer;
    protected bool activeFlag;
    protected bool pushFlag;
    protected Vector3 pushPos;


    const float pushLange = 1.5f;

    public bool PushFlag
    {
        set { pushFlag = value; }
    }
    public bool ActiveFlag
    {
        set { activeFlag = value; }
    }
    public Vector3 PushPos
    {
        set { pushPos = value; }
    }
    public float PushLange
    {
        get { return pushLange; }
    }
    void Awake()
    {
    }
    private void OnEnable()
    {
 
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void Hit(int damage)
    {
        hp -= damage;
        GameManager.Instance.EffectManager.UseDamageText(transform.position, damage.ToString());
        StartCoroutine(GameManager.Instance.MyCamera.CameraShake());
    }
    protected void SetMonster(int maxHp,float maxSpeed)
    {
        this.maxHp = maxHp;
        this.maxSpeed = maxSpeed;
        Reset();
    }
    protected void Reset()
    {
        hp = maxHp;
        activeFlag = false;
        pushFlag = false;
        speed = maxSpeed;
        pushSpeed = 2f;
        transform.position = new Vector3(0, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerSkill")
        {
            Hit(7);
        }        
    }

}
