using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFFECT_TYPE
{
    STAB = 0,
    CUT,
    SCRATCH,
    GAURD,
    END
}
public class EffectManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UseAttackEffect(int type, Vector3 position)
    {
        GameObject temp = ObjPool.Instance.PullObject(OBJ_TYPE.EFFECT);
        for (int i = 0; i < (int)EFFECT_TYPE.END; i++)
            temp.transform.GetChild(i).gameObject.SetActive(false);

        temp.transform.position = position;
        temp.transform.GetChild(type).gameObject.SetActive(true);
    }
    public void UseDamageText(Vector3 position,string damage)
    {
        DamageText temp = ObjPool.Instance.PullObject(OBJ_TYPE.DAMAGE_TEXT).GetComponent<DamageText>();
        temp.damgeText.text = damage;
        temp.transform.position = position;
        Debug.Log("DamagePos : " + position);
    }
}
