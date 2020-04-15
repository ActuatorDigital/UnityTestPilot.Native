using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AIR.UnityTestPilot.Interactions {
    
    public class UiElementNative : UiElement
    {
        private Object _unityObject => _object as Object;
        public UiElementNative(Object obj) : base(obj) { }

        public override string Name => _unityObject.name;
        
        public override bool IsActive {
            get {
                if(_unityObject is MonoBehaviour mgo)
                    return mgo.isActiveAndEnabled;
                if(_unityObject is GameObject go)
                    return go.activeInHierarchy;
                return false;
            }
        }

        public override string Text {
            get {
                
                if (_unityObject is Text goText) 
                    return goText.text;
                
                if (_unityObject is MonoBehaviour goMb) {
                    goText = goMb.GetComponent<Text>();
                    if (goText != null)
                        return goText.text;
                }

                if (_unityObject is GameObject go) {
                    goText = go.GetComponent<Text>();
                    if (goText != null)
                        return goText.text;
                }

                return string.Empty;
            }
        }



        public override void MiddleClick() => SimulateClick(PointerEventData.InputButton.Middle);
        public override void RightClick() => SimulateClick(PointerEventData.InputButton.Right);
        public override void LeftClick() => SimulateClick(PointerEventData.InputButton.Left);
        public override void LeftClickDown() => SimulateClickDown(PointerEventData.InputButton.Left);
        public override void RightClickDown() => SimulateClickDown(PointerEventData.InputButton.Right);
        public override void MiddleClickDown() => SimulateClickDown(PointerEventData.InputButton.Middle);
        public override void LeftClickUp() => SimulateClickUp(PointerEventData.InputButton.Left);
        public override void RightClickUp() => SimulateClickUp(PointerEventData.InputButton.Right);
        public override void MiddleClickUp() => SimulateClickUp(PointerEventData.InputButton.Middle);
        private void SimulateClick(PointerEventData.InputButton mouseButton) {
            ButtonEventInvoke<IPointerClickHandler>((b, es) => 
                b.OnPointerClick( new PointerEventData(es) { button = mouseButton } ));
        }

        private void SimulateClickDown(PointerEventData.InputButton mouseButton) {
            ButtonEventInvoke<IPointerDownHandler>((b, es) => 
                b.OnPointerDown( new PointerEventData(es) { button = mouseButton } ));
        }

        private void SimulateClickUp(PointerEventData.InputButton mouseButton) {
            ButtonEventInvoke<IPointerUpHandler>((b, es) => 
                b.OnPointerUp( new PointerEventData(es) { button = mouseButton } ));
        }

        private void ButtonEventInvoke<T>(Action<T, EventSystem> buttonAction) {
            if (_unityObject is MonoBehaviour mb) {
                var handlers = mb.GetComponents<T>();
                foreach (var handler in handlers)
                    buttonAction?.Invoke(handler, EventSystem.current);
            }

            if (_unityObject is GameObject go) {
                var button = go.GetComponent<Button>();
                if (button != null) {
                    var handlers = button.GetComponents<T>();
                    foreach (var handler in handlers)
                        buttonAction?.Invoke(handler, EventSystem.current);
                }
            }
        } 

        public override void SimulateKeys(KeyCode[] keys) {
            throw new NotImplementedException("Send keys to unity input system.");
        }

        public override void SimulateKeys(string keys) {
            throw new NotImplementedException("Convert string to keys then call SimulateKeys.");
        }
    }
    
}