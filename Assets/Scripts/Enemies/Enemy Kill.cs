using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{
    [SerializeField] private float _killRange = 3f;
    private bool _stopped;

    void Start()
    {
        LightDetection lightDetection = GetComponent<LightDetection>();
        if (lightDetection == null) return;
        lightDetection.SpottedEvent += stop;
        lightDetection.EyesAvertedEvent += resume;
    }
    
    void Update()
    {
        kill();
    }

    private void stop()
    {
        _stopped = true;
    }

    private void resume()
    {
        _stopped = false;
    }

    private void kill()
    {
        if (_stopped) return;
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
            if (player != null) player.Kill();
        }
    }


    public delegate void GenericHandler();
}
