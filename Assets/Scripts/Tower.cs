using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public static event Action<Transform> OnSelected;
    public static event Action OnPlaced;
    public static event Action<bool> OnHeld;
    public static event Action<bool> OnSelectRingReset;

    [SerializeField]
    Transform _towerHolder;

    [SerializeField]
    Image _image;

    Transform _placementZone;

    bool _isPlaced = false;
    bool _isheld = false;
    bool _cantBeHeld = false;

    private void OnEnable()
    {
        PlacementZone.OnCanPlace += OverPlacementZone;
        Tower.OnHeld += NotHeld;
    }

    private void Update()
    {
        if (_isheld)
        {
            transform.position = Input.mousePosition;
            if (Input.GetMouseButtonDown(1))
            {
                TankSelected();
                OnHeld(false);
            }
            //check if over place zone
            //if so change color to reflect placeable
        }
    }
    public void TankSelected()
    {
        if (_cantBeHeld)
            return;

        if (_isPlaced)
            OnSelected(this.transform);
        else
        {
            transform.position = _towerHolder.position;
            _isheld = !_isheld;
            if (_isheld)
            {
                _image.raycastTarget = false;
                OnHeld(true);
                OnSelectRingReset(false);
            }
            else
                _image.raycastTarget = true;
        }

    }

    public void OverPlacementZone (Transform toPlace)
    {
        if (_isheld)
        {
            _image.raycastTarget = true;
            _isheld = false;
            _placementZone = toPlace;
            transform.position = _placementZone.position;
            _isPlaced = true;
            OnPlaced();
            OnHeld(false);
        }
    }

    public void NotHeld (bool isHeld)
    {
        if (!_isheld)
        {
            _cantBeHeld = isHeld;
        }
    }    

    private void OnDisable()
    {
        PlacementZone.OnCanPlace -= OverPlacementZone;
    }
}
