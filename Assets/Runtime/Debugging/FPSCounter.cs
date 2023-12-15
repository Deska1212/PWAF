using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Debugging
{
	public class FPSCounter : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI counterText;
		[SerializeField] private float fps;
		[SerializeField] private int maxSampleSize;
		[SerializeField] private float[] deltaTimeSamples;
		private int lastFrameIdx;

		private void Awake()
		{
			deltaTimeSamples = new float[maxSampleSize];
		}

		private void Update()
		{
			deltaTimeSamples[lastFrameIdx] = Time.unscaledDeltaTime;
			lastFrameIdx = (lastFrameIdx + 1) % deltaTimeSamples.Length;

			counterText.text = Mathf.RoundToInt(CalculateFPS()).ToString();
		}

		private float CalculateFPS()
		{
			float total = 0f;
			foreach (var dt in deltaTimeSamples)
			{
				total += dt;
			}

			return deltaTimeSamples.Length / total;
		}

	}
}