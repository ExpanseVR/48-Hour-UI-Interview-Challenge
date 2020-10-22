using InterviewTest.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterviewTest.Manager
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        [SerializeField]
        private GameObject _enemy;

        [SerializeField]
        private SpawnPoint[] _spawnPoints;
        
        [SerializeField]
        private Transform _enemyContainer;

        [SerializeField]
        private int _enemyWaveCount;

        [SerializeField]
        private float _spawnRate;

        List<GameObject> _enemies = new List<GameObject>();

        private void OnEnable()
        {
            MiniGameManager.OnIsRoundCommenced += StartRound;
            Enemy.OnDestroyedEnemy += RemoveEnemyFromList;
        }

        private void StartRound (bool hasRoundStarted)
        {
            //check to see if round has started or ended
            if (hasRoundStarted)
                StartCoroutine("SpawnEnemies");
            else
                StopCoroutine("SpawnEnemies");
        }

        IEnumerator SpawnEnemies ()
        {
            int count = 0;
            int previousRandomNum = 0;
            while (count < _enemyWaveCount)
            {
                int randomNum = Random.Range(0, _spawnPoints.Length);
                //generate different spot to last one
                while (randomNum == previousRandomNum)
                {
                    randomNum = Random.Range(0, _spawnPoints.Length);
                }
                previousRandomNum = randomNum;

                var newEnemy = Instantiate(_enemy, _spawnPoints[randomNum].transform.position, Quaternion.identity);
                _enemies.Add(newEnemy);
                newEnemy.transform.SetParent(_enemyContainer);
                newEnemy.GetComponent<Enemy>().SetDirection(_spawnPoints[randomNum].GetSpawnPointType());
                yield return new WaitForSeconds(_spawnRate);
            }
        }

        public void RemoveEnemyFromList(int i, GameObject enemy, bool b)
        {
            _enemies.Remove(enemy);
        }

        // Update is called once per frame
        private void OnDisable()
        {
            Enemy.OnDestroyedEnemy -= RemoveEnemyFromList;
            MiniGameManager.OnIsRoundCommenced -= StartRound;
        }
    }
}