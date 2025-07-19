using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2 : SceneBase
{
    public readonly string sceneName = "Scene2";

    public override void EnterScene()
    {
        //UIManager.GetInstance().OpenPanel(/*new StartPanel()*/);
        //UIManager.GetInstance().OpenPanel("StartPanel");
        //UIManager.GetInstance().OpenPanel<StartPanel>();
    }

    public override void ExitScene()
    {

    }
}