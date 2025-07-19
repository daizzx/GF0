using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private ObjectPool _fatherPool;



    /// <summary>
    /// �������������
    /// </summary>
    /// <param name="pool">���������</param>
    public void SetPool(ObjectPool pool)
    {
        _fatherPool = pool;
    }

    /// <summary>
    /// ��������յ�����
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
    /// �ӳٻ���
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

