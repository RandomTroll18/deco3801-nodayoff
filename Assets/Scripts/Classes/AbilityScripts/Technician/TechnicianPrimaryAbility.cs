using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TechnicianPrimaryAbility : Ability {

	GameObject mainCamera; // The main camera for the player
	GameObject surveillanceCameraContainer; // The surveillance camera container
	List<GameObject> surveillanceCameras; // List of surveillance cameras
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
		surveillanceCameraContainer = GameObject.FindGameObjectWithTag("SurveillanceCameraContainer");
		surveillanceCameras = new List<GameObject>();
		foreach (Transform surveillanceCamera in surveillanceCameraContainer.transform) {
			surveillanceCamera.gameObject.SetActive(false);
			surveillanceCameras.Add(surveillanceCamera.gameObject);
		}
		numberOfSurveillanceCameras = surveillanceCameras.Count;

		// Get Prefabs for Next and Previous Camera Buttons and initialize them
		nextCameraButton = Resources.Load<GameObject>("Prefabs/AbilityPrefabs/Technician/NextCameraButton");
		previousCameraButton = Resources.Load<GameObject>("Prefabs/AbilityPrefabs/Technician/PreviousCameraButton");

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
		foreach (Transform surveillanceCamera in surveillanceCameraContainer.transform) {
			surveillanceCamera.gameObject.SetActive(false);
			if (!surveillanceCameras.Contains(surveillanceCamera.gameObject)) { // Add new surveillance camera
				surveillanceCameras.Add(surveillanceCamera.gameObject);
			}
		}
		numberOfSurveillanceCameras = surveillanceCameras.Count;
		Debug.Log("Number of surveillance cameras: " + numberOfSurveillanceCameras);
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
