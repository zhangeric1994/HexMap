using UnityEngine;
using System;

[Serializable] public struct IntVector2 : IEquatable<IntVector2>
{
    #region Member Variables

    public int x;
    public int y;

    #endregion Member Variables
    #region Accessors and Mutators

    public int this[int dimension]
    {
        get
        {
            switch (dimension)
            {
                case 0:
                    return this.x;
                case 1:
                    return this.y;
                default:
                    throw new IndexOutOfRangeException(string.Format("Invalid dimension ({0})", dimension));
            }
        }

        set
        {
            switch (dimension)
            {
                case 0:
                    this.x = value;
                    break;
                case 1:
                    this.y = value;
                    break;
                default:
                    throw new IndexOutOfRangeException(string.Format("Invalid dimension ({0})", dimension));
            }
        }
    }

    public int sqrMagnitude
    {
        get
        {
            return this.x * this.x + this.y * this.y;
        }
    }

    public float magnitude
    {
        get
        {
            return Mathf.Sqrt(sqrMagnitude);
        }
    }

    #endregion Accessors and Mutators
    #region Constructors

    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public IntVector2(int[] coordinates)
    {
        this.x = coordinates[0];
        this.y = coordinates[1];
    }

    public IntVector2(IntVector2 other)
    {
        this.x = other.x;
        this.y = other.y;
    }

    #endregion Constructors
    #region Operators

    public static bool operator ==(IntVector2 left, IntVector2 right)
    {
        return left.x == right.x && left.y == right.y;
    }

    public static bool operator !=(IntVector2 left, IntVector2 right)
    {
        return left.x != right.x || left.y != right.y;
    }

    public static IntVector2 operator +(IntVector2 left, IntVector2 right)
    {
        return new IntVector2(left.x + right.x, left.y + right.y);
    }

    public static IntVector2 operator -(IntVector2 v)
    {
        return new IntVector2(-v.x, -v.y);
    }

    public static IntVector2 operator -(IntVector2 left, IntVector2 right)
    {
        return new IntVector2(left.x - right.x, left.y - right.y);
    }

    public static int operator *(IntVector2 left, IntVector2 right)
    {
        return left.x * right.x + left.y * right.y;
    }

    public static IntVector2 operator *(IntVector2 v, int factor)
    {
        return new IntVector2(v.x * factor, v.y * factor);
    }

    public static Vector2 operator *(IntVector2 v, float factor)
    {
        return new Vector2(v.x * factor, v.y * factor);
    }

    public static IntVector2 operator *(int factor, IntVector2 v)
    {
        return new IntVector2(v.x * factor, v.y * factor);
    }

    public static IntVector2 operator /(IntVector2 v, float divider)
    {
        return new IntVector2(Mathf.FloorToInt(v.x / divider), Mathf.FloorToInt(v.y / divider));
    }

    public static implicit operator Vector2(IntVector2 v)
    {
        return new Vector2(v.x, v.y);
    }

    #endregion Operators
    #region Public Non-static Functions

    public override string ToString()
    {
        return string.Format("({0}, {1})", this.x, this.y);
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
    }

    public bool Equals(IntVector2 other)
    {
        return this.x.Equals(other.x) && this.y.Equals(other.y);
    }

    public override bool Equals(object o)
    {
        if (!(o is IntVector2))
            return false;

        return this.Equals((IntVector2)o);
    }

    #endregion Public Functions
    #region Public Static Functions

    public static Vector2 Lerp(IntVector2 a, IntVector2 b, float t)
    {
        return new Vector2(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t));
    }

    public static int ManhattanDistance(IntVector3 A, IntVector3 B)
    {
        return Math.Abs(A.x - B.x) + Math.Abs(A.y - B.y);
    }

    public static float EuclideanDistance(IntVector3 A, IntVector3 B)
    {
        int dx = A.x - B.x;
        int dy = A.y - B.y;

        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    public static int ChebyshevDistance(IntVector3 A, IntVector3 B)
    {
        return Math.Max(Math.Abs(A.x - B.x), Math.Abs(A.y - B.y));
    }

    #endregion Public Static Functions
}

