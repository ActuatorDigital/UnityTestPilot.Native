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
public class UiElementSimulateDropdownSelectTests
{
    private Transform _rootTransform;
    private Dropdown _dropdown;
    private UnityDriver _driver;

    private UiElement _dropdownElement;

    [SetUp]
    public void Setup()
    {
        var rootGO = new GameObject("rootGo");
        _rootTransform = rootGO.transform;
        var canvasGO = new GameObject("Canvas", typeof(Canvas));
        canvasGO.transform.SetParent(rootGO.transform);
        var eventSystemGO = new GameObject("EventSystem", typeof(EventSystem));
        eventSystemGO.transform.SetParent(rootGO.transform);
        var testDropdownGO = new GameObject("TestDropdown", typeof(RectTransform), typeof(Dropdown));
        testDropdownGO.transform.SetParent(canvasGO.transform);
        _dropdown = rootGO.GetComponentInChildren<Dropdown>();
        _dropdown.options = new System.Collections.Generic.List<Dropdown.OptionData>()
        {
            new Dropdown.OptionData("None"),
            new Dropdown.OptionData("Option 1"),
            new Dropdown.OptionData("Option 2"),
        };

        var agent = new NativeUnityDriverAgent();
        _driver = new UnityDriver(agent);
        _dropdownElement = _driver.FindElement(By.Name("TestDropdown"));
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_rootTransform.gameObject);
    }

    [UnityTest]
    [Ignore("Here as reference, the template object in the dropdown is not configured.")]
    public IEnumerator OnValueChanged_OnSelect_ElementIsSelected()
    {
        // Arrange
        int selected = -1;
        var expected = 1;
        _dropdown.onValueChanged.AddListener((x) => selected = x);

        // Act
        _dropdownElement.LeftClick();
        yield return new WaitForEndOfFrame();
        var _dropdownOptionElement = _driver.FindElement(By.Path("*[contains(Option 1)]"));
        _dropdownOptionElement.LeftClick();
        yield return new WaitForEndOfFrame();

        // Assert
        Assert.AreEqual(expected, selected);
    }

    [UnityTest]
    public IEnumerator Text_DefaultDropdown_IsNone()
    {
        // Arrange
        var expected = "None";
        var result = "";

        // Act
        result = _dropdownElement.Text;
        yield return new WaitForEndOfFrame();

        // Assert
        Assert.AreEqual(expected, result);
    }
}