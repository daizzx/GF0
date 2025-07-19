using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[PanelInfo(UILayer.Menu, UILife.S0)]
public class MenuPanel : BasePanel
{
    protected override void HideStartAnimation()
    {
        base.HideStartAnimation();
    }

    protected override void OnFocus()
    {
        base.OnFocus();
        transform.Find("Background").Find("Exit").GetComponent<Button>().onClick.AddListener(CloseMenuPanel);

        transform.Find("Background").Find("SideBar").Find("Option").GetComponent<Button>().onClick.AddListener(OpenOptionPnale);

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
        transform.Find("Background").Find("Exit").GetComponent<Button>().onClick.RemoveAllListeners();
        transform.Find("Background").Find("SideBar").Find("Option").GetComponent<Button>().onClick.RemoveAllListeners();
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


    private  void CloseMenuPanel()
    {
        UIManager.Instance.ClosePanel("MenuPanel");
    }

    private void OpenOptionPnale()
    {
        UIManager.Instance.OpenPanel("OptionPanel");
    }

}
