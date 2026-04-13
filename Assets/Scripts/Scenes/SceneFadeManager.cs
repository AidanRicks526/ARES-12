using UnityEngine;
using UnityEngine.UI;
public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager instance;

    [SerializeField] private Image _fadeOutImage;
    [Range(0.1f, 10f), SerializeField] private float _fadeOutSpeed = 5f;
    [Range(0.1f, 10f), SerializeField] private float _fadeInSpeed = 5f;

    [SerializeField] private Color _fadeOutStartColor;

    public bool IsFadingOut { get; private set; }
    public bool IsFadingIn { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }

    }

    private void Update()
    {
        Color currentColor = _fadeOutImage.color;

        if (IsFadingOut)
        {
            if (currentColor.a < 1f)
            {
                currentColor.a += Time.deltaTime * _fadeOutSpeed;
                _fadeOutImage.color = currentColor;
            }
            else
            {
                currentColor.a = 1f;
                _fadeOutImage.color = currentColor;
                IsFadingOut = false;
            }
        }

        if (IsFadingIn)
        {
            if (currentColor.a > 0f)
            {
                currentColor.a -= Time.deltaTime * _fadeInSpeed;
                _fadeOutImage.color = currentColor;
            }
            else
            {
                currentColor.a = 0f;
                _fadeOutImage.color = currentColor;
                IsFadingIn = false;
            }
        }
    }


    public void StartFadeOut()
    {
        Color startColor = _fadeOutStartColor;
        startColor.a = 0f;
        _fadeOutImage.color = startColor;
        IsFadingOut = true;
    }

    public void StartFadeIn()
    {
        Color c = _fadeOutImage.color;
        c.a = 1f;
        _fadeOutImage.color = c;
        IsFadingIn = true;
    }
}