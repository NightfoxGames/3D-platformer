using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RobotActionsAnimation : MonoBehaviour
{
    public  TMP_Text textelement;
    public TMP_Text eyes;
    // Sets the text
    public void SetText(string text)
    {
        textelement.text = text;
    }

      public void SetEyes(string text)
    {
        eyes.text = text;
    }

    
}
