using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;


[TestFixture]
public class CodePlaground01 
{
	[Test]
	public void CodeTest()
	{
		Vector3 v1 = new Vector3(1, 0, 0);
        Vector3 v2 = new Vector3(1, 0, 0);

        float v = Vector3.Dot(v1, v2);

        Debug.Log(v);
	}
}
