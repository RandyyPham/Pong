using UnityEngine;
using UnityEngine.EventSystems;

public class ScoringZone : MonoBehaviour
{
    [SerializeField] private EventTrigger.TriggerEvent scoreTrigger;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // gets the ball object when the gameObject collides with anything
        Ball ball = collision2D.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            // triggers an event (e.g scoring)
            BaseEventData eventData = new BaseEventData(EventSystem.current);
            scoreTrigger.Invoke(eventData);
        }
    }
}
