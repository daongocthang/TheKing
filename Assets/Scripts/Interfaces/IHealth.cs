using UnityEngine;

namespace Interfaces
{
    public interface IHealth
    {
        void DealDamage(float damage, GameObject attacker);
    }
}