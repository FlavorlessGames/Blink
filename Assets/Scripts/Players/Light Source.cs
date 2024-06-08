using UnityEngine;

public class LightSource : MonoBehaviour
{
    [SerializeField] private Light _light;
    void Update()
    { 
        lightRayCast();
    }

    // Todo: Separate this so it can apply to other light sources
    private void lightRayCast()
    {
        if (!_light.enabled) return;
        if (EntityManager.Instance == null) return;
        foreach(Vector3 enemyPosition in EntityManager.Instance.GetEnemyPositions())
        {
            detectWithLight(enemyPosition);
        }
    }

    private void detectWithLight(Vector3 enemyPosition)
    {
        if (!inFlashlightCone(enemyPosition)) return;
        LightDetection detectable = rayCastCheck(enemyPosition);
        if (detectable == null) return;
        detectable.Spotted();
    }

    private bool inFlashlightCone(Vector3 enemyPosition)
    {
        if (Vector3.Distance(_light.transform.position, enemyPosition) > _light.range) return false;
        Vector3 point1 = _light.transform.forward;
        Vector3 point2 = enemyPosition - _light.transform.position;
        if (Vector3.Angle(point1, point2) > _light.spotAngle / 2f) return false;
        return true;
    }

    private LightDetection rayCastCheck(Vector3 enemyPosition)
    {
        Vector3 direction = (enemyPosition - _light.transform.position).normalized;
        Ray ray = new Ray(_light.transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            LightDetection lightDetection = hit.transform.GetComponent<LightDetection>();
            return lightDetection;
        }
        return null;
    }

}