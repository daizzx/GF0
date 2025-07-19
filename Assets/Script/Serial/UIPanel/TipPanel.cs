using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[PanelInfo(UILayer.Tips, UILife.S0)]
public class TipPanel : BasePanel
{

    private Transform tipTransform;

    protected override void HideStartAnimation()
    {
        base.HideStartAnimation();
    }

    protected override void OnFocus()
    {
        base.OnFocus();
        tipTransform = transform.Find("Tip");
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
        UIManager.Instance.AdjustTipPanelPosition(tipTransform);
        
    }

    protected override void ShowStartAnimation()
    {
        base.ShowStartAnimation();
    }


}
