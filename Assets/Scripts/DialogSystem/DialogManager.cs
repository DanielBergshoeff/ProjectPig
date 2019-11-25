using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    private GameObject dialogBox;
    private TextMeshProUGUI dialogText;
    private AudioSource dialogAudio;

    private DialogObject lastDialogObject;
    private int dialogIndex;

    private void Awake()
    {
        //Get the DialogBox
        dialogBox = GameObject.Find("DialogBox");

        //If there is no DialogBox create one
        if (dialogBox == null)
        {
            GameObject original = (GameObject)Resources.Load("Prefabs/DialogBox");
            dialogBox = Instantiate(original, transform);
            dialogBox.name = original.name;
        }

        //Get references to components
        dialogText = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        dialogAudio = dialogBox.GetComponentInChildren<AudioSource>();

        dialogBox.SetActive(false);
    }

    public void StartDialog(DialogObject dialog)
    {
        CheckDialog(dialog);

        //If new dialog then start directly
        if (dialogIndex == 0)
        {
            dialogBox.SetActive(true);
            ShowDialog(dialog.lines[dialogIndex]);
            dialogIndex++;
        }
        else if (dialogIndex < dialog.lines.Length)
        {
            //Wait for input before showing next line
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowDialog(dialog.lines[dialogIndex]);
                dialogIndex++;
            }
        }
        else
        {
            //Reset the dialog system
            dialogBox.SetActive(false);
            lastDialogObject = null;
            dialogIndex = 0;
        }
    }

    private void ShowDialog(DialogLine dialogLine)
    {
        //Display the text
        dialogText.text = dialogLine.line;

        //Play the audio clip with it
        dialogAudio.Stop();
        dialogAudio.clip = dialogLine.audio;
        dialogAudio.Play();
    }

    private void CheckDialog(DialogObject dialog)
    {
        //Check if dialog is loaded, and if it's the same dialog
        if (lastDialogObject == null || lastDialogObject != dialog)
        {
            lastDialogObject = dialog;
            dialogIndex = 0;
        }
    }
}
