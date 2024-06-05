using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class DollyCartMovement : MonoBehaviour
{
    public CinemachineDollyCart dollyCart;
    private readonly float _duration = 5f; // Duration for a full loop of the track

    private void Start()
    {
        // Ensure the m_Position starts at 0
        dollyCart.m_Position = 0f;

        // Tween the m_Position property
        DOTween.To(() => dollyCart.m_Position, x => dollyCart.m_Position = x, dollyCart.m_Path.PathLength, _duration)
            .SetEase(Ease.OutBack);
    }
}