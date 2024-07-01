using UnityEngine;

[RequireComponent(typeof(DormantBehavior))]
public class AwakenIfSeen : MonoBehaviour
{
    [Range(1, 100)]
    [SerializeField] private int _timesSeenTillAwaken = 1;
    private int _seenCounter = 0;
    private DormantBehavior dormantBehavior;
    private bool _averted = true;

    void Start()
    {
        LightDetection lightDetection = GetComponent<LightDetection>();
        lightDetection.SpottedEvent += seen;
        lightDetection.EyesAvertedEvent += averted;
        dormantBehavior = GetComponent<DormantBehavior>();
    }

    private void seen()
    {
        if (!_averted) return;

        _seenCounter += 1;
        _averted = false;

        if (_seenCounter < _timesSeenTillAwaken) return;

        dormantBehavior.Awaken();
    }

    private void averted()
    {
        _averted = true;
    }
}