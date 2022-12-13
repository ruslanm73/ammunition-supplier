using System.Collections;
using Runtime.Enemies;
using UnityEngine;

namespace Runtime.Turret
{
    public class TurretMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private float _turretPrice;
        [SerializeField] private bool _turretPurchased;
        [SerializeField] private GameObject _purchaseObject;
        [SerializeField] private GameObject _turretModel;

        [Header("Shells")] [SerializeField] private int _maxShellsNumbers;
        [SerializeField] private int _currentShellsNumber;

        private EnemiesService _enemiesService;
        private GameObject _targetGameObject;
        private IEnumerator _shootingEnumerator;

        public void Inject()
        {
            if (_turretPurchased)
            {
                ActivatedTurret();
            }
        }

        public void PurchaseTurret()
        {
            _turretPurchased = true;
            _purchaseObject.SetActive(false);
            _turretModel.SetActive(true);
            ActivatedTurret();
        }

        public void ActivatedTurret()
        {
            _purchaseObject.SetActive(false);
            _turretModel.SetActive(true);

            _enemiesService = FindObjectOfType<EnemiesService>();
            _shootingEnumerator = ShootingEnumerator();
            StartCoroutine(_shootingEnumerator);
        }

        public void AddShell()
        {
            _currentShellsNumber++;
        }

        public bool ShellsFillingAvailable()
        {
            return _currentShellsNumber < _maxShellsNumbers;
        }

        private void GetTarget()
        {
            if (_targetGameObject == null && _currentShellsNumber > 0 && _turretPurchased)
            {
                for (var i = 0; i < _enemiesService.EnemiesGameObjects.Count; i++)
                {
                    var currentEnemy = _enemiesService.EnemiesGameObjects[i];
                    var currentDistance = Vector3.Distance(transform.position, currentEnemy.transform.position);

                    if (currentDistance <= 7f)
                    {
                        _targetGameObject = currentEnemy;
                    }
                }
            }
        }

        private void SetTargetRotation()
        {
            if (_targetGameObject != null && _currentShellsNumber > 0 && _turretPurchased)
            {
                transform.rotation = Quaternion.LookRotation(_targetGameObject.transform.position - transform.position);
            }
        }

        private IEnumerator ShootingEnumerator()
        {
            while (true)
            {
                if (_targetGameObject != null & _currentShellsNumber > 0)
                {
                    if (_targetGameObject.TryGetComponent(out EnemyMonoBehaviour enemyMonoBehaviour))
                    {
                        _currentShellsNumber--;

                        enemyMonoBehaviour.TakeDamage(10f);

                        if (enemyMonoBehaviour.CurrentHealth <= 0)
                        {
                            _targetGameObject = default;

                            _enemiesService.EnemiesGameObjects.Remove(enemyMonoBehaviour.gameObject);
                        }
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        private void FixedUpdate()
        {
            GetTarget();
            SetTargetRotation();
        }

        public int ShellsNumber
        {
            get => _currentShellsNumber;
            set => _currentShellsNumber = value;
        }

        public float TurretPrice
        {
            get => _turretPrice;
            set => _turretPrice = value;
        }
    }
}