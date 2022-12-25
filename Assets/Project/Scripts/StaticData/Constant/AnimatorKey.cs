using UnityEngine;

namespace AloneSpace
{
    public static class AnimatorKey
    {
        public static int Default { get; } = Animator.StringToHash("Default");
        public static int IsSelect { get; } = Animator.StringToHash("IsSelect");
        public static int On { get; } = Animator.StringToHash("On");
        public static int Off { get; } = Animator.StringToHash("Off");
        public static int Confirming { get; } = Animator.StringToHash("Confirming");
        public static int Confirm { get; } = Animator.StringToHash("Confirm");
    }
}