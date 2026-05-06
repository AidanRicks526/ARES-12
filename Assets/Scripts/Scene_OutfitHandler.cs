using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_OutfitHandler : MonoBehaviour
{
    [Header("Scene Settings")]
    public string requiredSceneName = "";

    [Header("Animator Settings")]
    public string animatorBoolName = "InSuit";

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        ApplyIfCorrectScene();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyIfCorrectScene();
    }

    void ApplyIfCorrectScene()
    {
        if (animator == null)
        {
            Debug.LogWarning("[Outfit] No Animator found.");
            return;
        }

        if (!string.IsNullOrEmpty(requiredSceneName) &&
            SceneManager.GetActiveScene().name != requiredSceneName)
        {
            animator.SetBool(animatorBoolName, false);
            return;
        }

        Debug.Log("[Outfit] Setting animator bool: " + animatorBoolName);

        animator.SetBool(animatorBoolName, true);
    }
}