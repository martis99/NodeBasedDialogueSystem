using Dialogue.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Top level dialogue manager
/// </summary>
public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// Dialogue graph reference
    /// </summary>
    public DialogueGraph Dialogue;

    /// <summary>
    /// Question change event
    /// </summary>
    public UnityEvent<DialoguePerson, DialogueQuestion, List<DialogueChoice>> OnQuestion;

    private DialoguePerson person;
    private DialogueQuestion question;
    private List<DialogueChoice> choices;

    private void Awake()
    {
        Dialogue = Dialogue.Clone();
        Dialogue.Init();
        Dialogue.Updates();
    }

    /// <summary>
    /// Retrieves given person question and available choices and invokes <see cref="OnQuestion"/> callback
    /// </summary>
    /// <param name="person_name">Name of the person to talk with</param>
    public void GetQuestion(string person_name)
    {
        person = Dialogue.GetPerson(person_name);
        question = Dialogue.GetQuestion(person);
        choices = Dialogue.GetChoices(question);
        OnQuestion.Invoke(person, question, choices);
    }

    /// <summary>
    /// Selects choice and invokes <see cref="OnQuestion"/> callback
    /// </summary>
    /// <param name="id">Choice id</param>
    public void SelectChoice(int id)
    {
        Dialogue.SelectChoice(question, choices[id]);
        question = Dialogue.GetNextQuestion(person);
        choices = Dialogue.GetChoices(question);
        OnQuestion.Invoke(person, question, choices);
    }
}
