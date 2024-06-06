using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Tweens
{
    public class MainMenuTween : MonoBehaviour
    {
        public TMP_Text targetText; // The Text component to animate
        public float targetFontSize = 30f; // The target font size to animate to
        public float fontSizeAnimationDuration = 1f; // The duration of the animation
        public float menuButtonsAnimationDuration = 0.5f;
        [SerializeField] private GameObject menuButtons;
        [SerializeField] private RectTransform menuButtonRectTransform;
        private Vector2 hiddenPosition;
        private Vector2 visiblePosition;

        private void Start()
        {
            menuButtonRectTransform = menuButtons.GetComponent<RectTransform>();
            visiblePosition = menuButtonRectTransform.anchoredPosition;
            hiddenPosition = new Vector2(menuButtonRectTransform.anchoredPosition.x,
                -(Screen.height + menuButtonRectTransform.rect.height * 2));
            menuButtonRectTransform.anchoredPosition = hiddenPosition;
            menuButtons.SetActive(false);
            targetText.fontSize = 0;
            AnimateFontSize();
            ShowMenuButtons();
        }


        // Method to animate the font size
        private void AnimateFontSize()
        {
            if (targetText != null)
                // Create a tween to animate the font size
                DOTween.To(() => targetText.fontSize, x => targetText.fontSize = x, targetFontSize,
                    fontSizeAnimationDuration).SetEase(Ease.OutBack);
        }

        private void ShowMenuButtons()
        {
            menuButtons.SetActive(true);
            menuButtonRectTransform.DOAnchorPos(visiblePosition, menuButtonsAnimationDuration).SetEase(Ease.OutBack);
        }

        private void HideMenuButtons()
        {
            menuButtonRectTransform.DOAnchorPos(hiddenPosition, menuButtonsAnimationDuration).SetEase(Ease.InBack)
                .OnComplete(Hide);

            void Hide()
            {
                menuButtons.SetActive(false);
            }
        }
    }
}