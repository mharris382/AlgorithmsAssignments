namespace Assignment1
{
    public class RxProperty<T>
    {
        private T _value;
        public delegate void PropertyValueChanged<in T>(T previousValue, T newValue);
        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value) == false) 
                    OnValueChanged?.Invoke(_value, _value = value);
            }
        } 
        public event PropertyValueChanged<T> OnValueChanged;

    }
}