using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public float shakeTimer = 0;
    public float shakeAmount;    
    Vector3 originPos;
    // Start is called before the first frame update
    void Start()
    {
        ShakeCamera(0.05f, 0.1f);
        originPos = transform.localPosition;
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void ShakeCamera(float shakePwr, float shakeDur)
    {
        shakeAmount = shakePwr;
        shakeTimer = shakeDur;
    }


    public IEnumerator CameraShake()
    {
        while (shakeTimer >= 0)
        {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;

            transform.position = transform.position + new Vector3(ShakePos.x, ShakePos.y, 0) + originPos;

            shakeTimer -= Time.deltaTime;
            yield return null;
        }
        transform.position = originPos;
        shakeTimer = 0;
    }


}
