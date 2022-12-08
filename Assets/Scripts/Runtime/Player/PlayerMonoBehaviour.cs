using System.Collections;
using Runtime.Core;
using Runtime.Factories;
using Runtime.Player.Components;
using Runtime.Turret;
using UnityEngine;

namespace Runtime.Player
{
    public interface IPlayerMonoBehaviour
    {
        void Inject(GameGraph gameGraph, UltimateJoystick ultimateJoystick);
    }

    public class PlayerMonoBehaviour : MonoBehaviour, IPlayerMonoBehaviour
    {
        private GameGraph _gameGraph;
        private UltimateJoystick _ultimateJoystick;
        private IPlayerReferences _playerReferences;
        private IPlayerControl _playerControl;
        private IPlayerTrigger _playerTrigger;
        private IPlayerArmful _playerArmful;

        private IEnumerator _fillingArmfulEnumerator;
        private IEnumerator _fillingObjectEnumerator;

        public void Inject(GameGraph gameGraph, UltimateJoystick ultimateJoystick)
        {
            _gameGraph = gameGraph;
            _ultimateJoystick = ultimateJoystick;
            _playerReferences = GetComponent<PlayerReferences>();
            _playerTrigger = GetComponent<PlayerTrigger>();
            _playerArmful = GetComponent<PlayerArmful>();
            _playerControl = new PlayerControl(this, ultimateJoystick, gameObject, _playerReferences.PlayerAnimator);

            _playerArmful.Inject(_playerReferences);

            PlayerTriggerCallbacks();
        }

        private void PlayerTriggerCallbacks()
        {
            _playerTrigger.OnTurretEnterTrigger += turretTrigger =>
            {
                var turretComponent = turretTrigger.GetComponentInParent<TurretMonoBehaviour>();

                if (_playerArmful.CurrentArmfulResources.Count > 0 && turretComponent.ShellsFillingAvailable())
                {
                    _fillingObjectEnumerator = FillingObjectEnumerator(turretComponent);

                    StartCoroutine(_fillingObjectEnumerator);
                }
            };

            _playerTrigger.OnTurretExitTrigger += turretTrigger =>
            {
                if (_fillingObjectEnumerator == null) return;

                StopCoroutine(_fillingObjectEnumerator);
            };

            _playerTrigger.OnAmmunitionFactoryEnterTrigger += ammunitionTrigger =>
            {
                var ammunitionFactory = ammunitionTrigger.GetComponentInParent<AmmunitionFactory>();

                if (ammunitionFactory.AvailableResources.Count > 0 && _playerArmful.ArmfulAvailable())
                {
                    if (_fillingArmfulEnumerator != null)
                    {
                        StopCoroutine(_fillingArmfulEnumerator);
                    }

                    _fillingArmfulEnumerator = FillingArmfulEnumerator(ammunitionFactory);

                    StartCoroutine(_fillingArmfulEnumerator);
                }
            };

            _playerTrigger.OnAmmunitionFactoryExitTrigger += ammunitionTrigger =>
            {
                if (_fillingArmfulEnumerator == null) return;

                StopCoroutine(_fillingArmfulEnumerator);
            };

            _playerTrigger.OnTurretPurchaseEnterTrigger += turretPurchaseTrigger =>
            {
                var turretRoot = turretPurchaseTrigger.transform.parent;
                var turretMonoBehaviour = turretRoot.GetComponentInParent<TurretMonoBehaviour>();

                if (_gameGraph.DataGraph.gameData.money >= turretMonoBehaviour.TurretPrice)
                {
                    turretMonoBehaviour.PurchaseTurret();
                }
            };

            _playerTrigger.OnAmmunitionFactoryPurchaseEnterTrigger += ammunitionFactoryPurchaseTrigger =>
            {
                var ammunitionFactoryRoot = ammunitionFactoryPurchaseTrigger.transform.parent;
                var ammunitionFactoryMonoBehaviour = ammunitionFactoryRoot.GetComponentInParent<AmmunitionFactory>();

                if (_gameGraph.DataGraph.gameData.money >= ammunitionFactoryMonoBehaviour.Price)
                {
                    ammunitionFactoryMonoBehaviour.PurchaseFactory();
                }
            };

            _playerTrigger.OnLootMoneyEnterTrigger += moneyTrigger =>
            {
                _gameGraph.DataGraph.gameData.money += 10;
                _gameGraph.ScreensManager.RegularScreen.UpdateMoneyCounter(_gameGraph.DataGraph.gameData.money);

                Destroy(moneyTrigger);
            };
        }

        // TODO: Refactoring.
        private IEnumerator FillingArmfulEnumerator(AmmunitionFactory ammunitionFactory)
        {
            while (true)
            {
                if (ammunitionFactory.AvailableResources.Count > 0 && _playerArmful.ArmfulAvailable())
                {
                    _playerArmful.AddResource();
                    ammunitionFactory.IssueResource();
                }

                yield return new WaitForSeconds(0.25f);
            }
        }

        private IEnumerator FillingObjectEnumerator(TurretMonoBehaviour turretMonoBehaviour)
        {
            while (true)
            {
                if (_playerArmful.CurrentArmfulResources.Count > 0 && turretMonoBehaviour.ShellsFillingAvailable())
                {
                    _playerArmful.RemoveResource();
                    turretMonoBehaviour.AddShell();
                }

                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}