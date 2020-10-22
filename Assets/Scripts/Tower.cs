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
    private Transform _towerHolder;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private float _firingRate;

    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private Transform _spawnPoint, _bulletParent;

    private Transform _placementZone;

    private bool _isPlaced = false;
    private bool _isheld = false;
    private bool _cantBeHeld = false;

    private void OnEnable()
    {
        PlacementZone.OnCanPlace += OverPlacementZone;
        MiniGameManager.OnIsRoundCommenced += HasRoundStarted;
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
                OnHeld?.Invoke(false);
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
            OnSelected?.Invoke(this.transform);
        else
        {
            transform.position = _towerHolder.position;
            _isheld = !_isheld;
            if (_isheld)
            {
                _image.raycastTarget = false;
                OnHeld(true);
                OnSelectRingReset?.Invoke(false);
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
            OnPlaced?.Invoke();
            OnHeld(false);
        }
    }

    public void HasRoundStarted(bool hasStarted)
    {
        if (hasStarted)
            StartCoroutine("Firing");
        else
            StopCoroutine("Firing");
    }

    public void NotHeld (bool isHeld)
    {
        if (!_isheld)
        {
            _cantBeHeld = isHeld;
        }
    }

    IEnumerator Firing()
    {
        while (true & _isPlaced)
        {
            var newBullet = Instantiate(_bullet, _spawnPoint.position, Quaternion.identity);
            newBullet.transform.SetParent(_bulletParent.transform);
            newBullet.GetComponent<Rigidbody2D>().AddForce(this.transform.up * 15000);
            yield return new WaitForSeconds(_firingRate);
        }
    }

    private void OnDisable()
    {
        PlacementZone.OnCanPlace -= OverPlacementZone;
        MiniGameManager.OnIsRoundCommenced -= HasRoundStarted;
    }
}
