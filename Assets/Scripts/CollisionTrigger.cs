using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class TriggeredEvents : UnityEvent<GameObject> { }

public class CollisionTrigger : MonoBehaviour {
    public string targetTag;
    public enum OnWhat
    {
        Enter,
        Stay,
        Exit
    }
    public OnWhat onWhat;
    [SerializeField]
    public TriggeredEvents reaction;

    void Start() { }
    void Update() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (onWhat == OnWhat.Enter && other.gameObject.tag == targetTag) reaction.Invoke(gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (onWhat == OnWhat.Stay && other.gameObject.tag == targetTag) reaction.Invoke(gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (onWhat == OnWhat.Exit && other.gameObject.tag == targetTag) reaction.Invoke(gameObject);
    }
}
