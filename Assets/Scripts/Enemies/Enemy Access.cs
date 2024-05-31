using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAccess : MonoBehaviour
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
}
