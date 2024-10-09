
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatueTrap))]
[RequireComponent(typeof(DoorSwitch))]
public class TrapSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StatueTrap st = GetComponent<StatueTrap>();
        st.TrapTriggered += open;
    }

    void open()
    {
        DoorSwitch ds = GetComponent<DoorSwitch>();
        ds.OpenSwitch();
    }
}
