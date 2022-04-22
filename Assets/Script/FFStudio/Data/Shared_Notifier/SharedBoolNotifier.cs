/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using Sirenix.OdinInspector;

namespace FFStudio
{
	[ InlineEditor, CreateAssetMenu( fileName = "notif_", menuName = "FF/Data/Shared/Notifier/Boolean" ) ]
	public class SharedBoolNotifier : SharedDataNotifier< bool >
	{
	}
}