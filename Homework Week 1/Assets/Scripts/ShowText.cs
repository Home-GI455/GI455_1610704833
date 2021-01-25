using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    public string NumbersName;
    public GameObject TypeWord;
    public GameObject NumbersvocabularyDisplay;
    public string[] NumberWords;

    private bool Checker;
   
    public void ShowName()
    {
        NumbersName = TypeWord.GetComponent<Text>().text;
        NumbersvocabularyDisplay.GetComponent<Text>().text = NumbersName + " has been found";
        Checker = true;
        if(NumbersName == NumberWords[0] && Checker)
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=green>{NumbersName}</color>" + " has been found";
        }
        else if (NumbersName == NumberWords[1] && Checker)
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=green>{NumbersName}</color>" + " has been found";
        }
        else if (NumbersName == NumberWords[2] && Checker)
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=green>{NumbersName}</color>" + " has been found";
        }
        else if (NumbersName == NumberWords[3] && Checker)
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=green>{NumbersName}</color>" + " has been found";
        }
        else if (NumbersName == NumberWords[4] && Checker)
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=green>{NumbersName}</color>" + " has been found";
        }
        else if (NumbersName == NumberWords[5] && Checker)
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=green>{NumbersName}</color>" + " has been found";
        }
        else if (NumbersName == NumberWords[6] && Checker)
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=green>{NumbersName}</color>" + " has been found";
        }
        else if (NumbersName == NumberWords[7] && Checker)
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=green>{NumbersName}</color>" + " has been found";
        }
        else if (NumbersName == NumberWords[8] && Checker)
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=green>{NumbersName}</color>" + " has been found";
        }
        else if (NumbersName == NumberWords[9] && Checker)
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=green>{NumbersName}</color>" + " has been found";
        }
        else
        {
            NumbersvocabularyDisplay.GetComponent<Text>().text = $"<color=black>{NumbersName}</color>" + " has been not found";
        }
    }
}
