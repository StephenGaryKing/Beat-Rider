using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class StoryModeManager : MonoBehaviour
	{
		List<EndGameCondition> m_conditionsCompleated;

		public FinalStoryNode[] m_FinalStoryNodes;

		public void SaveProgress()
		{
			List<float> data = new List<float>();

			foreach (FinalStoryNode node in m_FinalStoryNodes)
			{
				int steps = 0;
				bool unlockFound = false;

				if (node.m_unlocked)
				{
					data.Add(steps);
					continue;
				}
				// if the final node has not been unlocked
				StoryNode parent = node.m_parent;
				// look through nodes and tally steps till unlocked
				while (!unlockFound)
				{
					steps++;
					if (!parent)
					{
						data.Add(steps);
						unlockFound = true;
						break;
					}
					if (parent.m_unlocked)
					{
						data.Add(steps);
						unlockFound = true;
						break;
					}
					parent = parent.m_parent;
				}
			}
		}

		public void LoadProgress()
		{
			// look through nodes and apply tally of unlocked steps
		}

		public void AddCondition(EndGameCondition condition)
		{
			if (condition == EndGameCondition.NONE)
				return;
			if (!m_conditionsCompleated.Contains(condition))
				m_conditionsCompleated.Add(condition);
		}

		public void InitializeConditions(StoryNode node)
		{
			m_conditionsCompleated.Clear();
			AddCondition(node.m_EndGameCondition);
			StoryNode parent = node.m_parent;
			while(parent != null)
			{
				AddCondition(parent.m_EndGameCondition);
				parent = parent.m_parent;
			}
		}
	}
}
