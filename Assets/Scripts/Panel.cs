using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649 


// Assumes Main Camera has an AudioSource component


public class Panel : MonoBehaviour
{
    // A single step
    [System.Serializable]
    public class Step
    {
        public string Notes;
        public string stepSpanish;
        public string stepEnglish;
        public AudioClip sfx;
        public GameObject[] stepObjects;
    }


    // List of all steps for this panel
    [SerializeField] public Step[] steps;

    // The current step
    int state = -1;

    // List of steps text (mutli-line Text Bubble)
    Text stepsText;

    // List of all artwork contained in the steps for this panel
    // It also has a boolean flag that will used in SetState to
    // hide/show the artwork for the current step
    Dictionary<GameObject,bool> artwork = new Dictionary<GameObject, bool>();

    // Language
    static bool isSpanish = true;
    GameObject spanishButton, englishButton;

    AudioSource audioSource;



    void Awake()
    {
        stepsText = GameObject.Find("StepsText").GetComponent<Text>();
        audioSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();

        FindAllArtwork();

        InitLanguage();

        SetState(0);
    }


    // Compile a list all artwork in all steps defined in the Panel.
    // This list is used to hide artwork that ISN'T included in each step.
    void FindAllArtwork()
    {
        foreach (Step step in steps)
            foreach (GameObject art in step.stepObjects)
                artwork[art] = false;
    }


    // Called when a clickable object has been clicked.
    // Parameter: art - The game object that was clicked
    public void OnClick(GameObject art)
    {
        Debug.Log("OnClick(" + art.name + ") state=" + state);

        if (art == steps[state].stepObjects[0])
            SetState(state + 1);
    }


    void InitLanguage()
    {
        spanishButton = GameObject.Find("SpanishButton");
        englishButton = GameObject.Find("EnglishButton");
        SetSpanish(true);
    }


    public void SetSpanish(bool newIsSpanish)
    {
        if (newIsSpanish != isSpanish)
            audioSource.Play();

        isSpanish = newIsSpanish;
        spanishButton.SetActive(isSpanish);
        englishButton.SetActive(!isSpanish);

        for (int i = 0; i <= state; ++i)
            DisplayStepText(i);
    }


    // Show and hide artwork for this step
    // Also update step text
    void SetState(int newState)
    {
        if (state >= 0 && steps[state].sfx != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(steps[state].sfx);
        }

        state = newState;

        Debug.Log("SetState(" + state + ")");

        foreach (GameObject art in artwork.Keys.ToList())
            artwork[art] = false;

        foreach (GameObject art in steps[state].stepObjects)
            artwork[art] = true;

        foreach (var item in artwork)
            item.Key.SetActive(item.Value);

        DisplayStepText(state);
    }


    void DisplayStepText(int state)
    {
        if (state == 0)
            stepsText.text = "";
        string text = isSpanish ? steps[state].stepSpanish : steps[state].stepEnglish;
        if (!string.IsNullOrEmpty(text))
            stepsText.text += (state + 1).ToString() + $". {text}\n";
    }
}
