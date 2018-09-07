using NUnit.Framework;
using System;
using System.Collections.Generic;

using Grid = HexGrid<IntVector2>;

public class HexagonGridTest
{
    private static readonly IntVector2[,] testRectangleData2x2 = new IntVector2[2, 2] { { new IntVector2(0, 0), new IntVector2(1, 0) },
                                                                                        { new IntVector2(0, 1), new IntVector2(1, 1) } };
    private static readonly IntVector2[,] testHexagonData4x3 = new IntVector2[3, 9] { { new IntVector2(-2, 0), new IntVector2(-1, 0), new IntVector2(0, 0), new IntVector2(1, 0), new IntVector2( 2,  0), new IntVector2(3,  0), new IntVector2(1,  0), new IntVector2(2,  0), new IntVector2(3,  0) },
                                                                                      { new IntVector2(-2, 1), new IntVector2(-1, 1), new IntVector2(0, 1), new IntVector2(1, 1), new IntVector2( 2,  1), new IntVector2(0, -2), new IntVector2(1, -2), new IntVector2(2, -2), new IntVector2(3, -2) },
                                                                                      { new IntVector2(-2, 2), new IntVector2(-1, 2), new IntVector2(0, 2), new IntVector2(1, 2), new IntVector2(-1, -1), new IntVector2(0, -1), new IntVector2(1, -1), new IntVector2(2, -1), new IntVector2(3, -1) }  };

    private static readonly Grid testRectangleGrid2x2 = new Grid(testRectangleData2x2, HexGridShape.RECTANGLE, HexGridType.FLAT_TOPPED);
    private static readonly Grid testHexagonGrid4x3 = new Grid(testHexagonData4x3, HexGridShape.HEXAGON, HexGridType.FLAT_TOPPED);

    [Test] public void BuildGridWithData()
    {
        Assert.AreEqual(testHexagonGrid4x3.length, 4, "[4x3 Hexagon] The length of constructed grid is incorrect");
        Assert.AreEqual(testHexagonGrid4x3.width, 3, "[4x3 Hexagon] The width of constructed grid is incorrect");
    }

    [Test] public void AccessCells()
    {
        for (int y = -2; y < 3; y++)
            for (int x = -2 - Math.Min(0, y); x < 4 - Math.Max(0, y); x++)
            {
                string message = string.Format("[4x3 Hexagon] Incorrect cell (@({0}, {1}))", x, y);

                Assert.AreEqual(testHexagonGrid4x3[x, y].ToString(), string.Format("({0}, {1})", x, y), "[x, y]\n" + message);
                Assert.AreEqual(testHexagonGrid4x3[new IntVector2(x, y)].ToString(), string.Format("({0}, {1})", x, y), "[(x, y)]\n" + message);
                Assert.AreEqual(testHexagonGrid4x3[x, -x - y, y].ToString(), string.Format("({0}, {1})", x, y), "[x, y, z]\n" + message);
                Assert.AreEqual(testHexagonGrid4x3[new IntVector3(x, -x - y, y)].ToString(), string.Format("({0}, {1})", x, y), "[(x, y, z)]\n" + message);
            }
    }

    [Test] public void GetNeighbor()
    {
        int x = 0;
        int y = 0;
        int z = 0;

        for (int direction = 0; direction < 6; direction++)
        {
            IntVector2 expected = Grid.GetAxialDirection(direction);
            string message = string.Format("[4x3 Hexagon] Incorrect neighbor (@({0}, {1})  direction = {2})", x, z, direction);

            Assert.AreEqual(testHexagonGrid4x3.GetNeighbor(x, z, direction).ToString(), expected.ToString(), "GetNeighbor(x, y, direction)\n" + message);
            Assert.AreEqual(testHexagonGrid4x3.GetNeighbor(new IntVector2(x, z), direction).ToString(), expected.ToString(), "GetNeighbor((x, y), direction)\n" + message);
            Assert.AreEqual(testHexagonGrid4x3.GetNeighbor(x, y, z, direction).ToString(), expected.ToString(), "GetNeighbor(x, y, z, direction)\n" + message);
            Assert.AreEqual(testHexagonGrid4x3.GetNeighbor(new IntVector3(x, y, z), direction).ToString(), expected.ToString(), "GetNeighbor((x, y, z), direction)\n" + message);
        }
    }

    [Test] public void GetNeighbors2()
    {
        TestGetNeighbors(testRectangleGrid2x2, 0, 0, new IntVector2[2] { new IntVector2(1, 0),
                                                                         new IntVector2(0, 1) });
    }

