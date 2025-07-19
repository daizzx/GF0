using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;


    private UIManager m_UIManager;
    public UIManager UIManager_Root {  get { return m_UIManager; } }

    private SceneControl m_SceneControl;
    public SceneControl SceneControl_Root {  get { return m_SceneControl; } }


    public static GameManager GetInstance()
    {
        if (instance == null)
            return instance;

        return instance;
    }
    //private void Awake()
    //{
    //    if(instance == null)
    //    {
    //        instance = this;
    //    }
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }

    //    instance = this;
    //    m_UIManager = new UIManager();
    //    m_SceneControl = new SceneControl();

    //}

    //private void Start()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //    UIManager_Root.uiCanvasObj = UIMethod.GetInstance().FindCanvas();

    //    Scene1 scene1 = new Scene1();
    //    SceneControl_Root.dict_Scene.Add(scene1.sceneName,scene1);



    //   // UIManager_Root.OpenPanel(new StartPanel());

       
    //}

}
