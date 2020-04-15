using System;
using System.Linq;
using AIR.UnityTestPilot.Interactions;

namespace AIR.UnityTestPilot.Queries
{
    public class TypedElementQueryNative : TypedElementQuery
    {
        public TypedElementQueryNative(Type type, string name) : base(type, name) { }

        public TypedElementQueryNative(Type type) : base(type) { }

        public override UiElement[] Search()
        {
            var hits = UnityEngine.Object
                .FindObjectsOfType(_queryType)
                .ToArray();

            if (!hits.Any())
                return null;

            if (_queryName != null)
                hits = hits
                    .Where(h => h.name == _queryName)
                    .ToArray();

            if (hits.Any())
                return hits
                    .Select(h => new UiElementNative(h))
                    .ToArray();

            return null;
        }
    }
}