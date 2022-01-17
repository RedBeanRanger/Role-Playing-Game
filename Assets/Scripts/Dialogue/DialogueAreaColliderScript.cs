using System;
using UnityEngine;

public class DialogueAreaColliderScript : MonoBehaviour
{
    [SerializeField]
    public int DialogueAreaNumber;
    public Vector3 ThisAreaPosition;
    //string that checks for the right direction you want the player to face

    //public UnityEvent<Colllider> DialogueTriggerEvent;
    public event Action<DialogueAreaColliderScript> OnDialogueEnter;

    void Start()
    {
        ThisAreaPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        string tag = collider.tag;
        if (tag != null)
        {
            if (tag == "Player")
            {
                Debug.Log("Player has entered Area " + DialogueAreaNumber);
                OnDialogueEnter?.Invoke(this); 
                //? is shorthand to check that OnDialogueEnter is not null before trying to invoke it
            }
        }
        /*
        if (collider.tag == "Player")
        {
            Debug.Log("Player has entered Area " + DialogueAreaNumber);
        }
        */
    }
}
