using UnityEngine;

namespace Sensors
{
	public class Touch : Sense
	{
		void OnTriggerEnter(Collider other)
		{
			Aspect aspect = other.GetComponent<Aspect>();
			if (aspect != null)
			{
				//Check the aspect
				if (aspect.aspectType != aspectName)
				{
					print("The player touched me");
				}
			}
		}
	}
}

