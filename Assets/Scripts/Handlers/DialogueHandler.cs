using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    public GameObject CharacterDialogueBox;
    public GameObject Portrait;
    public GameObject FloatingInteractionPrompt;
    Vector3 FloatingInteractionPromptPos;

    public GameObject DefaultDialogueBox;
    public GameObject DialogueTextObject;
    public TextMeshPro NameText;
    public TextMeshProUGUI DialogueText;
    [SerializeField] public float TypeSpeed;

    List<string> test = new List<string> { "Hello!", "Hm, very fascinating...", "What a strange piece of code!" };

    List<List<string>> trialDialogue = new List<List<string>>
    {
        new List<string> {"A computer.", "It seems to be playing some nice background music."},
        new List<string> {"A comfy pink bed.", "There is a plush of the famous character, Kurbi.", "Come to think of it, the carpet is also Kurbi-themed.", 
            "...This isn't infringing on anything okay! It's an homage!! You can't copyright this!!!"},
        new List<string> {"A representation of a piano.", "It doesn't have 88 keys like a real piano does, but whoever made this has clearly tried her best."},
        new List<string> {"Hey look, paper!"},
        new List<string> {"A Dintendo Ditch console.", "This is also an homage, by the way!"},
        new List<string> {"It's Mr. Lemony. A very good rabbit. He is named for his slightly yellowish color."},
        new List<string> {"A laundry basket.", "The Developer left all of her clothes in the laundry basket. \n ...She probably won't have anything to wear tomorrow."},
        new List<string> {"A violin case. The violin's name is Frank.", "Naming your violin is very important."},
        new List<string> {"It's a closet.", "The closet is empty.", "You now feel slightly guilty for being impolite and peeking without asking first."},
        new List<string> {"It's the Developer!"},
        new List<string> {"A nice ice-cream lamp!", "The Developer has great tastes. I'm sure that's what you're thinking right now."}
    };

    public bool isShowingDialogue = false;
    public bool isCoroutineRunning = false;
    public bool stopCoroutine = false;
    public bool dialogueEndReached = false;

    public int ind = 0;
    int indexToRetrieve = 0;

    [SerializeField]
    List<GameObject> DialogueAreas;
    List<DialogueAreaColliderScript> DialogueAreaScriptList;

    //things we've found out: the dialogue object needs text in a list of strings
    // one way we can handle this is: store dialogues in a database, assign them area codes and trigger conditions, let DialogueHandler fetch em.
    // the Dialogue handler will load all the possible spritesheets that could be used for dialogues once OnStart and never again for the rest of the game.

    public void OnStart()
    {
        if (DialogueTextObject != null)
        {
            DialogueText = DialogueTextObject.GetComponent<TextMeshProUGUI>();
        }

        DialogueAreaScriptList = new List<DialogueAreaColliderScript>(10);
        foreach (GameObject obj in DialogueAreas)
        {
            DialogueAreaScriptList.Add(obj.GetComponent<DialogueAreaColliderScript>());
            //Debug.Log("Dialogue Area Script List Added" + DialogueAreaScriptList);
        }

        //Handle enter area event
        foreach (var areaScript in DialogueAreaScriptList)
        {
            areaScript.OnDialogueEnter += HandleDialogueArea;
            //register HandlDialogueArea function on the onDialogueEnter event.
            //the event basically acts as the delegate for the HandleDialogueArea function of this script now!
        }
    }

    void HandleDialogueArea(DialogueAreaColliderScript areaScript)
    {
        indexToRetrieve = areaScript.DialogueAreaNumber;
        FloatingInteractionPromptPos = areaScript.ThisAreaPosition;
        FloatingInteractionPrompt.transform.position = FloatingInteractionPromptPos;
    }

    
    public void ShowDialogue()
    {
        isShowingDialogue = true;
        DefaultDialogueBox.SetActive(true);
        //StartCoroutine(TypeDialogue(test[ind]));
        StartCoroutine(TypeDialogue(trialDialogue[indexToRetrieve][ind]));
    }

    /*

    public void ShowCharacterDialogue(CharacterDialogue dialogue)
    {
        CharacterDialogueBox.SetActive(true);
        StartCoroutine(TypeDialogue(dialogue.Lines[ind]));
    }
    */

    public IEnumerator TypeDialogue(string line)
    {
        isCoroutineRunning = true;
        DialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            if (stopCoroutine){
                break;
            }
            DialogueText.text += letter;
            yield return new WaitForSeconds(TypeSpeed);
        }
        if (stopCoroutine)
        {
            DialogueText.text = line;
        }
        isCoroutineRunning = false;
        stopCoroutine = false;
    }

    public void ContinueDialogue()
    {
        if (isCoroutineRunning)
        {
            stopCoroutine = true;
        }
        if (ind < trialDialogue[indexToRetrieve].Count - 1 && !isCoroutineRunning)
        {
            ind++;
            StartCoroutine(TypeDialogue(trialDialogue[indexToRetrieve][ind]));
        }
        else if (ind == trialDialogue[indexToRetrieve].Count - 1 && !isCoroutineRunning)
        {
            DefaultDialogueBox.SetActive(false);
            isShowingDialogue = false;
            dialogueEndReached = true;
            ind = 0;
            //Debug.Log("Dialogue End Reach confirmed!");
        }
    }
}
