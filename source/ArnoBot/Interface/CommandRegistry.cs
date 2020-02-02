using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Interface
{
    public class CommandRegistry : IDictionary<string, ICommand>, IReadOnlyCommandRegistry
    {
        private const string ARGUMENT_INVALID_EX_MSG = "A command key must start with an alphanumeric character and can not be empty.";

        private Dictionary<string, ICommand> internalDictionary = new Dictionary<string, ICommand>();

        public ICommand this[string key]
        {
            get => internalDictionary[key.ToLower()];
            set
            {
                if (IsValidKey(key))
                    internalDictionary[key.ToLower()] = value;
                else
                    throw new ArgumentException(ARGUMENT_INVALID_EX_MSG);
            }
        }

        public ICollection<string> Keys => internalDictionary.Keys;

        public ICollection<ICommand> Values => internalDictionary.Values;

        public int Count => internalDictionary.Count;

        bool ICollection<KeyValuePair<string, ICommand>>.IsReadOnly { get => false; }

        IEnumerable<string> IReadOnlyDictionary<string, ICommand>.Keys => (internalDictionary as IReadOnlyDictionary<string, ICommand>).Keys;

        IEnumerable<ICommand> IReadOnlyDictionary<string, ICommand>.Values => (internalDictionary as IReadOnlyDictionary<string, ICommand>).Values;

        public void Add(string key, ICommand value)
        {
            if (IsValidKey(key))
                internalDictionary.Add(key.ToLower(), value);
            else
                throw new ArgumentException(ARGUMENT_INVALID_EX_MSG);
        }

        public void Add(KeyValuePair<string, ICommand> item)
            => Add(item.Key, item.Value);

        public void Clear()
            => internalDictionary.Clear();

        bool ICollection<KeyValuePair<string, ICommand>>.Contains(KeyValuePair<string, ICommand> item)
            => (internalDictionary as ICollection<KeyValuePair<string, ICommand>>).Contains(new KeyValuePair<string, ICommand>(item.Key.ToLower(), item.Value));

        public bool ContainsKey(string key)
            => internalDictionary.ContainsKey(key.ToLower());

        void ICollection<KeyValuePair<string, ICommand>>.CopyTo(KeyValuePair<string, ICommand>[] array, int arrayIndex)
            => (internalDictionary as ICollection<KeyValuePair<string, ICommand>>).CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<string, ICommand>> GetEnumerator()
            => internalDictionary.GetEnumerator();

        public bool Remove(string key)
            => internalDictionary.Remove(key.ToLower());

        public bool Remove(KeyValuePair<string, ICommand> item)
            => Remove(item.Key);

        public bool TryGetValue(string key, out ICommand value)
            => internalDictionary.TryGetValue(key.ToLower(), out value);

        IEnumerator IEnumerable.GetEnumerator()
            => (internalDictionary as IEnumerable).GetEnumerator();

        private bool IsValidKey(string key)
        {
            return key.Length > 0
                && char.IsLetterOrDigit(key[0]);
        }
    }

    public interface IReadOnlyCommandRegistry : IReadOnlyDictionary<string, ICommand>
    { }
}
