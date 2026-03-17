using UnityEngine;
using UnityEngine.UI;


public class HandGrinderUI : MonoBehaviour
{
    public GameObject LeftUI, RightUI;
    public Image leftBeans, leftGrounds, rightBeans, rightGrounds;

    private void Start()
    {
        DisableUI(true);
        DisableUI(false);
    }
    public void ActivateUI(bool _left)
    {
        if (_left)
            LeftUI.SetActive(true);
        else
            RightUI.SetActive(true);
    }

    public void DisableUI(bool _left)
    {
        if (_left)
            LeftUI.SetActive(false);
        else
            RightUI.SetActive(false);
    }

    public void SetBeanBar(bool _left, float amount)
    {
        if (_left)
            leftBeans.fillAmount = amount;
        else
            rightBeans.fillAmount = amount;
    }

    public void SetGroundsBar(bool _left, float amount)
    {
        if (_left)
            leftGrounds.fillAmount = amount;
        else
            rightGrounds.fillAmount = amount;
    }
}
