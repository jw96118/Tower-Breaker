using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMan : Monster
{
    private void OnEnable()
    {
        SetMonster(20, 1.5f);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            GameManager.Instance.UIManager.ChangeMonsterUI();
            ObjPool.Instance.PushObject(this.gameObject, OBJ_TYPE.MONSTER);
        }
        if (pushFlag)
        {
            transform.position = Vector3.Lerp(transform.position, pushPos, Time.deltaTime * pushSpeed);
            tempTimer += Time.deltaTime;
            if (tempTimer > 0.5f)
            {
                tempTimer = 0f;
                pushFlag = false;
            }
        }
        else
        {
            if (activeFlag)
            {
                Vector3 tempPos = transform.position;
                tempPos.x -= Time.deltaTime * speed;
                transform.position = tempPos;
            }
        }
    }
}
