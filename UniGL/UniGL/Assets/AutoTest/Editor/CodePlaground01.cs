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
		Vector3 v1 = new Vector3( 2, 3, 4 );
		Vector4 v2 = v1;
		v2.w = 2;

		Debug.Log( (v2).ToString() );
	}
}
