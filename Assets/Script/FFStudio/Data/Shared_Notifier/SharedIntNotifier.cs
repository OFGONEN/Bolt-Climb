/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

namespace FFStudio
{
	[ CreateAssetMenu( fileName = "notif_", menuName = "FF/Data/Shared/Notifier/Integer" ) ]
	public class SharedIntNotifier : SharedDataNotifier< int >
	{

		public void Add( int value )
		{
			SharedValue += value;
		}

		public void Subtract( int value )
		{
			SharedValue -= value;
		}
	}
}