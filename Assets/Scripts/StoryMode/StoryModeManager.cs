using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class StoryModeManager : MonoBehaviour
	{
		public List<Cutscene> m_cutscenes;
		CutsceneManager m_cutsceneManager;
		List<EndGameCondition> m_conditionsCompleated;

		private void Start()
		{
			m_cutsceneManager = FindObjectOfType<CutsceneManager>();
		}

		//public void LoadCutscene(int cutsceneNumber)
		//{
		//	m_cutsceneManager.PlayCutscene(m_cutscenes[cutsceneNumber]);
		//}

		public void UpdateStoryProgress(StoryNode nodeToUnlock)
		{
			nodeToUnlock.Unlock();
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
