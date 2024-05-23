using Dialogue.Data;
using Dialogue.Windows;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Dialogue.Elements
{
    public class DialogueGroupView : Group, IDialogueView
    {
        public DialogueGroup Data { get; set; }
        public string OldTitle { get; set; }

        private Color defaultBorderBottomColor;
        private float defaultBorderBottomWidth;
        private Color defaultBorderLeftColor;
        private float defaultBorderLeftWidth;
        private Color defaultBorderTopColor;
        private float defaultBorderTopWidth;
        private Color defaultBorderRightColor;
        private float defaultBorderRightWidth;

        public Action<IDialogueView> OnNodeSelected;

        public virtual void Init(DialogueGraphView graphView, DialogueBase node)
        {
            Data = (DialogueGroup)node;
            title = Data.Name;
            viewDataKey = node.ID;

            style.left = node.Position.x;
            style.top = node.Position.y;

            defaultBorderBottomColor = contentContainer.style.borderBottomColor.value;
            defaultBorderBottomWidth = contentContainer.style.borderBottomWidth.value;
            defaultBorderLeftColor = contentContainer.style.borderLeftColor.value;
            defaultBorderLeftWidth = contentContainer.style.borderLeftWidth.value;
            defaultBorderTopColor = contentContainer.style.borderTopColor.value;
            defaultBorderTopWidth = contentContainer.style.borderTopWidth.value;
            defaultBorderRightColor = contentContainer.style.borderRightColor.value;
            defaultBorderRightWidth = contentContainer.style.borderRightWidth.value;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        public void SetOnNodeSelected(Action<IDialogueView> value)
        {
            OnNodeSelected = value;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Data.Position.x = newPos.xMin;
            Data.Position.y = newPos.yMin;
        }

        public void SetBorder(Color color)
        {
            contentContainer.style.borderBottomColor = color;
            contentContainer.style.borderBottomWidth = 2f;
            contentContainer.style.borderLeftColor = color;
            contentContainer.style.borderLeftWidth = 2f;
            contentContainer.style.borderTopColor = color;
            contentContainer.style.borderTopWidth = 2f;
            contentContainer.style.borderRightColor = color;
            contentContainer.style.borderRightWidth = 2f;
        }

        public void ResetBorder()
        {
            contentContainer.style.borderBottomColor = defaultBorderBottomColor;
            contentContainer.style.borderBottomWidth = defaultBorderBottomWidth;
            contentContainer.style.borderLeftColor = defaultBorderLeftColor;
            contentContainer.style.borderLeftWidth = defaultBorderLeftWidth;
            contentContainer.style.borderTopColor = defaultBorderTopColor;
            contentContainer.style.borderTopWidth = defaultBorderTopWidth;
            contentContainer.style.borderRightColor = defaultBorderRightColor;
            contentContainer.style.borderRightWidth = defaultBorderRightWidth;
        }

        public string GetID()
        {
            return Data.ID;
        }

        public void LoadConnections(DialogueGraphView graphView, Dictionary<string, IDialogueView> nodes)
        {

        }
    }
}
