using UnityEngine;
using TMPro;

public class NoteDisplay : MonoBehaviour
{
    public GameObject panel; // The root panel (for show/hide)
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowNote(LabNote note)
    {
        titleText.text = note.itemName;
        contentText.text = note.noteContent;
        panel.SetActive(true);
        Time.timeScale = 0f; // Pause game while reading
    }

    public void CloseNote()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}