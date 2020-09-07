using AIR.UnityTestPilot.Agents;
using AIR.UnityTestPilot.Drivers;
using AIR.UnityTestPilot.Queries;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class UiElementPositionedObjectTests
{
    private UnityDriver _driver;
    private Transform _testRootGo;

    [SetUp]
    public void SetUp()
    {
        var agent = new NativeUnityDriverAgent();
        _driver = new UnityDriver(agent);
        _testRootGo = new GameObject("TestRoot").transform;
    }

    [Test]
    public void EulerRotation_OnRotatedObject_ReturnsEulerRotation()
    {
        // Arrange
        const string EXPECTED_OBJECT_NAME = nameof(EulerRotation_OnRotatedObject_ReturnsEulerRotation);
        const float EXPECTED_ROTATION = 25f;
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = EXPECTED_OBJECT_NAME;
        var expectedRotation = Quaternion.Euler(EXPECTED_ROTATION, EXPECTED_ROTATION, EXPECTED_ROTATION);
        go.transform.localRotation = expectedRotation;
        var goQuery = _driver.FindElement(By.Type<GameObject>(EXPECTED_OBJECT_NAME));

        // Act
        var actualPos = goQuery.EulerRotation;

        // Assert
        Assert.That(expectedRotation.eulerAngles.x, Is.EqualTo(actualPos.X).Within(float.Epsilon));
        Assert.That(expectedRotation.eulerAngles.y, Is.EqualTo(actualPos.Y).Within(float.Epsilon));
        Assert.That(expectedRotation.eulerAngles.z, Is.EqualTo(actualPos.Z).Within(float.Epsilon));
    }

    [Test]
    public void Position_OnMovedObject_ReturnsObjectPosition()
    {
        // Arrange
        const string EXPECTED_OBJECT_NAME = nameof(Position_OnMovedObject_ReturnsObjectPosition);
        const float EXPECTED_MOVE = 1f;
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = EXPECTED_OBJECT_NAME;
        var expectedMoveOffset = new Vector3(EXPECTED_MOVE, EXPECTED_MOVE, EXPECTED_MOVE);
        go.transform.localPosition = expectedMoveOffset;
        var goQuery = _driver.FindElement(By.Type<GameObject>(EXPECTED_OBJECT_NAME));

        // Act
        var actualPos = goQuery.LocalPosition;

        // Assert
        Assert.That(expectedMoveOffset.x, Is.EqualTo(actualPos.X).Within(float.Epsilon));
        Assert.That(expectedMoveOffset.y, Is.EqualTo(actualPos.Y).Within(float.Epsilon));
        Assert.That(expectedMoveOffset.z, Is.EqualTo(actualPos.Z).Within(float.Epsilon));
    }
}