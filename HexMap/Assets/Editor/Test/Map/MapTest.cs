using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;

public class MapTest
{
	[Test] public void GenerateRandomInt()
	{
		Map map = new Map();

        int min = -5;
        int max = 5;
        int n = 100;

        for (int i = 0; i < n; i++)
        {
            Assert.GreaterOrEqual(map.GenerateRandomInt(min, max), -5);
            Assert.LessOrEqual(map.GenerateRandomInt(min, max), 5);
        }
	}
}
