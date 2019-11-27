using UnityEngine;

/// <summary>
/// Contains multiple lines for the entire dialogue
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "DialogueObject", menuName = "DialogueObject", order = 1)]
public class DialogueObject : ScriptableObject
{
    public DialogueLine[] lines;
}
