using System.Collections.Generic;

public class MovingAverage
{
    private Queue<double> samples = new Queue<double>();
    private int windowSize = 16;
    private double sampleAccumulator;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="size">The number of samples in the moving average window</param>
    public MovingAverage(int size)
    {
        windowSize = size;
    }

    /// <summary>
    /// Computes a new windowed average each time a new sample arrives
    /// </summary>
    /// <param name="newSample"></param>
    public double ComputeAverage(double newSample)
    {
        sampleAccumulator += newSample;
        samples.Enqueue(newSample);

        if (samples.Count > windowSize)
        {
            sampleAccumulator -= samples.Dequeue();
        }

        return sampleAccumulator / samples.Count;
    }
}