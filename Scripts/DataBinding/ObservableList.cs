using System.Collections;
using System.Collections.Generic;

namespace MVVMBase.DataBinding
{
    public class ObservableList<T>:IList<T>
    {

        public delegate void ValueChangedHandler(List<T> oldValue, List<T> newValue);
        public ValueChangedHandler OnValueChanged;

        public delegate void AddHandler(T instance);
        public AddHandler OnAdd;

        public delegate void InsertHandler(int index,T instance);
        public InsertHandler OnInsert;

        public delegate void RemoveHandler(T instance);
        public RemoveHandler OnRemove;
       
        //预先初始化，防止空异常
        private List<T> _value=new List<T>();
        public List<T> Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value)) return;
                var old = _value;
                _value = value;

                ValueChanged(old, _value);
            }
        }


        private void ValueChanged(List<T> oldValue, List<T> newValue)
        {
            OnValueChanged?.Invoke(oldValue, newValue);
        }


        public IEnumerator<T> GetEnumerator()
        {
            return _value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _value.Add(item);
            OnAdd?.Invoke(item);
        }

        public void Clear()
        {
            _value.Clear();
        }

        public bool Contains(T item)
        {
            return _value.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _value.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (!_value.Remove(item)) return false;
            OnRemove?.Invoke(item);
            return true;
        }

        public int Count => _value.Count;

        public bool IsReadOnly { get; private set; }
        public int IndexOf(T item)
        {
            return _value.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _value.Insert(index,item);
            OnInsert?.Invoke(index, item);
        }

        public void RemoveAt(int index)
        {
           _value.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _value[index];
            set => _value[index] = value;
        }
    }
}
