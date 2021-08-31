using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour
{
    public void ButtonDown()
    {
        transform.localPosition = new Vector3(0, -25f, 0);
    }
    public void ButtonUp()
    {
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
