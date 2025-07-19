using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public interface IPoolable
{
    /// <summary>
    /// 当对象从池中取出时重置状态
    /// </summary>
    void OnWake();


    /// <summary>
    /// 当对象回收到池中时清空状态
    /// </summary>
    void OnSleep();
}
