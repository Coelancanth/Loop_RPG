using NUnit.Framework;
using UnityEngine;

public class RandomManagerTests
{
    private GameObject _gameObject;
    private RandomManager _randomManager;

    [SetUp]
    public void Setup()
    {
        _gameObject = new GameObject();
        _randomManager = _gameObject.AddComponent<RandomManager>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_gameObject);
    }

    [Test]
    public void SameSeed_GeneratesSameNumbers()
    {
        // Arrange
        const int seed = 12345;
        _randomManager.Initialize(seed);
        
        // Store first set of random numbers
        int int1 = _randomManager.GetRandomInt(0, 100);
        float float1 = _randomManager.GetRandomFloat(0f, 1f);
        bool bool1 = _randomManager.GetRandomBool();
        Vector2 vector21 = _randomManager.GetRandomVector2(-1f, 1f);
        Vector3 vector31 = _randomManager.GetRandomVector3(-1f, 1f);

        // Reinitialize with same seed
        _randomManager.Initialize(seed);
        
        // Generate second set of numbers
        int int2 = _randomManager.GetRandomInt(0, 100);
        float float2 = _randomManager.GetRandomFloat(0f, 1f);
        bool bool2 = _randomManager.GetRandomBool();
        Vector2 vector22 = _randomManager.GetRandomVector2(-1f, 1f);
        Vector3 vector32 = _randomManager.GetRandomVector3(-1f, 1f);

        // Assert
        Assert.That(int2, Is.EqualTo(int1), "Random integers should be equal with same seed");
        Assert.That(float2, Is.EqualTo(float1), "Random floats should be equal with same seed");
        Assert.That(bool2, Is.EqualTo(bool1), "Random bools should be equal with same seed");
        Assert.That(vector22, Is.EqualTo(vector21), "Random Vector2s should be equal with same seed");
        Assert.That(vector32, Is.EqualTo(vector31), "Random Vector3s should be equal with same seed");
    }

    [Test]
    public void DifferentSeeds_GenerateDifferentNumbers()
    {
        // Arrange
        const int seed1 = 12345;
        const int seed2 = 54321;
        bool foundDifference = false;

        // Act
        _randomManager.Initialize(seed1);
        var numbers1 = new float[100];
        for (int i = 0; i < 100; i++)
        {
            numbers1[i] = _randomManager.GetRandomFloat(0f, 1f);
        }

        _randomManager.Initialize(seed2);
        var numbers2 = new float[100];
        for (int i = 0; i < 100; i++)
        {
            numbers2[i] = _randomManager.GetRandomFloat(0f, 1f);
            if (!Mathf.Approximately(numbers1[i], numbers2[i]))
            {
                foundDifference = true;
                break;
            }
        }

        // Assert
        Assert.That(foundDifference, Is.True, "Different seeds should generate different number sequences");
    }

    [Test]
    public void GetRandomInt_ReturnsNumbersWithinRange()
    {
        // Arrange
        const int min = -50;
        const int max = 50;
        const int iterations = 1000;
        
        _randomManager.Initialize(12345);

        // Act & Assert
        for (int i = 0; i < iterations; i++)
        {
            int value = _randomManager.GetRandomInt(min, max);
            Assert.That(value, Is.GreaterThanOrEqualTo(min));
            Assert.That(value, Is.LessThan(max));
        }
    }

    [Test]
    public void GetRandomFloat_ReturnsNumbersWithinRange()
    {
        // Arrange
        const float min = -1f;
        const float max = 1f;
        const int iterations = 1000;
        
        _randomManager.Initialize(12345);

        // Act & Assert
        for (int i = 0; i < iterations; i++)
        {
            float value = _randomManager.GetRandomFloat(min, max);
            Assert.That(value, Is.GreaterThanOrEqualTo(min));
            Assert.That(value, Is.LessThanOrEqualTo(max));
        }
    }

    [Test]
    public void GetRandomBool_GeneratesBothTrueAndFalse()
    {
        // Arrange
        const int iterations = 1000;
        bool foundTrue = false;
        bool foundFalse = false;
        
        _randomManager.Initialize(12345);

        // Act
        for (int i = 0; i < iterations; i++)
        {
            bool value = _randomManager.GetRandomBool();
            if (value) foundTrue = true;
            else foundFalse = true;

            if (foundTrue && foundFalse) break;
        }

        // Assert
        Assert.That(foundTrue, Is.True, "Should generate at least one true value");
        Assert.That(foundFalse, Is.True, "Should generate at least one false value");
    }
} 