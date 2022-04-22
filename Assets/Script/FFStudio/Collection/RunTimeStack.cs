/* Created by and for usage of FF Studios (2021). */

using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FFStudio
{
	public abstract class RuntimeStack< T > : ScriptableObject
	{
#region Fields
		public int stackSize;
		
		[ ShowInInspector, ReadOnly ]
		protected Stack< T > stack;

        public Stack< T > Stack
		{
			get
			{
                return stack;
            }
		}
#endregion
	}
}
