using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class BasePanel : MonoBehaviour
{
    public float lifeTime { get; set; }
    public bool isOpened { get; private set; }
    public bool isShowed { get; private set; }
    public bool isFocused { get; private set; }
    public bool isShowInProgress { get; private set; }
    public bool isHideInProgress { get; private set; }

    private CanvasGroup _canvasGroup;

    private bool _isInitialized;


    public CanvasGroup CanvasGroup
    {
        get
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            return _canvasGroup;
        }
    }


    private Action<BasePanel> ShowStartAction { get; set; }

    private Action<BasePanel> ShowFinishAction { get; set; }

    private Action<BasePanel> HideStartAction { get; set; }

    private Action<BasePanel> HideFinishAction { get; set; }


    public GameObject SelfGameObject
    {
        get => gameObject;
    }

    public void Initialize()
    {
        if (!_isInitialized)
        {
            OnInitialized();
            _isInitialized = true;
        }
    }

    public void Show(bool _immediate = true, Action<BasePanel> _startAction = null, Action<BasePanel> _finishAction = null)
    {
        if (_startAction != null)
        {
            ShowStartAction += _startAction;
        }
        if (_finishAction != null)
        {
            ShowFinishAction += _finishAction;
        }

        OnShowStart(_immediate);

    }
    public void Hide(bool _immediate = true, Action<BasePanel> _startAction = null, Action<BasePanel> _finishAction = null)
    {
        if (_startAction != null)
        {
            HideStartAction += _startAction;
        }
        if (_finishAction != null)
        {
            HideFinishAction += _finishAction;
        }

        OnHideStart(_immediate);

    }

    public void Focus()
    {
        OnFocus();
    }
    public void LoseFocus()
    {
        OnLoseFocues();
    }



    protected virtual void OnShowStart(bool _immediate=true)
    {
        isOpened = true;
        isShowInProgress = true;
        gameObject.SetActive(true);

        ShowStartAction?.Invoke(this);
        ShowStartAction = null;

        if (_immediate)
        {


            CanvasGroup.alpha = 1.0f;
            OnShowFinished();
        }
        else
        {
            ShowStartAnimation();
            gameObject.SetActive(true);
            OnShowFinished();
        }

    }

    protected virtual void OnShowFinished()
    {
        isShowInProgress = false;
        isShowed = true;

        ShowFinishAction?.Invoke(this);
        ShowFinishAction = null;

    }




    protected virtual void OnHideStart(bool immediate=true)
    {

        isHideInProgress = true;
        HideStartAction?.Invoke(this);
        HideStartAction = null;

        if (immediate)
        {
            CanvasGroup.alpha = 0f;
            gameObject.SetActive(false);
            OnHideFinished();
        }
        else
        {
            HideStartAnimation();

            OnHideFinished();
            gameObject.SetActive(false);
        }

    }

    protected virtual void OnHideFinished()
    {
        isOpened = false;
        isHideInProgress = false;
        isShowed = false;

        HideFinishAction?.Invoke(this);
        HideFinishAction = null;
    }






    protected virtual void OnFocus()
    {
        isFocused = true;

    }
    protected virtual void OnLoseFocues()
    {
        isFocused = false;

    }




    protected virtual void OnInitialized()
    {

    }
    protected virtual void HideStartAnimation()
    {

    }

    protected virtual void ShowStartAnimation()
    {

    }
}

