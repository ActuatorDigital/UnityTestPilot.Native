using System;
using System.Diagnostics;
using TachyonCommon;

namespace AIR.UnityTestPilot.Queries {
    public static class By {

        public static ElementQuery Name(string name) => 
            ByBase.Name<NamedElementQueryNative>(name);

        public static ElementQuery Type<TQueryType>(string name) => 
            ByBase.Type<TQueryType, TypedElementQueryNative>(name);

        public static ElementQuery Type<TQueryType>() => 
            ByBase.Type<TQueryType, TypedElementQueryNative>();
    }
}