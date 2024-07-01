using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance { get; private set; }
    private Dictionary<PlayerAccess, Vector3> _playerPositions;
    private Dictionary<EnemyAccess, Vector3> _enemyPositions;
    private Dictionary<EnemyAccess, PlayerAccess> _targeting;
    private List<PlayerAccess> _livingPlayers;

    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    void Start()
    {
        _livingPlayers = new List<PlayerAccess>();
        _playerPositions = new Dictionary<PlayerAccess, Vector3>();
        _enemyPositions = new Dictionary<EnemyAccess, Vector3>();
        _targeting = new Dictionary<EnemyAccess, PlayerAccess>();
    }

    public void PlayerKilled(PlayerAccess pa)
    {
        if (!_livingPlayers.Contains(pa)) return;
        _livingPlayers.Remove(pa);
        if (_livingPlayers.Count > 0) return;
        GameManager.Instance.AllPlayersKilled();
    }

    public void AddLiving(PlayerAccess pa)
    {
        if (_livingPlayers.Contains(pa)) return;
        _livingPlayers.Add(pa);
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

    public List<PlayerAccess> GetPlayers()
    {
        List<PlayerAccess> pas = new List<PlayerAccess>();
        foreach (PlayerAccess pa in _playerPositions.Keys)
        {
            pas.Add(pa);
        }
        return pas;
    }

    public void RegisterTarget(EnemyAccess ea, PlayerAccess pa)
    {
        _targeting[ea] = pa;
    }

    public void ClearTarget(EnemyAccess ea)
    {
        _targeting.Remove(ea);
    }

    public Vector3 GetPath(EnemyAccess ea)
    {
        PlayerAccess pa = _targeting[ea];
        EnemyAccess closest = getClosestEnemy(pa);
        if (closest == ea) return pa.Position;
        if (!ea.CanSee(closest)) return pa.Position;
        return getIndirectDestination(ea.Position, pa.Position, closest.Position);
    }

    private EnemyAccess getClosestEnemy(PlayerAccess pa)
    {
        EnemyAccess closest = null;
        foreach (EnemyAccess ea in _targeting.Keys)
        {
            if (_targeting[ea] == pa) 
            {
                if (closest == null) closest = ea;
                closest = closerTargeting(closest, pa, ea);
            }
        }
        if (closest == null) throw new Exception("No closes enemy found for this check");
        return closest;
    }

    private EnemyAccess closerTargeting(EnemyAccess current, PlayerAccess pa, EnemyAccess checking)
    {
        float oldDistance = getDistance(pa, current);
        float newDistance = getDistance(pa, checking);
        return newDistance < oldDistance ? checking : current;
    }

    private Vector3 getIndirectDestination(Vector3 from, Vector3 to, Vector3 closerEnemy)
    {
        float opposite = Vector3.Distance(to, closerEnemy);
        float hyp = Vector3.Distance(from, to);
        float distance = (float) Math.Sqrt(hyp * hyp - opposite * opposite); 
        float angle = (float) (Math.Asin(opposite / hyp) * 180 / Math.PI);
        Vector3 toOther = closerEnemy - from;
        Vector3 forwardDirection = (to - from).normalized;
        float angleOfOtherEnemy = Vector3.SignedAngle(forwardDirection, toOther, Vector3.up);
        int negative = angleOfOtherEnemy > 0 ? -1 : 1;
        Vector3 turnedDirection = Quaternion.AngleAxis(angle * negative, Vector3.up) * forwardDirection;

        return from + turnedDirection * distance;
    }

    private float getDistance(PlayerAccess pa, EnemyAccess ea)
    {
        return Vector3.Distance(ea.Position, pa.Position);
    }
}
