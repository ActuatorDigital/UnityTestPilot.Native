// Copyright (c) AIR Pty Ltd. All rights reserved.

using System.Collections.Generic;
using AIR.UnityTestPilot.Agents;
using AIR.UnityTestPilot.Drivers;
using AIR.UnityTestPilot.Queries;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

[TestFixture]
public class UnityDriverTests
{
    private const string TEST_GO_NAME = "TestGo";
    private UnityDriver _driver;
    private List<GameObject> _testGos = new List<GameObject>();

    [SetUp]
    public void Setup()
    {
        var agent = new NativeUnityDriverAgent();
        _driver = new UnityDriver(agent);
    }

    [TearDown]
    public void TearDown()
    {
        foreach (var go in _testGos)
            Object.DestroyImmediate(go);
    }

    [Test]
    public void FindElementByPath_ElementPartialNameExists_FindsElement()
    {
        // Arrange
        const string CHILD_GO_NAME = "baby";
        const string CHILD_GO_PARTIAL_QUERY = "*[contains(ab)]";
        var parent = new GameObject(TEST_GO_NAME);
        var child = new GameObject(CHILD_GO_NAME);
        child.transform.parent = parent.transform;
        _testGos.Add(parent);
        _testGos.Add(child);

        // Act
        var uiElement = _driver.FindElement(By.Path(TEST_GO_NAME + "/" + CHILD_GO_PARTIAL_QUERY));

        // Assert
        Assert.IsNotNull(uiElement);
    }

    [Test]
    public void FindElementByPath_NoElementContaints_FindsElement()
    {
        // Arrange
        const string CHILD_GO_NAME = "baby";
        const string CHILD_GO_PARTIAL_QUERY = "*[contains(1234)]";
        var parent = new GameObject(TEST_GO_NAME);
        var child = new GameObject(CHILD_GO_NAME);
        child.transform.parent = parent.transform;
        _testGos.Add(parent);
        _testGos.Add(child);

        // Act
        var uiElement = _driver.FindElement(By.Path(TEST_GO_NAME + "/" + CHILD_GO_PARTIAL_QUERY));

        // Assert
        Assert.IsNull(uiElement);
    }

    [Test]
    public void FindElementByPath_ElementExists_FindsElement()
    {
        // Arrange
        const string CHILD_GO_NAME = "baby";
        var parent = new GameObject(TEST_GO_NAME);
        var child = new GameObject(CHILD_GO_NAME);
        child.transform.parent = parent.transform;
        _testGos.Add(parent);
        _testGos.Add(child);

        // Act
        var uiElement = _driver.FindElement(By.Path(TEST_GO_NAME + "/" + CHILD_GO_NAME));

        // Assert
        Assert.IsNotNull(uiElement);
    }

    [Test]
    public void FindElementByPath_ElementExistsWithFullPathName_FindsElementAsIfNameQuery()
    {
        // Arrange
        const string CHILD_GO_NAME = "baby";
        var parent = new GameObject(TEST_GO_NAME + "/" + CHILD_GO_NAME);
        _testGos.Add(parent);

        // Act
        var uiElement = _driver.FindElement(By.Path(TEST_GO_NAME + "/" + CHILD_GO_NAME));

        // Assert
        Assert.IsNotNull(uiElement);
    }

    [Test]
    public void FindElementByPath_NoElementExists_FindsNothing()
    {
        // Arrange
        const string CHILD_GO_NAME = "baby";

        // Act
        var uiElement = _driver.FindElement(By.Path(TEST_GO_NAME + "/" + CHILD_GO_NAME));

        // Assert
        Assert.IsNull(uiElement);
    }

    [Test]
    public void FindElementByName_ElementExists_FindsElement()
    {
        // Arrange
        _testGos.Add(new GameObject(TEST_GO_NAME));

        // Act
        var uiElement = _driver.FindElement(By.Name(TEST_GO_NAME));

        // Assert
        Assert.IsNotNull(uiElement);
    }

    [Test]
    public void FindElementByName_NoElementExists_FindsNothing()
    {
        // Act
        var uiElement = _driver.FindElement(By.Name(TEST_GO_NAME));

        // Assert
        Assert.IsNull(uiElement);
    }

    [Test]
    public void FindElementByType_ElementExists_FindsElement()
    {
        // Arrange
        var go = new GameObject(TEST_GO_NAME).AddComponent<Button>();
        _testGos.Add(go.gameObject);

        // Act
        var uiElement = _driver.FindElement(By.Type<Button>());

        // Assert
        Assert.IsNotNull(uiElement);
    }

    [Test]
    public void FindElementByType_MultipleTypeHitsExist_FindslastInHierarchy()
    {
        // Arrange
        GameObject last = null;
        for (int i = 0; i < 10; i++) {
            var go = new GameObject(TEST_GO_NAME + "_" + i);
            go.AddComponent<Button>();
            _testGos.Add(go);
            last = go;
        }

        // Act
        var uiElement = _driver.FindElement(By.Type<Button>());

        // Assert
        Assert.IsNotNull(uiElement);
        Assert.IsNotNull(last);
        Assert.AreEqual(last.name, uiElement.Name);
    }

    [Test]
    public void FindElementByTypeWithName_MultipleTypeHitsExist_FindsMatchingName()
    {
        // Arrange
        var targetName = TEST_GO_NAME + "_" + 1;
        for (int i = 0; i < 10; i++) {
            var go = new GameObject(TEST_GO_NAME + "_" + i);
            go.AddComponent<Button>();
            _testGos.Add(go);
        }

        // Act
        var query = By.Type<Button>(targetName);
        var actualElement = _driver.FindElement(query);

        // Assert
        Assert.IsNotNull(actualElement);
        Assert.AreEqual(targetName, actualElement.Name);
    }

    [Test]
    public void FindElementByTypeWithName_MultipleTypeAndNameHitsExist_FindsMatchingName()
    {
        // Arrange
        var targetName = TEST_GO_NAME;
        for (int i = 0; i < 10; i++) {
            var go = new GameObject(TEST_GO_NAME);
            go.AddComponent<Button>();
            _testGos.Add(go);
        }

        // Act
        var query = By.Type<Button>(targetName);
        var actualElements = _driver.FindElements(query);

        // Assert
        foreach (var element in actualElements)
            Assert.AreEqual(element.Name, TEST_GO_NAME);
    }
}