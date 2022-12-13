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

        [SerializeField] private GameObject _resourceTemplate;
        [SerializeField] private Vector2 _gridSize;
        [SerializeField] private Vector2 _gridSpace;
        [SerializeField] private List<Vector2> _gridPoints;
        private readonly List<GameObject> _currentArmfulResources = new();
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
            var newArmfulResource = Instantiate(_resourceTemplate, _playerReferences.PlayerArmfulTransform);
            newArmfulResource.transform.localPosition = new Vector3(0f, _gridPoints[_currentArmfulResources.Count].y, _gridPoints[_currentArmfulResources.Count].x);
            newArmfulResource.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

            _currentArmfulResources.Add(newArmfulResource);

            _playerReferences.PlayerAnimator.SetBool(PlayerCarrying, true);
        }

        public void RemoveResource()
        {
            var targetResource = _currentArmfulResources[^1];
            Destroy(targetResource);

            _currentArmfulResources.RemoveAt(_currentArmfulResources.Count - 1);

            if (_currentArmfulResources.Count == default)
            {
                _playerReferences.PlayerAnimator.SetBool(PlayerCarrying, false);
            }
        }

        public void CreateArmfulGrid()
        {
            if (_gridPoints.Count != default) _gridPoints.Clear();

            for (var i = 0; i < _gridSize.x; i++)
            {
                for (var j = 0; j < _gridSize.y; j++)
                {
                    var newPoint = new Vector2(i * _gridSpace.x, j * _gridSpace.y);

                    _gridPoints.Add(newPoint);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            for (var i = 0; i < _gridPoints.Count; i++)
            {
                Gizmos.color = Color.blue;
                var localPosition = _playerReferences.PlayerArmfulTransform.localPosition;
                Gizmos.DrawSphere(new Vector3(0f, localPosition.y + _gridPoints[i].y, localPosition.z + _gridPoints[i].x), 0.05f);
            }
        }

        [field: SerializeField] public int TotalArmfulNumber { get; set; }
        public List<GameObject> CurrentArmfulResources => _currentArmfulResources;
    }
}