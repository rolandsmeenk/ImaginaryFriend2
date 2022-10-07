using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

/// <summary>
/// Determines if users hand is in handshake position (pointing at target, palm to the side, fingers open)
/// </summary>
public class HandShakeGestureDetector: MonoBehaviour
{
    [SerializeField]
    private float PalmThresholdAngle = 45f;

    [SerializeField]
    private Transform _targetTransform;

    public bool HandShakePose = false;
    void Update()
    {
        HandShakePose = false;
        var rightHand = HandJointUtils.FindHand(Handedness.Right);
        if (rightHand != null && rightHand.TrackingState == Microsoft.MixedReality.Toolkit.TrackingState.Tracked)
        {
            MixedRealityPose palmPose;
            if (rightHand.TryGetJoint(TrackedHandJoint.Palm, out palmPose))
            {
                float upAngle = Vector3.Angle(palmPose.Rotation * Vector3.up, Vector3.up);
                HandShakePose = Mathf.Abs(upAngle - 90) < PalmThresholdAngle;

                float pointingAngle = Vector3.Angle(palmPose.Rotation * Vector3.forward, _targetTransform.position - palmPose.Position);
                HandShakePose &= pointingAngle < PalmThresholdAngle;
            }

#if !UNITY_EDITOR
            HandShakePose &= (HandPoseUtils.ThumbFingerCurl(Handedness.Right) < 0.3);
            HandShakePose &= (HandPoseUtils.IndexFingerCurl(Handedness.Right) < 0.3);
            HandShakePose &= (HandPoseUtils.MiddleFingerCurl(Handedness.Right) < 0.3);
            HandShakePose &= (HandPoseUtils.RingFingerCurl(Handedness.Right) < 0.3);
            HandShakePose &= (HandPoseUtils.PinkyFingerCurl(Handedness.Right) < 0.3);
#endif

            // If within reach of the hand helper object force handShakePose
            MixedRealityPose thumbPose;
            if (rightHand.TryGetJoint(TrackedHandJoint.ThumbTip, out thumbPose))
            {
                HandShakePose |= Vector3.Distance(thumbPose.Position, transform.position) < 0.1f;
            }
        }
    }
}

