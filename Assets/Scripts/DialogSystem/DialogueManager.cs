using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Shows and plays the dialogue objects contents
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager _instance;
    public static DialogueManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/DialogueManager"));
                _instance = go.GetComponent<DialogueManager>();
            }
            return _instance;
        }
        private set { }
    }

    public List<KeyCode> keycodes = new List<KeyCode>() { KeyCode.E, KeyCode.Space };

    private GameObject dialogueBox;
    private TMP_Text[] textObjects;
    private AudioSource dialogueAudio;

    private DialogueObject currentDialogue;
    private int dialogueIndex;
    private bool dialogueMode = false;

    /// <summary>
    /// Skip through all the dialogue options
    /// </summary>
    private void Update()
    {
        if (!dialogueMode) { return; }

        //Wait for input before showing next line
        foreach (var keycode in keycodes)
        {
            if (!Input.GetKeyDown(keycode)) { continue; }

            dialogueIndex++;

            if (dialogueIndex < currentDialogue.lines.Length)
            {
                ShowDialogue(currentDialogue.lines[dialogueIndex]);
                return;
            }
            else
            {
                //Reset the dialogue system
                dialogueMode = false;
                dialogueBox.SetActive(false);
                currentDialogue = null;
                dialogueIndex = 0;
                return;
            }
        }
    }

    /// <summary>
    /// Start playing the a dialogue object or skip through it
    /// </summary>
    /// <param name="dialogue">The dialogue object to play</param>
    public void StartDialogue(DialogueObject dialogue, GameObject box = default)
    {
        if (!CheckDialogue(dialogue)) { return; }

        dialogueMode = true;

        if (box != default)
            dialogueBox = box;

        GetDialogueBox();

        InnitDialogueBox();

        //If new dialogue then start directly
        if (dialogueIndex == 0)
        {
            dialogueBox.SetActive(true);
            ShowDialogue(dialogue.lines[dialogueIndex]);
        }
        else
        {
            dialogueMode = true;
        }
    }

    private void InnitDialogueBox()
    {
        //Get references to components
        textObjects = dialogueBox.GetComponentsInChildren<TMP_Text>();

        dialogueAudio = dialogueBox.GetComponentInChildren<AudioSource>();

        if (dialogueAudio == null)
            dialogueAudio = dialogueBox.AddComponent<AudioSource>();

        dialogueBox.SetActive(false);
    }

    /// <summary>
    /// Gets a text container to show text
    /// </summary>
    private void GetDialogueBox()
    {
        //If there is no DialogueBox create one
        if (dialogueBox != null) { return; }

        //Get the DialogueBox
        dialogueBox = GameObject.Find("DialogueBox");

        if (dialogueBox != null) { return; }

        GameObject original = (GameObject)Resources.Load("Prefabs/DialogueBox");
        dialogueBox = Instantiate(original, transform);
        dialogueBox.name = original.name;
    }

    /// <summary>
    /// This handles the displaying of the text and playing of the audio.
    /// </summary>
    /// <param name="dialogueLine">The dialogue line to play</param>
    private void ShowDialogue(DialogueLine dialogueLine)
    {
        //Display the text
        try
        {
            textObjects[0].text = dialogueLine.line;
            textObjects[1].text = dialogueLine.speaker;
        }
        catch
        {
            Debug.LogError(dialogueBox.name + " is missing a speaker box");
        }

        //Play the audio clip with it
        dialogueAudio.Stop();
        dialogueAudio.clip = dialogueLine.audio;
        dialogueAudio.Play();
    }

    /// <summary>
    /// Check if there is a dialogue.false If there is and it's not the same, overwrite it.
    /// </summary>
    /// <param name="dialogue">Current dialogue</param>
    /// <returns></returns>
    private bool CheckDialogue(DialogueObject dialogue)
    {
        //Check if dialogue is loaded, and if it's the same dialogue
        if (currentDialogue == null || currentDialogue != dialogue)
        {
            currentDialogue = dialogue;
            dialogueIndex = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}
