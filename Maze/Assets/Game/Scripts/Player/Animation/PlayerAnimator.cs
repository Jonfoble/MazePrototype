using System;
using DG.Tweening;
using UnityEngine;

namespace Player.Animation
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private float _animationTransitionDuration;
        private Animator m_animator;
        private static readonly int s_horizontal = Animator.StringToHash("Horizontal");
        private static readonly int s_vertical = Animator.StringToHash("Vertical");

        private float m_currentValue = 0;
        private Tween m_transitionTween;
        private void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        public void UpdateBlendTree(float value)
        {
            if (Math.Abs(value - m_currentValue) < .01f) return;
            
            MakeTransition(m_currentValue, value);
            m_currentValue = value;
        }

        private void MakeTransition(float from, float target)
        {
            if (m_transitionTween != null && m_transitionTween.IsActive())
                m_transitionTween.Kill();
            
            m_transitionTween = DOVirtual.Float(from,
                target,
                _animationTransitionDuration,
                f=>m_animator.SetFloat(s_vertical, f));
        }
    }
}