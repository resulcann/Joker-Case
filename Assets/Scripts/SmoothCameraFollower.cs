using UnityEngine;

public class SmoothCameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 5f;
    [SerializeField] private Vector3 lookAtOffset;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private bool followPosition;
    [SerializeField] private bool followRotation;
    
    // Deadzone settings
    [SerializeField] private Vector2 pitchRange;
    [SerializeField] private Vector2 yawRange;

    private void LateUpdate()
    {
        if (positionOffset != Vector3.zero && followPosition)
        {
            transform.position = target.position + positionOffset;
        }
        
        var targetPosition = target.position + lookAtOffset;
        var targetDirection = targetPosition - transform.position;
        
        var targetRotation = Quaternion.LookRotation(targetDirection);

        if (followRotation)
        {
            var eulerAngles = targetRotation.eulerAngles;
            
            var yaw = Mathf.Clamp(eulerAngles.y, NormalizeAngle(yawRange.x), NormalizeAngle(yawRange.y));
            var pitch = Mathf.Clamp(eulerAngles.x, NormalizeAngle(pitchRange.x), NormalizeAngle(pitchRange.y));
            
            var clampedRotation = Quaternion.Euler(pitch, yaw, 0);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, clampedRotation, Time.deltaTime * smoothTime);
        }
    }
    
    private float NormalizeAngle(float angle)
    {
        angle %= 360;
        if (angle < 0) angle += 360;
        return angle;
    }
}
