// Copyright (c) AIR Pty Ltd. All rights reserved.

using System;
using System.Collections;
using AIR.UnityTestPilot.Agents;
using AIR.UnityTestPilot.Drivers;
using AIR.UnityTestPilot.Interactions;
using AIR.UnityTestPilot.Queries;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[TestFixture]
public class UiElementSimulateClickTests
{
    private Transform _rootTransform;
    private Button _button;

    private UiElement _buttonElement;

    [SetUp]
    public void Setup()
    {
        var rootGO = new GameObject("rootGo");
        _rootTransform = rootGO.transform;
        var canvasGO = new GameObject("Canvas", typeof(Canvas));
        canvasGO.transform.SetParent(rootGO.transform);
        var eventSystemGO = new GameObject("EventSystem", typeof(EventSystem));
        eventSystemGO.transform.SetParent(rootGO.transform);
        var testButtonGO = new GameObject("TestButton", typeof(RectTransform), typeof(Button));
        testButtonGO.transform.SetParent(canvasGO.transform);
        _button = rootGO.GetComponentInChildren<Button>();

        var agent = new NativeUnityDriverAgent();
        var driver = new UnityDriver(agent);
        _buttonElement = driver.FindElement(By.Type<Button>());
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_rootTransform.gameObject);
    }

    [Test]
    public void LeftClick_TargetIsButton_ButtonIsClicked()
    {
        // Arrange
        bool clicked = false;
        _button.onClick.AddListener(() => clicked = true);

        // Act
        _buttonElement.LeftClick();

        // Assert
        Assert.True(clicked);
    }

    [UnityTest]
    public IEnumerator LeftClickDown_NoRelease_ButtonNotClicked()
    {
        // Arrange
        bool clicked = false;
        _button.onClick.AddListener(() => clicked = true);

        // Act
        _buttonElement.LeftClickDown();
        yield return new WaitForEndOfFrame();

        // Assert
        UnityEngine.Assertions.Assert.IsFalse(clicked);
    }

    [Test]
    public void LeftClickUp_EventTriggerRegistered_PointerUpTriggered()
    {
        // Arrange
        bool pointerUpTriggered = false;

        var pointerDownTrigger = new EventTrigger.Entry();
        pointerDownTrigger.eventID = EventTriggerType.PointerUp;
        pointerDownTrigger.callback.AddListener((data) => pointerUpTriggered = true);

        var et = _button.gameObject.AddComponent<EventTrigger>();
        et.triggers.Add(pointerDownTrigger);

        // Act
        _buttonElement.LeftClickUp();

        // Assert
        Assert.IsTrue(pointerUpTriggered);
    }

    [Test]
    public void LeftClickDown_EventTriggerRegistered_PointerDownTriggered()
    {
        // Arrange
        bool pointerDownTriggered = false;

        var pointerDownTrigger = new EventTrigger.Entry();
        pointerDownTrigger.eventID = EventTriggerType.PointerDown;
        pointerDownTrigger.callback.AddListener((data) => pointerDownTriggered = true);

        var et = _button.gameObject.AddComponent<EventTrigger>();
        et.triggers.Add(pointerDownTrigger);

        // Act
        _buttonElement.LeftClickDown();

        // Assert
        Assert.IsTrue(pointerDownTriggered);
    }
    
    [Timeout(1500)]
    [UnityTest]
    public IEnumerator LeftClickAndHold_EventTriggersRegistered_PointerDownAndUpTriggered()
    {
        // Arrange
        bool pointerDownTriggered = false;
        bool pointerUpTriggered = false;

        var pointerDownTrigger = new EventTrigger.Entry();
        pointerDownTrigger.eventID = EventTriggerType.PointerDown;
        pointerDownTrigger.callback.AddListener((data) => pointerDownTriggered = true);

        var pointerUpTrigger = new EventTrigger.Entry();
        pointerUpTrigger.eventID = EventTriggerType.PointerUp;
        pointerUpTrigger.callback.AddListener((data) => pointerUpTriggered = true);

        var et = _button.gameObject.AddComponent<EventTrigger>();
        et.triggers.Add(pointerDownTrigger);
        et.triggers.Add(pointerUpTrigger);

        // Act
        _buttonElement.LeftClickAndHold(TimeSpan.FromSeconds(1));
        Assert.IsFalse(pointerUpTriggered);
        yield return new WaitForSeconds(1.1f);

        // Assert
        Assert.IsTrue(pointerDownTriggered);
        Assert.IsTrue(pointerUpTriggered);
    }
}