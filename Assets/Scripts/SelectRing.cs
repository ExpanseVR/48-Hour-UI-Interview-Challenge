using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectRing : MonoBehaviour
{
    [SerializeField]
    Material _material;

    [SerializeField]
    private float _revealSpeed = 0.5f;

    [Range(5,25)][SerializeField]
    private int _rotationSpeed = 10;

    [SerializeField]
    private RawImage _directionArrows;

    [SerializeField]
    private Button _buttonClockwise, _buttonCounter;

    private bool _isPlaying = false;
    private Transform _currentTower;

    private void OnEnable()
    {
        Tower.OnSelected += MoveSelection;
        Tower.OnSelectRingReset += RingStatus;
        RingStatus(false);
    }

    IEnumerator ExposeRing()
    {
        //reset ring value
        float progress = -3.3f;
        _directionArrows.gameObject.SetActive(false);

        //loop through value until exposed
        while (progress < 1)
        {
            _material.SetFloat("_RingExpose", progress);
            progress += Time.deltaTime * _revealSpeed;
            yield return null;
        }
        _isPlaying = false;
        RingStatus(true);
    }

    private void RingStatus(bool status)
    {
        _directionArrows.gameObject.SetActive(status);
        _buttonClockwise.gameObject.SetActive(status);
        _buttonCounter.gameObject.SetActive(status);
        if (!status)
            _material.SetFloat("_RingExpose", -3.3f);
    }

    public void ArrowPressed (bool clockwise)
    {
        if (clockwise)
            _currentTower.transform.Rotate(0, 0, -_rotationSpeed, Space.Self);
        else
            _currentTower.transform.Rotate(0, 0, _rotationSpeed, Space.Self);
    }

    public void MoveSelection (Transform target)
    {
        _currentTower = target;
        this.transform.position = _currentTower.position;

        //start ring shader animation
        if (_isPlaying)
            StopCoroutine("ExposeRing"); //JON <--- better way to stop/start a coroutine?

        _isPlaying = true;
        StartCoroutine("ExposeRing");
    }

    private void OnDisable()
    {
        Tower.OnSelected -= MoveSelection;
        Tower.OnSelectRingReset -= RingStatus;
    }
}
