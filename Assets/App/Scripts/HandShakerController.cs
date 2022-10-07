using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandShakerController : MonoBehaviour
{
    public float radius = 0.1f;
    public float handShakeHeight;
    public float clampAngle;

    private Vector3 defaultDir;
    private Vector3 currentVelocity;

    private void Start()
    {
        defaultDir = (transform.localPosition - new Vector3(0, handShakeHeight, 0)).normalized;
    }

    void Update()
    {
        var rightHand = HandJointUtils.FindHand(Handedness.Right);
        if (rightHand != null && rightHand.TrackingState == Microsoft.MixedReality.Toolkit.TrackingState.Tracked)
        {
            MixedRealityPose fingerTipPose;
            if (rightHand.TryGetJoint(TrackedHandJoint.IndexTip, out fingerTipPose))
            {
                var targetDir = (transform.parent.InverseTransformPoint(fingerTipPose.Position) - new Vector3(0, handShakeHeight, 0)).normalized;

                // Clamp target direction
                var angle = Vector3.Angle(defaultDir, targetDir);
                if (angle > clampAngle)
                {
                    targetDir = Vector3.Lerp(defaultDir, targetDir, clampAngle / angle);
                }

                // Hand target based on the clamped direction
                var target = targetDir.normalized * radius + new Vector3(0, handShakeHeight, 0);

                // Smooth target update
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, target, ref currentVelocity, 0.5f);
            }
        }
    }
}
