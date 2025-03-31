using UnityEngine;

namespace Config
{
    public class AnimHash
    {
        public static readonly int Close = Animator.StringToHash("Close");

        public static readonly int IsOn = Animator.StringToHash("IsOn");
        public static readonly int Skip = Animator.StringToHash("Skip");
        public static readonly int SkillCount = Animator.StringToHash("SkillCount");

        public static readonly int Submit = Animator.StringToHash("Submit");
        public static readonly int SubmitSuccess = Animator.StringToHash("SubmitSuccess");
        public static readonly int SubmitFail = Animator.StringToHash("SubmitFail");

        public static readonly int Approve = Animator.StringToHash("Approve");
        public static readonly int Return = Animator.StringToHash("Return");

        public static readonly int Finish = Animator.StringToHash("Finish");
    }
}
