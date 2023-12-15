using UnityEngine;

public static class DeskaUtils
{
	public static int RandomSign()
	{
		return Random.value < 0.5f ? -1 : 1;
	}
}
