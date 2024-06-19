using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    [SerializeField] private GameObject _batteryText;
    [SerializeField] private GameObject _batteryPackCount;
    private TMP_Text _textMesh;
    private TMP_Text _textMeshBC;
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
        _textMeshBC = _batteryPackCount.GetComponent<TMP_Text>();
        if (_textMeshBC == null) throw new Exception("Text Mesh Element not found");
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

    public void SetBatteryPackCount(int count)
    {
        _textMeshBC.text = count.ToString();
    }
    
    public void AddBatteryPack()
    {
        Debug.Log("Add");
    }

    public void RemoveBatteryPack()
    {
        Debug.Log("Remove");
    }
}
