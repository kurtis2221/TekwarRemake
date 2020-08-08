using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
	public float sensitivityX = 5F;
	public float sensitivityY = 5F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	Transform player;
	
	void Awake()
	{
		player = transform.root;
	}
	
	void Update()
	{
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
		transform.localRotation = Quaternion.Euler(-rotationY, 0, 0);
		player.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
	}
}