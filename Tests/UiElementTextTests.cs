// Copyright (c) AIR Pty Ltd. All rights reserved.

using AIR.UnityTestPilot.Agents;
using AIR.UnityTestPilot.Drivers;
using AIR.UnityTestPilot.Interactions;
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
    public void Text_OnTextMeshElement_ReturnsText()
    {
        // Arrange
        var go = new GameObject(TEST_TEXT_GO_NAME, typeof(TextMesh));
        go.transform.SetParent(_testRootGo);
        var testText = go.GetComponent<TextMesh>();
        const string EXPECTED_TEXT = "Text_OnTextMeshElement_ReturnsText";
        testText.text = EXPECTED_TEXT;
        var textQuerty = _driver.FindElement(By.Type<TextMesh>(TEST_TEXT_GO_NAME));

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
        const string EXPECTED_TEXT = "Text_OnTextElement_ReturnsText";
        testText.text = EXPECTED_TEXT;
        var textQuerty = _driver.FindElement(By.Name(TEST_TEXT_GO_NAME));

        // Act
        var actualText = textQuerty.Text;

        // Assert
        Assert.AreEqual(EXPECTED_TEXT, actualText);
    }

    [Test]
    public void Text_OnElementWithNoTextFoundByName_ReturnsNullOrEmptyString()
    {
        // Arrange
        var goQuery = _driver.FindElement(By.Name(_testRootGo.name));

        // Act
        var goText = goQuery.Text;

        // Assert
        Assert.IsTrue(string.IsNullOrEmpty(goText));
    }

    [Test]
    public void Text_OnElementWithNoTextFoundByType_ReturnsNullOrEmptyString()
    {
        // Arrange
        var goQuery = _driver.FindElement(By.Type<Text>());

        // Act
        var goText = goQuery?.Text;

        // Assert
        Assert.IsTrue(string.IsNullOrEmpty(goText));
    }

    [Test]
    public void Text_NoElementFoundByTypeWithName_ReturnsNullOrEmptyString()
    {
        // Arrange
        const string NOT_TEST_TEXT_GO_NAME = TEST_TEXT_GO_NAME + "NOT";
        var go = new GameObject(TEST_TEXT_GO_NAME, typeof(Text));
        go.transform.SetParent(_testRootGo);
        var goQuery = _driver.FindElement(By.Type<Text>(NOT_TEST_TEXT_GO_NAME));

        // Act
        var goText = goQuery?.Text;

        // Assert
        Assert.IsTrue(string.IsNullOrEmpty(goText));
    }

    [Test]
    public void Text_OnInputFiedElementFoundByType_ReturnsExpectedValue()
    {
        // Arrange
        const string EXPECTED_TEXT = "Text_OnTextElement_ReturnsText";
        var go = new GameObject(TEST_TEXT_GO_NAME, typeof(InputField));
        var inputFieldComp = go.GetComponent<InputField>();
        inputFieldComp.text = EXPECTED_TEXT;
        go.transform.SetParent(_testRootGo);
        var goQuery = _driver.FindElement(By.Type<InputField>());

        // Act
        var goText = goQuery?.Text;

        // Assert
        Assert.AreEqual(EXPECTED_TEXT, goText);
    }

    [Test]
    public void Text_OnInputFiedElementFoundByTypeInexact_ReturnsExpectedValue()
    {
        // Arrange
        const string EXPECTED_TEXT = "Text_OnTextElement_ReturnsText";
        var go = new GameObject(TEST_TEXT_GO_NAME, typeof(InputField));
        var inputFieldComp = go.GetComponent<InputField>();
        inputFieldComp.text = EXPECTED_TEXT;
        go.transform.SetParent(_testRootGo);
        var goQuery = _driver.FindElement(By.Type<MonoBehaviour>());

        // Act
        var goText = goQuery?.Text;

        // Assert
        Assert.AreEqual(EXPECTED_TEXT, goText);
    }

    [Test]
    public void Text_OnTextMeshElementFoundByTypeInexactParentGameObject_ReturnsExpectedValue()
    {
        // Arrange
        const string EXPECTED_TEXT = "Text_OnTextElement_ReturnsText";
        var go = new GameObject(TEST_TEXT_GO_NAME, typeof(TextMesh));
        var textMeshComp = go.GetComponent<TextMesh>();
        textMeshComp.text = EXPECTED_TEXT;
        go.transform.SetParent(_testRootGo);
        var goQuery = _driver.FindElement(By.Name(TEST_TEXT_GO_NAME));

        // Act
        var goText = goQuery?.Text;

        // Assert
        Assert.AreEqual(EXPECTED_TEXT, goText);
    }

    [Test]
    public void Text_OnTextElementFoundBySiblingComponentType_ReturnsExpectedValue()
    {
        // Arrange
        const string EXPECTED_TEXT = "Text_OnTextElement_ReturnsText";
        var go = new GameObject(TEST_TEXT_GO_NAME, typeof(Text), typeof(Button));
        var textComp = go.GetComponent<Text>();
        textComp.text = EXPECTED_TEXT;
        go.transform.SetParent(_testRootGo);
        var goQuery = _driver.FindElement(By.Type<Button>());

        // Act
        var goText = goQuery?.Text;

        // Assert
        Assert.AreEqual(EXPECTED_TEXT, goText);
    }
}