using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultEvent : MonoBehaviour
{
    Vector3 scale;
    float scaleFloat;
    float tempTimer;
    private void Awake()
    {
        scale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 1);
        scaleFloat = scale.x;
    }
    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 1);
        tempTimer = 0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tempTimer += Time.deltaTime;
        if (tempTimer < 1f)
        {
            transform.localScale = new Vector3(tempTimer * scaleFloat, tempTimer * scaleFloat, 1);
        }
        else
        {
            transform.localScale = new Vector3(scaleFloat, scaleFloat, 1);
        }
    }
}
