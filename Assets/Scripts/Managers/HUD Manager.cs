using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    [SerializeField] private GameObject _batteryText;
    private TMP_Text _textMesh;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null) 
        {
            Destroy(this);
            return;
        }
        Instance = this;
        _textMesh = _batteryText.GetComponent<TMP_Text>();
        if (_textMesh == null) throw new Exception("Text Mesh Element not found");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBatteryLevel(float level)
    {
        SetBatteryLevel((int) level);
    }
    public void SetBatteryLevel(int level)
    {
        _textMesh.text = level.ToString();
    }
}
