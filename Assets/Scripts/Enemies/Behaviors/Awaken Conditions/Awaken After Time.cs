using UnityEngine;

[RequireComponent(typeof(DormantBehavior))]
public class AwakenAfterTime : MonoBehaviour
{
    [Range(1, 1000)]
    [SerializeField] private int _awakenTime = 60;
    private float _timer = 0f;
    private bool _awake = false;

    void Start()
    {
    }

    void Update()
    {
        checkTime();
    }

    private void checkTime()
    {
        _timer += Time.deltaTime;

        if (_timer < _awakenTime) return;
        if (_awake) return;

        DormantBehavior dormantBehavior = GetComponent<DormantBehavior>();

        _awake = true;
        dormantBehavior.Awaken();
    }
}