using UnityEngine;

namespace Interfaces
{
    public interface IAttackingGroup
    {
        Vector2 FindNearestEnemy(Vector2 targetPosition);
    }
}