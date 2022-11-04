using System;
using Timmy;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TimmyBehaviourParroting : MonoBehaviour
{
    [SerializeField]
    private Rig _lookAtRig;
    [SerializeField]
    private Rig _handRig;

    private float _currentVelocity = 0;
    private float _currentHandVelocity = 0;
    private bool _dwellActive;

    public TimmyTextToSpeech _textToSpeech;
    private bool listenedToUser;

    public HandShakeGestureDetector handShakeGestureDetector;

    public void OnEyeFocusStart()
    {
        Debug.Log("Focus start");
        _dwellActive = true;
    }

    public void OnEyeFocusStop()
    {
        Debug.Log("Focus stop");
        _dwellActive = false;
        previousRecognizedText = string.Empty;
    }

    public void Speak(string text)
    {
        if (!_textToSpeech.IsSpeaking())
        {
            _textToSpeech.StartSpeaking(text);
        }
    }

    private void Update()
    {
        bool lookAtUser = _dwellActive;

        _lookAtRig.weight = Mathf.SmoothDamp(_lookAtRig.weight, lookAtUser ? 1 : 0, ref _currentVelocity, .5f);

        _handRig.weight = Mathf.SmoothDamp(_handRig.weight, handShakeGestureDetector.HandShakePose ? 1 : 0, ref _currentHandVelocity, .5f);

        listenedToUser |= (!_textToSpeech.IsSpeaking() && _dwellActive);
    }

    private string previousRecognizedText = string.Empty;

    public void RecognizedText(string text)
    {
        // Quick fix for multiple triggers
        if (previousRecognizedText == text)
        {
            return;
        }

        if (_dwellActive && !_textToSpeech.IsSpeaking() && listenedToUser)
        {
            string textToParrot = text;
            if (text.Length > previousRecognizedText.Length && previousRecognizedText.Length > 0)
            {
                textToParrot = textToParrot.Substring(previousRecognizedText.Length);
            }

            Speak(textToParrot);
            listenedToUser = false;
        }
        previousRecognizedText = text;
    }
}
