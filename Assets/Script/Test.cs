using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject TestEnemy;
    // Start is called before the first frame update
    void Start()
    {
        //UIManager.Instance.OpenPanel("StartPanel",OpenTheCheckInAndActivity);
        ObjectPoolManager.Instance.Get(TestEnemy,Vector3.zero,Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            //UIManager.Instance.ClosePanel("");
            UIManager.Instance.OpenPanel("StartPanel");
        }


    }


    private void OpenTheCheckInAndActivity(BasePanel panel)
    {
        UIManager.Instance.AddPanelToWaitQueue<Pop1Panel>();
        UIManager.Instance.AddPanelToWaitQueue<Pop2Panel>();
        UIManager.Instance.AddPanelToWaitQueue<Pop3Panel>();
    }

}
