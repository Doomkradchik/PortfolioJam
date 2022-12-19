using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMono<T> where T: MonoBehaviour
{
    public T Prefab { get; }
    public Transform Root { get; } = null;

    public bool IsAutoExpand { get; set; }

    private List<T> _pool;

    public T getFreeElement
    {
        get
        {
            if (HasFreeElement(out T element))
                return element;

            if (IsAutoExpand)
                return CreateObject(true);

            throw new System.InvalidOperationException($"No more free " +
                $"elements contain in {_pool}. Need to expand pool.");
        }
    }


    public PoolMono(T prefab, int count)
    {
        Prefab = prefab;
        CreatePool(count);
    }

    public PoolMono(T prefab, int count, Transform root)
    {
        Prefab = prefab;
        Root = root;
        CreatePool(count);
    }

    private void CreatePool(int count)
    {
        _pool = new List<T>();

        for (int i = 0; i < count; i++)
        {
            CreateObject();
        }
    }

    private T CreateObject(bool isActiveByDefault = false)
    {
        var newObject = Object.Instantiate(Prefab, Root);

        newObject.gameObject.SetActive(isActiveByDefault);
        _pool.Add(newObject);

        return newObject;
    }

    private bool HasFreeElement(out T element)
    {
        foreach(var mono in _pool)
        {
            if(mono.gameObject.activeInHierarchy == false)
            {
                element = mono;
                return true;
            }
        }

        element = null;
        return false;
    }
}
