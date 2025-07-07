using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundedTrigger : MonoBehaviour
{
    [Tooltip("Event invoked when player comes in contact with ground.")]
    public UnityEvent PlayerGroundedEvent;

    [Tooltip("Event invoked when player comes out of contact with ground.")]
    public UnityEvent PlayerAirbornEvent;

    [Tooltip("GameObjects to interact with.")]
    public GameObject[] TriggerCandidates;

    private HashSet<GameObject> triggerCandidates;

    private void Awake()
    {
        this.triggerCandidates = new HashSet<GameObject>(this.TriggerCandidates);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTriggerCandidateOrChild(other.gameObject))
        {
            this.PlayerGroundedEvent.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsTriggerCandidateOrChild(other.gameObject))
        {
            this.PlayerAirbornEvent.Invoke();
        }
    }

    private bool IsTriggerCandidateOrChild(GameObject obj)
    {
        Transform current = obj.transform;
        while (current != null)
        {
            if (triggerCandidates.Contains(current.gameObject))
            {
                return true;
            }
            current = current.parent;
        }
        return false;
    }
}
