using UnityEngine;
using System;

public class FocusBeamReceiver : MonoBehaviour
{
    public float Range { get { return _range; } }
    public float ChargePercentage { get { return getChargePercent(); } }
    public float SinceTargeted { get { return Time.time - _targeted; } }
    [Range(1f, 100f)]
    [SerializeField] private float _range = 10f;
    [Range(.01f, 5f)]
    [SerializeField] private float _chargeMax;
    [Range(.01f, 2f)]
    [SerializeField] private float _decayDelay;
    private float _targeted;
    private float _charge;

    void Start()
    {
        _targeted = 0f;
        _charge = 0f;
    }

    void Update()
    {
        decay();
    }

    public void Target()
    {
        _targeted = Time.time;
        _charge += Time.deltaTime;
        _charge = Math.Min(_chargeMax, _charge);
    }

    private void decay()
    {
        if (SinceTargeted < _decayDelay) return;

        _charge -= Time.deltaTime;
        if (_charge < 0) _charge = 0f;
    }

    private float getChargePercent()
    {
        return Math.Min(100f, _charge/_chargeMax) * 100f;
    }
}