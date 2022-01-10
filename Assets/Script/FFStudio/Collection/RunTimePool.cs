/* Created by and for usage of FF Studios (2021). */

using System.Collections.Generic;
using UnityEngine;
using FFStudio;

namespace FFStudio
{
	public abstract class RunTimePool< T > : RunTimeStack< T >
	{
#region Fields
        public T poolEntity;
#endregion

#region Unity API
#endregion

#region API
		public abstract void InitPool();
		public abstract T GetEntity();
		public abstract void ReturnEntity( T entity );
#endregion

#region Implementation
		protected abstract T InitEntity();
#endregion
	}
}