using System;

public interface ISetAction
{
    public void SetAction(Action callback);
}

public interface ISetAction<T> 
{
    public void SetAction(Action<T> callback);
}

public interface ISetAction<T1, T2>
{
    public void SetAction(Action<T1, T2> callback);
}

public interface ISetAction<T1, T2, T3>
{
    public void SetAction(Action<T1, T2, T3> callback);
}