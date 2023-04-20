using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class NeptuniLightTrigger : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private bool isExit;
    [SerializeField] private Color areaColor;
    [SerializeField] private float transitionTime = 5f;

    private Color defaultColor;

    void Start()
    {
        defaultColor = globalLight.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            var color = isExit ? defaultColor : areaColor;
            DOTween.To(() => globalLight.color, x => globalLight.color = x, color, transitionTime);
        }
    }
}
