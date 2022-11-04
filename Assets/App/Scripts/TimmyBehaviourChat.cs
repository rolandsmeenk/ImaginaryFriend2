using OpenAi.Unity.V1;
using System.Collections.Generic;
using Timmy;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TimmyBehaviourChat : MonoBehaviour
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

    public string humanPrefix = "Human:";
    public string aiPrefix = "Timmy:";

    [Multiline(5)]
    [SerializeField]
    private string preText;

    [Multiline(5)]
    [SerializeField]
    private string exampleText;

    [SerializeField]
    private TextMeshProUGUI BalloonText;

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
        bool lookAtUser = _dwellActive;

        _lookAtRig.weight = Mathf.SmoothDamp(_lookAtRig.weight, lookAtUser ? 1 : 0, ref _currentVelocity, .5f);

        _handRig.weight = Mathf.SmoothDamp(_handRig.weight, handShakeGestureDetector.HandShakePose ? 1 : 0, ref _currentHandVelocity, .5f);

        listenedToUser |= (!_textToSpeech.IsSpeaking() && _dwellActive);
    }

    private string previousRecognizedText = string.Empty;

    public void RecognizedText(string text)
    {
        // Quick fix for multiple triggers
        if (text == previousRecognizedText)
            return;

        if (_dwellActive && !_textToSpeech.IsSpeaking() && listenedToUser)
        {
            string newlyRecognizedText = text;
            if (text.Length > previousRecognizedText.Length && previousRecognizedText.Length > 0)
            {
                newlyRecognizedText = newlyRecognizedText.Substring(previousRecognizedText.Length);
            }

            GenerateAnswer(newlyRecognizedText);

            listenedToUser = false;
        }
        else
        {
            Debug.Log("Ignored " + text);
        }
        previousRecognizedText = text;
    }

    private void GenerateAnswer(string text)
    {
        string prompt = preText + "\n";
        prompt += exampleText + "\n";

        foreach (var line in _history)
        {
            prompt += line + "\n";
        }

        prompt += humanPrefix + text + "\n";
        prompt += aiPrefix;

        BalloonText.text = humanPrefix + text;

        AddHistory(false, text);

        OpenAiCompleterV1.Instance.Complete(
            prompt,
            s => TimmyAnswer(s),
            e => Debug.Log($"ERROR: StatusCode: { e.responseCode}"));
    }

    private void TimmyAnswer(string text)
    {
        string timmyAnswer = text;

        var aiTextIndex = text.IndexOf(aiPrefix);

        // Cut of AI prefix
        if (aiTextIndex >= 0)
        {
            timmyAnswer = timmyAnswer.Substring(aiTextIndex + aiPrefix.Length);
        }

        // Cut of extra human completion lines
        var humanTextIndex = timmyAnswer.IndexOf(humanPrefix);
        if (humanTextIndex >= 0)
        {
            timmyAnswer = timmyAnswer.Substring(0, humanTextIndex);
        }

        // Cleanup
        timmyAnswer = timmyAnswer.Replace("\\", " ");
        timmyAnswer = timmyAnswer.Replace(".", ".\n");
        timmyAnswer = timmyAnswer.Replace("!", "!\n");
        timmyAnswer = timmyAnswer.Replace("?", "?\n");
        timmyAnswer = timmyAnswer.Replace(",", ",\n");

        BalloonText.text = BalloonText.text + "\n" + aiPrefix + timmyAnswer;

        AddHistory(true, timmyAnswer);

        Speak(timmyAnswer);
    }

    [SerializeField]
    private int maxHistoryLength = 0;

    public List<string> _history = new List<string>();

    private void AddHistory(bool ai, string text)
    {
        _history.Add(ai ? aiPrefix + text :
                          humanPrefix + text);

        while (_history.Count > maxHistoryLength)
        {
            _history.RemoveAt(0);
        }
    }

    public void DictationError(string text)
    {
        Debug.LogError(text);
    }
}

