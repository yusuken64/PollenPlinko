using System;
using System.Collections.Generic;

public class SimplePool<T>
{
    private readonly Func<T> _createFunc;
    private readonly Stack<T> _stack = new Stack<T>();

    public SimplePool(Func<T> createFunc)
    {
        _createFunc = createFunc;
    }

    public T Get()
    {
        return _stack.Count > 0 ? _stack.Pop() : _createFunc();
    }

    public void Release(T obj)
    {
        _stack.Push(obj);
    }
}