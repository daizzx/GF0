using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[PanelInfo(UILayer.Bottom, UILife.Permanent)]
public class BottomPanel : BasePanel
{



    protected override void HideStartAnimation()
    {
        base.HideStartAnimation();
    }

    protected override void OnFocus()
    {
        base.OnFocus();
        transform.Find("Bag").GetComponent<Button>().onClick.AddListener(OpenBagPanel);
        transform.Find("Menu").GetComponent<Button>().onClick.AddListener(OpenMenuPanel);


    }

    protected override void OnHideFinished()
    {
        base.OnHideFinished();
    }

    protected override void OnHideStart(bool immediate)
    {
        base.OnHideStart(immediate);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();


    }

    protected override void OnLoseFocues()
    {
        base.OnLoseFocues();
        transform.Find("Bag").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("Menu").GetComponent<Button>().onClick.RemoveAllListeners();
    }

    protected override void OnShowFinished()
    {
        base.OnShowFinished();
    }

    protected override void OnShowStart(bool _immediate)
    {
        base.OnShowStart(_immediate);
    }

    protected override void ShowStartAnimation()
    {
        base.ShowStartAnimation();
    }



    private void OpenBagPanel()
    {
        UIManager.Instance.OpenPanel("BagPanel");

    }
    private void OpenMenuPanel()
    {
        UIManager.Instance.OpenPanel("MenuPanel");
    }

}
