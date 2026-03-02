using UnityEngine;

namespace Game
{
    public sealed class HUD : MonoBehaviour
    {
        [field: SerializeField] public HealthView HealthView { get; private set; }
    }
}