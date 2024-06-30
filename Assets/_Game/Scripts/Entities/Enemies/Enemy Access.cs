using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAccess : MonoBehaviour
{
    public Vector3 Position { get { return transform.position; } }
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

    public bool CanSee(EnemyAccess ea)
    {
        Vector3 direction = (ea.Position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            EnemyAccess hitEA = hit.transform.GetComponent<EnemyAccess>();
            return hitEA == ea;
        }
        return false;
    }
}
