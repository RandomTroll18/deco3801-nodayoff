using UnityEngine;
using System.Collections;

public class DeployableStunTurretActionScript : MonoBehaviour {

	Material readyMaterial; // The material for when stun turret is ready
	Material coolDownMaterial; // The material for when stun turret is cooling down
	bool coolingDown; // Record if the turret is cooling down
	const int USE_PER_TURN = 3; // The turret can only be used 3 times before needing to cool down
	int currentNumberOfUses = 0; // The current number of uses left before cooling down

	// Use this for initialization
	void Start () {
		readyMaterial = Resources.Load<Material>("ItemMaterials/Deployable/StunTurretReadyMaterial");
		coolDownMaterial = Resources.Load<Material>("ItemMaterials/Deployable/StunTurretCooldownMaterial");
	}
	
	// Update is called once per frame
	void Update () {
		if (coolingDown) { // Cooling down
			GetComponent<Renderer>().material = coolDownMaterial;
		} else { // Ready to use
			GetComponent<Renderer>().material = readyMaterial;
			// Find a player to stun
		}
	}
}
