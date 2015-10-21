using UnityEngine;
using System.Collections;

/**
 * Consumable for deployable stun turrets
 */
public class DeployableStunTurret : SupportConsumables {

	// Use this for initialization
	void Start () {
		ItemDescription = "Deployable Stun Turrets For Your Automatic Stunning Needs";
		InstantEffects = null;
		Range = 3.0;

		/* Square range and supportive */
		ItemRangeType = RangeType.SQUARERANGE;
		ItemActivationType = ActivationType.SUPPORTIVE;
		
		/* Activatable and droppable */
		Activatable = true;
		Droppable = true;
	}
	
	public override void Activate(Tile targetTile)
	{
		GameObject instantiatedStunTurret;
		Vector3 instantiatePosition; // Where to instantiate
		if (Amount == 0) // Out of turrets
			return;
		else { // Instantiate Turret
			Amount--;
			instantiatePosition = new Vector3(Tile.TileMiddle(targetTile).x, 0f, Tile.TileMiddle(targetTile).z);
			instantiatedStunTurret = PhotonNetwork.Instantiate(
					"Prefabs/DeployableStunTurret", 
					instantiatePosition, 
					Quaternion.identity, 
					0
			);
			UpdateContextAwareBox();
			instantiatedStunTurret.GetComponent<DeployableStunTurretActionScript>().enabled = true; // Enable script
			if (Amount == 0) { // Remove the item
				Player.MyPlayer.GetComponent<Player>().RemoveItem(this, false);
				Destroy(gameObject);
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}
	
	/* Override abstract functions so compiler doesn't whine */
	public override void Activate()
	{
		throw new System.NotImplementedException();
	}
	
	public override void StartAfterInstantiate()
	{
		throw new System.NotImplementedException();
	}
	
	public override string ToString()
	{
		string toReturn = "Item Name: " + ItemName + StringMethodsScript.NEWLINE;
		toReturn += "Description: " + ItemDescription + StringMethodsScript.NEWLINE;
		toReturn += "Amount: " + Amount + StringMethodsScript.NEWLINE;
		// Next, add range type and activation type
		toReturn += "Range Type: " + EnumsToString.ConvertRangeTypeEnum(ItemRangeType) + StringMethodsScript.NEWLINE;
		toReturn += "Activation Type: " + EnumsToString.ConvertActivationTypeEnum(ItemActivationType) 
			+ StringMethodsScript.NEWLINE;
		return toReturn;
	}
}
