using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterviewTest.MiniGame
{
    public class Obstacle : MonoBehaviour
    {
        private void OnEnable()
        {
            Bullet.OnBulletPosition += CheckIfHit;
        }

        private void CheckIfHit(GameObject bullet)
        {
            float bulletX = bullet.transform.position.x;
            float bulletY = bullet.transform.position.y;
            float thisX = transform.position.x;
            float thisY = transform.position.y;

            //check bullet fall within bounds if so destroy bullet
            if (bulletY >= thisY - 60 && bulletY <= thisY + 60 && bulletX >= thisX - 60 && bulletX <= thisX + 60)
            {
                Destroy(bullet);
            }
        }

        private void OnDisable()
        {
            Bullet.OnBulletPosition -= CheckIfHit;
        }
    }
}