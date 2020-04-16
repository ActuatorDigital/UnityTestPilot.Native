// Copyright (c) AIR Pty Ltd. All rights reserved.

using AIR.UnityTestPilot.Agents;

namespace AIR.UnityTestPilot.Drivers
{
    public class UnityDriverNative : UnityDriver
    {
        public UnityDriverNative()
            : base(new NativeUnityDriverAgent()) { }
    }
}