using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatueBase))]
public class EnemyKill : MonoBehaviour
{
    [SerializeField] private float _killRange = 3f;
    private StatueBase _base;


    void Start()
    {
        _base = GetComponent<StatueBase>();
    }
    
    void Update()
    {
        kill();
    }

    private void kill()
    {
        if (_base.Stopped) return;
        foreach (Vector3 player in EntityManager.Instance.GetPlayerPositions())
        {
            if (!Utility.InRange(transform.position, player, _killRange)) continue;
            raycastKill(player);
        }

    }

    private void raycastKill(Vector3 playerPosition)
    {
        Vector3 direction = (playerPosition - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _killRange))
        {
            PlayerAccess player = hit.transform.GetComponent<PlayerAccess>();
            if (player != null) killPlayer(player);
        }
    }

    private void killPlayer(PlayerAccess pa)
    {
        pa.Kill(); 
        EntityManager.Instance.ClearTarget(GetComponent<EnemyAccess>());
        gameObject.SetActive(false); // Todo: remove and add real death behavior
    }


    public delegate void GenericHandler();
}
