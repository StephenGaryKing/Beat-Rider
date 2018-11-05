using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class WaitForVoiceLineEnd : CustomYieldInstruction
	{
		float totalTime = 0;
		float timeLeft = 0;
		KeyCode skip;

		public WaitForVoiceLineEnd(float time = 0, KeyCode skipKey = KeyCode.Space)
		{
			totalTime = time;
			timeLeft = time;
			skip = skipKey;
		}

		public override bool keepWaiting
		{
			get
			{
				if (totalTime - timeLeft > 0.1 && Input.GetKeyDown(skip))
					return false;
				timeLeft -= Time.deltaTime;
				return timeLeft > 0;
			}
		}
	}
}