using UnityEngine;
using System.Collections;

/**
 * Consumable for deployable surveillance cameras
 */
public class UsableSurveillanceCam : SupportConsumables {

	void Start () {
		ItemDescription = "Deployable Surveillance Cameras. For when you need to " +
			"watch over things while you're gone";

		InstantEffects = null;

		Range = 3.0; // Range of 3 tiles

		/* Square range and supportive */
		ItemRangeType = RangeType.SQUARERANGE;
		ItemActivationType = ActivationType.SUPPORTIVE;

		/* Activatable and droppable */
		Activatable = true;
		Droppable = true;
	}

	public override void Activate(Tile targetTile)
	{
		GameObject instantiatedCamera; // The instantiated camera
		Vector3 instantiatePosition; // Where to instantiate
		Quaternion cameraQuaternion; // The quaternion of this object
		if (Amount == 0) { // Out of cameras
			return;
		} else { // Instantiate camera
			Amount--;
			instantiatePosition = new Vector3(Tile.TileMiddle(targetTile).x, 5f, Tile.TileMiddle(targetTile).z);
			instantiatedCamera = PhotonNetwork.Instantiate(
					"SurveillanceCamera", 
					instantiatePosition, 
					Quaternion.identity, 
			        0
			);
			instantiatedCamera.transform.LookAt(Player.MyPlayer.transform.position);
			cameraQuaternion = instantiatedCamera.transform.rotation;
			instantiatedCamera.transform.Rotate(-cameraQuaternion.x + 3.0f, 0f, 0f);
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
