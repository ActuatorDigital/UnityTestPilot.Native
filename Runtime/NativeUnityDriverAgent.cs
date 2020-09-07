// Copyright (c) AIR Pty Ltd. All rights reserved.

using AIR.UnityTestPilot.Interactions;
using AIR.UnityTestPilot.Queries;
using UnityEngine;

namespace AIR.UnityTestPilot.Agents {
    public class NativeUnityDriverAgent : IUnityDriverAgent {

        public void Shutdown() {
            #if !UNITY_EDITOR
            Application.Quit();
            #endif
        }

        public void SetTimeScale(float timeScale) {
            Time.timeScale = timeScale;
        }

        public UiElement[] Query(ElementQuery query) {
            return query.Search();
        }
    }
}