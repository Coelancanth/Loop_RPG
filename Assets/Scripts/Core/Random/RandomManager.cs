using UnityEngine;

public class RandomManager : MonoBehaviour
{
    private static RandomManager _instance;
    public static RandomManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("RandomManager");
                _instance = go.AddComponent<RandomManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private System.Random _random;
    private int _currentSeed;

    public int CurrentSeed => _currentSeed;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize(int seed)
    {
        _currentSeed = seed;
        _random = new System.Random(seed);
        Debug.Log($"RandomManager initialized with seed: {seed}");
    }

    public int GetRandomInt(int minInclusive, int maxExclusive)
    {
        EnsureInitialized();
        return _random.Next(minInclusive, maxExclusive);
    }

    public float GetRandomFloat(float minInclusive, float maxInclusive)
    {
        EnsureInitialized();
        float randomValue = (float)_random.NextDouble();
        return minInclusive + (randomValue * (maxInclusive - minInclusive));
    }

    public bool GetRandomBool()
    {
        EnsureInitialized();
        return _random.Next(2) == 1;
    }

    public Vector2 GetRandomVector2(float minInclusive, float maxInclusive)
    {
        return new Vector2(
            GetRandomFloat(minInclusive, maxInclusive),
            GetRandomFloat(minInclusive, maxInclusive)
        );
    }

    public Vector3 GetRandomVector3(float minInclusive, float maxInclusive)
    {
        return new Vector3(
            GetRandomFloat(minInclusive, maxInclusive),
            GetRandomFloat(minInclusive, maxInclusive),
            GetRandomFloat(minInclusive, maxInclusive)
        );
    }

    private void EnsureInitialized()
    {
        if (_random == null)
        {
            Initialize(Random.Range(int.MinValue, int.MaxValue));
        }
    }
} 