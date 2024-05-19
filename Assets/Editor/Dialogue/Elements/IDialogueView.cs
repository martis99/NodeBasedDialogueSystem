using Dialogue.Data;
using Dialogue.Windows;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Elements
{
    public interface IDialogueView
    {
        public abstract void Init(DialogueGraphView graphView, DialogueBase node);
        public abstract void SetBorder(Color color);
        public abstract void ResetBorder();
        public abstract void SetOnNodeSelected(Action<IDialogueView> value);
        public abstract void LoadConnections(DialogueGraphView graphView, Dictionary<string, IDialogueView> nodes);
    }
}
