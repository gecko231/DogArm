using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CollisionTrigger : MonoBehaviour {
    public string targetTag;
    public enum OnWhat
    {
        Enter,
        Stay,
        Exit
    }
    public OnWhat onWhat;
    public UnityEvent reaction;

    void Start() { }
    void Update() { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (onWhat == OnWhat.Enter && other.gameObject.tag == targetTag) reaction.Invoke();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (onWhat == OnWhat.Stay && other.gameObject.tag == targetTag) reaction.Invoke();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (onWhat == OnWhat.Exit && other.gameObject.tag == targetTag) reaction.Invoke();
    }
}
