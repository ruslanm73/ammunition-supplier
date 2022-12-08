using UnityEngine;

namespace Runtime.Player.Components
{
    public interface IPlayerReferences
    {
        Transform PlayerModelTransform { get; set; }
        Transform PlayerMeshTransform { get; set; }
        Transform PlayerArmfulTransform { get; set; }

        Animator PlayerAnimator { get; set; }
    }

    public class PlayerReferences : MonoBehaviour, IPlayerReferences
    {
        [field: SerializeField] public Transform PlayerModelTransform { get; set; }
        [field: SerializeField] public Transform PlayerMeshTransform { get; set; }
        [field: SerializeField] public Transform PlayerArmfulTransform { get; set; }
        [field: SerializeField] public Animator PlayerAnimator { get; set; }
    }
}