using UnityEngine;
using System;
using System.Collections.Generic;

public struct MathUtility
{
    public static readonly float SQRT2 = Mathf.Sqrt(2);

    public static int ManhattanDistance(int xA, int yA, int xB, int yB)
    {
        return Math.Abs(xA - xB) + Math.Abs(yA - yB);
    }

    public static float EuclideanDistance(int xA, int yA, int xB, int yB)
    {
        int dx = xA - xB;
        int dy = yA - yB;

        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    public static int ChebyshevDistance(int xA, int yA, int xB, int yB)
    {
        return Math.Max(Math.Abs(xA - xB), Math.Abs(yA - yB));
    }

    public static int ManhattanDistance(int xA, int yA, int zA, int xB, int yB, int zB)
    {
        return Math.Abs(xA - xB) + Math.Abs(yA - yB) + Math.Abs(zA - zB);
    }

    public static float EuclideanDistance(int xA, int yA, int zA, int xB, int yB, int zB)
    {
        int dx = xA - xB;
        int dy = yA - yB;
        int dz = zA - zB;

        return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public static int ChebyshevDistance(int xA, int yA, int zA, int xB, int yB, int zB)
    {
        return Math.Max(Math.Abs(xA - xB), Math.Max(Math.Abs(yA - yB), Math.Abs(zA - zB)));
    }

    public static float CubicCurve1(float x)
    {
        return (Mathf.Pow(3 * x - 1, 3) + 1) / 9;
    }

    public static float CubicCurve2(float x)
    {
        return (Mathf.Pow(4 * x - 2, 3) + 8) / 16;
    }

}

public struct BitOperationUtility
{
    public static int ReadBit(int number, int bit)
    {
        if (bit < 0 || bit > 31)
            throw new ArgumentException(string.Format("[BitOperationUtility] Invalid bit to read ({0})", bit));

        return number & (1 << bit);
    }

    public static void WriteBit(ref int number, int bit, int value)
    {
        if (value != 0 && value != 1)
            throw new ArgumentException(string.Format("[BitOperationUtility] Invalid value to write ({0})", value));

        if (ReadBit(number, bit) != value)
        {
            if (value == 0)
                number -= 1 << bit;
            else
                number += 1 << bit;
        }
    }
}

public struct TimeUtility
{
    public static int localTime
    {
        get
        {
            return (int)decimal.Divide(DateTime.UtcNow.Ticks - 621355968000000000, 10000000);
        }
    }
}

internal struct AStarBlock
{
    public float distance;
    public int x;
    public int y;

    AStarBlock(float _distance, int _x, int _y)
    {
        distance = _distance;
        x = _x;
        y = _y;
    }
}

public struct MapUtility
{
    public static List<int[,]> FindPathBetween(int[] startPoint, int[] endPoint, bool[] isBlocked)
    {
        int length = isBlocked.GetLength(0);
        int width = isBlocked.GetLength(1);
        int[,] distance = new int[length, width];

        for (int x = 0; x < length; x++)
            for (int y = 0; y < width; y++)
                distance[x, y] = ManhattanDistance(startPoint, endPoint);

        int[] currentPoint = null;

        BinaryHeap<int[]>.CompareFunction compare = delegate (int[] pointA, int[] pointB)
        {
            int FA = ManhattanDistance(currentPoint, pointA) + distance[pointA[0], pointA[1]];
            int FB = ManhattanDistance(currentPoint, pointB) + distance[pointB[0], pointB[1]];

            return FA < FB;
        };

        BinaryHeap<int[]> openPoints = new BinaryHeap<int[]>(compare);

        openPoints.Push(startPoint);
        while (!openPoints.IsEmpty())
        {
            currentPoint = openPoints.Pop();

        }

        return null;
    }

    public static int ManhattanDistance(int[] startPoint, int[] endPoint)
    {
        int distance = 0;
        for (int d = 0; d < startPoint.Length; d++)
            distance += Mathf.Abs(startPoint[d] - endPoint[d]);

        return distance;
    }
}

