using UnityEngine;
using System.Collections;

public class DeadCell : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y-1), 0.1f).Length == 0)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 1);
        }
	}
}
