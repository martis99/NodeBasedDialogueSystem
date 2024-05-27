# NodeBasedDialogueSystem Documentation

Welcome to the NodeBasedDialogueSystem documentation. This guide will help you understand how to create and manage node based dialogue system in Unity.

## Table of Contents

1. [Scriptable Object](#scriptable-object)
2. [Node Editor](#node-editor)
3. [Integration](#integration)
4. [Example Usage](#example-usage)

## Scriptable Object

Dialogue graphs are saved as ScriptableObjects, which makes them easy to manage and integrate into your project.

Creating a Dialogue Graph ScriptableObject
1. Right-click in the `Assets` folder.
2. Select `Create > Dialogue Graph`.

This action will create a new Dialogue Graph asset that you can use and edit.

## Node Editor

The Dialogue Node Graph is edited using a node-based editor, providing an intuitive and visual way to design dialogues.

### Opening the Node Editor
1. Go to `Window > Dialogue > Graph`.
2. Select (left-click) the Dialogue Graph ScriptableObject you want to edit.

### Editing the Graph
- Adding a New Node:
  - Right-click in the graph window.
  - Select the type of node you want to create from the context menu.
- Deleting a Node:
  - Select the node you want to delete.
  - Press the `Delete` key.

## Integration

The Unity API provides methods to manage dialogues within your game, including starting dialogues, managing choices, and handling dialogue events.

### Creating a `DialogueManager`
1. Add the `DialogueManager` Component:
    - Attach the `DialogueManager` component to a GameObject in your scene.
2. Assign the Dialogue ScriptableObject:
    - Drag the Dialogue Graph ScriptableObject to the `Dialogue` variable in the `DialogueManager` component.
3. Set Up Event Handlers (optional):
    - Assign a method to the OnQuestion event to handle when a new question is asked.

### Initiating Dialogue
To start a dialogue, call the `GetQuestion` method on the `DialogueManager` instance:
```csharp
dialogueManager.GetQuestion("CharacterName");
```

Replace "CharacterName" with the name of the character you want to initiate the dialogue with.

### Selecting a Choice
To select a dialogue choice, use the `SelectChoice` method:

```csharp
dialogueManager.SelectChoice(choiceNumber);
```
Replace choiceNumber with the index of the choice you want to select.

## Example Usage
Here is a basic example of how to set up and use the `DialogueManager` in a script:

```csharp
using Dialogue.Data;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager.OnQuestion.AddListener(OnQuestionChanged);
    }

    private void OnQuestionChanged(DialoguePerson person, DialogueQuestion question, List<DialogueChoice> choices)
    {
        // Handle the new question
        Debug.Log($"New question: {question.Text}");
    }

    public void SelectDialogueChoice(int choiceNumber)
    {
        dialogueManager.SelectChoice(choiceNumber);
    }
}
```
