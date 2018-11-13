using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class FinalStoryNode : StoryNode
	{
		public GameObject m_animaticPrefab;

		public override void Select()
		{
			PlayAnimatic();
		}

		public void PlayAnimatic()
		{
			// Create the stage for the animatic to play on
			Instantiate(m_animaticPrefab);
		}
	}
}