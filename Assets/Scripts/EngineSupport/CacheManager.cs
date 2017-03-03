/*
    NOTE: This is a temporary test for decoupling singleton implementation (ThreadsafeDictionary should not be instantiated only one
    time in some cases.

    This class encapsulates a ThreadsafeDictionary object in order to make it accessible everywhere as a single instance in all
    the project for the purpose of managing cache of any kind of elements.

    The reasons that brought to make this class are managing GameObjects, but this can be used for caching other kind of values too.

    It uses Singleton and Adapter pattern, plus locking mechanism for thread-safe use.

    Usage:
    CacheManager<GameObject> x = CacheManager<GameObject>.getInstance();
    x.Add("go_player_groundcheck",this.transform.FindChild("GroundCheck").gameObject);
    Debug.Log("cached: "+x.Get("go_player_groundcheck").name);

 */
public class CacheManager<V>
{
    private ThreadsafeDictionary<string, V> cache_dictionary;    // Object where data are stored.
    private object lock_op; // Lock for concurrency.

    // Singleton pattern.
    private static class SingletonLoader
    {
        public static readonly CacheManager<V> instance = new CacheManager<V>();
    }

    public static CacheManager<V> getInstance()
    {
        return SingletonLoader.instance;
    }

    // Initialize the dictionary that this class encapsulates.
    public CacheManager()
    {
        cache_dictionary = new ThreadsafeDictionary<string, V>();
        lock_op = new object();
    }

    public void Add(string key, V value, bool force_rewrite = false)
    {
        lock (lock_op)
        {
            cache_dictionary.Add(key, value, force_rewrite);
        }
    }

    public void Remove(string key)
    {
        lock (lock_op)
        {
            cache_dictionary.Remove(key);
        }
    }

    public V Get(string key)
    {
        lock (lock_op)
        {
            return cache_dictionary.Get(key);
        }
    }



}