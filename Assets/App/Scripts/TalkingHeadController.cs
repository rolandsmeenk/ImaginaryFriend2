using UnityEngine;

public class TalkingHeadController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    private MovingAverage slowAverage = new MovingAverage(20);
    private MovingAverage fastAverage = new MovingAverage(10);
    private float lastCalc = 0;
    public float fastMVA = 0;
    public float slowMVA = 0;
    private float currentVelocity = 0;
    private float smoothMovement;
    private float loudNess = 0;

    int _sampleWindow = 128;
    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];

        _audioSource.GetOutputData(waveData, 0);

        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }


    // Update is called once per frame
    void Update()
    {
        loudNess = LevelMax();

        if ((Time.realtimeSinceStartup - lastCalc) > .0166666f)
        {
            lastCalc = Time.realtimeSinceStartup;
            fastMVA = (float)fastAverage.ComputeAverage((double)loudNess);
            slowMVA = (float)slowAverage.ComputeAverage((double)loudNess);
        }

        // Move head up or down
        float movement = (slowMVA < 0.00001f) ? 0 : ((fastMVA / slowMVA) - 1f);
        movement = Mathf.Clamp(movement, -1, 1);

        // Smooth the movement
        smoothMovement = Mathf.SmoothDamp(smoothMovement, movement, ref currentVelocity, 0.1f);
    }

    void LateUpdate()
    {
        // Apply rotation
        transform.localRotation = transform.localRotation * Quaternion.AngleAxis(15 * smoothMovement, Vector3.left);
    }
}
