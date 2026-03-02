using UnityEngine;

namespace Game
{
    public abstract class Character : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; protected set; }
        [field: SerializeField] public float Speed { get; protected set; }

        public Vector3 Velocity { get; protected set; }
    }
}