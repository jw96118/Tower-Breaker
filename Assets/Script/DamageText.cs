using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text damgeText;
    float alpha;
    float tempTimer;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 1f;
        tempTimer = 0f;
        speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        tempTimer += Time.deltaTime;
        if (tempTimer >= 0.5f)
        {
            alpha -= Time.deltaTime * speed;
            if (alpha <= 0)
            {
                tempTimer = 0f;
                alpha = 1f;
                ObjPool.Instance.PushObject(gameObject,OBJ_TYPE.DAMAGE_TEXT);
            }    
        }
        Vector3 tempPos = transform.position;
        tempPos.y += Time.deltaTime * speed;
        transform.position = tempPos;
        damgeText.color = new Color(1f,1f,1f,alpha);
    }

}
