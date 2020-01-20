using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using System.Collections;

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

    private List<KeyCode> keycodes = new List<KeyCode>() { KeyCode.Mouse0 };

    private GameObject dialogueBox;
    private TMP_Text[] textObjects;
    private AudioSource dialogueAudio;

    private UnityEvent dialogueMethod;
    private DialogueObject currentDialogue;
    private int dialogueIndex;
    private bool dialogueMode = false;
    private bool firstLine;
    private bool custom = false;

    /// <summary>
    /// Skip through all the dialogue options
    /// </summary>
    private void Update()
    {
        if (!dialogueMode) { return; }

        if (firstLine) { firstLine = false; return; }

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
                if (!custom)
                    dialogueBox.SetActive(false);
                currentDialogue = null;
                dialogueIndex = 0;
                custom = false;

                //If there is a method to call at the end of the dialogue, call it
                if (dialogueMethod == null)
                    return;

                dialogueMethod.Invoke();
                dialogueMethod = null;
                return;
            }
        }
    }

    /// <summary>
    /// Start playing the a dialogue object or skip through it
    /// </summary>
    /// <param name="dialogue">The dialogue object to play</param>
    public void StartDialogue(DialogueObject dialogue, GameObject box = default, UnityEvent dm = null)
    {
        if (!CheckDialogue(dialogue)) { return; }

        dialogueMode = true;
        dialogueMethod = dm;

        if (box != default) {
            dialogueBox = box;
            custom = true;
        }
        else
            dialogueBox = null;

        GetDialogueBox();

        InnitDialogueBox();

        //If new dialogue then start directly
        if (dialogueIndex == 0)
        {
            firstLine = true;
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
        StopAllCoroutines();
        //Display the text
        try
        {
            StartCoroutine(TypeWriteText(textObjects[0], dialogueLine.line));
            //textObjects[1].text = dialogueLine.speaker;
        }
        catch
        {
            Debug.LogWarning(dialogueBox.name + " is missing a speaker box");
        }

        //Play the audio clip with it
        dialogueAudio.Stop();
        dialogueAudio.clip = dialogueLine.audio;
        dialogueAudio.Play();
    }

    /// <summary>
    /// Takes a text and writes it down in the specified time
    /// </summary>
    /// <param name="container">Where to write the text</param>
    /// <param name="text">Text to write</param>
    /// <param name="duration">Speed to write the text at</param>
    /// <returns></returns>
    IEnumerator TypeWriteText(TMP_Text container, string text, float duration = 1f)
    {
        int AmountOfCharactersPossible = 0;
        container.text = "";
        for (float t = 0; t < text.Length; t += Time.deltaTime * text.Length / duration)
        {
            int charactersTyped = (int)Mathf.Clamp(t, 0f, text.Length-1);
            int beginIndex = AmountOfCharactersPossible == 0 ? AmountOfCharactersPossible : charactersTyped - AmountOfCharactersPossible;
            container.text = text.Substring(beginIndex, charactersTyped - beginIndex);

            container.ForceMeshUpdate();
            if (container.isTextTruncated && AmountOfCharactersPossible == 0)
                AmountOfCharactersPossible = charactersTyped;

            yield return null;
        }
        container.text = text;
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
