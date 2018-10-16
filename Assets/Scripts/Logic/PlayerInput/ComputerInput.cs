using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInput : PlayerInput {
	public override float GatherInput()
	{
		return Input.GetAxisRaw("Horizontal");
	}
}
