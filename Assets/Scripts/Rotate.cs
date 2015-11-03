using UnityEngine;
using System.Collections;

/**
 * Class for rotating a game object
 */
public class Rotate : MonoBehaviour {
	
	void Update () {
		transform.Rotate(new Vector3(15, 30, 54) * Time.deltaTime);
	}
}
