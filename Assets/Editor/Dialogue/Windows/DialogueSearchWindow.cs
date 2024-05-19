using Dialogue.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialogue.Windows
{
    public class DialogueSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueGraphView graphView;
        private Texture2D indentationIcon;

        public void Initialize(DialogueGraphView graphView)
        {
            this.graphView = graphView;

            indentationIcon = new(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
            };

            var nodes = TypeCache.GetTypesDerivedFrom<DialogueBase>();
            foreach (var type in nodes)
            {
                if (type.GetTypeInfo().IsAbstract)
                {
                    continue;
                }

                searchTreeEntries.Add(new SearchTreeEntry(new GUIContent(type.Name, indentationIcon))
                {
                    level = 1,
                    userData = type,
                });
            }

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePositon = graphView.GetLocalMousePosition(context.screenMousePosition, true);
            graphView.CreateNode((Type)SearchTreeEntry.userData, localMousePositon);
            return true;
        }
    }
}
