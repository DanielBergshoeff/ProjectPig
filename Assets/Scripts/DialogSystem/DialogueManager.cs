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
    private TextMeshProUGUI dialogueSpeaker;
    private TextMeshProUGUI dialogueText;
    private AudioSource dialogueAudio;

    private DialogueObject currentDialogue;
    private int dialogueIndex;
    private bool dialogueMode = false;

    /// <summary>
    /// Initializes the dialogue manager and his dependencies 
    /// </summary>
    private void Awake()
    {
        //Get the DialogueBox
        dialogueBox = GameObject.Find("DialogueBox");

        //If there is no DialogueBox create one
        if (dialogueBox == null)
        {
            GameObject original = (GameObject)Resources.Load("Prefabs/DialogueBox");
            dialogueBox = Instantiate(original, transform);
            dialogueBox.name = original.name;
        }

        //Get references to components
        TextMeshProUGUI[] textMeshProUGUI = dialogueBox.GetComponentsInChildren<TextMeshProUGUI>();
        dialogueSpeaker = textMeshProUGUI[0];
        dialogueText = textMeshProUGUI[1];
        dialogueAudio = dialogueBox.GetComponentInChildren<AudioSource>();

        dialogueBox.SetActive(false);
    }

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
    public void StartDialogue(DialogueObject dialogue)
    {
        if (!CheckDialogue(dialogue)) { return; }

        dialogueMode = true;

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

    /// <summary>
    /// This handles the displaying of the text and playing of the audio.
    /// </summary>
    /// <param name="dialogueLine">The dialogue line to play</param>
    private void ShowDialogue(DialogueLine dialogueLine)
    {
        //Display the text
        dialogueSpeaker.text = dialogueLine.speaker;
        dialogueText.text = dialogueLine.line;

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
