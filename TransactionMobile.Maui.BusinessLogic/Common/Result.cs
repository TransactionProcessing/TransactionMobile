namespace TransactionMobile.Maui.BusinessLogic.Common;

public abstract class Result<T> : Result
{
    private T _data;

    protected Result(T data)
    {
        this.Data = data;
    }

    public T Data
    {
        get => Success ? this._data : throw new Exception($"You can't access .{nameof(this.Data)} when .{nameof(Success)} is false");
        set => this._data = value;
    }
}

public abstract class Result
{
    public bool Success { get; protected set; }
    public bool Failure => !Success;
}