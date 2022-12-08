using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Factories
{
    public class AmmunitionFactory : MonoBehaviour
    {
        [SerializeField] private float price;
        [SerializeField] private bool purchased;
        [SerializeField] private GameObject purchasePlace;
        [SerializeField] private GameObject factoryModel;
        [SerializeField] private Transform resourcesRootTransform;
        [SerializeField] private Transform resourcesPlaces;
        [SerializeField] private List<Transform> resourcesPlacesList;
        [SerializeField] private List<GameObject> availableResources;

        [Header("Capacity")] [SerializeField] private int maxResourcesNumber;

        [Header("Resource")] [SerializeField] private GameObject resourcePrefab;
        [SerializeField] private Vector3 resourcePositionOffset;
        [SerializeField] private Vector3 resourceRotationOffset;
        [SerializeField] private Vector3 resourceScaleOffset;

        [Header("Info")] private bool _productionStarted;

        public void Inject()
        {
            if (purchased)
            {
                ActivateFactory();
            }
        }

        public void PurchaseFactory()
        {
            purchased = true;
            purchasePlace.SetActive(false);
            factoryModel.SetActive(true);

            ActivateFactory();
        }

        public void ActivateFactory()
        {
            purchasePlace.SetActive(false);
            factoryModel.SetActive(true);

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
            for (var i = 0; i < resourcesPlaces.childCount; i++)
            {
                resourcesPlacesList.Add(resourcesPlaces.GetChild(i));
            }

            maxResourcesNumber = resourcesPlacesList.Count;
        }

        private void StartProduction()
        {
            _productionStarted = true;

            StartCoroutine(StartProductionEnumerator());
        }

        private void AddResource()
        {
            var newResourceGameObject = Instantiate(resourcePrefab, resourcesRootTransform);
            newResourceGameObject.transform.position = resourcesPlacesList[AvailableResources.Count].position;

            AvailableResources.Add(newResourceGameObject);
        }

        private IEnumerator StartProductionEnumerator()
        {
            while (true)
            {
                if (AvailableResources.Count < maxResourcesNumber)
                {
                    AddResource();
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        public List<GameObject> AvailableResources => availableResources;

        public float Price
        {
            get => price;
            set => price = value;
        }
    }
}