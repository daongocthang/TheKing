using UnityEngine;

namespace Interfaces
{
    public interface IGroupCenter
    {
        Vector3 GetCenterPoint();

        void RemoveTarget(Transform target);
    }
}