/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace FFStudio
{
	public class NotifyUpdater< SharedDataNotifierType, SharedDataType > : MonoBehaviour
			where SharedDataNotifierType : SharedDataNotifier< SharedDataType >
	{
#region Fields
	[ Title( "Setup" ) ]
		[ SerializeField ] protected SharedDataNotifierType sharedDataNotifier;
		[ SerializeField ] protected UnityEvent< SharedDataType > notify_event;
#endregion

#region Unity API
		private void OnEnable()
		{
			sharedDataNotifier.Subscribe( OnSharedDataChange );
		}

		private void OnDisable()
		{
			sharedDataNotifier.Unsubscribe( OnSharedDataChange );
		}
#endregion

#region Base Class API
		protected virtual void OnSharedDataChange()
		{
			notify_event.Invoke( sharedDataNotifier.SharedValue );
		}
#endregion
	}
}