using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WinCondition", menuName = "Beat Rider/Win Condition", order = 1)]

public class WinCondition : ScriptableObject
{
	string[] m_Conditions;
	Image m_Cutscene;
}
