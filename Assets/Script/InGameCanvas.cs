using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.transform.localPosition = new Vector3(0, 0f, 0);
        gameObject.transform.localScale = new Vector2((1.5f / Screen.width), (1.5f / Screen.width));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
