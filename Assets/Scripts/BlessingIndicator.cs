using UnityEngine;
using UnityEngine.UI;

public class BlessingIndicator : MonoBehaviour
{
    [SerializeField] private Color collectedColor;
    [SerializeField] private float blinkDuration = 1f;
    private Color defaultColor;
    private Image image;
    public bool saved;
    public bool collected;
    private float timer;

    private void Awake()
    {
        image = GetComponent<Image>();
        defaultColor = image.color;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (collected == true && saved == false && timer > blinkDuration)
        {
            image.enabled = !image.enabled;
            timer = 0f;
        }
    }

    public void SetSaved(bool newSaved)
    {
        Debug.Log(name + ".saved=" + newSaved);
        saved = newSaved;
        collected = saved;

        if (collected)
        {
            image.color = collectedColor;
        }
        else
        {
            image.color = defaultColor;
        }
        image.enabled = true;
    }

    public void SetCollected()
    {
        collected = true;
        image.color = collectedColor;
        timer = 0f;
    }

    public void OnSavegameSaved()
    {
        if (collected == true && saved == false)
        {
            SetSaved(true);
        }
    }
}
