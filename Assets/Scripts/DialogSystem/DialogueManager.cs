using UnityEngine;
using TMPro;

/// <summary>
/// Shows and plays the dialogue objects contents
/// </summary>
public class DialogueManager : MonoBehaviour
{
    private GameObject dialogueBox;
    private TextMeshProUGUI dialogueText;
    private AudioSource dialogueAudio;

    private DialogueObject lastDialogueObject;
    private int dialogueIndex;

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
        dialogueText = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        dialogueAudio = dialogueBox.GetComponentInChildren<AudioSource>();

        dialogueBox.SetActive(false);
    }

    /// <summary>
    /// Start playing the a dialogue object or skip through it
    /// </summary>
    /// <param name="dialogue">The dialogue object to play</param>
    public void StartDialogue(DialogueObject dialogue)
    {
        if (!CheckDialogue(dialogue)) { return; }

        //If new dialogue then start directly
        if (dialogueIndex == 0)
        {
            dialogueBox.SetActive(true);
            ShowDialogue(dialogue.lines[dialogueIndex]);
            dialogueIndex++;
        }
        else if (dialogueIndex < dialogue.lines.Length)
        {
            //Wait for input before showing next line
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowDialogue(dialogue.lines[dialogueIndex]);
                dialogueIndex++;
            }
        }
        else
        {
            //Reset the dialogue system
            dialogueBox.SetActive(false);
            lastDialogueObject = null;
            dialogueIndex = 0;
        }
    }

    /// <summary>
    /// This handles the displaying of the text and playing of the audio.
    /// </summary>
    /// <param name="dialogueLine">The dialogue line to play</param>
    private void ShowDialogue(DialogueLine dialogueLine)
    {
        //Display the text
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
        if (lastDialogueObject == null || lastDialogueObject != dialogue)
        {
            lastDialogueObject = dialogue;
            dialogueIndex = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}
