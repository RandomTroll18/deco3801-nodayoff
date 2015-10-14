using UnityEngine;
using System.Collections;

public class DeployableStunTurretActionScript : MonoBehaviour {
	
	bool coolingDown; // Record if the turret is cooling down
	bool coolDownReduced; // Record if the cool down time was already reduced
	const int RANGE = 2; // The range of the stun turret
	const int COOLDOWN_TURNS = 3; // The number of turns to cool down
	const int STUN_DURATION = 2; // The stun duration
	int coolDownTurnsRemaining; // The remaining number of turns for cooling down
	GameManager gameManagerScript; // The game manager script
	GameObject owner; // The owner

	// Use this for initialization
	void Start() {
		gameManagerScript = Object.FindObjectOfType<GameManager>();
		coolingDown = coolDownReduced = false;
		owner = Player.MyPlayer;
	}
	
	// Update is called once per frame
	void Update() {
		GameObject target; // The target
		Tile targetPosition; // The tile at the target's position
		if (coolingDown) { // Cooling down
			GetComponent<PhotonView>().RPC("ChangeMaterial", PhotonTargets.All, new object[] {0});
			ChangeMaterial(0);
			if (!gameManagerScript.IsValidTurn() && !coolDownReduced) { // Cool Down
				Debug.Log("Stun Turret: Cool down");
				coolDownTurnsRemaining--;
				coolDownReduced = true;
				if (coolDownTurnsRemaining == 0) {
					Debug.Log("Stun Turret: Done Cooling Down");
					coolingDown = false;
				}
			} else if (gameManagerScript.IsValidTurn() && coolDownReduced) // We can reduce cool down next turn
				coolDownReduced = false;
		} else { // Ready to use
			GetComponent<PhotonView>().RPC("ChangeMaterial", PhotonTargets.All, new object[] {1});
			ChangeMaterial(1);
			target = findPlayerToStun();
			if (target != null) { // Found a target
				Debug.Log("Stun Turret: Target found: " + target.ToString());
				targetPosition = target.GetComponent<Player>().PlayerPosition();
				target.GetComponent<PhotonView>().RPC("Stun", 0, STUN_DURATION); // Found a valid target
				StunGun.StaticShowEffect(targetPosition.X * 2, 0.001f, targetPosition.Z * 2);
				/* Set turret to cool down */
				coolDownTurnsRemaining = COOLDOWN_TURNS;
				coolingDown = true;
				coolDownReduced = false;
			}
		}
	}

	/**
	 * RPC Call to change material
	 * 
	 * Arguments
	 * - int materialToSet - variable indicating what material to change into
	 */
	[PunRPC]
	public void ChangeMaterial(int materialToSet) {
		switch (materialToSet) {
		case 0: // Cool down material
			GetComponent<Renderer>().material = 
					Resources.Load<Material>("ItemMaterials/Deployable/StunTurretCooldownMaterial");
			break;
		case 1: // Ready Material
			GetComponent<Renderer>().material = 
					Resources.Load<Material>("ItemMaterials/Deployable/StunTurretReadyMaterial");
			break;
		}
	}

	/**
	 * Find the closest player to stun
	 * 
	 * Returns
	 * - A valid player to stun if one exists. null if otherwise
	 */
	GameObject findPlayerToStun() {
		GameObject target = null; // The target
		int distance; // The distance to a character
		int smallestDistance = -1; // The smallest distance recorded

		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
			if (player.Equals(owner))
				continue; // Invalid target

			distance = MovementController.TileDistance(transform.position, player.transform.position, 
					Player.MyPlayer.GetComponent<MovementController>().GetBlockedTiles());

			if (distance == -1)
				continue;
			else if (distance >= 0 && distance <= 2) { // Found a player within range
				if (smallestDistance == -1 || smallestDistance > distance) { // Found a closer player
					smallestDistance = distance;
					target = player;
				}
			}
		}

		return target;
	}
}
