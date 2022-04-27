namespace Assignment1
{
    public interface IPriorityKey
    {
        public float Key { get; }
    }

    public interface IDynamicKey : IPriorityKey
    {
        public event System.Action OnKeyChanged;
    }
}