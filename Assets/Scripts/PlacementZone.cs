using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementZone : MonoBehaviour
{
    public static event Action<Transform> OnCanPlace;

    bool _hasTower = false;
    bool _isTowerOver = false;

    private void OnEnable()
    {
        Tower.OnPlaced += HasTower;
    }
    public void CanPlace()
    {
        if (!_hasTower)
        {
            OnCanPlace?.Invoke(this.transform);
            _isTowerOver = true;
        }
    }
    
    public void HasTower()
    {
        if (_isTowerOver)
            _hasTower = true;
    }
}
