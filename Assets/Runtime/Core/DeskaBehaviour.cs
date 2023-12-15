using System;
using UnityEngine;

public class DeskaBehaviour : MonoBehaviour
{
	private DeskaLogger logger;

	protected DeskaLogger Logger
	{
		get
		{
			return logger;
		}
	}

	protected virtual void InitLogger()
	{
		logger = new DeskaLogger();
	}
}
