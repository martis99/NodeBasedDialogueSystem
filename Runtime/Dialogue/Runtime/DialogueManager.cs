using Dialogue.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public DialogueGraph Dialogue;

    public UnityEvent<DialoguePerson, DialogueQuestion, List<DialogueChoice>> OnQuestion;

    DialoguePerson person;
    DialogueQuestion question;
    List<DialogueChoice> choices;

    private void Awake()
    {
        Dialogue = Dialogue.Clone();
        Dialogue.Init();
        Dialogue.Updates();
    }

    public void GetQuestion(string person_name)
    {
        person = Dialogue.GetPerson(person_name);
        question = Dialogue.GetQuestion(person);
        choices = Dialogue.GetChoices(question);
        OnQuestion.Invoke(person, question, choices);
    }

    public void SelectChoice(int id)
    {
        Dialogue.SelectChoice(question, choices[id]);
        question = Dialogue.GetNextQuestion(person);
        choices = Dialogue.GetChoices(question);
        OnQuestion.Invoke(person, question, choices);
    }
}
