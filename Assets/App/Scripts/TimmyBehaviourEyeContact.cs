using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TimmyBehaviourEyeContact : MonoBehaviour
{
    [SerializeField]
    private Rig _lookAtRig;

    private float _currentVelocity = 0;
    private bool _dwellActive;

    public void OnEyeFocusStart()
    {
        Debug.Log("Focus start");
        _dwellActive = true;
    }

    public void OnEyeFocusStop()
    {
        Debug.Log("Focus stop");
        _dwellActive = false;
    }

    private void Update()
    {
        _lookAtRig.weight = Mathf.SmoothDamp(_lookAtRig.weight, _dwellActive ? 1 : 0, ref _currentVelocity, .5f);
    }
}
