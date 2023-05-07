using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class NeptuniLightTrigger : MonoBehaviour, IPlayerRespawnListener
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private bool isExit;
    [SerializeField] private Color areaColor;
    [SerializeField] private float transitionTime = 5f;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private RawImage minimapImage;
    [SerializeField] private Image minimapBorder;

    private Color defaultColor;
    private TweenerCore<Color, Color, ColorOptions> lightTween;
    private bool started = false;

    private void Start()
    {
        defaultColor = globalLight.color;
        AddListener();

        started = true;
    }

    private void OnValidate()
    {
        if (started)
        {
            AddListener();
        }
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            StopTweens();

            var color = isExit ? defaultColor : areaColor;
            lightTween = DOTween.To(() => globalLight.color, x => globalLight.color = x, color, transitionTime);

            minimapImage.enabled = isExit;
            minimapBorder.enabled = isExit;
        }
    }

    void IPlayerRespawnListener.OnPlayerRespawn()
    {
        StopTweens();
        globalLight.color = defaultColor;
        minimapImage.enabled = true;
        minimapBorder.enabled = true;
    }

    private void AddListener()
    {
        playerStatus.AddRespawnListener(this);
    }

    private void RemoveListener()
    {
        playerStatus.RemoveRespawnListener(this);
    }

    private void StopTweens()
    {
        if (lightTween != null)
        {
            lightTween.Kill();
            lightTween = null;
        }
    }
}
