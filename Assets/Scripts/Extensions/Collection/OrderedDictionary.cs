using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Alkawa
{
    public class OrderedDictionary<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable,
    IDictionary,
    ICollection
  {
    private OrderedDictionary privateDictionary;

    public OrderedDictionary() => this.privateDictionary = new OrderedDictionary();

    public OrderedDictionary(IDictionary<TKey, TValue> dictionary)
    {
      if (dictionary == null)
        return;
      this.privateDictionary = new OrderedDictionary();
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) dictionary)
        this.privateDictionary.Add((object) keyValuePair.Key, (object) keyValuePair.Value);
    }

    public int Count => this.privateDictionary.Count;

    public bool IsReadOnly => false;

    public TValue this[TKey key]
    {
      get
      {
        if ((object) key == null)
          throw new ArgumentNullException(nameof (key));
        return this.privateDictionary.Contains((object) key) ? (TValue) this.privateDictionary[(object) key] : throw new KeyNotFoundException("Key Not Found In Dictionary");
      }
      set
      {
        if ((object) key == null)
          throw new ArgumentNullException(nameof (key));
        this.privateDictionary[(object) key] = (object) value;
      }
    }

    public ICollection<TKey> Keys
    {
      get
      {
        List<TKey> keys = new List<TKey>(this.privateDictionary.Count);
        foreach (TKey key in (IEnumerable) this.privateDictionary.Keys)
          keys.Add(key);
        return (ICollection<TKey>) keys;
      }
    }

    public ICollection<TValue> Values
    {
      get
      {
        List<TValue> values = new List<TValue>(this.privateDictionary.Count);
        foreach (TValue obj in (IEnumerable) this.privateDictionary.Values)
          values.Add(obj);
        return (ICollection<TValue>) values;
      }
    }

    public void Add(KeyValuePair<TKey, TValue> item) => this.Add(item.Key, item.Value);

    public void Add(TKey key, TValue value)
    {
      if ((object) key == null)
        throw new ArgumentNullException(nameof (key));
      this.privateDictionary.Add((object) key, (object) value);
    }

    public void Clear() => this.privateDictionary.Clear();

    public bool Contains(KeyValuePair<TKey, TValue> item) => (object) item.Key != null && this.privateDictionary.Contains((object) item.Key) && this.privateDictionary[(object) item.Key].Equals((object) item.Value);

    public bool ContainsKey(TKey key) => (object) key != null ? this.privateDictionary.Contains((object) key) : throw new ArgumentNullException(nameof (key));

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex));
      if (array.Rank > 1 || arrayIndex >= array.Length || array.Length - arrayIndex < this.privateDictionary.Count)
        throw new Exception("Bad Copy To Array");
      int index = arrayIndex;
      foreach (DictionaryEntry dictionaryEntry in this.privateDictionary)
      {
        array[index] = new KeyValuePair<TKey, TValue>((TKey) dictionaryEntry.Key, (TValue) dictionaryEntry.Value);
        ++index;
      }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      foreach (DictionaryEntry dictionaryEntry in this.privateDictionary)
        yield return new KeyValuePair<TKey, TValue>((TKey) dictionaryEntry.Key, (TValue) dictionaryEntry.Value);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
      if (!this.Contains(item))
        return false;
      this.privateDictionary.Remove((object) item.Key);
      return true;
    }

    public bool Remove(TKey key)
    {
      if ((object) key == null)
        throw new ArgumentNullException(nameof (key));
      if (!this.privateDictionary.Contains((object) key))
        return false;
      this.privateDictionary.Remove((object) key);
      return true;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      bool flag = (object) key != null ? this.privateDictionary.Contains((object) key) : throw new ArgumentNullException(nameof (key));
      value = flag ? (TValue) this.privateDictionary[(object) key] : default (TValue);
      return flag;
    }

    void IDictionary.Add(object key, object value) => this.privateDictionary.Add(key, value);

    void IDictionary.Clear() => this.privateDictionary.Clear();

    bool IDictionary.Contains(object key) => this.privateDictionary.Contains(key);

    IDictionaryEnumerator IDictionary.GetEnumerator() => this.privateDictionary.GetEnumerator();

    bool IDictionary.IsFixedSize => ((IDictionary) this.privateDictionary).IsFixedSize;

    bool IDictionary.IsReadOnly => this.privateDictionary.IsReadOnly;

    ICollection IDictionary.Keys => this.privateDictionary.Keys;

    void IDictionary.Remove(object key) => this.privateDictionary.Remove(key);

    ICollection IDictionary.Values => this.privateDictionary.Values;

    object IDictionary.this[object key]
    {
      get => this.privateDictionary[key];
      set => this.privateDictionary[key] = value;
    }

    void ICollection.CopyTo(Array array, int index) => this.privateDictionary.CopyTo(array, index);

    int ICollection.Count => this.privateDictionary.Count;

    bool ICollection.IsSynchronized => ((ICollection) this.privateDictionary).IsSynchronized;

    object ICollection.SyncRoot => ((ICollection) this.privateDictionary).SyncRoot;
  }
}