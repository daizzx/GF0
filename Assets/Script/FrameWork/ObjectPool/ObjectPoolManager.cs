using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{


    private static ObjectPoolManager _instance;

    #region ����


    private Dictionary<GameObject, ObjectPool> _poolsDic = new Dictionary<GameObject, ObjectPool>();// ������ֵ䣺Ԥ���� -> ��Ӧ�Ķ����

    
    private Dictionary<int, ObjectPool> _instanceToPoolMapDic = new Dictionary<int, ObjectPool>();// �����IDӳ�䣺ʵ��ID -> ���������

    
    public int defaultInitialSize = 10;// ���ã���ʼ�ش�С

    
    public int defaultMaxSize = 100;// ���ã����ش�С��0��ʾ�����ƣ�

    #endregion

    public static ObjectPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ObjectPoolManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ObjectPoolManager");
                    _instance = obj.AddComponent<ObjectPoolManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }




    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Ԥ���ض����
    /// </summary>
    /// <param name="prefab">Ŀ�����</param>
    /// <param name="initialSize">��ʼ�ش�С</param>
    /// <param name="maxSize">��������</param>
    public void PreloadPool(GameObject prefab, int initialSize = -1, int maxSize = -1)
    {
        if (initialSize < 0) initialSize = defaultInitialSize;
        if (maxSize < 0) maxSize = defaultMaxSize;

        if (!_poolsDic.ContainsKey(prefab))
        {
            CreateNewPool(prefab, initialSize, maxSize);
        }
    }

    /// <summary>
    /// �Ӷ���ػ�ȡ����
    /// </summary>
    /// <param name="prefab">Ŀ�����</param>
    /// <param name="position">����λ��</param>
    /// <param name="rotation">������ת</param>
    /// <returns></returns>
    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // ����ز����ڣ��Զ�����
        if (!_poolsDic.ContainsKey(prefab))
        {
            PreloadPool(prefab);
        }

        GameObject obj = _poolsDic[prefab].Get();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }   
    public GameObject Get(GameObject prefab)
    {
        // ����ز����ڣ��Զ�����
        if (!_poolsDic.ContainsKey(prefab))
        {
            PreloadPool(prefab);
        }

        GameObject obj = _poolsDic[prefab].Get();
        return obj;
    }




    /// <summary>
    /// ���ն��󵽶����
    /// </summary>
    /// <param name="obj">���ն���</param>
    public void Release(GameObject obj)
    {
        int instanceId = obj.GetInstanceID();

        if (_instanceToPoolMapDic.ContainsKey(instanceId))
        {
            _instanceToPoolMapDic[instanceId].Release(obj);
        }
        else
        {
            Debug.LogWarning($"Trying to release not found object: {obj.name}");
            Destroy(obj);
        }
    }

    /// <summary>
    /// ������ж����
    /// </summary>
    public void ClearAllPools()
    {
        foreach (var pool in _poolsDic.Values)
        {
            pool.Clear();
        }

        _poolsDic.Clear();
        _instanceToPoolMapDic.Clear();
    }

    /// <summary>
    /// �����µĶ����
    /// </summary>
    /// <param name="prefab">Ŀ�����</param>
    /// <param name="initialSize">��ʼ�ش�С</param>
    /// <param name="maxSize">��������</param>
    private void CreateNewPool(GameObject prefab, int initialSize, int maxSize)
    {
        ObjectPool newPool = new ObjectPool(prefab, initialSize, maxSize, this);
        _poolsDic.Add(prefab, newPool);
    }

    /// <summary>
    /// ע�����ʵ���������ӳ��
    /// </summary>
    /// <param name="instance">Ŀ�����</param>
    /// <param name="pool">��Ӧ�Ķ����</param>
    internal void RegisterInstance(GameObject instance, ObjectPool pool)
    {
        _instanceToPoolMapDic.Add(instance.GetInstanceID(), pool);
    }
}

