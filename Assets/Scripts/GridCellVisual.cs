using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellVisual : MonoBehaviour
{
    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _selected;
    
    private bool _isSelected;
    private bool _isLastSelectedValue = false;
    
    public void SetSelected(bool value)
    {
        _isSelected = value;
    }
    
    private void Start()
    {
        _isSelected = false;
        _isLastSelectedValue = false;
        
        UpdateSelected();
    }
    
    private void Update()
    {
        if (_isLastSelectedValue != _isSelected)
        {
            UpdateSelected();
            _isLastSelectedValue = _isSelected;
        }
    }
    
    private void UpdateSelected()
    {
        _visual.SetActive(_isSelected ? false : true);
        _selected.SetActive(_isSelected ? true : false);   
    }
}
