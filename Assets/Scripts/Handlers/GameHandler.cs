using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //Singleton Instances
    // The GameHandler never references things like the player or dialogue componenents directly. 
    public static GameObject GameInstance = null;
    public static GameObject PlayerInstance = null;
    public static GameObject CameraInstance = null;
    public static GameObject EnvironmentInstance = null;

    //public variables
    public GameObject PlayerHandlerObject;
    public GameObject CameraHandlerObject;
    public GameObject EnvironmentHandlerObject;
    public GameObject DialogueHandlerObject;
    public bool OnActiveZoom; // enable zooming when player hits a key

    //private variables
    //handling
    private PlayerHandler playerHandler;
    private CameraHandler cameraHandler;
    private EnvironmentHandler environmentHandler;
    private DialogueHandler dialogueHandler;
    private Animator targetAnim;

    /* currently not in use
    [SerializeField]
    private Vector3 StartAreaSpawnPosition;
    [SerializeField]
    private Vector3 CampsiteSpawnPosition;
    */

    // Start is called before the first frame update
    void Awake()
    {
        // Order is: Awake -> Enable -> Start
        // I want GameHandler to run all of these following things whenever the game starts.
        // Awake will always be called so it is safe to use Awake.
        Debug.Log("Game Start Called");
        if (GameInstance == null)
        {
            GameInstance = this.gameObject;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (PlayerInstance == null)
        {
            PlayerInstance = PlayerHandlerObject;
        }
        else
        {
            Destroy(PlayerHandlerObject);
        }

        if (CameraInstance == null)
        {
            CameraInstance = CameraHandlerObject;
        }
        else
        {
            Destroy(CameraHandlerObject);
        }

        if (EnvironmentInstance == null)
        {
            EnvironmentInstance = EnvironmentHandlerObject;
        }
        else
        {
            Destroy(EnvironmentHandlerObject);
        }
        /*
        if (DialogueInstance == null)
        {
            DialogueInstance = DialogueHandlerObject;
        }
        else
        {
            Destroy(DialogueHandlerObject);
        }
        */

        PlayerHandlerObject = GameObject.Find("PlayerHandler");
        CameraHandlerObject = GameObject.Find("CameraHandler");
        EnvironmentHandlerObject = GameObject.Find("EnvironmentHandler");
        DialogueHandlerObject = GameObject.Find("DialogueHandler");

        playerHandler = PlayerHandlerObject.GetComponent<PlayerHandler>();
        cameraHandler = CameraHandlerObject.GetComponent<CameraHandler>();
        environmentHandler = EnvironmentHandlerObject.GetComponent<EnvironmentHandler>();
        dialogueHandler = DialogueHandlerObject.GetComponent<DialogueHandler>();

        playerHandler.OnStart();
        cameraHandler.OnStart(playerHandler.CurrentTransform, OnActiveZoom);
        environmentHandler.OnStart();
        dialogueHandler.OnStart();

        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(PlayerHandlerObject);
        DontDestroyOnLoad(CameraHandlerObject);
        DontDestroyOnLoad(EnvironmentHandlerObject); //this one I might actually have to swap out between scenes.
    }

    // Update is called once per frame
    void Update()
    {
        playerHandler.OnUpdate();

        //player switching camera logistics
        if (playerHandler.CharacterSwitched)
        {
            cameraHandler.MainVC.m_Follow = playerHandler.CurrentTransform;
            playerHandler.CharacterSwitched = false;
        }

        //player collision with decoration layer
        if (playerHandler.IsCollidingWithDecor)
        {
            environmentHandler.AdjustDecorativeAlpha();
        }

        if (!(playerHandler.IsCollidingWithDecor))
        {
            environmentHandler.RevertDecorativeAlpha();
        }

        //camera movement when enemy nearby logistics
        cameraHandler.OnUpdate(playerHandler.CurrentCharacterScript.enemyInRange);

        //dialogue and interaction logistics

        if (dialogueHandler.dialogueEndReached)
        {
            Debug.Log("Dialogue ended.");
            playerHandler.IsInteracting = false;
            dialogueHandler.dialogueEndReached = false;
        }

        if (playerHandler.IsAttemptingInteraction && !playerHandler.IsInteracting)
        {
            //since isAttemptingInteraction is only switched to true if the player is already on an interactable space,
            //we can safely check which dialogueArea the player has entered.
            //in the case where the player has physically moved out of the talk zone while pressing the interaction key
            //then the following check exists to allow the dialogue to close

            if (dialogueHandler.isShowingDialogue)
            {
                dialogueHandler.ContinueDialogue();
            }

            dialogueHandler.ShowDialogue();
            playerHandler.IsAttemptingInteraction = false;
            playerHandler.IsInteracting = true;
        }
        if (playerHandler.IsAttemptingInteraction && playerHandler.IsInteracting)
        {
            if (dialogueHandler.isShowingDialogue)
            {
                dialogueHandler.ContinueDialogue();
                playerHandler.IsAttemptingInteraction = false;
            }
            else if (!dialogueHandler.isShowingDialogue && !dialogueHandler.isCoroutineRunning)
            {
                playerHandler.IsInteracting = true;
                playerHandler.IsAttemptingInteraction = false;
                dialogueHandler.ShowDialogue();
            }
        }
    }

    void FixedUpdate()
    {
        playerHandler.OnFixedUpdate();
    }
}
