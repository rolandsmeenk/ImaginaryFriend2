using UnityEngine;

public class AudioLevelDetector : MonoBehaviour
{
    private MovingAverage slowAverage = new MovingAverage(300);
    private MovingAverage fastAverage = new MovingAverage(60);

    public Material material;

    public bool High = false;
    
    public float fastMVA = 0;
    public float slowMVA = 0;
    private float lastCalc = 0;

    void Update()
    {
        if ((Time.realtimeSinceStartup - lastCalc) > .0166666f)
        {
            lastCalc = Time.realtimeSinceStartup;
            fastMVA = (float)fastAverage.ComputeAverage((double)MicInput.MicLoudness);
            slowMVA = (float)slowAverage.ComputeAverage((double)(High ? 0.5f * MicInput.MicLoudness : MicInput.MicLoudness));
        }

        if (High)
        {
            if (fastMVA < slowMVA * .5)
            {
                material.SetColor("_Color", Color.green);
                High = false;
            }
        }
        else
        {
            if (fastMVA > slowMVA * 2)
            {
                material.SetColor("_Color", Color.red);
                High = true;
            }
        }
    }
}
