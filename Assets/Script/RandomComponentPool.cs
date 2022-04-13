/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public abstract class RandomComponentPool< T > : ScriptableObject  where T: Component
{
    public ComponentPool< T >[] componentPool;

	public T GetEntity()
	{
		var pool = componentPool.ReturnRandom();
		return pool.GetEntity();
	}
}
