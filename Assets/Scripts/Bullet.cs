using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action<GameObject> OnBulletPosition;
    
    [SerializeField]
    float _lifetime;

    [SerializeField]
    int _hitAccuracy = 10;

    private int _counter;

    private void OnEnable()
    {
        MiniGameManager.OnReset += Destroyed;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lifetime());
    }

    private void FixedUpdate()
    {
        _counter++;

        if (_counter % 10 == 0)
            OnBulletPosition?.Invoke(this.gameObject);
    }

    private void Destroyed (bool b)
    {
        if (this.gameObject != null)
            Destroy(this.gameObject);
    }

    IEnumerator Lifetime ()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(this.gameObject);
    }
}
