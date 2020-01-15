using UnityEngine;

public class ToggleBots : MonoBehaviour, IIntractable
{
    private Robot[] bots;

    private void Start()
    {
        bots = FindObjectsOfType<Robot>();

        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void Interact()
    {
        foreach (var bot in bots) { bot.evil = !bot.evil; }
    }
}
