using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class SpeechBubble : MonoBehaviour
{
    public Image image;
    private Color startColor;
    private Color endColor;
    private float startTime;
    public float defaultDuration = 0.75f;
    private float duration;

    public bool IsFading { get; private set; }

    void Start()
    {
        if (image == null) image = GetComponent<Image>();
        startColor = endColor = image.color;
        endColor.a = 0;
    }

    void Update()
    {
        if (IsFading)
        {
            var current_duration = Time.time - startTime;

            var percent_done = current_duration / duration;

            image.color = Color.Lerp(startColor, endColor, percent_done);

            if (current_duration > duration) IsFading = false;
        }
    }

    void OnValidate()
    {
        if (defaultDuration < 0) Debug.LogError("A negative duration doesn't make sense");
    }

    public void Fade(float duration)
    {
        if (!IsFading)
        {
            startTime = Time.time;
            this.duration = duration;
            IsFading = true;
        }
    }

    public void Fade()
    {
        Fade(defaultDuration);
    }

    public void ResetFade()
    {
        IsFading = false;
        image.color = startColor;
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
