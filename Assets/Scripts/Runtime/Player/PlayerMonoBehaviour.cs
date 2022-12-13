using Runtime.Core;
using Runtime.Player.Components;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerMonoBehaviour : MonoBehaviour
    {
        private GameGraph _gameGraph;
        private UltimateJoystick _ultimateJoystick;
        private IPlayerReferences _playerReferences;
        private IPlayerControl _playerControl;
        private IPlayerTrigger _playerTrigger;
        private IPlayerArmful _playerArmful;
        private PlayerTriggerCallbacks _playerTriggerCallbacks;

        public void Inject(GameGraph gameGraph, UltimateJoystick ultimateJoystick)
        {
            _gameGraph = gameGraph;
            _ultimateJoystick = ultimateJoystick;
            _playerReferences = GetComponent<PlayerReferences>();
            _playerTrigger = GetComponent<PlayerTrigger>();
            _playerArmful = GetComponent<PlayerArmful>();
            _playerControl = new PlayerControl(this, ultimateJoystick, gameObject, _playerReferences.PlayerAnimator);
            _playerTriggerCallbacks = GetComponent<PlayerTriggerCallbacks>();

            _playerArmful.Inject(_playerReferences);
            _playerTriggerCallbacks.Inject(gameGraph, _playerTrigger, _playerArmful);
        }
    }
}