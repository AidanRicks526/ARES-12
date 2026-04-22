using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager instance;
    [SerializeField] private Image _fadeOutImage;
    [SerializeField] private float _fadeOutSpeed = 5f;
    [SerializeField] private float _fadeInSpeed = 5f;

    public bool IsFadingOut { get; private set; }
    public bool IsFadingIn { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // This moves the object to the root of the hierarchy
            transform.SetParent(null);

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        HidePanel();
    }

    private void HidePanel()
    {
        if (_fadeOutImage != null)
        {
            _fadeOutImage.gameObject.SetActive(false);
            _fadeOutImage.raycastTarget = false;
            Color c = _fadeOutImage.color;
            c.a = 0f;
            _fadeOutImage.color = c;
        }
    }

    private void Update()
    {
        if (!IsFadingOut && !IsFadingIn) return;

        Color c = _fadeOutImage.color;
        if (IsFadingOut)
        {
            c.a = Mathf.MoveTowards(c.a, 1f, Time.deltaTime * _fadeOutSpeed);
            _fadeOutImage.color = c;
            if (c.a >= 1f) IsFadingOut = false;
        }
        else if (IsFadingIn)
        {
            c.a = Mathf.MoveTowards(c.a, 0f, Time.deltaTime * _fadeInSpeed);
            _fadeOutImage.color = c;
            if (c.a <= 0f) { IsFadingIn = false; _fadeOutImage.gameObject.SetActive(false); }
        }
    }

    public void StartFadeOut() { _fadeOutImage.gameObject.SetActive(true); _fadeOutImage.raycastTarget = true; IsFadingOut = true; IsFadingIn = false; }
    public void StartFadeIn() { _fadeOutImage.raycastTarget = false; IsFadingIn = true; IsFadingOut = false; }
}