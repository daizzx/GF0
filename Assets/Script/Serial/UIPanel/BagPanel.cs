using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[PanelInfo(UILayer.Menu, UILife.S0)]
public class BagPanel : BasePanel
{
    protected override void HideStartAnimation()
    {
        base.HideStartAnimation();
    }


    protected override void OnFocus()
    {
        base.OnFocus();

        transform.Find("Background").Find("Close").GetComponent<Button>().onClick.AddListener(CloseBagPanel);
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
        transform.Find("Background").Find("Close").GetComponent<Button>().onClick.RemoveAllListeners();
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


    private void CloseBagPanel()
    {
        UIManager.Instance.ClosePanel("BagPanel");

    }


}
