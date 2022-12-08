using Runtime.Core;
using UnityEngine;

namespace Runtime.Screens
{
    public interface IScreensManager
    {
        RegularScreen RegularScreen { get; set; }
    }

    public class ScreensManager : MonoBehaviour, IScreensManager
    {
        [field: SerializeField] public RegularScreen RegularScreen { get; set; }
    }
}