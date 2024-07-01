using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavMeshManager : MonoBehaviour
{
    private NavMeshSurface _navMeshSurface;
    public static NavMeshManager Instance;
    private Dictionary<string, NavMeshData> _cachedDatas;
    private NavMeshDataInstance _current;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        _cachedDatas = new Dictionary<string, NavMeshData>();
        _navMeshSurface = GetComponent<NavMeshSurface>();
    }

    // Todo: Try making a caching system for all the versions of the navmesh using Navdata??

    public void Reload()
    {
        removeCurrent();
        if (_cachedDatas.ContainsKey(Lantern.State))
        {
            _current = NavMesh.AddNavMeshData(_cachedDatas[Lantern.State]);
            return;
        }
        _navMeshSurface.BuildNavMesh();
        _cachedDatas[Lantern.State] = _navMeshSurface.navMeshData;
    }

    private void removeCurrent()
    {
        NavMesh.RemoveAllNavMeshData();
        // try
        // {
        //     _current.Remove();
        // }
        // catch
        // {
        //     return;
        // }
    }
}