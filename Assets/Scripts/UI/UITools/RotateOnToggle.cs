using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateOnToggle : MonoBehaviour
{
    [SerializeField] private float RotationDuration;
    [SerializeField] private Vector3 untoggledAngle, toggledAngle;

    public void OnToggle(bool value)
    {
        DOTween.Kill(gameObject.name + "_rotate");
        Vector3 newAngle = value ? toggledAngle : untoggledAngle;
        transform.DOLocalRotate(newAngle, RotationDuration).SetEase(Ease.InOutCubic).SetId(gameObject.name + "_rotate");
    }
}
