//TODO: Rewrite runtime logic to have only these methods: GetQuestion(), GetChoices(), SetBool(), GetBool(), Choice.Select(), so Update() and GetNext() is not needed
//TODO: Add and, or gates, e.g: when all questions are completed SetBool to true
//TODO: Add events, e.g: when bool is set invoke event

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Dialogue.Data
{
    [CreateAssetMenu()]
    public class DialogueGraph : ScriptableObject
    {
        public SerializableDictionary<string, DialogueBase> Nodes = new();

        public DialogueBase CreateNode(Type type, Vector2 position)
        {
#if UNITY_EDITOR
            DialogueBase node = CreateInstance(type) as DialogueBase;
            node.Position = position;
            Nodes.Add(node.ID, node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();

            return node;
#else
            Debug.LogError("Node creation available only in Editor");
            return null;
#endif
        }

        public void DeleteNode(DialogueBase node)
        {
#if UNITY_EDITOR
            Nodes.Remove(node.ID);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
#else
            Debug.LogError("Node deletion available only in Editor");
#endif
        }

        public DialogueGraph Clone()
        {
            DialogueGraph clone = CreateInstance<DialogueGraph>();

            foreach (KeyValuePair<string, DialogueBase> node in Nodes)
            {
                clone.Nodes.Add(node.Key, Instantiate(node.Value));
            }

            return clone;
        }

        public void Init()
        {
            foreach (DialogueBase node in Nodes.Values)
            {
                node.Init();
            }

            Nodes.Values.OfType<DialoguePerson>().ToList().ForEach(q => q.GetNextQuestion(this));
        }

        public DialoguePerson GetPerson(string name)
        {
            return Nodes.Values.OfType<DialoguePerson>().First(x => x.Name == name);
        }

        public DialogueQuestion GetNextQuestion(DialogueBase node)
        {
            return node == null ? null : (DialogueQuestion)node.GetNextQuestion(this);
        }

        public DialogueQuestion GetQuestion(DialogueBase node)
        {
            return node == null ? null : (DialogueQuestion)node.GetQuestion(this);
        }

        public List<DialogueChoice> GetChoices(DialogueQuestion question)
        {
            return question == null ? new List<DialogueChoice>() : question.GetChoices(this).OfType<DialogueChoice>().ToList();
        }

        public void SelectChoice(DialogueQuestion question, DialogueChoice choice)
        {
            question.Choice = choice.ID;
        }

        public void SetBool(string name, bool value)
        {
            Nodes.Values.OfType<DialogueCondition>().ToList().FindAll(c => c.Name == name).ForEach(c =>
            {
                c.Forget(this);
                c.Value = value;
                c.GetNextQuestion(this);
            });
        }

        public int Updates()
        {
            return Nodes.Values.OfType<DialoguePerson>().ToList().Aggregate(0, (acc, c) => acc + c.Updates(this));
        }
    }
}
