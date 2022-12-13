using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Factories
{
    public class AmmunitionFactory : MonoBehaviour
    {
        [SerializeField] private float _price;
        [SerializeField] private bool _purchased;
        [SerializeField] private GameObject _purchasePlace;
        [SerializeField] private GameObject _factoryModel;
        [SerializeField] private Transform _resourcesRootTransform;
        [SerializeField] private Transform _resourcesPlaces;
        [SerializeField] private List<Transform> _resourcesPlacesList;
        [SerializeField] private List<GameObject> _availableResources;

        [Header("Capacity")] [SerializeField] private int _maxResourcesNumber;

        [Header("Resource")] [SerializeField] private GameObject _resourcePrefab;
        [SerializeField] private Vector3 _resourcePositionOffset;
        [SerializeField] private Vector3 _resourceRotationOffset;
        [SerializeField] private Vector3 _resourceScaleOffset;

        [Header("Info")] private bool _productionStarted;

        public void Inject()
        {
            if (_purchased)
            {
                ActivateFactory();
            }
        }

        public void PurchaseFactory()
        {
            _purchased = true;
            _purchasePlace.SetActive(false);
            _factoryModel.SetActive(true);

            ActivateFactory();
        }

        public void ActivateFactory()
        {
            _purchasePlace.SetActive(false);
            _factoryModel.SetActive(true);

            InitialResourcesPlaces();
            StartProduction();
        }

        public void IssueResource()
        {
            var targetResource = AvailableResources[^1];
            Destroy(targetResource);

            AvailableResources.RemoveAt(AvailableResources.Count - 1);
        }

        private void InitialResourcesPlaces()
        {
            for (var i = 0; i < _resourcesPlaces.childCount; i++)
            {
                _resourcesPlacesList.Add(_resourcesPlaces.GetChild(i));
            }

            _maxResourcesNumber = _resourcesPlacesList.Count;
        }

        private void StartProduction()
        {
            _productionStarted = true;

            StartCoroutine(StartProductionEnumerator());
        }

        private void AddResource()
        {
            var newResourceGameObject = Instantiate(_resourcePrefab, _resourcesRootTransform);
            newResourceGameObject.transform.position = _resourcesPlacesList[AvailableResources.Count].position;

            AvailableResources.Add(newResourceGameObject);
        }

        private IEnumerator StartProductionEnumerator()
        {
            while (true)
            {
                if (AvailableResources.Count < _maxResourcesNumber)
                {
                    AddResource();
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        public List<GameObject> AvailableResources => _availableResources;

        public float Price
        {
            get => _price;
            set => _price = value;
        }
    }
}