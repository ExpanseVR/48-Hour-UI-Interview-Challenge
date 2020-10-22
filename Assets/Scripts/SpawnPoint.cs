using UnityEngine;

namespace InterviewTest.Scripts
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private MiniGameManager.SpawnPoint _spawnPointType;

        public MiniGameManager.SpawnPoint GetSpawnPointType ()
        {
            return _spawnPointType;
        }
    }
}