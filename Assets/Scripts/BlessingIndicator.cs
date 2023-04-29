using UnityEngine;
using UnityEngine.UI;

public class BlessingIndicator : MonoBehaviour
{
    [SerializeField] private Color collectedColor;
    [SerializeField] private float blinkDuration = 1f;

    private Image image;
    private Color defaultColor;
    public bool saved;
    public bool collected;

    private void Start()
    {
        image = GetComponent<Image>();
        defaultColor = image.color;
    }

    private void Update()
    {
        image.enabled = IsImageEnabled();
        if (collected)
        {
            image.color = collectedColor;
        }
        else
        {
            image.color = defaultColor;
        }
    }

    /* Might be called during Start() from another object */
    public void SetSavedAndCollected(bool newSaved)
    {
        saved = newSaved;
        collected = newSaved;
    }

    public void SetSaved(bool newSaved)
    {
        saved = newSaved;
    }

    public void SetCollected()
    {
        collected = true;
    }

    private bool IsImageEnabled()
    {
        if (collected == true && saved == false)
        {
            return (Time.time % (2 * blinkDuration)) >= blinkDuration;
        }

        return true;
    }
}
