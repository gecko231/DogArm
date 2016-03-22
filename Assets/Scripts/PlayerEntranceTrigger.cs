using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerEntranceTrigger : MonoBehaviour {

    public UnityEvent onPlayerEntrance;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") onPlayerEntrance.Invoke();
    }
}
