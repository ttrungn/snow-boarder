using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.Events;

public class FinishLineTrigger : MonoBehaviour
{
    [Tooltip("Event invoked when player crosses finish line.")]
    public UnityEvent FinishLineCrossedEvent;

    [Tooltip("GameObjects to interact with.")]
    public GameObject[] TriggerCandidates;

    private HashSet<GameObject> triggerCandidates;

    private SqLiteGameDb sqLiteGameDb;


    private void Awake()
    {
        this.triggerCandidates = new HashSet<GameObject>(this.TriggerCandidates);
        sqLiteGameDb = FindObjectOfType<SqLiteGameDb>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.triggerCandidates.Contains(other.gameObject))
        {
            sqLiteGameDb.AddToDb(PlayerMovement.Instance.score, true);
            this.FinishLineCrossedEvent.Invoke();
        }
    }
}
