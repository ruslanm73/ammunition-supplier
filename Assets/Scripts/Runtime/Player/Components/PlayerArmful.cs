using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Player.Components
{
    public interface IPlayerArmful
    {
        List<GameObject> CurrentArmfulResources { get; }

        void Inject(IPlayerReferences playerReferences);

        bool ArmfulAvailable();
        void AddResource();
        void RemoveResource();
    }

    public class PlayerArmful : MonoBehaviour, IPlayerArmful
    {
        private static readonly int PlayerCarrying = Animator.StringToHash("PlayerCarrying");

        [SerializeField] private GameObject resourceTemplate;

        public List<GameObject> currentArmfulResources;

        [SerializeField] private Vector2 gridSize;
        [SerializeField] private Vector2 gridSpace;
        [SerializeField] private List<Vector2> gridPoints;

        private IPlayerReferences _playerReferences;

        public void Inject(IPlayerReferences playerReferences)
        {
            _playerReferences = playerReferences;

            CreateArmfulGrid();
        }

        public bool ArmfulAvailable()
        {
            return CurrentArmfulResources.Count < TotalArmfulNumber;
        }

        public void AddResource()
        {
            var newArmfulResource = Instantiate(resourceTemplate, _playerReferences.PlayerArmfulTransform);
            newArmfulResource.transform.localPosition = new Vector3(0f, gridPoints[currentArmfulResources.Count].y, gridPoints[currentArmfulResources.Count].x);
            newArmfulResource.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

            currentArmfulResources.Add(newArmfulResource);

            _playerReferences.PlayerAnimator.SetBool(PlayerCarrying, true);
        }

        public void RemoveResource()
        {
            var targetResource = currentArmfulResources[^1];
            Destroy(targetResource);

            currentArmfulResources.RemoveAt(currentArmfulResources.Count - 1);

            if (currentArmfulResources.Count == default)
            {
                _playerReferences.PlayerAnimator.SetBool(PlayerCarrying, false);
            }
        }

        public void CreateArmfulGrid()
        {
            if (gridPoints.Count != default) gridPoints.Clear();

            for (var i = 0; i < gridSize.x; i++)
            {
                for (var j = 0; j < gridSize.y; j++)
                {
                    var newPoint = new Vector2(i * gridSpace.x, j * gridSpace.y);

                    gridPoints.Add(newPoint);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            for (var i = 0; i < gridPoints.Count; i++)
            {
                Gizmos.color = Color.blue;
                var localPosition = _playerReferences.PlayerArmfulTransform.localPosition;
                Gizmos.DrawSphere(new Vector3(0f, localPosition.y + gridPoints[i].y, localPosition.z + gridPoints[i].x), 0.05f);
            }
        }

        [field: SerializeField] public int TotalArmfulNumber { get; set; }
        public List<GameObject> CurrentArmfulResources => currentArmfulResources;
    }
}