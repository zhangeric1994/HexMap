using UnityEngine;
using System.Collections;

public class RandomNumberGenerator
{
    private int _initialSeed;
    private int _currentSeed;
    private int _count;

    private System.Random _generator;

    public RandomNumberGenerator() : this(TimeUtility.localTime)
    {
    }

    public RandomNumberGenerator(int seed)
    {
        Reset(seed);
    }

    public void Reset()
    {
        _currentSeed = _initialSeed;
        _count = 0;
    }

    public void Reset(int seed)
    {
        _initialSeed = seed;
        Reset();
    }

    public int GenerateRandomInt()
    {
        return Next();
    }

    public int GenerateRandomInt(int max)
    {
        return Next(max);
    }

    public int GenerateRandomInt(int min, int max)
    {
        return Next(min, max);
    }

    public float GenerateRandomFloat()
    {
        return GenerateRandomFloat(0, 1);
    }

    public float GenerateRandomFloat(float min, float max)
    {
        return Mathf.Lerp(min, max, (float)NextDouble());
    }

    private int Next()
    {
        IncrementCount();
        return _generator.Next();
    }

    private int Next(int max)
    {
        IncrementCount();
        return _generator.Next(max);
    }

    private int Next(int min, int max)
    {
        IncrementCount();
        return _generator.Next(min, max);
    }

    private double NextDouble()
    {
        IncrementCount();
        return _generator.NextDouble();
    }

    private void IncrementCount()
    {
        if (_count % 20 == 0)
            _generator = new System.Random(_currentSeed++);

        _count++;
    }
}
