using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    # region 组件
    private Canvas mainCanvas;
    private RectTransform rectTransform;
    public Canvas MainCanvas => mainCanvas;

    [HideInInspector] public Vector2 CanvasSize = Vector2.zero;

    private Dictionary<UILayer, RectTransform> layerRootDict = new Dictionary<UILayer, RectTransform>();

    #endregion


    #region 数据
    private const string DefaultViewPrefabPath = "UIPanel/";


    private Dictionary<string, PanelInfoAttribute> panelInfoDict = new Dictionary<string, PanelInfoAttribute>();  //panel的信息
    private Dictionary<string, GameObject> panelPrefabDict = new Dictionary<string, GameObject>();  //panel的预制体
    private Dictionary<string, bool> panelIsCreatingDict = new Dictionary<string, bool>();    //panel的创建状态
    private Dictionary<string, BasePanel> panelDict = new Dictionary<string, BasePanel>();  //panel实例


    private PanelInfoAttribute currentPanelInfo = null;
    private LinkedList<PanelInfoAttribute> panelLinkedList = new LinkedList<PanelInfoAttribute>(); //已打开界面
    private Dictionary<UILayer, int> layerPanelNumDict = new Dictionary<UILayer, int>();
    private Dictionary<string, Dictionary<string, int>> panelCanvasOriginalValDict = new Dictionary<string, Dictionary<string, int>>();

    public PanelInfoAttribute CurrentPanelNameInfo
    {
        get => currentPanelInfo;
    }


    public BasePanel CurPanel
    {
        get => currentPanelInfo == null ? null : GetPanel(currentPanelInfo.Name);
    }

    #endregion


    #region 初始化
    private void Awake()
    {
        Instance = this;

        mainCanvas = transform.Find("Canvas").GetComponent<Canvas>();
        rectTransform = mainCanvas.GetComponent<RectTransform>();

        CanvasSize = rectTransform.sizeDelta;

        Init();
    }

    private void Init()
    {
        AnalysisAllPanelInfo();
    }

    private void AnalysisAllPanelInfo()
    {
        var assembly = Assembly.GetAssembly(typeof(PanelInfoAttribute));
        Type[] types = assembly.GetTypes();

        foreach (var t in types)
        {
            var attr = t.GetCustomAttribute(typeof(PanelInfoAttribute));

            var info = attr as PanelInfoAttribute;

            if (info != null && t.BaseType.Name == typeof(BasePanel).Name)
            {
                var tName = t.ToString();
                info.SetName(tName, DefaultViewPrefabPath + tName);
                panelInfoDict.Add(tName, info);
            }

        }


    }




    #endregion


    #region 界面操作
    #region 公开
    /// <summary>
    /// 打开界面
    /// </summary>
    /// <typeparam name="T">界面类型</typeparam>
    /// <param name="startAction">打开前的回调</param>
    /// <param name="finishAction">打开后的回调</param>
    public void OpenPanel<T>(Action<BasePanel> startAction, Action<BasePanel> finishAction = null) where T : BasePanel
    {
        OpenPanel<T>(false, startAction, finishAction);
    }
    /// <summary>
    /// 打开界面
    /// </summary>
    /// <typeparam name="T">界面类型</typeparam>
    /// <param name="immediate">是否跳过开启动画</param>
    /// <param name="startAction">打开前回调</param>
    /// <param name="finishAction">打开后回调</param>
    private void OpenPanel<T>(bool immediate = true, Action<BasePanel> startAction = null, Action<BasePanel> finishAction = null) where T : BasePanel
    {
        var name = typeof(T).Name;

        OpenPanel(name, immediate, startAction, finishAction);
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="tName">界面名称</param>
    /// <param name="startAction">打开前回调</param>
    /// <param name="finishAction">打开后回调</param>
    public void OpenPanel(string tName, Action<BasePanel> startAction = null, Action<BasePanel> finishAction = null)
    {
        OpenPanel(tName, false, startAction, finishAction);
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="tName">界面名称</param>
    /// <param name="immediate">是否跳过开启动画</param>
    /// <param name="startAction">打开前回调</param>
    /// <param name="finishAction">打开后回调</param>
    public void OpenPanel(string tName, bool immediate, Action<BasePanel> startAction = null, Action<BasePanel> finishAction = null)
    {
        if (panelInfoDict.ContainsKey(tName))
        {
            var panelInfo = panelInfoDict[tName];

            if (panelDict.ContainsKey(tName))
            {
                if (panelDict[tName] is BasePanel panel)
                {
                    if (panel.isOpened)
                    {
                        Debug.Log($"界面{tName}已打开");
                    }
                    else
                    {
                        ShowPanel(panelInfo, panel, immediate, startAction, finishAction);
                    }

                }
            }
            else
            {
                CreatePanel(panelInfo, immediate, startAction, finishAction);
            }
        }
        else
        {
            Debug.Log($"未找到对应{tName}界面信息");
        }

    }


    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <typeparam name="T">界面类型</typeparam>
    /// <param name="startAction">关闭前回调</param>
    /// <param name="finishAction">关闭后回调</param>
    public void ClosePanel<T>(Action<BasePanel> startAction, Action<BasePanel> finishAction = null) where T : BasePanel
    {
        ClosePanel<T>(false, startAction, finishAction);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <typeparam name="T">界面类型</typeparam>
    /// <param name="immediate">是否跳过关闭动画</param>
    /// <param name="startAction">关闭前回调</param>
    /// <param name="finishAction">关闭后回调</param>
    public void ClosePanel<T>(bool immediate = true, Action<BasePanel> startAction = null, Action<BasePanel> finishAction = null)
    where T : BasePanel
    {
        var tName = typeof(T).ToString();

        ClosePanel(tName, immediate, startAction, finishAction);
    }


    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="tName">界面名称</param>
    /// <param name="startAction">关闭前回调</param>
    /// <param name="finishAction">关闭后回调</param>
    public void ClosePanel(string tName, Action<BasePanel> startAction = null, Action<BasePanel> finishAction = null)
    {
        ClosePanel(tName, false, startAction, finishAction);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="tName">界面名称</param>
    /// <param name="immediate">是否跳过关闭动画</param>
    /// <param name="startAction">关闭前回调</param>
    /// <param name="finishAction">关闭后回调</param>
    public void ClosePanel(string tName, bool immediate, Action<BasePanel> startAction = null, Action<BasePanel> finishAction = null)
    {

        if (panelDict.ContainsKey(tName))
        {
            if (panelDict[tName] is BasePanel panel)
            {
                var panelInfo = panelInfoDict[tName];
                HidePanel(panelInfo, panel, immediate, startAction, finishAction);
            }
        }
        else
        {
            Debug.Log($"关闭失败 {tName} 尚未打开");
        }


    }

    public T GetPanel<T>() where T : BasePanel
    {
        var typeName = typeof(T).ToString();
        if (panelDict.ContainsKey(typeName))
        {
            return panelDict[typeName] as T;
        }
        else
        {
            // Debug.LogError($"不存在此Panel 检查 {typeName} 是否声明 {typeof(T).ToString()}");
        }

        return null;
    }


    public BasePanel GetPanel(string typeName)
    {
        if (panelDict.ContainsKey(typeName))
        {
            return panelDict[typeName];
        }
        else
        {
            // Debug.LogError($"不存在此Panel 检查 {typeName} 是否声明 {typeof(T).ToString()}");
        }

        return null;
    }

    #endregion

    #region 私有

    private void CreatePanel(PanelInfoAttribute panelInfo, bool immediate = true,Action<BasePanel> startAction = null,Action<BasePanel> finishAction = null)
    {
        if (panelPrefabDict.ContainsKey(panelInfo.Name))
        {
            var prefab = panelPrefabDict[panelInfo.Name];

            CreatePanel(prefab, panelInfo, immediate, startAction, finishAction);
        }
        else
        {
            if (panelIsCreatingDict.TryGetValue(panelInfo.Name, out bool isLoading))
            {
                if (isLoading)
                {
                    Debug.Log($"此Panel {panelInfo.Name} 正在加载中");
                    return;
                }
                else
                {
                    panelIsCreatingDict[panelInfo.Name] = true;
                }
            }
            else
            {
                panelIsCreatingDict.Add(panelInfo.Name, true);
            }

#if ADDRESSABLE
            UIAssetHelper.LoadPanelPrefab(panelInfo.Path, (prefab, handle) =>
            {
                _panelPrefabDict.Add(panelInfo.Name, prefab);
                CreatePanel(prefab, panelInfo, immediate, startAction, finishAction);
                _panelIsCreatingDict[panelInfo.Name] = false;

                _addressableHandleDict.Add(panelInfo.Name, handle);
            });
#else
            UIAssetHelper.LoadPanelPrefab(panelInfo.Path, (prefab) =>
            {
                panelPrefabDict.Add(panelInfo.Name, prefab);
                CreatePanel(prefab, panelInfo, immediate, startAction, finishAction);
                panelIsCreatingDict[panelInfo.Name] = false;
            });
#endif
        }
    }


    private void CreatePanel(GameObject prefab, PanelInfoAttribute panelInfo, bool immediate = true,Action<BasePanel> startAction = null,Action<BasePanel> finishAction = null)
    {

        var panelObj = Instantiate(prefab);
       // SetPanelParent(panelObj, panelInfo.Layer);
        var panel = panelObj.GetComponent(panelInfo.Name) as BasePanel;
        if (panel == null)
            Debug.LogError($"无法获取 {panelInfo.Name} 脚本，检查是否挂载");

        panel.Initialize();


        panelDict.Add(panelInfo.Name, panel);
        //panel.gameObject.SetActiveEx(false);

        ShowPanel(panelInfo, panel, immediate, startAction, finishAction);
    }



    private void ShowPanel(PanelInfoAttribute panelInfo, BasePanel panel, bool immediate, Action<BasePanel> startAction = null, Action<BasePanel> finishAction = null)
    {
        if(!CheckIsHaveHigherLayerInStack(panelInfo.Layer))
        {
            panel.Focus();

            if(panelLinkedList.Count > 0)
            {
                var stackTopPanelInfo = panelLinkedList.Last.Value;
                if(stackTopPanelInfo.Name!="UICurrencyPanel")
                {
                    GetPanel(stackTopPanelInfo.Name)?.LoseFocus();
                }

            }
        }


        panel.Show(immediate, startAction, finishAction);
        panel.SelfGameObject.transform.SetAsLastSibling();  //设置面板为同级最后一个

        if(panelInfo.Name!="UICurrencyPanel")
        {
            AddToLinkedList(panelInfo);
        }
        if (panelLinkedList.Count > 0)
            currentPanelInfo = panelLinkedList.Last.Value;

        ReSetCanvasOrder(panelInfo);


    }

    private void HidePanel(PanelInfoAttribute panelInfo, BasePanel panel, bool immediate = true, Action<BasePanel> startAction = null, Action<BasePanel> finishAction = null)
    {
        var panelIsInStackTop = panelLinkedList.Last.Value.Name == panelInfo.Name;

        RemoveFromLinkedList(panelInfo.Name);
        if (panelLinkedList.Count > 0)
            currentPanelInfo = panelLinkedList.Last.Value;
        else
            currentPanelInfo = null;

        if (!CheckIsHaveHigherLayerInStack(panelInfo.Layer) && panelIsInStackTop)
        {
            panel.LoseFocus();

            if (panelLinkedList.Count > 0)
            {
                var stackTopPanelInfo = panelLinkedList.Last.Value;


                GetPanel(stackTopPanelInfo.Name)?.Focus();
            }
        }

        panel.Hide(immediate, startAction, (p) =>
        {
            finishAction?.Invoke(p);

            if (CheckHaveNextWaitPanel())
            {
                OpenPanelInWaitQueue();
            }
        });

        //panel.Hide(immediate, startAction,finishAction);
        //if (CheckHaveNextWaitPanel())
        //{

        //    OpenPanelInWaitQueue();
        //}

    }

    private void ReSetCanvasOrder(PanelInfoAttribute panelInfo)
    {
        if (layerPanelNumDict.ContainsKey(panelInfo.Layer))
        {
            layerPanelNumDict[panelInfo.Layer] += 1;
        }
        else
        {
            layerPanelNumDict.Add(panelInfo.Layer, 1);
        }


        int index = 0;
        foreach (var info in panelLinkedList)
        {
            if (info.Layer == panelInfo.Layer)
            {
                ResetPanelOrder(info, (int)panelInfo.Layer, index);
                index++;
            }
        }
    }
    private void ResetPanelOrder(PanelInfoAttribute info, int orderOffset, int index)
    {
        int increment = 100;

        var panel = GetPanel(info.Name);

        var canvas = panel.GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = panel.gameObject.AddComponent<Canvas>();
        }

        var gr = panel.GetComponent<GraphicRaycaster>();
        if (gr == null)
        {
            panel.gameObject.AddComponent<GraphicRaycaster>();
        }

        canvas.overrideSorting = true;
        var startOrder = orderOffset + index * increment;
        canvas.sortingOrder = startOrder;

        var canvasArr = panel.GetComponentsInChildren<Canvas>(true);
        foreach (var ca in canvasArr)
        {
            if (object.ReferenceEquals(ca, canvas)) continue;

            ca.overrideSorting = true;

            int originalVal = 0;
            var routePath = GetRoute(ca.transform, canvas.transform);
            if (panelCanvasOriginalValDict.ContainsKey(info.Name))
            {
                if (panelCanvasOriginalValDict[info.Name].ContainsKey(routePath))
                {
                    originalVal = panelCanvasOriginalValDict[info.Name][routePath];
                }
                else
                {
                    originalVal = ca.sortingOrder;
                    panelCanvasOriginalValDict[info.Name].Add(routePath, originalVal);
                }
            }
            else
            {
                originalVal = ca.sortingOrder;
                panelCanvasOriginalValDict.Add(info.Name, new Dictionary<string, int>() { { routePath, originalVal } });
            }

            ca.sortingOrder = startOrder + originalVal;
        }

        var particleArr = panel.GetComponentsInChildren<ParticleSystemRenderer>(true);
        foreach (var pa in particleArr)
        {
            int originalVal = 0;
            var routePath = GetRoute(pa.transform, canvas.transform);
            if (panelCanvasOriginalValDict.ContainsKey(info.Name))
            {
                if (panelCanvasOriginalValDict[info.Name].ContainsKey(routePath))
                {
                    originalVal = panelCanvasOriginalValDict[info.Name][routePath];
                }
                else
                {
                    originalVal = pa.sortingOrder;
                    panelCanvasOriginalValDict[info.Name].Add(routePath, originalVal);
                }
            }
            else
            {
                originalVal = pa.sortingOrder;
                panelCanvasOriginalValDict.Add(info.Name, new Dictionary<string, int>() { { routePath, originalVal } });
            }

            pa.sortingOrder = startOrder + originalVal;
        }
    }

    private void SetPanelParent(GameObject go, UILayer layer)
    {
        var parent = mainCanvas.transform.Find(layer.ToString());
        go.transform.SetParent(parent, false);
    }

    private void DestroyPanel(string viewName)
    {
        var view = panelDict[viewName];
        if (!(view is null)) Destroy(view.SelfGameObject);

        panelDict.Remove(viewName);

        ReleaseAsset(viewName);
    }

    private void ReleaseAsset(string viewName)
    {
        var viewInfo = panelInfoDict[viewName];
        if (viewInfo.ReleaseAssetWhenDestory)
        {
            var prefab = panelPrefabDict[viewInfo.Name];
            panelPrefabDict.Remove(viewInfo.Name);
#if ADDRESSABLE
            var handle = _addressableHandleDict[viewInfo.Name];
            _addressableHandleDict.Remove(viewInfo.Name);
            UIAssetHelper.ReleasePanelPrefab(prefab, viewInfo.Path, handle);
#else
            UIAssetHelper.ReleasePanelPrefab(prefab, viewInfo.Path);
#endif
        }
    }

    private void AddToLinkedList(PanelInfoAttribute panelInfo)
    {
        if (panelLinkedList.Count <= 0)
        {
            panelLinkedList.AddLast(panelInfo);
            return;
        }

        var node = panelLinkedList.First;
        while (node != null)
        {
            var nodeNext = node.Next;
            if (nodeNext == null)
            {
                panelLinkedList.AddAfter(node, panelInfo);
                return;
            }
            else
            {
                if (panelInfo.Layer >= node.Value.Layer && panelInfo.Layer < nodeNext.Value.Layer)
                {
                    panelLinkedList.AddAfter(node, panelInfo);
                    return;
                }
                else
                    node = node.Next;
            }
        }
    }

    private void RemoveFromLinkedList(string panelName)
    {
        var node = panelLinkedList.First;
        while (node != null)
        {
            if (node.Value.Name.Equals(panelName))
            {
                panelLinkedList.Remove(node.Value);
                return;
            }
            else
                node = node.Next;
        }
    }

    #endregion

    #endregion


    #region 针对弹窗的界面队列

    private Queue<WaitPanelInfo> panelWaitQueue = new Queue<WaitPanelInfo>();

    private class WaitPanelInfo
    {
        public string tName = "";
        public bool immediate = true;
        public Action<BasePanel> startAction = null;
        public Action<BasePanel> finishAction = null;
    }

    public void AddPanelToWaitQueue<T>(bool immediate = true, Action<BasePanel> startAction = null,
        Action<BasePanel> finishAction = null)
        where T : BasePanel
    {
        WaitPanelInfo info = new WaitPanelInfo();
        info.tName = typeof(T).ToString();
        info.immediate = immediate;
        info.startAction = startAction;
        info.finishAction = finishAction;

        panelWaitQueue.Enqueue(info);

    }

    public void OpenPanelInWaitQueue()
    {
        if (panelWaitQueue.Count <= 0) return;

        var info = panelWaitQueue.Dequeue();



        OpenPanel(info.tName, info.immediate, info.startAction, info.finishAction);
    }

    public bool CheckHaveNextWaitPanel()
    {
        return panelWaitQueue.Count > 0;
    }

    #endregion


    #region 生命周期

    private int _frame = 0;
    private int _frameMax = 2; //几帧更新一次


    
    private void Update()
    {
        _frame++;
        if (_frame >= _frameMax)
        {
            _frame = 0;

            var panelNameList = panelDict.Keys.ToList<string>();

            for (int i = 0; i < panelNameList.Count; i++)
            {

                var panelName = panelNameList[i];

                var panel = panelDict[panelName];
                if (panel == null)
                {
                    Debug.LogError($"界面 {panelName} 为空");
                    continue;
                }

                var panelInfo = panelInfoDict[panelName];

                if ((int)panelInfo.Life <= -1) continue;


                if (panel.isShowed || panel.isShowInProgress) continue;


                panel.lifeTime += Time.deltaTime * _frameMax;



                if (panel.lifeTime > (int)panelInfo.Life)
                {
                    DestroyPanel(panelName);
                }
            }
        }

    }

    #endregion


    #region 其他UI内容




    public RectTransform GetUILayerRoot(UILayer layer)
    {
        if (layerRootDict.ContainsKey(layer))
            return layerRootDict[layer];
        else
        {
            var rect = transform.Find($"Canvas/{layer.ToString()}") as RectTransform;
            layerRootDict.Add(layer, rect);
            return rect;
        }
    }

    private bool CheckIsHaveHigherLayerInStack(UILayer _layer)
    {
        foreach (var item in panelLinkedList)
        {
            var order = (int)item.Layer;
            if (order != -1 && order > (int)_layer)
            {
                return true;
            }
        }

        return false;
    }

    #endregion




    public string GetRoute(Transform childTrans, Transform rootTrans = null, string splitter = ".")
    {
        var result = childTrans.name;
        var parent = childTrans.parent;
        while (null == rootTrans ? parent != null : !ReferenceEquals(rootTrans, parent))
        {
            result = $"{parent.name}{splitter}{result}";
            parent = parent.parent;
        }

        return result;
    }


    public void AdjustTipPanelPosition(Transform _transform)
    {
        float xLimit = 960;
        float yLimit = 540;
        float xOffset = 200;
        float yOffset = 200;

        Vector2 mousePosition = Input.mousePosition;

        float newXoffset = 0;
        float newYoffset = 0;

        if (mousePosition.x > xLimit)
            newXoffset = -xOffset;
        else
            newXoffset = xOffset;

        if (mousePosition.y > yLimit)
            newYoffset = -yOffset;
        else
            newYoffset = yOffset;

        _transform.position = new Vector2(mousePosition.x + newXoffset, mousePosition.y + newYoffset);



    }



}
