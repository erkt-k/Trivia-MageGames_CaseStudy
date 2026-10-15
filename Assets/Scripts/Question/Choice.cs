using UnityEngine;
using UnityEngine.Events;

public class Choice : MonoBehaviour
{
    public UnityEvent<string> OnChoiceSelected = new UnityEvent<string>();
    public string choiceIndex = "a";

    public void OnSelect()
    {
        OnChoiceSelected.Invoke(choiceIndex);
    }
}