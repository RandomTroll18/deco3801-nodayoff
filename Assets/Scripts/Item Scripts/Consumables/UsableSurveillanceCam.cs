using UnityEngine;

/**
 * Consumable for deployable surveillance cameras
 */
public class UsableSurveillanceCam : SupportConsumables {

	void Start() {
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
		Vector3 instantiatePosition; // Where to instantiate
		if (Amount == 0) // Out of cameras
			return;
		else { // Instantiate camera
			Amount--;
			instantiatePosition = new Vector3(Tile.TileMiddle(targetTile).x, 13f, Tile.TileMiddle(targetTile).z);
			Player.MyPlayer.GetComponent<PhotonView>().RPC("InstantiateSurvCamera", PhotonTargets.All, 
					new object[] {instantiatePosition});
			if (SoundManagerScript.Singleton != null) { /* Play activated sound effect */
				// Move sound manager to this object
				SoundManagerScript.Singleton.gameObject.transform.position = gameObject.transform.position;
				SoundManagerScript.Singleton.PlaySingle3D(ActivateEfx);
			}
			UpdateContextAwareBox();
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
