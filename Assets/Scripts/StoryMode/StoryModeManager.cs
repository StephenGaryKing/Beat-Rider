using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatRider
{
	public class StoryModeManager : MonoBehaviour
	{
		[SerializeField] List<EndGameCondition> m_conditionsCompleated = new List<EndGameCondition>();
		public string m_saveFileName = "LevelProgression";
		public FinalStoryNode[] m_FinalStoryNodes;
		public bool m_debugMode = false;

		private void Start()
		{
			LoadProgress();
		}

		public void UnlockNode(Cutscene cs)
		{
			foreach (FinalStoryNode node in m_FinalStoryNodes)
			{
				if (node.m_cutsceneToPlay != null && node.m_cutsceneToPlay.name == cs.name)
				{
					if (CheckConditions(GetConditions(node)))
					{
						node.Unlock();
						node.PlayAnimatic();
						return;
					}
					else
					{
						if (m_debugMode)
						{
							Debug.LogError("Cutscene named " + cs.name + " tried to play but couldn't due to the conditions provided");
							Debug.LogError(LogList("conditions to check", GetConditions(node)));
							Debug.LogError(LogList("compleated conditions", m_conditionsCompleated));
						}
					}
				}

				StoryNode currentNode = node.m_parent;
				while (currentNode != null)
				{
					if (currentNode.m_cutsceneToPlay != null && currentNode.m_cutsceneToPlay.name == cs.name)
					{
						if (m_debugMode)
							Debug.LogError(currentNode.name);
						if (CheckConditions(GetConditions(currentNode)))
						{
							if (m_debugMode)
								Debug.LogError("Unlocking  " + currentNode.name);
							Debug.Log("Unlocking  " + currentNode.name);
							// this is the correct node
							currentNode.Unlock();
							return;
						}
						else
							currentNode = currentNode.m_parent;
					}
					else
						currentNode = currentNode.m_parent;
				}
			}
		}

		// Build a list of past events in that line
		List<EndGameCondition> GetConditions(StoryNode startingNode)
		{
			StoryNode nodeToCheck = startingNode;
			List<EndGameCondition> conditions = new List<EndGameCondition>();
			while (nodeToCheck != null)
			{
				if (nodeToCheck.m_EndGameCondition != EndGameCondition.NONE)
				{
					//Debug.LogError("node " + nodeToCheck.name + " added " + nodeToCheck.m_EndGameCondition + " using cutscene " + nodeToCheck.m_cutsceneToPlay.name);
					conditions.Add(nodeToCheck.m_EndGameCondition);
				}
				nodeToCheck = nodeToCheck.m_parent;
				//if (nodeToCheck)
					//Debug.LogError("now at node " + nodeToCheck.name);
			}
			return conditions;
		}

		// Check the list (which is backwards due to data gathering technique) against our current list
		bool CheckConditions(List<EndGameCondition> conditions)
		{
			if (conditions.Count != m_conditionsCompleated.Count)
			{
				if (m_debugMode)
					Debug.LogError("Node was rejected due to it having " + conditions.Count + " conditions vs the requested " + m_conditionsCompleated.Count + " conditions");
				return false;
			}
			//LogList("conditions to check", conditions);
			//LogList("compleated conditions", m_conditionsCompleated);
			for (int i = 0; i < m_conditionsCompleated.Count; i ++)
			{
				int invertedIndex = (m_conditionsCompleated.Count - 1) - i;
				if (m_conditionsCompleated[i] != conditions[invertedIndex])
				{
					if (m_debugMode)
						Debug.LogError("Node was rejected due to wrong condition " + LogList("conditions to check", conditions) + " VS " + LogList("compleated conditions", m_conditionsCompleated));
					return false;
				}
			}
			return true;
		}

		string LogList(string name, List<EndGameCondition> list)
		{
			string log = "";
			foreach (EndGameCondition con in list)
				log += con.ToString() + " : ";
			return (name + " : " + log);
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
                if (parent)
				    parent.Unlock();
			}
		}

		public void AddCondition(EndGameCondition condition)
		{
			if (condition == EndGameCondition.NONE || m_conditionsCompleated.Contains(condition))
				return;
			m_conditionsCompleated.Add(condition);
			if (m_debugMode)
				Debug.Log("Added condition " + condition.ToString());
		}

		public void InitializeConditions(StoryNode node)
		{
			List<EndGameCondition> conditions = new List<EndGameCondition>();
			m_conditionsCompleated.Clear();
			StoryNode nodeToCheck = node;
			while(nodeToCheck != null)
			{
				if (nodeToCheck.m_EndGameCondition != EndGameCondition.NONE)
					conditions.Add(nodeToCheck.m_EndGameCondition);
				nodeToCheck = nodeToCheck.m_parent;
			}
			for(int i = conditions.Count-1; i >= 0; i--)
				AddCondition(conditions[i]);
		}

		public void ClearConditions()
		{
			m_conditionsCompleated.Clear();
		}
	}
}
