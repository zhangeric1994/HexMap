using UnityEngine;
using NUnit.Framework;
using System;
using System.Collections.Generic;

internal struct TestData
{
    private int _key;
    public int key
    {
        get
        {
            return _key;
        }
    }

    private Char _value;
    public Char value
    {
        get
        {
            return _value;
        }
    }

    public TestData(int key, Char value)
    {
        _key = key;
        _value = value;
    }
}

public class HeapTest
{
    [Test] public void BuildHeap()
    {
        int N = 100;

        System.Random rng = new System.Random((int)Decimal.Divide(DateTime.UtcNow.Ticks - 621355968000000000, 10000000));

        BinaryHeap<int> heap = new BinaryHeap<int>((a, b) => a < b);

        List<int> sortedList = new List<int>();
        for (int i = 0; i < N; i++)
        {
            int r = rng.Next(0, 10);
            heap.Push(r);
            sortedList.Add(r);
        }

        sortedList.Sort();

        string message = "";
        int[] heapData = heap.data;
        int heapSize = heap.count;
        message += string.Format("Original Heap Size: {0}\n Original Heap: ", heapSize);
        for (int i = heap.rootIndex; i < N; i++)
            if (i < heapSize)
                message += string.Format("{0} ", heapData[i]);

        for (int i = 0; i < N; i++)
        {
            int expected = sortedList[i];
            int actual = heap.Pop();

            bool isCorrect = expected == actual;

            Assert.AreEqual(expected, actual, message + string.Format("Current Index: {0}\n", i) + "\n");
        }
    }

    [Test] public void BuildHeap_Hard()
    {
        String input = "Hello World!";
        List<TestData> list = new List<TestData>();
        for (int i = 0; i < input.Length; i++)
            list.Add(new TestData(i, input[i]));

        System.Random rng = new System.Random((int)Decimal.Divide(DateTime.UtcNow.Ticks - 621355968000000000, 10000000));

        BinaryHeap<TestData> heap = new BinaryHeap<TestData>((a, b) => a.key < b.key);
        while (list.Count > 1)
        {
            int i = rng.Next(0, list.Count);
            heap.Push(list[i]);
            list.RemoveAt(i);
        }
        heap.Push(list[0]);

        String output = "";
        for (int i = 0; i < input.Length; i++)
            output += heap.Pop().value;

        Assert.True(input.Equals(output), input + " vs " + output);
    }
}

