using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void QuitButtonPressed()
    {
        //for debug only
        Debug.Log("This line ran!");
        Application.Quit();
    }
}
