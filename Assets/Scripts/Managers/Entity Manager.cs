using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance { get; private set; }
    private Dictionary<PlayerAccess, Vector3> _playerPositions;
    private Dictionary<EnemyAccess, Vector3> _enemyPositions;
    void Start()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
        _playerPositions = new Dictionary<PlayerAccess, Vector3>();
        _enemyPositions = new Dictionary<EnemyAccess, Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePosition(PlayerAccess playerPosition, Vector3 location)
    {
        _playerPositions[playerPosition] = location;
    }

    public void UpdatePosition(EnemyAccess enemyPosition, Vector3 location)
    {
        _enemyPositions[enemyPosition] = location;
    }

    public List<Vector3> GetPlayerPositions()
    {
        List<Vector3> playerPostions = new List<Vector3>();
        foreach (Vector3 value in _playerPositions.Values)
        {
            playerPostions.Add(value);
        }
        return playerPostions;
    }

    public List<Vector3> GetEnemyPositions()
    {
        List<Vector3> enemyPositions = new List<Vector3>();
        foreach (Vector3 value in _enemyPositions.Values)
        {
            enemyPositions.Add(value);
        }
        return enemyPositions;
    }
}
