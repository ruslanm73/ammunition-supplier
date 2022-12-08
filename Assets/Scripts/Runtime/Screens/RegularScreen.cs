using System.Globalization;
using TMPro;
using UnityEngine;

namespace Runtime.Screens
{
    public class RegularScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        public void UpdateMoneyCounter(float newValue)
        {
            textMeshProUGUI.text = $"{newValue.ToString(CultureInfo.InvariantCulture)}$";
        }
    }
}