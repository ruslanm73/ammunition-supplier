using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Enemies
{
    public class EnemiesController : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject enemyPool;
        [SerializeField] private bool spawnAvailable;
        [SerializeField] private List<GameObject> enemiesGameObjects;

        public void Inject()
        {
            spawnAvailable = true;

            StartCoroutine(EnemySpawnEnumerator());
        }

        public void CreateEnemy()
        {
            var newEnemyGameObject = Instantiate(enemyPrefab, enemyPool.transform);
            newEnemyGameObject.transform.localPosition = GetRandomPosition();

            var newEnemyMonoBehaviour = newEnemyGameObject.GetComponent<EnemyMonoBehaviour>();
            newEnemyMonoBehaviour.Inject(this);

            EnemiesGameObjects.Add(newEnemyGameObject);
        }

        private IEnumerator EnemySpawnEnumerator()
        {
            while (spawnAvailable)
            {
                CreateEnemy();

                yield return new WaitForSeconds(5f);
            }
        }

        private static Vector3 GetRandomPosition()
        {
            var randomX = Random.Range(-4f, 4f);
            var randomZ = Random.Range(23f, 15f);
            var randomPosition = new Vector3(randomX, 0f, randomZ);

            return randomPosition;
        }

        public List<GameObject> EnemiesGameObjects
        {
            get => enemiesGameObjects;
            set => enemiesGameObjects = value;
        }
    }
}