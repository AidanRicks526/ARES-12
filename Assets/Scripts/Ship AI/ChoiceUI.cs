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

    [Header("Next Button")]
    public Button nextButton;

    public Action<int> OnChoiceSelected;
    public Action OnNextPressed;

    void Awake()
    {
        Instance = this;

        if (panel == null)
            Debug.LogError("ChoiceUI: Panel not assigned!");

        panel.SetActive(false);

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() =>
            {
                panel.SetActive(false);
                OnNextPressed?.Invoke();
            });
        }
    }

    // =========================
    // SHOW CHOICES OR NEXT
    // =========================
    public void Show(DialogueChoice[] choices)
    {
        if (panel == null || buttons == null || buttonTexts == null)
        {
            Debug.LogError("ChoiceUI: UI not fully assigned!");
            return;
        }

        panel.SetActive(true);

        // ---------------------------
        // NO CHOICES → SHOW NEXT
        // ---------------------------
        if (choices == null || choices.Length == 0)
        {
            SetChoiceButtonsActive(false);

            if (nextButton != null)
                nextButton.gameObject.SetActive(true);

            return;
        }

        // ---------------------------
        // HAS CHOICES
        // ---------------------------
        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < choices.Length && choices[i] != null)
            {
                int index = i;

                buttons[i].gameObject.SetActive(true);

                if (buttonTexts[i] != null)
                    buttonTexts[i].text = choices[i].choiceText;

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

    void SetChoiceButtonsActive(bool state)
    {
        foreach (var b in buttons)
        {
            if (b != null)
                b.gameObject.SetActive(state);
        }
    }
}