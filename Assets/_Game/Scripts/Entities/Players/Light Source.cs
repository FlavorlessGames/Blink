using UnityEngine;

public class LightSource : MonoBehaviour
{
    [SerializeField] private Light _light;
    void Update()
    { 
        switch (_light.type)
        {
            case LightType.Spot:
                lightRayCast();
                break;
            case LightType.Point:
                pointLight();
                break;
            default:
                Debug.LogError("This Light Type is not supported");
                return;
        }
    }

    // Todo: refactor for cleaner code
    
    private void pointLight()
    {
        if (!_light.enabled) return;
        if (EntityManager.Instance == null) return;
        foreach(Vector3 enemyPosition in EntityManager.Instance.GetEnemyPositions())
        {
            detectWithLightPoint(enemyPosition);
        }
    }

    private void detectWithLightPoint(Vector3 enemyPosition)
    {
        if (!Utility.InRange(transform.position, enemyPosition, _light.range)) return;
        LightDetection detectable = hasLineOfSight(enemyPosition);
        if (detectable == null) return;
        detectable.Spotted();
    }

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
        LightDetection detectable = hasLineOfSight(enemyPosition);
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

    private LightDetection hasLineOfSight(Vector3 enemyPosition)
    {
        Vector3 direction = (enemyPosition - _light.transform.position).normalized;
        LineOfSightDetection losd = rayCastLineOfSight(direction);
        return losd.Entity;
    }

    private LineOfSightDetection rayCastLineOfSight(Vector3 direction)
    {
        RaycastHit[] hits;
        LineOfSightDetection losd = new LineOfSightDetection(false, false);
        hits = Physics.RaycastAll(_light.transform.position, direction, _light.range);
        for (int i=0; i<hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            LightDetection ld = hit.transform.GetComponent<LightDetection>();

            if (ld != null)
            {
                losd.Entity = ld;
                losd.InBounds = true;
                losd.CanSee = i == 0;
                return losd;
            }
        }
        return losd;
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

    private struct LineOfSightDetection
    {
        public bool CanSee;
        public bool InBounds;
        public LightDetection Entity;

        public LineOfSightDetection(bool canSee, bool inBounds)
        {
            this.CanSee = canSee;
            this.InBounds = inBounds;
            this.Entity = null;
        }

        public override string ToString()
        {
            string s = CanSee ? "Can" : "Can Not";
            string b = InBounds ? "In" : "Out of";
            return string.Format("{0} See, {1} Bounds", s, b);
        }
    }
}