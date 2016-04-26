using UnityEngine;
using System.Collections;

public class PlayerDiedResponder : MonoBehaviour {

    public GameObject player;

    public Vector3 startPosition;

	// Use this for initialization
	void Start () {
        if (!player) player = GameObject.Find("Player");
        startPosition = player.transform.localPosition;
	}
	
	void Update () { }

    public void SetCheckpoint(GameObject checkpoint)
    {
        Debug.Log("Setting checkpoint to " + checkpoint.name);
        startPosition = checkpoint.transform.localPosition;
    }

    public void OhFuck()
    {
        Debug.Log("Can you fucking not");
        player.transform.localPosition = startPosition;
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
