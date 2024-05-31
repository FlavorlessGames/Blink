using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccess : MonoBehaviour
{
    // Update is called once per frame
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
    }
}
