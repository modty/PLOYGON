using UnityEngine;
using System.Collections;
using System.Threading;
namespace Scripts
{
	public class ClickArrowController : MonoBehaviour
	{

		public float xSpeed;
		public float ySpeed;
		public float zSpeed;
		Quaternion rotation;
		private void Start()
		{
			rotation = transform.localRotation;
		}
		void OnDisable ()
		{
			transform.localRotation = rotation;
		}
		void Update ()
		{
			transform.Rotate (xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime);
		}
	}
}

