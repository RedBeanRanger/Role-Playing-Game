using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentHandler : MonoBehaviour
{

    private GameObject decorativeObject = null;
    private Color decorativeColor;
    private SpriteRenderer decorativeRenderer;

    private Color adjustedAlphaColor;
    private Color tempColor;

    // Start is called before the first frame update
    public void OnStart()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Decorative");
        if (!(obj is null))
        {
            decorativeObject = obj;
            decorativeRenderer = decorativeObject.GetComponent<SpriteRenderer>();
            decorativeColor = decorativeRenderer.color;
            adjustedAlphaColor = new Color(1, 1, 1, .20f);
            tempColor = decorativeColor;
        }
    }

    //adjust alpha of the decorative layer
    public void AdjustDecorativeAlpha()
    {
        //decorativeObject.GetComponent<SpriteRenderer>().color = adjustedAlphaColor;
        //AlphaAdjusted = true;
        if (!(decorativeObject is null))
        {
            tempColor = Color.Lerp(tempColor, adjustedAlphaColor, Time.deltaTime * 3);
            decorativeRenderer.color = tempColor;
        }

    }

    public void RevertDecorativeAlpha()
    {
        if(!(decorativeObject is null))
        {
            tempColor = Color.Lerp(tempColor, decorativeColor, Time.deltaTime * 3);
            decorativeRenderer.color = tempColor;
        }
    }

}
