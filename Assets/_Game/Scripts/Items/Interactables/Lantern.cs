using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.Collections.Generic;

public class Lantern : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private NavMeshModifierVolume _modifier;
    [SerializeField] private bool _on;
    [Range(1f, 300f)]
    [SerializeField] private float _illuminateDuration = 1f;
    private float _timer = 0f;
    private int _id;
    private static List<bool> _lanternStates;
    public static string State { get { return GetState(); } }

    void Awake()
    {
        if (_lanternStates == null)
        {
            _lanternStates = new List<bool>();
        }

        _id = _lanternStates.Count;
        _lanternStates.Add(_on);
    }

    void Start()
    {
        FocusBeamReceiver receiver = GetComponent<FocusBeamReceiver>();
        receiver.FullChargeEvent += on;
    }

    void Update()
    {
        if (_timer <= 0f) return;
        _timer -= Time.deltaTime;

        if (_timer > 0f) return;
        off();
        _timer = 0f;

    }

    public void OnOffSwitch()
    {
        switch (_on)
        {
            case true:
                off();
                break;
            case false:
                on();
                break;
        }
    }

    private void on()
    {
        _on = true;
        _timer = _illuminateDuration;
        _lanternStates[_id] = _on;
        _light.enabled = true;
        _modifier.area = NavMesh.GetAreaFromName("Illuminated");
        NavMeshManager.Instance.Reload();
    }

    private void off()
    {
        _on = false;
        _lanternStates[_id] = _on;
        _light.enabled = false;
        _modifier.area = NavMesh.GetAreaFromName("Dark");
        NavMeshManager.Instance.Reload();
    }

    private static string GetState()
    {
        string stateString = "";
        for (int i=0; i<_lanternStates.Count; i++)
        {
            stateString = string.Concat(
                stateString, 
                string.Format("|{0}:{1}|", i, _lanternStates[i])
            );
        }
        return stateString;
    }
}