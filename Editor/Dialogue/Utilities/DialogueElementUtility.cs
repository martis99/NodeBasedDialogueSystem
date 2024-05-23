using Dialogue.Elements;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Dialogue.Utilities
{
    public static class DialogueElementUtility
    {
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new(onClick)
            {
                text = text,
            };

            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new()
            {
                text = title,
                value = !collapsed,
            };

            return foldout;
        }

        public static Port CreatePort(this DialogueNodeView node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));

            port.portName = portName;

            return port;
        }

        public static TextField CreateTextField(SerializedObject so, string value = null, string name = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField field = new()
            {
                value = value,
                label = name,
            };

            if (onValueChanged != null)
            {
                field.RegisterValueChangedCallback(onValueChanged);
            }

            field.bindingPath = name;
            field.Bind(so);

            return field;
        }

        public static TextField CreateTextArea(SerializedObject so, string value = null, string name = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField field = CreateTextField(so, value, name, onValueChanged);

            field.multiline = true;

            return field;
        }

        public static Toggle CreateToggle(SerializedObject so, bool value = false, string name = null, EventCallback<ChangeEvent<bool>> onValueChanged = null)
        {
            Toggle toggle = new()
            {
                value = value,
                label = name,
            };

            if (onValueChanged != null)
            {
                toggle.RegisterValueChangedCallback(onValueChanged);
            }

            toggle.bindingPath = name;
            toggle.Bind(so);

            return toggle;
        }

        public static EnumField CreateEnumField(SerializedObject so, Enum value = null, string name = null, EventCallback<ChangeEvent<Enum>> onValueChanged = null)
        {
            EnumField field = new()
            {
                value = value,
                label = name,
            };

            if (onValueChanged != null)
            {
                field.RegisterValueChangedCallback(onValueChanged);
            }

            field.bindingPath = name;
            field.Bind(so);

            return field;
        }
    }
}

