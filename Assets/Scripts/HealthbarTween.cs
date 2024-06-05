using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarTween : MonoBehaviour
{
    public Slider healthSlider;
    [SerializeField] private float _fillDuration = 3f;
    [SerializeField] private float _rotationDuration = 1;

    public Vector3 targetRotation = new(0f, 0f, 0f);
    private float _startHealth = 1;

    private void Start()
    {
        healthSlider.value = 0;
        transform.eulerAngles = new Vector3(90, transform.eulerAngles.x, transform.eulerAngles.z);
        var rotationTween = transform.DORotate(targetRotation, _rotationDuration).SetEase(Ease.OutSine);
        rotationTween.OnComplete(FillTween);
    }

    private void FillTween()
    {
        healthSlider.DOValue(1, _fillDuration).SetEase(Ease.OutSine);
    }
}