using UnityEngine;

namespace InterviewTest.UI
{
    public class ToggleType : MonoBehaviour
    {
        [SerializeField]
        private UIManager.Toggles _toggleType;

        public UIManager.Toggles GetToggleType ()
        {
            return _toggleType;
        }
    }
}