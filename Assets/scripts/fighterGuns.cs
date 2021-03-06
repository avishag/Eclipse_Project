﻿using UnityEngine;
using System.Collections;

public class fighterGuns : Photon.MonoBehaviour {

	public Rigidbody gunAimRigidBody;
	public float yMouseTop;
	public float yMouseBottom;
	bool addForceAtPosition = false;
	Rigidbody myRigidBody;
	int counter=0;

	public int shotBuffer;
	public Vector3[] gunnerMountPoints; //where we are shooting from
	public Transform gunnerShotPrefab;
	float mouseY;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetAxis ("leftGun") == 1)){

			gameObject.GetComponent<PhotonView> ().RPC ("shootGun", PhotonTargets.All, 1);
		}
		if ((Input.GetAxis ("rightGun") == 1)){
			gameObject.GetComponent<PhotonView> ().RPC ("shootGun", PhotonTargets.All, 0);
		}
		if ((Input.GetAxis ("leftGun") == 1) && (Input.GetAxis ("rightGun") == 1)) {
			gameObject.GetComponent<PhotonView> ().RPC ("shootBoth", PhotonTargets.All,null);
		}


		if (mouseY < -yMouseTop) {
			mouseY = -yMouseTop;
			
		}
		if (mouseY > yMouseBottom) {
			mouseY = yMouseBottom;
			
		}
		//gunAimRigidBody.transform.position = new Vector3 (gunAimRigidBody.transform.position.x + Input.GetAxis ("Mouse X"), mouseY, gunAimRigidBody.transform.position.z);
		//gunAimRigidBody.transform.position = new Vector3(transform.position.x+Input.GetAxis("Mouse X"),transform.position.y,transform.position.z);
		//gunAimRigidBody.MovePosition(transform.position+Input.GetAxis ("Mouse Y"));
		//Debug.Log ("x"+Input.GetAxis("Mouse X"));

	
	}

	[RPC]
	void shootGun(int gunPos){
		counter++;
		//Debug.Log ("counter is " + counter);
		if (counter == shotBuffer) {
						// Calculate where the position is in world space for the mount point
						Vector3 pos = transform.position + transform.right * gunnerMountPoints [gunPos].x + transform.up * gunnerMountPoints [gunPos].y + transform.forward * gunnerMountPoints [gunPos].z;
						// Instantiate the laser prefab at position with the spaceships rotation
						Transform gunShot = (Transform)Instantiate (gunnerShotPrefab, pos, transform.rotation);
						// Specify which transform it was that fired this round so we can ignore it for collision/hit
						gunShot.GetComponent<SU_LaserShot> ().firedBy = transform;
						counter = 0;
				}

	}

	[RPC]
	void shootBoth(){
		counter++;
		//Debug.Log ("counter is " + counter);
		if (counter == shotBuffer) {
			foreach (Vector3 gun in gunnerMountPoints) {
				// Calculate where the position is in world space for the mount point
				Vector3 pos = transform.position + transform.right * gun.x + transform.up * gun.y + transform.forward * gun.z;
				// Instantiate the laser prefab at position with the spaceships rotation
				Transform gunShot = (Transform)Instantiate (gunnerShotPrefab, pos, transform.rotation);
				// Specify which transform it was that fired this round so we can ignore it for collision/hit
				gunShot.GetComponent<SU_LaserShot> ().firedBy = transform;
				counter = 0;
				
			}
		}
	}
}
