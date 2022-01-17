using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public bool CharacterSwitched = false;
    public Animator HudAnim0;
    public Animator HudAnim1;
    public Animator HudAnim2;

    //position and movement
    public Vector3 spawnPosition;
    private Vector3 position;
    private Vector2 movement;

    //targeting enemy
    public GameObject targeter;
    private Animator targetAnim;

    //character handling
    //current character
    public Transform CurrentTransform { get; private set; }

    //default character
    public GameObject defaultCharacter;

    //playable characters
    public GameObject wayfarerCharacter;

    //equipped characters
    public GameObject[] characters = new GameObject[3];

    //character instance tracker
    private static GameObject[] characterInstances = null;

    //the index of the character who is currently active
    private static int activeIndex = 0;

    //references within the currentCharacter
    private GameObject currentCharacter;
    private SpriteRenderer currentRenderer;
    private Rigidbody2D currentRigidBody2D;
    private Animator currentAnim;
    private Transform currentAttackPoint;
    private int currentDirection; // counterclockwise: right = 0, down = 1, left = 2, up = 3
    public bool currentTargetingOn;

    //private bool IsInvincible = false;
    public bool IsCollidingWithDecor = false;
    public bool IsAttemptingInteraction = false;
    public bool IsInteracting = false;
    public bool IsMoveable = true;

    //make the script a property
    public BaseCharacter CurrentCharacterScript { get; private set; }

    //*****public methods*****
    //Start, update and fixedupdate
    public void OnStart()
    {
        Debug.Log("Player Start Called");
        position = spawnPosition;
        InstantiateCharactersAtPosition(position);
        currentCharacter = characters[activeIndex];
        StoreCurrentReferences();
        EnableCharacterAtPosition(currentCharacter, CurrentTransform.position);
        targetAnim = targeter.GetComponent<Animator>();
        currentTargetingOn = CurrentCharacterScript.targetingOn;
    }

    public void OnUpdate()
    {
        //movement
        if (IsInteracting)
        {
            IsMoveable = false;
        }
        if (!IsInteracting)
        {
            IsMoveable = true;
        }

        if (IsMoveable)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        else if (!IsMoveable)
        {
            movement.x = 0;
            movement.y = 0;
        }

        CurrentCharacterScript.DetectEnemies();
        if(!(CurrentCharacterScript.enemyInRange) && targetAnim.GetBool("Targeting"))
        {
            targetAnim.SetBool("Targeting", false);
        }
        IsCollidingWithDecor = CurrentCharacterScript.hasCollidedWithDecor;

        if (currentAnim != null)
        {
            if (!Input.anyKey || !IsMoveable || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.H))
            {
                currentAnim.SetBool("isStill", true);
            }
            else
            {
                currentAnim.SetBool("isStill", false);

                if (Input.GetKey(KeyCode.S))
                {
                    currentDirection = 1;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    currentDirection = 3;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    currentDirection = 2;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    currentDirection = 0;
                }

                currentAnim.SetInteger("facingDirection", currentDirection);
                //Debug.Log(currentDirection);

            }
        }

        //other inputs

        if (Input.GetKeyDown(KeyCode.Space) && IsMoveable)
        {
            SwitchCharacter();
        }

        //Skill1
        if (Input.GetKeyDown(KeyCode.J))
        {
            CurrentCharacterScript.BasicAttack();
            HudAnim0.SetTrigger("SkillActivated");
        }

        //Skill2
        if (Input.GetKeyDown(KeyCode.K))
        {
            CurrentCharacterScript.Skill0();
            HudAnim1.SetTrigger("SkillActivated");
        }

        //Skill3
        if (Input.GetKeyDown(KeyCode.L))
        {
            CurrentCharacterScript.Skill1();
            HudAnim2.SetTrigger("SkillActivated");
        }

        //Skill4
        if (Input.GetKeyDown(KeyCode.U))
        {

        }

        //Skill5
        if (Input.GetKeyDown(KeyCode.I))
        {

        }

        //Item1
        if (Input.GetKeyDown(KeyCode.N))
        {

        }

        //Item2
        if (Input.GetKeyDown(KeyCode.M))
        {

        }

        //Target an enemy
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (CurrentCharacterScript.Target())
            {
                targetAnim.SetBool("Targeting", true);
                currentTargetingOn = true;
                //Debug.Log(targetAnim.GetCurrentAnimatorStateInfo(0).IsName("TargetAnimation"));
            }
            else
            {
                targetAnim.SetBool("Targeting", false);
                currentTargetingOn = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsAttemptingInteraction == false && CurrentCharacterScript.hasEnteredDialogueArea)
            {
                //player handler only switch isAttemptingInteraction to True if the player is already on a dialogue area.
                //game handler switches it back to false
                IsAttemptingInteraction = true;
            }
        }

    }

    public void OnFixedUpdate()
    {
        movement.Normalize();
        if (currentDirection == 0)
        {
            currentRenderer.flipX = false;
            currentAttackPoint.localPosition = new Vector3(.3f, .3f, 0f);
        }
        if (currentDirection == 1)
        {
            currentRenderer.flipX = false;
        }

        if (currentDirection == 3)
        {
            currentRenderer.flipX = false;
        }

        if (currentDirection == 2)
        {
            currentRenderer.flipX = true;
            currentAttackPoint.localPosition = new Vector3(-.3f, .3f, 0f);
        }

        currentRigidBody2D.MovePosition(currentRigidBody2D.position + (movement * CurrentCharacterScript.MaxMvtSpd * Time.fixedDeltaTime));
        UpdatePosition(currentRigidBody2D.position);

    }

    public void NewPositionAtLoad(Vector3 pos)
    {
        CurrentTransform.position = pos;
    }

    //*****private methods*****
    private void StoreCurrentReferences()
    {
        CurrentTransform = currentCharacter.transform;
        currentRenderer = currentCharacter.GetComponent<SpriteRenderer>();
        currentRigidBody2D = currentCharacter.GetComponent<Rigidbody2D>();
        currentAnim = currentCharacter.GetComponent<Animator>();
        CurrentCharacterScript = currentCharacter.GetComponent<BaseCharacter>();

        CurrentCharacterScript.targetingOn = currentTargetingOn;
        currentTargetingOn = CurrentCharacterScript.targetingOn;

        currentAttackPoint = currentCharacter.transform.Find("AttackPoint");
        currentDirection = CurrentCharacterScript.facingDirection;


    }

    private void InstantiateCharactersAtPosition(Vector3 pos)
    {
        if (characterInstances != null)
        {
            //no need to instantiate characters
            return;
        }
        defaultCharacter = Instantiate(defaultCharacter);
        defaultCharacter.transform.position = pos;
        defaultCharacter.SetActive(false);
        defaultCharacter.GetComponent<BaseCharacter>().indexNumber = 0;
        characters[0] = defaultCharacter;

        //can be modified
        characters[1] = Instantiate(wayfarerCharacter);
        characters[1].transform.position = pos;
        characters[1].SetActive(false);
        characters[1].GetComponent<BaseCharacter>().indexNumber = 1;

        //can be modified
        characters[2] = null;

        characterInstances = new GameObject[3];
        characterInstances = characters;
        Debug.Log(characterInstances[0].name);
        foreach (GameObject obj in characterInstances)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }

    private void UpdatePosition(Vector2 rigidBodyPosition)
    {
        position.x = rigidBodyPosition.x;
        position.y = rigidBodyPosition.y;
    }

    private GameObject EnableCharacterAtPosition(GameObject character, Vector3 pos)
    {
        character.transform.position = pos;
        if (currentRenderer.flipX == true)
        {
            character.GetComponent<SpriteRenderer>().flipX = true;
        }
        character.SetActive(true);
        return character;
    }

    private void DisableCharacter(GameObject character)
    {
        character.SetActive(false);
    }

    private void SwitchCurrentToSpecified(GameObject chara, Vector3 position)
    {
        DisableCharacter(currentCharacter);
        currentCharacter = EnableCharacterAtPosition(chara, position);
        StoreCurrentReferences();
        CharacterSwitched = true;
        activeIndex = CurrentCharacterScript.indexNumber;
    }

    private void SwitchCharacter()
    {
        if (!(currentCharacter is null))
        {
            UpdatePosition(currentRigidBody2D.position);
            switch (CurrentCharacterScript.indexNumber)
            {
                case 0:
                    if (!(characters[1] == null))
                    {
                        SwitchCurrentToSpecified(characters[1], position);
                    }
                    break;
                case 1:
                    if (!(characters[2] == null))
                    {
                        SwitchCurrentToSpecified(characters[2], position);
                    }
                    break;
                case 2:
                    break;
            }
            if (!CharacterSwitched && currentCharacter != defaultCharacter)
            {
                SwitchCurrentToSpecified(defaultCharacter, position);
            }
            //otherwise return
            return;
        }
    }
}