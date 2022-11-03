using UnityEngine;
using UnityEngine.Events;

namespace TemporaryGameCompany
{
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("Event registered with.")]
        public GameEvent Event;
        [Tooltip("Response invoked when event raised.")]
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}