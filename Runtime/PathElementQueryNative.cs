// Copyright (c) AIR Pty Ltd. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using AIR.UnityTestPilot.Interactions;
using UnityEngine;

namespace AIR.UnityTestPilot.Queries
{
    public class PathElementQueryNative : PathElementQuery
    {
        private const string WILDCARD_PREFIX = "*";
        private const string QUERY_KEYWORD_PREFIX = "[";
        private const string QUERY_KEYWORD_SUFFIX = "]";
        private const string CONTAINS_PREFIX = "contains(";
        private const string TERM_SUFFIX = ")";

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

                elements = SectionQuery(elements, querySection);

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

        private GameObject[] SectionQuery(GameObject[] elements, string querySection)
        {
            if (querySection.StartsWith(WILDCARD_PREFIX))
            {
                querySection = querySection.Substring(1);

                if (querySection.StartsWith(QUERY_KEYWORD_PREFIX) && querySection.EndsWith(QUERY_KEYWORD_SUFFIX))
                {
                    querySection = querySection.Substring(1, querySection.Length - 2);
                    if (querySection.StartsWith(CONTAINS_PREFIX) && querySection.EndsWith(TERM_SUFFIX))
                    {
                        var partialName = querySection.Substring(CONTAINS_PREFIX.Length, querySection.Length - 1 - CONTAINS_PREFIX.Length);

                        return elements.Where(x => x.name.Contains(partialName))
                            .ToArray();
                    }
                }

                // no specifics requsted, have them all back.
                return elements;
            }
            else
            {
                return elements.Where(x => x.name == querySection)
                    .ToArray();
            }
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