// Copyright (c) AIR Pty Ltd. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using AIR.UnityTestPilot.Interactions;
using UnityEngine;

namespace AIR.UnityTestPilot.Queries
{
    public class PathElementQueryNative : PathElementQuery
    {
        private class PathQuery
        {
            private readonly string _pathToFind;

            public PathQuery(string pathToFind) => _pathToFind = pathToFind;

            public IEnumerable<string> Sections() => _pathToFind.Split('\\', '/').AsEnumerable();
        }

        public PathElementQueryNative(string pathToFind)
            : base(pathToFind) { }

        public override UiElement[] Search()
        {
            var elements = Object
                .FindObjectsOfType<GameObject>();

            // First check for full name matches even with separators
            var namedElementQueryNative = new NamedElementQueryNative(PathToFind);
            var namedResult = namedElementQueryNative.Search();
            if (namedResult != null)
                return namedResult;

            var querySections = new PathQuery(PathToFind).Sections().ToList();

            for (int i = 0; i < querySections.Count; i++)
            {
                var querySection = querySections[i];

                if (!elements.Any())
                    break;

                elements = elements.Where(x => x.name == querySection)
                    .ToArray();

                if (i < querySections.Count - 1)
                {
                    elements = elements.SelectMany(x => GetAllChildren(x))
                        .ToArray();
                }
            }

            if (elements.Any())
            {
                return elements.Select(o =>
                    new UiElementNative(o)).ToArray();
            }

            return null;
        }

        private GameObject[] GetAllChildren(GameObject go)
        {
            GameObject[] res = new GameObject[go.transform.childCount];
            for (int i = 0; i < go.transform.childCount; i++)
                res[i] = go.transform.GetChild(i).gameObject;
            return res;
        }
    }
}