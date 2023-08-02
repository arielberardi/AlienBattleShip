using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectVisual : MonoBehaviour
{
    public bool isSelected { get; set; }

    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _selected;
    
    private bool _lastSelectedValue;
    
    private void Start()
    {
        isSelected = false;
        _lastSelectedValue = false;
        
        UpdateSelected();
    }
    
    private void Update()
    {
        if (_lastSelectedValue != isSelected)
        {
            UpdateSelected();
            _lastSelectedValue = isSelected;
        }
    }
    
    private void UpdateSelected()
    {
        _visual.SetActive(!isSelected);
        _selected.SetActive(isSelected);   
    }
}
