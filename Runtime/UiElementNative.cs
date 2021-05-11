// Copyright (c) AIR Pty Ltd. All rights reserved.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AIR.UnityTestPilot.Interactions
{
    public class UiElementNative : UiElement
    {
        public UiElementNative(Object obj)
            : base(obj) { }

        public override string Name => UnityObject.name;

        public override bool IsActive {
            get {
                if (UnityObject is MonoBehaviour mgo)
                    return mgo.isActiveAndEnabled;
                if (UnityObject is GameObject go)
                    return go.activeInHierarchy;
                return false;
            }
        }

        public override string Text {
            get {
                if (UnityObject is Text goText)
                    return goText.text;

                if (UnityObject is InputField inputText)
                    return inputText.text;

                if (UnityObject is MonoBehaviour goMb) {
                    goText = goMb.GetComponent<Text>();
                    if (goText != null)
                        return goText.text;
                }

                if (UnityObject is GameObject go) {
                    goText = go.GetComponent<Text>();
                    if (goText != null)
                        return goText.text;
                }

                // To support use of external GUI, like tmpro or ngui or TextMesh objects
                var reflectedTextField = UnityObject.GetType().GetProperty("text");
                if (reflectedTextField != null)
                {
                    var textPropertyGetResult = reflectedTextField.GetMethod.Invoke(UnityObject, new object[0]);
                    if (textPropertyGetResult is string textPropertyGetResultAsString)
                        return textPropertyGetResultAsString;
                }

                return string.Empty;
            }
        }

        public override Float3 LocalPosition {
            get {
                if (UnityObject is GameObject unityGameObject) {
                    var localPos = unityGameObject.transform.localPosition;
                    return new Float3(localPos.x, localPos.y, localPos.z);
                }

                return default;
            }
        }

        public override Float3 EulerRotation {
            get {
                if (UnityObject is GameObject unityGameObject) {
                    var localPos = unityGameObject.transform.rotation.eulerAngles;
                    return new Float3(localPos.x, localPos.y, localPos.z);
                }

                return default;
            }
        }

        private Object UnityObject => _object as Object;

        public override void MiddleClick() => SimulateClick(PointerEventData.InputButton.Middle);
        public override void RightClick() => SimulateClick(PointerEventData.InputButton.Right);
        public override void LeftClick() => SimulateClick(PointerEventData.InputButton.Left);
        public override void LeftClickDown() => SimulateClickDown(PointerEventData.InputButton.Left);
        public override void LeftClickAndHold(TimeSpan holdTime)
        {
            LeftClickDown();
            new GameObject(nameof(ClickHolder))
                .AddComponent<ClickHolder>()
                .ClickAndHold(holdTime, LeftClickUp);
        }

        public override void RightClickDown() => SimulateClickDown(PointerEventData.InputButton.Right);
        public override void MiddleClickDown() => SimulateClickDown(PointerEventData.InputButton.Middle);
        public override void LeftClickUp() => SimulateClickUp(PointerEventData.InputButton.Left);
        public override void RightClickUp() => SimulateClickUp(PointerEventData.InputButton.Right);
        public override void MiddleClickUp() => SimulateClickUp(PointerEventData.InputButton.Middle);

        public override void SimulateKeys(KeyCode[] keys)
        {
            throw new NotImplementedException("Send keys to unity input system.");
        }

        public override void SimulateKeys(string keys)
        {
            if (UnityObject is InputField inputField) {
                inputField.text = keys;
                return;
            }

            throw new NotImplementedException(UnityObject.GetType() + " input handling not yet implemented.");
        }

        private void SimulateClick(PointerEventData.InputButton mouseButton)
        {
            ButtonEventInvoke<IPointerClickHandler>((b, es) =>
                b.OnPointerClick(new PointerEventData(es) { button = mouseButton }));
        }

        private void SimulateClickDown(PointerEventData.InputButton mouseButton)
        {
            ButtonEventInvoke<IPointerDownHandler>((b, es) =>
                b.OnPointerDown(new PointerEventData(es) { button = mouseButton }));
        }

        private void SimulateClickUp(PointerEventData.InputButton mouseButton)
        {
            ButtonEventInvoke<IPointerUpHandler>((b, es) =>
                b.OnPointerUp(new PointerEventData(es) { button = mouseButton }));
        }

        private void ButtonEventInvoke<T>(Action<T, EventSystem> buttonAction)
        {
            if (UnityObject is MonoBehaviour mb) {
                var handlers = mb.GetComponents<T>();
                foreach (var handler in handlers)
                    buttonAction?.Invoke(handler, EventSystem.current);
            }

            if (UnityObject is GameObject go) {
                var button = go.GetComponent<Button>();
                if (button != null) {
                    var handlers = button.GetComponents<T>();
                    foreach (var handler in handlers)
                        buttonAction?.Invoke(handler, EventSystem.current);
                }
            }
        }

        private class ClickHolder : MonoBehaviour
        {
            private TimeSpan _holdTime;
            private Action _then;

            public void ClickAndHold(TimeSpan holdTime, Action then)
            {
                _then = then;
                _holdTime = holdTime;
                StartCoroutine(ClickAndHold());
            }

            private IEnumerator ClickAndHold()
            {
                yield return new WaitForSeconds((float)_holdTime.TotalSeconds);
                _then?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}