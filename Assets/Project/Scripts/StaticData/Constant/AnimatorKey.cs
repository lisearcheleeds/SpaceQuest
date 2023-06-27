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
        public static int Use { get; } = Animator.StringToHash("Use");
        public static int Play { get; } = Animator.StringToHash("Play");
        public static int In { get; } = Animator.StringToHash("In");
        public static int Out { get; } = Animator.StringToHash("Out");
        public static int Reset { get; } = Animator.StringToHash("Reset");
    }
}
