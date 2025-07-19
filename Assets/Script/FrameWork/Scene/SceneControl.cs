using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneControl 
{
    private static SceneControl instance;

    public Dictionary<string, SceneBase> dict_Scene;


    public static SceneControl GetInstance()
    {
        if (instance == null)
            instance = new SceneControl();

        return instance;
    }

    public SceneControl() 
    {
        instance = this;
        dict_Scene = new Dictionary<string, SceneBase>();
    }


    public void LoadScene(string _SceneName, SceneBase _SceneBase)
    {

        if(!dict_Scene.ContainsKey(_SceneName))
        {
            dict_Scene.Add(_SceneName, _SceneBase);
        }
        if(dict_Scene.ContainsKey(SceneManager.GetActiveScene().name))
        {
            dict_Scene[SceneManager.GetActiveScene().name].ExitScene();
        }
        else
        {
            Debug.Log("NULL");
        }
        
       // UIManager.GetInstance().ClosePanel(true);


        SceneManager.LoadScene(_SceneName);
        _SceneBase.EnterScene();

    }




}
