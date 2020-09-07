// Copyright (c) AIR Pty Ltd. All rights reserved.

using AIR.UnityTestPilot.Agents;
using AIR.UnityTestPilot.Drivers;
using AIR.UnityTestPilot.Queries;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[TestFixture]
public class UiElementTextTests
{
    private const string TEST_TEXT_GO_NAME = "TextTestGo";
    private UnityDriver _driver;
    private Transform _testRootGo;

    [SetUp]
    public void SetUp()
    {
        var agent = new NativeUnityDriverAgent();
        _driver = new UnityDriver(agent);
        _testRootGo = new GameObject("TestRoot").transform;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_testRootGo.gameObject);
        _driver.Dispose();
    }

    [Test]
    public void Text_OnTextElement_ReturnsText()
    {
        // Arrange
        var go = new GameObject(TEST_TEXT_GO_NAME, typeof(Text));
        go.transform.SetParent(_testRootGo);
        var testText = go.GetComponent<Text>();
        const string EXPECTED_TEXT = "Text_OnTextElement_ReturnsText";
        testText.text = EXPECTED_TEXT;
        var textQuerty = _driver.FindElement(By.Type<Text>(TEST_TEXT_GO_NAME));

        // Act
        var actualText = textQuerty.Text;

        // Assert
        Assert.AreEqual(EXPECTED_TEXT, actualText);
    }

    [Test]
    public void Text_OnElementWithTextComponet_ReturnsText()
    {
        // Arrange
        var go = new GameObject(TEST_TEXT_GO_NAME, typeof(Text));
        go.transform.SetParent(_testRootGo);
        var testText = go.GetComponent<Text>();
        const string EXPECTED_TEXT = "Text_OnElementWithTextComponet_ReturnsText";
        testText.text = EXPECTED_TEXT;
        var textQuerty = _driver.FindElement(By.Name(TEST_TEXT_GO_NAME));

        // Act
        var actualText = textQuerty.Text;

        // Assert
        Assert.AreEqual(EXPECTED_TEXT, actualText);
    }

    [Test]
    public void Text_OnElementWithNoText_ThrowsInvalidOperationException()
    {
        // Arrange
        var goQuery = _driver.FindElement(By.Name(_testRootGo.name));

        // Act
        var goText = goQuery.Text;

        // Assert
        Assert.IsTrue(string.IsNullOrEmpty(goText));
    }
}