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
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OhFuck()
    {
        Debug.Log("Can you fucking not");
        player.transform.localPosition = startPosition;
    }
}
