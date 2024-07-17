using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatueTrap))]
[RequireComponent(typeof(Lock))]
public class StatueTrapLock : MonoBehaviour
{
    private Lock _lock;
    // Start is called before the first frame update
    void Start()
    {
        _lock = GetComponent<Lock>();
        StatueTrap st = GetComponent<StatueTrap>();
        st.TrapTriggered += open;
    }

    void open()
    {
        _lock.Open();
    }
}
