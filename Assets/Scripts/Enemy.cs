using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<int, GameObject, bool> OnDestroyedEnemy;

    [SerializeField]
    int _health = 1;

    [SerializeField]
    int _pointValue = 5;

    [SerializeField]
    Rigidbody2D _rigidBody;

    [SerializeField]
    int _enemySpeed = 700;

    [SerializeField]
    ParticleSystem _explosion;

    private Vector2 _startDirection;
    bool _isDead = false;

    private void OnEnable()
    {
        MiniGameManager.OnReset += Destroyed;
        Bullet.OnBulletPosition += CheckIfHit;   
    }

    private void Update()
    {
        if (transform.position.x < 190 || transform.position.x > 1320 || transform.position.y < 410 || transform.position.y > 1140)
        {
            Destroyed(false);
        }
    }

    public void CheckIfHit (GameObject bullet)
    {
        if (_isDead)
            return;

        float bulletX = bullet.transform.position.x;
        float bulletY = bullet.transform.position.y;
        float thisX = transform.position.x;
        float thisY = transform.position.y;

        //check bullet fall within bounds
        if (bulletY >= thisY - 80 && bulletY <= thisY + 80 && bulletX >= thisX - 80 && bulletX <= thisX + 80)
        {
            TakeDamage();
            Destroy(bullet);
        }
    }

    private void TakeDamage ()
    {
        _health--;
        if (_health <= 0)
            StartCoroutine(DestroyWithExplosion());
    }

    public void SetDirection (MiniGameManager.SpawnPoint startSide)
    {
        switch(startSide)
        {
            case MiniGameManager.SpawnPoint.Top:
                _startDirection = new Vector2(0, -1);
                break;
            case MiniGameManager.SpawnPoint.Bottom:
                _startDirection = new Vector2(0, 1);
                break;
            case MiniGameManager.SpawnPoint.Left:
                _startDirection = new Vector2(1, 0);
                break;
            default:
                _startDirection = new Vector2(-1, -0);
                break;
        }

        _rigidBody.AddForce(_startDirection * _enemySpeed);
    }


    private void Destroyed (bool withPoints)
    {
        //signal that it is destroyed
        OnDestroyedEnemy?.Invoke(_pointValue, this.gameObject, withPoints);
        //play animation/effect
        Destroy(this.gameObject);
    }

    IEnumerator DestroyWithExplosion ()
    {
        _isDead = true;
        _rigidBody.velocity = Vector2.zero;
        _explosion.Play();
        OnDestroyedEnemy?.Invoke(_pointValue, this.gameObject, true);
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        Bullet.OnBulletPosition -= CheckIfHit;
        MiniGameManager.OnReset -= Destroyed;
    }
}
