using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceUI : MonoBehaviour
{
    public static ChoiceUI Instance;

    public GameObject panel;
    public Transform buttonContainer;
    public Button buttonPrefab;

    public Action<int> OnChoiceSelected;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(DialogueChoice[] choices)
    {
        panel.SetActive(true);

        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < choices.Length; i++)
        {
            int index = i;

            Button btn = Instantiate(buttonPrefab, buttonContainer);
            btn.GetComponentInChildren<TMP_Text>().text = choices[i].choiceText;

            btn.onClick.AddListener(() =>
            {
                panel.SetActive(false);
                OnChoiceSelected?.Invoke(index);
            });
        }
    }
}