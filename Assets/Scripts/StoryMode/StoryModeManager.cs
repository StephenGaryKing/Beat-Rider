using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class StoryModeManager : MonoBehaviour
	{
		List<EndGameCondition> m_conditionsCompleated = new List<EndGameCondition>();
		public string m_saveFileName = "LevelProgression";
		public FinalStoryNode[] m_FinalStoryNodes;

		private void Start()
		{
			LoadProgress();
		}

		public void UnlockNode(Cutscene cutscene)
		{
			foreach (FinalStoryNode node in m_FinalStoryNodes)
			{
				StoryNode currentNode = node;
				while (currentNode != null)
				{
					if (currentNode.m_cutsceneToPlay == cutscene)
					{
						currentNode.Unlock();
						return;
					}
					else
						currentNode = currentNode.m_parent;
				}
			}
		}

		public void SaveProgress()
		{
			List<int> data = new List<int>();

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
			SaveFile save = new SaveFile();
			save.AddList(data);
			save.Save(m_saveFileName);
		}

		public void LoadProgress()
		{
			SaveFile save = new SaveFile();
			save.Load(m_saveFileName);
			List<int> data = new List<int>();
			if (save.m_numbers.Count > 0)
				data = save.m_numbers[0].list;
			// look through nodes and apply tally of unlocked steps
			for (int i = 0; i < m_FinalStoryNodes.Length; i ++)
			{
				StoryNode parent = m_FinalStoryNodes[i].m_parent;
				for (int j = 0; j < data[i]; j ++)
					parent = parent.m_parent;
				parent.Unlock();
			}
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
