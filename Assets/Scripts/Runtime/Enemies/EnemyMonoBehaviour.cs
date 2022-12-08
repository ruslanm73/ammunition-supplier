using System;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Enemies
{
    public class EnemyMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        [SerializeField] private float startHealth;
        [SerializeField] private float currentHealth;
        [SerializeField] private GameObject lootGameObject;

        private EnemiesController _enemiesController;
        private NavMeshAgent _navMeshAgent;

        public void Inject(EnemiesController enemiesController)
        {
            _enemiesController = enemiesController;
            target = GameObject.Find("Turret[1]");
            _navMeshAgent = GetComponent<NavMeshAgent>();

            CurrentHealth = StartHealth;
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                _navMeshAgent.SetDestination(target.transform.position);
            }
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                DestroyEnemy();
            }
        }

        private void DestroyEnemy()
        {
            var newLootGameObject = Instantiate(lootGameObject);
            newLootGameObject.name = "LootMoney";
            newLootGameObject.transform.position = transform.position;

            Destroy(gameObject);
        }

        public float StartHealth
        {
            get => startHealth;
            set => startHealth = value;
        }

        public float CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }
    }
}