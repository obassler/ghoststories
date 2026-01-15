using UnityEngine;

public class TorchRaycast : MonoBehaviour
{
    public float range = 8f;

    public enum RayDirection { Forward, Back, Up, Down, Right, Left }
    public RayDirection rayDirection = RayDirection.Up;

    Vector3 GetDirection()
    {
        switch (rayDirection)
        {
            case RayDirection.Forward: return transform.forward;
            case RayDirection.Back: return -transform.forward;
            case RayDirection.Up: return transform.up;
            case RayDirection.Down: return -transform.up;
            case RayDirection.Right: return transform.right;
            case RayDirection.Left: return -transform.right;
            default: return transform.forward;
        }
    }

    void Update()
    {
        Vector3 direction = GetDirection();

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range))
        {
            PriestBanish priest = hit.collider.GetComponent<PriestBanish>();

            if (priest == null)
                priest = hit.collider.GetComponentInParent<PriestBanish>();

            if (priest == null)
                priest = hit.collider.GetComponentInChildren<PriestBanish>();

            if (priest != null)
                priest.HitByTorch();
        }
    }
}
