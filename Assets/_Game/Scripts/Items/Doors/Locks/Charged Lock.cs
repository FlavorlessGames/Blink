using UnityEngine;

[RequireComponent(typeof(FocusBeamReceiver))]
[RequireComponent(typeof(Lock))]
public class ChargedLock : MonoBehaviour
{
    private Lock _lock;
    void Start()
    {
        _lock = GetComponent<Lock>();
        FocusBeamReceiver rec = GetComponent<FocusBeamReceiver>();
        rec.FullChargeEvent += lockOpen;
    } 
    
    private void lockOpen()
    {
        _lock.Open();
    }
}