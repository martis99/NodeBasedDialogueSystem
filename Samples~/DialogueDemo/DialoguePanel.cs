using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Dialogue.Data;
using UnityEngine.Events;

public class DialoguePanel : MonoBehaviour
{
    public GameObject DialoguePanelGO;
    public GameObject ChoicesPanelGO;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Question;

    public UnityEvent<int> OnSelectChoice;

    private void Awake()
    {
        DialoguePanelGO.SetActive(false);
    }

    public void SetQuestion(DialoguePerson person, DialogueQuestion question, List<DialogueChoice> choices)
    {
        Name.SetText(person.Name);
        Question.SetText(question.Text);

        int cnt = 0;
        foreach (Transform choice in ChoicesPanelGO.transform)
        {
            choice.gameObject.SetActive(cnt < choices.Count);
            if (cnt >= choices.Count)
            {
                continue;
            }

            choice.GetComponentInChildren<TextMeshProUGUI>().SetText(choices[cnt].Text);
            cnt++;
        }

        DialoguePanelGO.SetActive(true);
    }

    public void SelectChoice(int id)
    {
        OnSelectChoice?.Invoke(id);
    }
}
