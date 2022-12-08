using System.Collections;
using Runtime.Enemies;
using UnityEngine;

namespace Runtime.Turret
{
    public class TurretMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private float turretPrice;
        [SerializeField] private bool turretPurchased;
        [SerializeField] private GameObject purchaseObject;
        [SerializeField] private GameObject turretModel;

        [Header("Shells")] [SerializeField] private int maxShellsNumbers;
        [SerializeField] private int currentShellsNumber;

        private EnemiesController _enemiesController;
        private GameObject _targetGameObject;
        private IEnumerator _shootingEnumerator;

        public void Inject()
        {
            if (turretPurchased)
            {
                ActivatedTurret();
            }
        }

        public void PurchaseTurret()
        {
            turretPurchased = true;
            purchaseObject.SetActive(false);
            turretModel.SetActive(true);
            ActivatedTurret();
        }

        public void ActivatedTurret()
        {
            purchaseObject.SetActive(false);
            turretModel.SetActive(true);

            _enemiesController = FindObjectOfType<EnemiesController>();
            _shootingEnumerator = ShootingEnumerator();
            StartCoroutine(_shootingEnumerator);
        }

        public void AddShell()
        {
            currentShellsNumber++;
        }

        public bool ShellsFillingAvailable()
        {
            return currentShellsNumber < maxShellsNumbers;
        }

        private void GetTarget()
        {
            if (_targetGameObject == null && currentShellsNumber > 0 && turretPurchased)
            {
                for (var i = 0; i < _enemiesController.EnemiesGameObjects.Count; i++)
                {
                    var currentEnemy = _enemiesController.EnemiesGameObjects[i];
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
            if (_targetGameObject != null && currentShellsNumber > 0 && turretPurchased)
            {
                transform.rotation = Quaternion.LookRotation(_targetGameObject.transform.position - transform.position);
            }
        }

        private IEnumerator ShootingEnumerator()
        {
            while (true)
            {
                if (_targetGameObject != null & currentShellsNumber > 0)
                {
                    if (_targetGameObject.TryGetComponent(out EnemyMonoBehaviour enemyMonoBehaviour))
                    {
                        currentShellsNumber--;

                        enemyMonoBehaviour.TakeDamage(10f);

                        if (enemyMonoBehaviour.CurrentHealth <= 0)
                        {
                            _targetGameObject = default;

                            _enemiesController.EnemiesGameObjects.Remove(enemyMonoBehaviour.gameObject);
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
            get => currentShellsNumber;
            set => currentShellsNumber = value;
        }

        public float TurretPrice
        {
            get => turretPrice;
            set => turretPrice = value;
        }
    }
}