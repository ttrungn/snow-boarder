using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.Events;

public class HeadTrigger : MonoBehaviour
{
    public BoolVariable IsAlive;

    [Tooltip("Event invoked when collision occurs.")]
    public UnityEvent HeadCollisionEvent;

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
        if (this.triggerCandidates.Contains(other.gameObject) && this.IsAlive.Value)
        {
            GameManager.Instance.PlayOughSound();
            sqLiteGameDb.AddToDb(PlayerMovement.Instance.score, false);
            this.HeadCollisionEvent.Invoke();
        }
    }
}
