using UnityEngine;
using System.Collections;

/**
 * Timeout class for an object which forces the 
 * object to be destroyed after 5 seconds of appearing
 */
public class HighlightTimeout : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject, 5f);
	}
}
