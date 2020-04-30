// Copyright (c) AIR Pty Ltd. All rights reserved.

using System;
using System.Collections;
using AIR.UnityTestPilot.Agents;
using AIR.UnityTestPilot.Drivers;
using AIR.UnityTestPilot.Interactions;
using AIR.UnityTestPilot.Queries;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

[TestFixture]
public class UnityDriverWaitTests
{
    private const string TEST_GO_NAME = "TEST";
    private UnityDriver _driver;
    private GameObject _testGo;

    [SetUp]
    public void Setup()
    {
        var agent = new NativeUnityDriverAgent();
        _driver = new UnityDriver(agent);
    }

    [TearDown]
    public void TearDown()
    {
        _driver.Dispose();
        Object.DestroyImmediate(_testGo);
    }

    [UnityTest]
    public IEnumerator Until_LateElementCreation_ElementFound()
    {
        // Arrange
        var startTime = Time.time;
        const int TIMEOUT = 2, TEST_DELAY = 1;
        yield return DelayedSpawnGO(TEST_GO_NAME, TEST_DELAY);
        UiElement element = null;
        var wait = new UnityDriverWait(_driver, TimeSpan.FromSeconds(TIMEOUT));

        // Act
        yield return wait.Until(
            d => d.FindElement(By.Name(TEST_GO_NAME)),
            (e) => element = e
        );

        // Assert
        Assert.IsNotNull(element);
        Assert.Less(Time.time, startTime + TIMEOUT);
        Assert.Greater(Time.time, startTime);
        Assert.GreaterOrEqual(Time.time, startTime + TEST_DELAY);
    }

    [UnityTest]
    public IEnumerator Until_ElementExists_FindsNextFrame()
    {
        // Arrange
        _testGo = new GameObject(TEST_GO_NAME);
        yield return null;
        const int NEXT_FRAME = 1;
        UiElement element = null;
        var startFrame = Time.frameCount;
        var wait = new UnityDriverWait(_driver, TimeSpan.FromSeconds(1f));

        // Act
        yield return wait.Until(
            d => d.FindElement(By.Name(TEST_GO_NAME)),
            (v) => element = v
        );

        // Assert
        Assert.IsNotNull(element);
        Assert.AreEqual(startFrame + NEXT_FRAME, Time.frameCount);
    }

    [UnityTest]
    public IEnumerator Until_MissingElement_TimesOut()
    {
        // Arrange
        const int TIMEOUT = 1;
        float startTime = Time.time;
        var wait = new UnityDriverWait(_driver, TimeSpan.FromSeconds(TIMEOUT));

        // Act
        yield return wait.Until(
            d => null,
            (e) => { }
        );

        // Assert
        Assert.Greater(Time.time, startTime + TIMEOUT);
    }

    private IEnumerator DelayedSpawnGO(string goName, float delay)
    {
        yield return new WaitForSeconds(delay);
        _testGo = new GameObject(goName);
    }
}