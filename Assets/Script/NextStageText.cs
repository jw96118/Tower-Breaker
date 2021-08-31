using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextStageText : MonoBehaviour
{
    Text text;
    float speed;
    float alpha;
    void Awake()
    {
        text = GetComponent<Text>();
        speed = -1f;
        alpha = 1f;
    }
    private void OnEnable()
    {
        text.color = new Color(1f,1f,1f,1f);
        speed = -1f;
        alpha = 1f;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        alpha += Time.deltaTime * speed;
        if (alpha <= 0f || alpha >= 1f)
        {
            speed = -speed;
        }
        text.color = new Color(1f, 1f, 1f, alpha);
    }
}