[Serializable] public struct IntVector3 : IEquatable<IntVector3>
{
    #region Member Variables

    public int x;
    public int y;
    public int z;

    #endregion Member Variables
    #region Accessors and Mutators

    public int this[int dimension]
    {
        get
        {
            if (dimension == 0)
                return this.x;
            else if (dimension == 1)
                return this.y;
            else if (dimension == 2)
                return this.z;
            else
                throw new IndexOutOfRangeException(string.Format("Invalid dimension! (d = {0})", dimension));
        }

        set
        {
            if (dimension == 0)
                this.x = value;
            else if (dimension == 1)
                this.y = value;
            else if (dimension == 2)
                this.z = value;
            else
                throw new IndexOutOfRangeException(string.Format("Invalid dimension! (d = {0})", dimension));
        }
    }

    public int squaredNorm
    {
        get
        {
            return this.x * this.x + this.y * this.y + this.z * this.z;
        }
    }

    public float norm
    {
        get
        {
            return Mathf.Sqrt(squaredNorm);
        }
    }

    public Vector2 direction
    {
        get
        {
            float n = this.norm;
            return new Vector3(this.x / n, this.y / n, this.z / n);
        }
    }

    public static int maxDimension
    {
        get
        {
            return 3;
        }
    }

    public static IntVector3 zero
    {
        get
        {
            return new IntVector3(0, 0, 0);
        }
    }

    #endregion Accessors and Mutators
    #region Constructors

    public IntVector3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public IntVector3(int[] coordinates)
    {
        this.x = coordinates[0];
        this.y = coordinates[1];
        this.z = coordinates[2];
    }

    public IntVector3(IntVector3 other)
    {
        this.x = other.x;
        this.y = other.y;
        this.z = other.z;
    }

    #endregion Constructors
    #region Operators

    public static bool operator ==(IntVector3 left, IntVector3 right)
    {
        return left.x == right.x && left.y == right.y && left.z == right.z;
    }

    public static bool operator !=(IntVector3 left, IntVector3 right)
    {
        return left.x != right.x || left.y != right.y || left.z != right.z;
    }

    public static IntVector3 operator +(IntVector3 left, IntVector3 right)
    {
        return new IntVector3(left.x + right.x, left.y + right.y, left.z + right.z);
    }

    public static IntVector3 operator -(IntVector3 v)
    {
        return new IntVector3(-v.x, -v.y, -v.z);
    }

    public static IntVector3 operator -(IntVector3 left, IntVector3 right)
    {
        return new IntVector3(left.x - right.x, left.y - right.y, left.z - right.z);
    }

    public static int operator *(IntVector3 left, IntVector3 right)
    {
        return left.x * right.x + left.y * right.y + left.z * right.z;
    }

    public static IntVector3 operator *(IntVector3 v, int factor)
    {
        return new IntVector3(v.x * factor, v.y * factor, v.z * factor);
    }

    public static Vector3 operator *(IntVector3 v, float factor)
    {
        return new Vector3(v.x * factor, v.y * factor, v.z * factor);
    }

    public static IntVector3 operator *(int factor, IntVector3 v)
    {
        return new IntVector3(v.x * factor, v.y * factor, v.z * factor);
    }

    public static IntVector3 operator /(IntVector3 v, float divider)
    {
        return new IntVector3(Mathf.FloorToInt(v.x / divider), Mathf.FloorToInt(v.y / divider), Mathf.FloorToInt(v.z / divider));
    }

    public static implicit operator Vector3(IntVector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    #endregion Operators
    #region Public Non-static Functions

    public override string ToString()
    {
        return string.Format("({0}, {1}, {2})", this.x, this.y, this.z);
    }

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode() << 2 ^ this.z.GetHashCode() >> 2;
    }

    public bool Equals(IntVector3 other)
    {
        return this.x.Equals(other.x) && this.y.Equals(other.y) && this.z.Equals(other.z);
    }

    public override bool Equals(object o)
    {
        if (!(o is IntVector3))
            return false;

        return this.Equals((IntVector3)o);
    }

    #endregion Public Functions
    #region Public Static Functions

    public static Vector3 Lerp(IntVector3 a, IntVector3 b, float t, float epsilon = Vector3.kEpsilon)
    {
        return new Vector3(Mathf.Lerp(a.x + epsilon, b.x + epsilon, t), Mathf.Lerp(a.y + epsilon, b.y + epsilon, t), Mathf.Lerp(a.z - 2 * epsilon, b.z - 2 * epsilon, t));
    }

    public static IntVector3 Round(Vector3 v)
    {
        int x = Mathf.RoundToInt(v.x);
        int y = Mathf.RoundToInt(v.y);
        int z = Mathf.RoundToInt(v.z);

        float xf = Mathf.Abs(x - v.x);
        float yf = Mathf.Abs(y - v.y);
        float zf = Mathf.Abs(z - v.z);

        if (xf > yf && xf > zf)
            x = -y - z;
        else if (yf > zf)
            y = -x - z;
        else
            z = -x - y;

        return new IntVector3(x, y, z);
    }

    public static int ManhattanDistance(IntVector3 A, IntVector3 B)
    {
        return Math.Abs(A.x - B.x) + Math.Abs(A.y - B.y) + Math.Abs(A.z - B.z);
    }

    public static float EuclideanDistance(IntVector3 A, IntVector3 B)
    {
        int dx = A.x - B.x;
        int dy = A.y - B.y;
        int dz = A.z - B.z;

        return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public static int ChebyshevDistance(IntVector3 A, IntVector3 B)
    {
        return Math.Max(Math.Abs(A.x - B.x), Math.Max(Math.Abs(A.y - B.y), Math.Abs(A.z - B.z)));
    }

    #endregion Public Static Functions
}
