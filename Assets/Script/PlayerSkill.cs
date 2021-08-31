using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    float speed;
    float tempTimer;
    float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        tempTimer += Time.deltaTime;
        if (tempTimer < lifeTime)
        {
            Vector3 tempPos = transform.position;
            tempPos.x += Time.deltaTime * speed;
            transform.position = tempPos;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void SetSkill(Vector3 position, float speed,float lifeTime)
    {
        transform.position = position;
        this.speed = speed;
        this.lifeTime = lifeTime;
        tempTimer = 0f;
    }
}
