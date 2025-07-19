using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject _prefab;
    private int _maxSize;
    private Stack<GameObject> _inactiveObjects = new Stack<GameObject>();
    private Transform _poolContainer;
    private ObjectPoolManager _manager;

    public int ActiveCount { get; private set; }
    public int InactiveCount => _inactiveObjects.Count;
    public int TotalCount => ActiveCount + InactiveCount;

    public ObjectPool(GameObject prefab, int initialSize, int maxSize, ObjectPoolManager manager)
    {
        _prefab = prefab;
        _maxSize = maxSize;
        _manager = manager;

        // 创建池容器
        _poolContainer = new GameObject($"{prefab.name} Pool").transform;
        _poolContainer.SetParent(_manager.transform);

        // 预创建对象
        Prewarm(initialSize);
    }

    /// <summary>
    /// 预创建对象
    /// </summary>
    /// <param name="count">对象数目</param>
    private void Prewarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = CreateNewObject();
            obj.SetActive(false);
            _inactiveObjects.Push(obj);
        }
    }

    /// <summary>
    /// 创建新对象
    /// </summary>
    /// <returns></returns>
    private GameObject CreateNewObject()
    {
        GameObject obj = Object.Instantiate(_prefab, _poolContainer);
        obj.name = $"{_prefab.name} (Pooled)";

        // 添加池对象组件
        PooledObject pooledObj = obj.AddComponent<PooledObject>();
        pooledObj.SetPool(this);

        // 注册到管理器
        _manager.RegisterInstance(obj, this);

        return obj;
    }

    /// <summary>
    /// 从池中获取对象
    /// </summary>
    public GameObject Get()
    {
        GameObject obj;

        if (_inactiveObjects.Count > 0)
        {
            obj = _inactiveObjects.Pop();
        }
        else
        {
            // 池中没有可用对象，检查是否达到最大限制
            if (_maxSize > 0 && TotalCount >= _maxSize)
            {
                Debug.LogWarning($"Object pool for {_prefab.name} reached max size ({_maxSize})");
                return null;
            }

            obj = CreateNewObject();
        }

        obj.SetActive(true);
        ActiveCount++;

        // 发送唤醒事件
        IPoolable[] poolables = obj.GetComponentsInChildren<IPoolable>();
        foreach (var poolable in poolables)
        {
            poolable.OnWake();
        }

        return obj;
    }

    /// <summary>
    /// 回收对象到池中
    /// </summary>
    public void Release(GameObject obj)
    {
        // 发送回收事件
        IPoolable[] poolables = obj.GetComponentsInChildren<IPoolable>();
        foreach (var poolable in poolables)
        {
            poolable.OnSleep();
        }

        obj.SetActive(false);
        obj.transform.SetParent(_poolContainer);
        obj.transform.localPosition = Vector3.zero;

        _inactiveObjects.Push(obj);
        ActiveCount--;
    }

    /// <summary>
    /// 清空对象池
    /// </summary>
    public void Clear()
    {
        while (_inactiveObjects.Count > 0)
        {
            GameObject obj = _inactiveObjects.Pop();
            if (obj != null)
            {
                Object.Destroy(obj);
            }
        }
    }
}
