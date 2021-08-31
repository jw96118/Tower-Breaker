using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void EndEvent()
    {
        gameObject.SetActive(false);
        ObjPool.Instance.PushObject(transform.parent.gameObject, OBJ_TYPE.EFFECT);
    }
}
