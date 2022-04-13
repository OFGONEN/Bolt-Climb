/* Created by and for usage of FF Studios (2021). */
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace FFStudio
{
	/* This class holds references to ScriptableObject assets. These ScriptableObjects are singletons, so they need to load before a Scene does.
	 * Using this class ensures at least one script from a scene holds a reference to these important ScriptableObjects. */
	public class AssetManager : MonoBehaviour
	{
#region Fields
	[ Title( "Setup" ) ]
		public GameSettings gameSettings;
		public CurrentLevelData currentLevelData;

	[ Title( "Pool" ) ]
		public Pool_UIPopUpText pool_UIPopUpText;
		public ShatterPool[] pool_shatter_array;

	[ Title( "Setup" ) ]
		public UnityEvent onEnable;
		public UnityEvent onDisable;
		public UnityEvent onAwake;
		public UnityEvent onStart;
#endregion

#region UnityAPI
		private void OnEnable()
		{
			onEnable.Invoke();
		}

		private void OnDisable()
		{
			onDisable.Invoke();
		}

		private void Awake()
		{
			pool_UIPopUpText.InitPool( transform, false );

			for( var i = 0; i < pool_shatter_array.Length; i++ )
			{
				pool_shatter_array[ i ].InitPool( transform, false );
			}

			onAwake.Invoke();
		}

		private void Start()
		{
			onStart.Invoke();
		}
#endregion
	}
}