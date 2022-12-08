using System.Collections.Generic;
using Runtime.Factories;
using Runtime.Turret;
using UnityEngine;

namespace Runtime.Core
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> ammunitionFactories;
        [SerializeField] private List<GameObject> turrets;

        public void Inject()
        {
            InitialLevelComponents();
        }

        private void InitialLevelComponents()
        {
            foreach (var ammunitionFactory in ammunitionFactories)
            {
                ammunitionFactory.GetComponent<AmmunitionFactory>().Inject();
            }

            foreach (var turret in turrets)
            {
                turret.GetComponent<TurretMonoBehaviour>().Inject();
            }
        }
    }
}