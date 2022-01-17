using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    //quit the application when the quit button is pressed
    public void QuitButtonPressed()
    {
        //for debug only
        Debug.Log("This line ran!");
        Application.Quit();
    }
}