using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCoolTime : MonoBehaviour
{
    float coolTime;
    bool coolStartFlag;
    Image filledImage;
    [SerializeField] GameObject disableButton;
    public bool CoolStartFlag
    {
        get { return coolStartFlag; }
    }
    void Awake()
    {
        coolTime = 0f;
        coolStartFlag = false;
        filledImage = transform.GetChild(1).transform.GetChild(2).GetComponent<Image>();
    }
    void Update()
    {
        if (coolStartFlag)
        {
            if (filledImage.fillAmount < 1f)
            {
                if (disableButton != null)
                {
                    if (!GameManager.Instance.gameStop)
                    {
                        filledImage.fillAmount += Time.deltaTime / coolTime;
                    }
                }
                else
                    filledImage.fillAmount += Time.deltaTime / coolTime;
            }
            else
            {
                coolStartFlag = false;
                filledImage.fillAmount = 0f;
                if (disableButton != null)
                {
                    if (disableButton.activeSelf)
                    {
                        transform.GetChild(1).gameObject.SetActive(false);
                        transform.GetChild(2).gameObject.SetActive(false);
                    }
                    else
                    {
                        transform.GetChild(0).gameObject.SetActive(false);
                        transform.GetChild(1).gameObject.SetActive(false);
                        transform.GetChild(2).gameObject.SetActive(true);
                    }
                }
                else
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                    transform.GetChild(2).gameObject.SetActive(true);
                }
            }
        }

    }
    public void StartCool(float coolTime)
    {
        this.coolTime = coolTime;
        coolStartFlag = true;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        filledImage.fillAmount = 0f;
        coolStartFlag = false;
    }
    public void Reset()
    {
        filledImage.fillAmount = 0f;
        coolStartFlag = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
    }
}
