using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ChoiceUI : MonoBehaviour
{
    public static ChoiceUI Instance;

    public GameObject panel;
    public Button[] buttons;
    public TextMeshProUGUI[] buttonTexts;

    public Action<int> OnChoiceSelected;

    void Awake()
    {
        Instance = this;

        if (panel == null)
            Debug.LogError("ChoiceUI: Panel not assigned!");

        panel.SetActive(false);
    }

    public void Show(DialogueChoice[] choices)
    {
        if (choices == null || choices.Length == 0)
        {
            Debug.LogError("ChoiceUI: No choices provided!");
            return;
        }

        if (panel == null || buttons == null || buttonTexts == null)
        {
            Debug.LogError("ChoiceUI: UI not fully assigned!");
            return;
        }

        panel.SetActive(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < choices.Length && choices[i] != null)
            {
                int index = i;

                buttons[i].gameObject.SetActive(true);

                if (buttonTexts[i] != null)
                    buttonTexts[i].text = choices[i].choiceText;
                else
                    Debug.LogError("Missing button text at index " + i);

                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() =>
                {
                    panel.SetActive(false);
                    OnChoiceSelected?.Invoke(index);
                });
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}