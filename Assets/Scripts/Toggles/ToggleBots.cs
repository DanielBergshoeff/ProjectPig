using UnityEngine;

public class ToggleBots : MonoBehaviour, IIntractable
{
    private Robot[] bots;
    public bool interacted { get; set; }


    private void Start()
    {
        bots = FindObjectsOfType<Robot>();

        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public void Interact()
    {
        GetComponent<Collider>().enabled = false;
        foreach (var bot in bots) { bot.evil = !bot.evil; }
        Done();
    }

    public void Done()
    {
        this.enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
