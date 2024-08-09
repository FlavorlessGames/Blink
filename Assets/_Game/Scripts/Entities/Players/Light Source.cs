using UnityEngine;
using System.Collections.Generic;

public class LightSource : MonoBehaviour
{
    [SerializeField] private Light _light;
    private const float c_incrementAngle = .2f;
    private const int c_checkedAngles = 30;
    public bool Visualize = false;
    private List<Vector3> queuedDirections = new List<Vector3>();
    void Update()
    { 
        if (Visualize) queuedDirections.Clear();
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
        // Debug.Log(string.Format("direct: {0}", losd));
        if (losd.CanSee) return losd.Entity;
        losd = checkFOV(direction);
        // Debug.Log(string.Format("fov: {0}", losd));
        return losd.Entity;
    }
    
    private LineOfSightDetection checkFOV(Vector3 direction)
    {
        Vector3 [] axes = {
            Vector3.up,
            Vector3.down,
            Vector3.right,
            Vector3.left,
        };
        foreach (Vector3 axis in axes)
        {
            LineOfSightDetection losd = spreadDetection(direction, c_incrementAngle, axis);
            if (losd.CanSee) return losd;
        }
        return new LineOfSightDetection(false, false);
    }

    private LineOfSightDetection spreadDetection(Vector3 direction, float increment, Vector3 axis)
    {
        for (int i=1; i<c_checkedAngles; i++)
        {
            float angle = increment * i;
            Vector3 newDirection = Quaternion.AngleAxis(angle, axis) * direction;
            Debug.Log(string.Format("newDirection {0}", newDirection));
            LineOfSightDetection losd = rayCastLineOfSight(newDirection);
            if (losd.CanSee || !losd.InBounds) return losd;
        }
        return new LineOfSightDetection(false, false);
    }

    void OnDrawGizmos()
    {
        if (!Visualize) return;
        foreach (var direction in queuedDirections)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_light.transform.position, _light.transform.position + direction * _light.range);
        }
    }

    private LineOfSightDetection rayCastLineOfSight(Vector3 direction)
    {
        if (Visualize) queuedDirections.Add(direction);
        RaycastHit[] hits;
        LineOfSightDetection losd = new LineOfSightDetection(false, false);
        losd.Entity = rayCastCheck(direction);
        if (losd.Entity != null)
        {
            losd.InBounds = losd.CanSee = true;
            return losd;
        }
        hits = Physics.RaycastAll(_light.transform.position, direction, _light.range);
        for (int i=0; i<hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            LightDetection ld = hit.transform.GetComponent<LightDetection>();

            if (ld != null)
            {
                losd.InBounds = true;
                return losd;
            }
        }
        return losd;
    }

    private LightDetection rayCastCheck(Vector3 direction)
    {
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