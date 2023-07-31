using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellVisual : MonoBehaviour
{
    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _selected;
    [SerializeField] private GameObject _attacked;
    
    private bool _isSelected;
    private bool _lastSelectedValue;
    
    private bool _isAttacked;
    private bool _lastAttackedValue;
    
    public void SetSelected(bool value)
    {
        _isSelected = value;
    }
    
    public void SetAttacked(bool value)
    {
        _isAttacked = value;
    }
    
    private void Start()
    {
        _isSelected = false;
        _lastSelectedValue = false;
        
        _isAttacked = false;
        _lastAttackedValue = false;
        
        UpdateSelected();
        UpdateAttacked();
    }
    
    private void Update()
    {
        if (_lastSelectedValue != _isSelected)
        {
            UpdateSelected();
            _lastSelectedValue = _isSelected;
        }
        
        if (_lastAttackedValue != _isAttacked)
        {
            UpdateAttacked();
            _lastAttackedValue = _isAttacked;
        }
    }
    
    private void UpdateSelected()
    {
        _visual.SetActive(!_isSelected);
        _selected.SetActive(_isSelected);   
    }
    
    private void UpdateAttacked()
    {
        _attacked.SetActive(_isAttacked);
    }
}
