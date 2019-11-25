using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "DialogObject", menuName = "DialogObject", order = 1)]
public class DialogObject : ScriptableObject
{
    public DialogLine[] lines;
}
