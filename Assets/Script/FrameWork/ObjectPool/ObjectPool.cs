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

        // ����������
        _poolContainer = new GameObject($"{prefab.name} Pool").transform;
        _poolContainer.SetParent(_manager.transform);

        // Ԥ��������
        Prewarm(initialSize);
    }

    /// <summary>
    /// Ԥ��������
    /// </summary>
    /// <param name="count">������Ŀ</param>
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
    /// �����¶���
    /// </summary>
    /// <returns></returns>
    private GameObject CreateNewObject()
    {
        GameObject obj = Object.Instantiate(_prefab, _poolContainer);
        obj.name = $"{_prefab.name} (Pooled)";

        // ��ӳض������
        PooledObject pooledObj = obj.AddComponent<PooledObject>();
        pooledObj.SetPool(this);

        // ע�ᵽ������
        _manager.RegisterInstance(obj, this);

        return obj;
    }

    /// <summary>
    /// �ӳ��л�ȡ����
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
            // ����û�п��ö��󣬼���Ƿ�ﵽ�������
            if (_maxSize > 0 && TotalCount >= _maxSize)
            {
                Debug.LogWarning($"Object pool for {_prefab.name} reached max size ({_maxSize})");
                return null;
            }

            obj = CreateNewObject();
        }

        obj.SetActive(true);
        ActiveCount++;

        // ���ͻ����¼�
        IPoolable[] poolables = obj.GetComponentsInChildren<IPoolable>();
        foreach (var poolable in poolables)
        {
            poolable.OnWake();
        }

        return obj;
    }

    /// <summary>
    /// ���ն��󵽳���
    /// </summary>
    public void Release(GameObject obj)
    {
        // ���ͻ����¼�
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
    /// ��ն����
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
