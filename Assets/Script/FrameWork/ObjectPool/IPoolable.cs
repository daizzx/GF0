using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public interface IPoolable
{
    /// <summary>
    /// ������ӳ���ȡ��ʱ����״̬
    /// </summary>
    void OnWake();


    /// <summary>
    /// ��������յ�����ʱ���״̬
    /// </summary>
    void OnSleep();
}
