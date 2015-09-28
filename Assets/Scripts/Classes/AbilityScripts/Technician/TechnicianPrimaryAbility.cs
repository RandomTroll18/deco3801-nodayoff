using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TechnicianPrimaryAbility : Ability {

	GameObject mainCamera; // The main camera for the player
	GameObject[] surveillanceCameras; // The surveillance cameras
	GameObject nextCameraButton; // Next camera button
	GameObject previousCameraButton; // Previous camera button
	GameObject nextCameraButtonInstance; // Instantiated next camera button
	GameObject previousCameraButtonInstance; // Instantiated previous camera button
	int currentCameraIndex; // The current camera we are looking at
	int numberOfSurveillanceCameras; // The number of surveillance cameras

	/**
	 * Constructor
	 * 
	 * Arguments
	 * - Player player - The player reference
	 */
	public TechnicianPrimaryAbility(Player player) {
		AbilityName = "Big Brother";
		Range = 0.0;
		AbilityRangeType = RangeType.GLOBALTARGETRANGE;
		AbilityActivationType = ActivationType.SUPPORTIVE;
		currentCameraIndex = 0;

		// Get cameras and set surveillance cameras to inactive
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		surveillanceCameras = GameObject.FindGameObjectsWithTag("SurveillanceCameras");
		foreach (GameObject surveillanceCamera in surveillanceCameras)
			surveillanceCamera.SetActive(false);
		numberOfSurveillanceCameras = surveillanceCameras.Length;

		// Get Prefabs for Next and Previous Camera Buttons and initialize them
		nextCameraButton = Resources.Load<GameObject>("AbilityPrefabs/Technician/NextCameraButton");
		previousCameraButton = Resources.Load<GameObject>("AbilityPrefabs/Technician/PreviousCameraButton");

	}

	/**
	 * Extra initialization
	 */
	public override void ExtraInitializing()
	{
		// Instantiate camera buttons and set their anchors
		nextCameraButtonInstance = Object.Instantiate(nextCameraButton);
		previousCameraButtonInstance = Object.Instantiate(previousCameraButton);
		nextCameraButtonInstance.SetActive(false);
		previousCameraButtonInstance.SetActive(false);
		nextCameraButtonInstance.transform.SetParent(ClassPanel.transform, false);
		previousCameraButtonInstance.transform.SetParent(ClassPanel.transform, false);
		nextCameraButtonInstance.GetComponent<Button>().onClick.AddListener(() => pickNextCamera());
		previousCameraButtonInstance.GetComponent<Button>().onClick.AddListener(() => pickPreviousCamera());
	}

	/**
	 * Activate this ability
	 */
	public override void Activate() {
		base.Activate();
		mainCamera.SetActive(false);
		nextCameraButtonInstance.SetActive(true);
		previousCameraButtonInstance.SetActive(true);
		surveillanceCameras[currentCameraIndex].SetActive(true);
	}

	/**
	 * Deactivate this ability
	 */
	public override void Deactivate() {
		base.Deactivate();
		mainCamera.SetActive(true);
		nextCameraButtonInstance.SetActive(false);
		previousCameraButtonInstance.SetActive(false);
	}

	/**
	 * Pick the previous camera
	 */
	void pickPreviousCamera() {
		surveillanceCameras[currentCameraIndex].SetActive(false);
		currentCameraIndex--;
		if (currentCameraIndex == -1) currentCameraIndex = numberOfSurveillanceCameras - 1;
		surveillanceCameras[currentCameraIndex].SetActive(true);
	}

	/**
	 * Pick the next camera
	 */
	void pickNextCamera() {
		surveillanceCameras[currentCameraIndex].SetActive(false);
		currentCameraIndex++;
		currentCameraIndex %= numberOfSurveillanceCameras;
		surveillanceCameras[currentCameraIndex].SetActive(true);
	}
}
