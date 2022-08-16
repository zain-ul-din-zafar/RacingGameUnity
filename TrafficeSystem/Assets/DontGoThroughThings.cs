using UnityEngine;

public class DontGoThroughThings : MonoBehaviour
{
	public bool sendTriggerMessage;

	public LayerMask layerMask = -1;

	public float skinWidth = 0.1f;

	private float minimumExtent;

	private float partialExtent;

	private float previousTime;

	private const float minDeltaTime = 0.3f;

	private Vector3 previousPosition;

	private Rigidbody myRigidbody;

	private Collider myCollider;

	private Vector3 offset;

	public void SetPrevPostion(Vector3 newPosition)
	{
		previousPosition = newPosition;
	}

	public void SetPrevPostion()
	{
		SetPrevPostion(base.transform.position);
	}

	private void Start()
	{
		offset = new Vector3(0f, 1f, 0f);
		myRigidbody = GetComponent<Rigidbody>();
		myCollider = GetComponent<Collider>();
		previousPosition = myRigidbody.position + offset;
		Vector3 extents = myCollider.bounds.extents;
		float x = extents.x;
		Vector3 extents2 = myCollider.bounds.extents;
		float a = Mathf.Min(x, extents2.y);
		Vector3 extents3 = myCollider.bounds.extents;
		minimumExtent = Mathf.Min(a, extents3.z);
		partialExtent = minimumExtent * (1f - skinWidth);
		previousTime = Time.fixedTime;
	}

	private void FixedUpdate()
	{
		Vector3 vector = myRigidbody.position + offset - previousPosition;
		float sqrMagnitude = vector.sqrMagnitude;
		float num = Mathf.Sqrt(sqrMagnitude);
			if (Time.fixedTime - previousTime < 0.3f && Physics.Raycast(previousPosition, vector, out RaycastHit hitInfo, num, layerMask.value))
			{
				if (!hitInfo.collider)
				{
					return;
				}
				if (hitInfo.collider.isTrigger)
				{
					//hitInfo.collider.SendMessage("OnTriggerEnter", myCollider);
				}
				if (!hitInfo.collider.isTrigger)
				{
					myRigidbody.position = hitInfo.point - vector / num * partialExtent;
					myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, 5f);
					previousTime = Time.fixedTime;
				}
			}
		previousTime = Time.fixedTime;
		previousPosition = myRigidbody.position + offset;
	}
}
