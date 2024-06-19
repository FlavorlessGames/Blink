using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    [SerializeField] private GameObject _batteryText;
    [SerializeField] private GameObject _batteryPackCount;
    [SerializeField] private UIDocument _chargeBar;
    private TMP_Text _textMesh;
    private TMP_Text _textMeshBC;
    private ProgressBar _chargeDisplay;
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
        _chargeDisplay = _chargeBar.rootVisualElement.Q<ProgressBar>(name: "progress_bar");
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

    public void SetChargeBar(bool show, float level)
    {
        _chargeDisplay.style.visibility = show? Visibility.Visible : Visibility.Hidden;
        _chargeDisplay.value = level * 100;
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
