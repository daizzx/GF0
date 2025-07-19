using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private ObjectPool _fatherPool;



    /// <summary>
    /// 设置所属对象池
    /// </summary>
    /// <param name="pool">所属对象池</param>
    public void SetPool(ObjectPool pool)
    {
        _fatherPool = pool;
    }

    /// <summary>
    /// 将对象回收到池中
    /// </summary>
    public void ReleaseToPool()
    {
        if (_fatherPool != null)
        {
            _fatherPool.Release(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 延迟回收
    /// </summary>
    public void DelayedReleaseToPool(float delay)
    {
        StartCoroutine(DelayedRelease(delay));
    }



    IEnumerator DelayedRelease(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReleaseToPool();
    }

}

