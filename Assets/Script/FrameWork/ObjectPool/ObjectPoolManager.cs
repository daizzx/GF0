using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{


    private static ObjectPoolManager _instance;

    #region 数据


    private Dictionary<GameObject, ObjectPool> _poolsDic = new Dictionary<GameObject, ObjectPool>();// 对象池字典：预制体 -> 对应的对象池

    
    private Dictionary<int, ObjectPool> _instanceToPoolMapDic = new Dictionary<int, ObjectPool>();// 对象池ID映射：实例ID -> 所属对象池

    
    public int defaultInitialSize = 10;// 配置：初始池大小

    
    public int defaultMaxSize = 100;// 配置：最大池大小（0表示无限制）

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
    /// 预加载对象池
    /// </summary>
    /// <param name="prefab">目标对象</param>
    /// <param name="initialSize">初始池大小</param>
    /// <param name="maxSize">最大池容量</param>
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
    /// 从对象池获取对象
    /// </summary>
    /// <param name="prefab">目标对象</param>
    /// <param name="position">对象位置</param>
    /// <param name="rotation">对象旋转</param>
    /// <returns></returns>
    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // 如果池不存在，自动创建
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
        // 如果池不存在，自动创建
        if (!_poolsDic.ContainsKey(prefab))
        {
            PreloadPool(prefab);
        }

        GameObject obj = _poolsDic[prefab].Get();
        return obj;
    }




    /// <summary>
    /// 回收对象到对象池
    /// </summary>
    /// <param name="obj">回收对象</param>
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
    /// 清空所有对象池
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
    /// 创建新的对象池
    /// </summary>
    /// <param name="prefab">目标对象</param>
    /// <param name="initialSize">初始池大小</param>
    /// <param name="maxSize">最大池容量</param>
    private void CreateNewPool(GameObject prefab, int initialSize, int maxSize)
    {
        ObjectPool newPool = new ObjectPool(prefab, initialSize, maxSize, this);
        _poolsDic.Add(prefab, newPool);
    }

    /// <summary>
    /// 注册对象实例到对象池映射
    /// </summary>
    /// <param name="instance">目标对象</param>
    /// <param name="pool">对应的对象池</param>
    internal void RegisterInstance(GameObject instance, ObjectPool pool)
    {
        _instanceToPoolMapDic.Add(instance.GetInstanceID(), pool);
    }
}