    [Test] public void GetNeighbors3()
    {
        TestGetNeighbors(testHexagonGrid4x3, -2, 2, new IntVector2[3] { new IntVector2(-1, 2),
                                                                        new IntVector2(-1, 1),
                                                                        new IntVector2(-2, 1) });
    }

    [Test] public void GetNeighbors4()
    {
        TestGetNeighbors(testHexagonGrid4x3, -1, 2, new IntVector2[4] { new IntVector2( 0, 2),
                                                                        new IntVector2( 0, 1),
                                                                        new IntVector2(-1, 1),
                                                                        new IntVector2(-2, 2) });
    }

    [Test] public void GetNeighbors6()
    {
        TestGetNeighbors(testHexagonGrid4x3, 0, 0, new IntVector2[6] { new IntVector2( 1,  0),
                                                                       new IntVector2( 1, -1),
                                                                       new IntVector2( 0, -1),
                                                                       new IntVector2(-1,  0),
                                                                       new IntVector2(-1,  1),
                                                                       new IntVector2( 0,  1) });
    }

    [Test] public void Distance()
    {
        TestDistance(0, 0, 0, 0,  0, 0, 0);
        TestDistance(0, 0, 0, 1, -1, 0, 1);
        TestDistance(0, 0, 0, 0, -2, 2, 2);
        TestDistance(0, 0, 0, 1, -3, 2, 3);
    }

    private void TestGetNeighbors(HexGrid<IntVector2> hexagonGrid, int x, int y, IntVector2[] expected)
    {
        IntVector2[] result1 = hexagonGrid.GetNeighbors(x, y);
        IntVector2[] result2 = hexagonGrid.GetNeighbors(new IntVector2(x, y));
        IntVector2[] result3 = hexagonGrid.GetNeighbors(x, -x - y, y);
        IntVector2[] result4 = hexagonGrid.GetNeighbors(new IntVector3(x, -x - y, y));

        string message = string.Format("[{0}x{1} Hexagon] Incorrect number of neighbors (@({2}, {3}))", hexagonGrid.length, hexagonGrid.width, x, y);

        int expectedLength = expected.Length;

        Assert.AreEqual(result1.Length, expectedLength, "GetNeighbors(x, y)\n" + message);
        Assert.AreEqual(result2.Length, expectedLength, "GetNeighbors((x, y))\n" + message);
        Assert.AreEqual(result3.Length, expectedLength, "GetNeighbors(x, y, z)\n" + message);
        Assert.AreEqual(result4.Length, expectedLength, "GetNeighbors((x, y, z))\n" + message);

        for (int i = 0; i < expectedLength; i++)
        {
            message = string.Format("[{0}x{1} Hexagon] Incorrect neighbor (@{2}, {3}  i = {4})", hexagonGrid.length, hexagonGrid.width, x, y, i);

            Assert.AreEqual(result1[i].ToString(), expected[i].ToString(), "GetNeighbors(x, y)\n" + message);
            Assert.AreEqual(result2[i].ToString(), expected[i].ToString(), "GetNeighbors((x, y))\n" + message);
            Assert.AreEqual(result3[i].ToString(), expected[i].ToString(), "GetNeighbors(x, y, z)\n" + message);
            Assert.AreEqual(result4[i].ToString(), expected[i].ToString(), "GetNeighbors((x, y, z))\n" + message);
        }
    }

    private static void TestDistance(int xA, int yA, int zA, int xB, int yB, int zB, int expected)
    {
        string message = string.Format("[4x3 Hexagon] Incorrect distance (({0}, {1}) --> ({2}, {3}))", xA, yA, xB, yB);

        Assert.AreEqual(Grid.Distance(xA, zA, xB, zB), expected, "Distance(xA, yA, xB, yB)\n" + message);
        Assert.AreEqual(Grid.Distance(new IntVector2(xA, zA), new IntVector2(xB, zB)), expected, "Distance((xA, yA), (xB, yB))\n" + message);
        Assert.AreEqual(Grid.Distance(xA, yA, zA, xB, yB, zB), expected, "Distance(xA, yA, zA, xB, yB, zB)\n" + message);
        Assert.AreEqual(Grid.Distance(new IntVector3(xA, yA, zA), new IntVector3(xB, yB, zB)), expected, "Distance((xA, yA, zA), (xB, yB, zB))\n" + message);
    }
}
