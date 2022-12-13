using System;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Enemies
{
    public class EnemyMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private float _startHealth;
        [SerializeField] private float _currentHealth;
        [SerializeField] private GameObject _lootGameObject;

        private EnemiesService _enemiesService;
        private NavMeshAgent _navMeshAgent;

        public void Inject(EnemiesService enemiesService)
        {
            _enemiesService = enemiesService;
            _target = GameObject.Find("Turret[0]");
            _navMeshAgent = GetComponent<NavMeshAgent>();

            CurrentHealth = StartHealth;
        }

        private void FixedUpdate()
        {
            if (_target != null)
            {
                _navMeshAgent.SetDestination(_target.transform.position);
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
            var newLootGameObject = Instantiate(_lootGameObject);
            newLootGameObject.name = "LootMoney";
            newLootGameObject.transform.position = transform.position;

            Destroy(gameObject);
        }

        public float StartHealth
        {
            get => _startHealth;
            set => _startHealth = value;
        }

        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }
    }
}