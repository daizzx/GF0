using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[PanelInfo(UILayer.PopUpWindow, UILife.S0)]
public class Pop2Panel : BasePanel
{
    protected override void HideStartAnimation()
    {
        base.HideStartAnimation();
    }

    protected override void OnFocus()
    {
        base.OnFocus();
        transform.Find("Background").Find("Close").GetComponent<Button>().onClick.AddListener(CloseThisPanel);

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

    private void CloseThisPanel()
    {
        UIManager.Instance.ClosePanel("Pop2Panel");
    }
}
