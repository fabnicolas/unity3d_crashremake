using System.Collections.Generic;

/*
    This class purpose is to replace class "ConcurrentDictionary", not present in .NET 3.5.
    Unity3D, actually (01/03/2017) does not support .NET 4.0, which has thread-safe efficient dictionary implementation.
    This is an attempt to make a thread-safe dictionary using lock mechanism.

    Generic types:
    - K (key);
    - V (value).
 */
public class ThreadsafeDictionary<K, V>
{    // K=key class type, V=value class type.
    private Dictionary<K, V> cache_dictionary;   // Under this class there is a Dictionary taking same properties as the class holder.
    private object lock_op; // Lock used for concurrency issues.

    // Initialize the dictionary by creating it and creating the associated lock object. Each object will have its own lock.
    public ThreadsafeDictionary()
    {
        cache_dictionary = new Dictionary<K, V>(); // The dictionary object.
        lock_op = new object();                 // The lock associated with the object (NOT the class).
    }

    /*
        Add operation for the Dictionary, with an extra:
        - Added lock mechanism for concurrency;
        - Added rewrite parameter (default false): if true and key is already present, it replaces its associated value.
     */
    public void Add(K key, V value, bool force_rewrite = false)
    {
        lock (lock_op)
        {
            if (!cache_dictionary.ContainsKey(key) || force_rewrite)    // If key is present and force_rewrite, or key isn't present...
                cache_dictionary[key] = value;   // Write the value!
        }
    }

    // Remove operation for the Dictionary plus lock mechanism.
    public void Remove(K key)
    {
        lock (lock_op)
        {
            cache_dictionary.Remove(key);
        }
    }

    /*
         Get operation for the Dictionary, instead of accessing directly to the dictionary structure, plus:
        - Added lock mechanism for concurrency;
        - Check first if key is present and in case it exists it returns the value associated to it, otherwise returns default value (usually null).
     */
    public V Get(K key)
    {
        lock (lock_op)
        {
            if (cache_dictionary.ContainsKey(key)) return cache_dictionary[key];
            else return default(V);
        }
    }

    /*
        Chainable add method.
     */
    public ThreadsafeDictionary<K, V> AddChain(K key, V value, bool force_rewrite = false)
    {
        this.Add(key, value, force_rewrite);
        return this;
    }

    /*
        Chainable remove method.
     */
    public ThreadsafeDictionary<K, V> RemoveChain(K key){
        this.Remove(key);
        return this;
    }
}