using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageFilled : MonoBehaviour
{
    [SerializeField] Image buttonFilled;
    Image imageFilled;
    // Start is called before the first frame update
    void Start()
    {
        imageFilled = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        imageFilled.fillAmount = buttonFilled.fillAmount;
    }
}
