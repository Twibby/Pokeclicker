using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private Transform animatedObjectTransform;
    [SerializeField] private float fadingduration;
    public float Duration { get { return fadingduration; } }

    private bool isOpen;
    private float delayProgression;
    public UnityEvent OnClosing;

    public void StartLoading()
    {
        if (isOpen)
            return;

        isOpen = true;

        DOTween.Kill(fadeCanvasGroup.name + "_fade");
        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.DOFade(1, fadingduration).SetEase(Ease.InOutCubic).SetId(fadeCanvasGroup.name + "_fade");

        DOTween.Kill(animatedObjectTransform.name + "_scale");
        animatedObjectTransform.localScale = Vector3.zero;
        animatedObjectTransform.DOScale(1, fadingduration).SetEase(Ease.InOutCubic).SetId(animatedObjectTransform.name + "_scale");
    }

    public void CloseAfterDelay(float delay)
    {
        DOTween.Kill(this.name + "_delayToClose");
        DOTween.To(() => delayProgression, x => delayProgression = x, delay, delay).SetId(this.name + "_delayToClose").OnComplete(StopLoading);
    }

    public void StopLoading()
    {
        if (!isOpen)
            return;

        DOTween.Kill(this.name + "_delayToClose");
        isOpen = false;

        DOTween.Kill(fadeCanvasGroup.name + "_fade");
        fadeCanvasGroup.DOFade(0, fadingduration).SetEase(Ease.InOutCubic).SetId(fadeCanvasGroup.name + "_fade");

        DOTween.Kill(animatedObjectTransform.name + "_scale");
        animatedObjectTransform.DOScale(0, fadingduration).SetEase(Ease.InOutCubic).SetId(animatedObjectTransform.name + "_scale").OnComplete(() => OnCloseComplete());
    }

    public void OnCloseComplete()
    {
        OnClosing.Invoke();

        OnClosing.RemoveAllListeners();

        this.gameObject.SetActive(false);
    }
}
