public class BinaryHeap<T>
{
    public delegate bool CompareFunction(T a, T b);

    private int _count;
    private T[] _data;
    private int _capacity;
    private CompareFunction _compare;

    private readonly static int ROOT_INDEX = 1;

    public int rootIndex
    {
        get
        {
            return ROOT_INDEX;
        }
    }
    public int count
    {
        get
        {
            return _count - ROOT_INDEX;
        }
    }
    public T[] data
    {
        get
        {
            return _data;
        }
    }

    public BinaryHeap()
    {
        _count = ROOT_INDEX;
        _capacity = 64;

        _data = new T[_capacity];
        for (int i = 0; i < ROOT_INDEX; i++)
            _data[i] = default(T);
    }

    public BinaryHeap(CompareFunction compare) : this()
    {        
        _compare = compare;
    }

    public void Push(T newData)
    {
        if (_count == _capacity)
            IncreaseCapacity();

        _data[_count] = newData;
        HeapifyUp(_count++);
    }

    public T Pop()
    {
        if (IsEmpty())
            return default(T);

        T result = _data[ROOT_INDEX];

        Swap(ROOT_INDEX, --_count);
        HeapifyDown(ROOT_INDEX);

        return result;
    }

    public bool IsEmpty()
    {
        return _count == ROOT_INDEX;
    }

    private void HeapifyUp(int i)
    {
        if (i == ROOT_INDEX)
            return;

        int parentIndex = GetParentIndex(i);
        if (CompareAndSwap(i, parentIndex))
            HeapifyUp(parentIndex);
    }

    private void HeapifyDown(int i)
    {
        if (!HasChildren(i))
            return;

        int childIndexWithHigherPriority = GetChildIndexWithHigherPriority(i);
        if (CompareAndSwap(childIndexWithHigherPriority, i))
            HeapifyDown(childIndexWithHigherPriority);
    }

    private bool HasHigherPriorityThan(int i, int j)
    {
        return _compare(_data[i], _data[j]);
    }

    private int GetParentIndex(int i)
    {
        return i >> 1;
    }

    private int GetLeftChildIndex(int i)
    {
        return i << 1;
    }

    private int GetRightChildIndex(int i)
    {
        return GetLeftChildIndex(i) + 1;
    }

    private bool IsValidIndex(int i)
    {
        return i >= ROOT_INDEX && i < _count;
    }

    private bool HasChildren(int i)
    {
        return IsValidIndex(GetLeftChildIndex(i));
    }

    private int GetChildIndexWithHigherPriority(int i)
    {
        int leftChildIndex = GetLeftChildIndex(i);
        int rightChildIndex = GetRightChildIndex(i);

        return IsValidIndex(rightChildIndex) ? (HasHigherPriorityThan(rightChildIndex, leftChildIndex) ? rightChildIndex : leftChildIndex) : leftChildIndex;
    }

    private bool CompareAndSwap(int i, int j)
    {
        if (HasHigherPriorityThan(i, j))
        {
            Swap(i, j);
            return true;
        }

        return false;
    }

    private void Swap(int i, int j)
    {
        T temp = _data[i];
        _data[i] = _data[j];
        _data[j] = temp;
    }

    private void IncreaseCapacity()
    {
        _capacity *= 2;
        T[] newDataBuffer = new T[_capacity];

        _data.CopyTo(newDataBuffer, 0);
        _data = newDataBuffer;
    }
}

