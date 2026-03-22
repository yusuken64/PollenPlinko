using System;
using System.Collections.Generic;

public class SimplePool<T>
{
    private readonly Func<T> _createFunc;
    private readonly Stack<T> _stack = new();
    private readonly HashSet<T> _inPool = new();

    public bool LimitCount = false;
    public int Limit;
    private int _count;

    public SimplePool(Func<T> createFunc)
    {
        _createFunc = createFunc;
    }

    public T Get()
    {
        if (_stack.Count == 0)
        {
            if (LimitCount && _count >= Limit)
                return default; // or throw / reuse depending on your design

            var obj = _createFunc();
            _count++;
            return obj;
        }

        var pooled = _stack.Pop();
        _inPool.Remove(pooled);
        return pooled;
    }

    public void Release(T obj)
    {
        if (!_inPool.Add(obj))
            return; // already released

        _stack.Push(obj);
    }
}