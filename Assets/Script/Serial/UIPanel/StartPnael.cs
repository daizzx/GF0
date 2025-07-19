using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[PanelInfo(UILayer.Menu, UILife.S0, "")]
public class StartPanel : BasePanel
{
    protected override void HideStartAnimation()
    {
        base.HideStartAnimation();
    }

    protected override void OnFocus()
    {
        base.OnFocus();
        transform.Find("Background").Find("Start").GetComponent<Button>().onClick.AddListener(StartTheGame);


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
        transform.Find("Background").Find("Start").GetComponent<Button>().onClick.RemoveAllListeners();


        

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

    private void StartTheGame()
    {
        UIManager.Instance.ClosePanel("StartPanel");
        UIManager.Instance.OpenPanel("BottomPanel");


    }




}
