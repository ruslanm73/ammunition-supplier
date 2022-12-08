using System;
using UnityEngine;

namespace Runtime.Player.Components
{
    public interface IPlayerTrigger
    {
        public Action<GameObject> OnTurretEnterTrigger { get; set; }
        public Action<GameObject> OnTurretExitTrigger { get; set; }

        public Action<GameObject> OnAmmunitionFactoryEnterTrigger { get; set; }
        public Action<GameObject> OnAmmunitionFactoryExitTrigger { get; set; }

        public Action<GameObject> OnTurretPurchaseEnterTrigger { get; set; }
        public Action<GameObject> OnTurretPurchaseExitTrigger { get; set; }

        public Action<GameObject> OnAmmunitionFactoryPurchaseEnterTrigger { get; set; }
        public Action<GameObject> OnAmmunitionFactoryPurchaseExitTrigger { get; set; }

        public Action<GameObject> OnLootMoneyEnterTrigger { get; set; }
        public Action<GameObject> OnLootMoneyExitTrigger { get; set; }
    }

    public class PlayerTrigger : MonoBehaviour, IPlayerTrigger
    {
        private const string TurretTrigger = "TurretTrigger";
        private const string AmmunitionFactoryTrigger = "AmmunitionFactoryTrigger";
        private const string TurretPurchaseTrigger = "TurretPurchaseTrigger";
        private const string AmmunitionFactoryPurchaseTrigger = "AmmunitionFactoryPurchaseTrigger";
        private const string LootMoney = "LootMoney";


        public Action<GameObject> OnTurretEnterTrigger { get; set; }
        public Action<GameObject> OnTurretExitTrigger { get; set; }

        public Action<GameObject> OnAmmunitionFactoryEnterTrigger { get; set; }
        public Action<GameObject> OnAmmunitionFactoryExitTrigger { get; set; }

        public Action<GameObject> OnTurretPurchaseEnterTrigger { get; set; }
        public Action<GameObject> OnTurretPurchaseExitTrigger { get; set; }

        public Action<GameObject> OnAmmunitionFactoryPurchaseEnterTrigger { get; set; }
        public Action<GameObject> OnAmmunitionFactoryPurchaseExitTrigger { get; set; }

        public Action<GameObject> OnLootMoneyEnterTrigger { get; set; }
        public Action<GameObject> OnLootMoneyExitTrigger { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == TurretTrigger)
            {
                OnTurretEnterTrigger?.Invoke(other.gameObject);
            }

            if (other.name == AmmunitionFactoryTrigger)
            {
                OnAmmunitionFactoryEnterTrigger?.Invoke(other.gameObject);
            }

            if (other.name == TurretPurchaseTrigger)
            {
                OnTurretPurchaseEnterTrigger?.Invoke(other.gameObject);
            }

            if (other.name == AmmunitionFactoryPurchaseTrigger)
            {
                OnAmmunitionFactoryPurchaseEnterTrigger?.Invoke(other.gameObject);
            }

            if (other.name == LootMoney)
            {
                OnLootMoneyEnterTrigger?.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.name == TurretTrigger)
            {
                OnTurretExitTrigger?.Invoke(other.gameObject);
            }

            if (other.name == AmmunitionFactoryTrigger)
            {
                OnAmmunitionFactoryExitTrigger?.Invoke(other.gameObject);
            }

            if (other.name == TurretPurchaseTrigger)
            {
                OnTurretPurchaseExitTrigger?.Invoke(other.gameObject);
            }

            if (other.name == AmmunitionFactoryPurchaseTrigger)
            {
                OnAmmunitionFactoryPurchaseExitTrigger?.Invoke(other.gameObject);
            }

            if (other.name == LootMoney)
            {
                OnLootMoneyExitTrigger?.Invoke(other.gameObject);
            }
        }
    }
}