// Copyright (c) AIR Pty Ltd. All rights reserved.

using AIR.UnityTestPilot.Interactions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

[TestFixture]
public class UiElementStateTests
{
    private const string TEST_NAME = "Test";
    private GameObject _go;

    [SetUp]
    public void SetUp()
    {
        _go = new GameObject(TEST_NAME);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_go);
    }

    [Test]
    public void Name_ElementIsNotGo_ShowsGoName()
    {
        // Arrange
        var button = _go.AddComponent<Button>();
        var element = new UiElementNative(button);

        // Act
        var buttonName = element.Name;

        // Assert
        Assert.AreEqual(buttonName, TEST_NAME);
    }

    [Test]
    public void ActiveInHierarchy_ActiveState_CorrectActivity(
        [Values(true, false)] bool expectedActive
    )
    {
        // Arrange
        _go.SetActive(expectedActive);
        var element = new UiElementNative(_go);

        // Act
        var actualActive = element.IsActive;

        // Assert
        Assert.AreEqual(expectedActive, actualActive);
    }
}