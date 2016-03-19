using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class LevelBoundary : MonoBehaviour {

    public UnityEvent onPlayerLeave;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("DED");
            onPlayerLeave.Invoke();
        }
        else
        {
            Debug.Log("Something exited, idk what tho");
        }
    }
}
