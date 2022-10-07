using System.Collections;
using System.Collections.Generic;
using Timmy;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TimmyBehaviourListenAndTalk : MonoBehaviour
{
    [SerializeField]
    private Rig _lookAtRig;

    private float _currentVelocity = 0;
    private bool _dwellActive;

    public AudioLevelDetector _audioLevelDetector;
    public string[] TextsToSay;

    public TimmyTextToSpeech _textToSpeech;
    private bool listenedToUser;
    private int currentTextIndex;    

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

    public void Speak(string text)
    {
        if (!_textToSpeech.IsSpeaking())
        {
            _textToSpeech.StartSpeaking(text);
        }
    }

    private void Update()
    {
        bool lookAtUser = _dwellActive || _audioLevelDetector.High;

        _lookAtRig.weight = Mathf.SmoothDamp(_lookAtRig.weight, lookAtUser ? 1 : 0, ref _currentVelocity, .5f);

        
        listenedToUser |= (!_textToSpeech.IsSpeaking() && _audioLevelDetector.High && _dwellActive);

        // Looking at me, nobody talking, but user said something before
        if (_dwellActive && !_textToSpeech.IsSpeaking() && !_audioLevelDetector.High && listenedToUser)
        {
            Speak(TextsToSay[currentTextIndex++]);

            if (currentTextIndex >= TextsToSay.Length)
            {
                currentTextIndex = 0;
            }

            listenedToUser = false;
        }
    }
}
