using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBuffer
{
    //Action buffer, takes in all of the inputs the player makes, and outputs the action the player character should take.
    //Prioritize direction inputs.

    //actual possible actions

    /*
    public static string[] actionList = new string[]
    {
        "right",
        "down",
        "left",
        "up",
        "attack0",
        "attack1",
        "attack2",
        "switch",
        "dodge"
    };
    */

    public static Dictionary<string, KeyCode> actionList = new Dictionary<string, KeyCode>();

    //list of inputs that are buffered
    public List<ActionBufferItem> inputList = new List<ActionBufferItem>();

    void OnStart()
    {
        actionList.Add("right", KeyCode.D);
        actionList.Add("down", KeyCode.S);
        actionList.Add("left", KeyCode.A);
        actionList.Add("up", KeyCode.W);
        actionList.Add("attack0", KeyCode.J);
        actionList.Add("attack1", KeyCode.K);
        actionList.Add("attack2", KeyCode.L);
        actionList.Add("switch", KeyCode.Space);
    }

    void InitializeBuffer()
    {
        inputList = new List<ActionBufferItem>();
        foreach (KeyCode action in actionList.Values) //string version: for each string action in actionList
        {
            //create a new action buffer item for every action in actionList
            ActionBufferItem item = new ActionBufferItem();
            item.action = action;
            inputList.Add(item);
        }
    }

    void Update()
    {
        if ((inputList.Count < actionList.Count) || inputList.Count == 0)
        {
            InitializeBuffer(); //.....could this possibly go into OnStart??
        }

        foreach (ActionBufferItem a in inputList)
        {
            //execute and resolve the action
            a.ResolveCommand();
            for (int x = 0; x < a.buffer.Count - 1; x++)
            {
                a.buffer[x].hold = a.buffer[x + 1].hold;
                a.buffer[x].used = a.buffer[x + 1].used;
            }

        }
    }
}


// class ActionBufferItem - holds the input and the buffer
public class ActionBufferItem
{
    //public string action;
    public KeyCode action;
    public List<ActionBufferItemState> buffer;
    public static int bufferWindow = 12; //the number of frames you buffer for
    // this is the amount of time  

    public ActionBufferItem()
    {
        buffer = new List<ActionBufferItemState>();
        for (int i = 0; i < bufferWindow; i++)
        {
            buffer.Add(new ActionBufferItemState());
        }
    }

    public void ResolveCommand()
    {
        if (Input.GetKey(action)) { 
        }
    }
}


// class ActionBufferItemState - holds the state of the input passed into the buffer
public class ActionBufferItemState
{
    public int hold; //how long you've held the input for. 0 by default.
                     //Increments for every frame you hold the input
                     //Becomes -1 when you release.
    public bool used;

    //if it's -1 to



}
