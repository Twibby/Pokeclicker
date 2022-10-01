using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private Transform animatedObjectTransform;
    [SerializeField] private float fadingduration;
    public float Duration { get { return fadingduration; } }

    public void StartLoading()
    {
        DOTween.Kill(fadeCanvasGroup.name + "_fade");
        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.DOFade(1, fadingduration).SetEase(Ease.InOutCubic).SetId(fadeCanvasGroup.name + "_fade");

        DOTween.Kill(animatedObjectTransform.name + "_scale");
        animatedObjectTransform.localScale = Vector3.zero;
        animatedObjectTransform.DOScale(1, fadingduration).SetEase(Ease.InOutCubic).SetId(animatedObjectTransform.name + "_scale");
    }

    public void StopLoading()
    {
        DOTween.Kill(fadeCanvasGroup.name + "_fade");
        fadeCanvasGroup.DOFade(0, fadingduration).SetEase(Ease.InOutCubic).SetId(fadeCanvasGroup.name + "_fade");

        DOTween.Kill(animatedObjectTransform.name + "_scale");
        animatedObjectTransform.DOScale(0, fadingduration).SetEase(Ease.InOutCubic).SetId(animatedObjectTransform.name + "_scale").OnComplete(() => this.gameObject.SetActive(false));
    }
}
