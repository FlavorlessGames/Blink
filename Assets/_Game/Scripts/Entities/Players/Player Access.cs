using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccess : MonoBehaviour
{
    public Vector3 Position { get { return transform.position; } }
    // Update is called once per frame
    void Start()
    {
        EntityManager.Instance.AddLiving(this);
    }

    void Update()
    {
        updatePostion();
    }

    public void updatePostion()
    {
        if (EntityManager.Instance == null) return;
        EntityManager.Instance.UpdatePosition(this, transform.position);
    }

    public void Kill()
    {
        Debug.Log("Killed");
        EntityManager.Instance.PlayerKilled(this);
    }
}
