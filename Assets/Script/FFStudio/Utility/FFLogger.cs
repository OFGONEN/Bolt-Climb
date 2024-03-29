/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

/// <summary>
/// A logger that wraps Unity's internal logger.
/// Calls to its methods are stripped in case the UNITY_EDITOR is not defined.
/// </summary>
namespace FFStudio
{
	public sealed class FFLogger
	{
		public const string LOGGER_SYMBOL = "UNITY_EDITOR";

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void Log( object message )
		{
			Debug.Log( message );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void Log( object message, Object context )
		{
			Debug.Log( message, context );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogFormat( string message, params object[] args )
		{
			Debug.LogFormat( message, args );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogFormat( Object context, string message, params object[] args )
		{
			Debug.LogFormat( context, message, args );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogWarning( object message )
		{
			Debug.LogWarning( message );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogWarning( object message, Object context )
		{
			Debug.LogWarning( message, context );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogWarningFormat( string message, params object[] args )
		{
			Debug.LogWarningFormat( message, args );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogWarningFormat( Object context, string message, params object[] args )
		{
			Debug.LogWarningFormat( context, message, args );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogError( object message )
		{
			Debug.LogError( message );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogError( object message, Object context )
		{
			Debug.LogError( message, context );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogErrorFormat( string message, params object[] args )
		{
			Debug.LogErrorFormat( message, args );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogErrorFormat( Object context, string message, params object[] args )
		{
			Debug.LogErrorFormat( context, message, args );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogException( System.Exception exception )
		{
			Debug.LogException( exception );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void LogException( System.Exception exception, Object context )
		{
			Debug.LogException( exception, context );
		}

#region UI

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void PopUpText( Vector3 position, string message )
		{
			var entity = Pool_Debug_UI_Text.Instance.GetEntity();
			entity.Spawn( position, message );
		}

		[ System.Diagnostics.Conditional( LOGGER_SYMBOL ) ]
		public static void PopUpText( Vector3 position, string message, Object context = null )
		{
			var entity = Pool_Debug_UI_Text.Instance.GetEntity();
			entity.Spawn( position, message );

			Debug.Log( message, context );
		}
#endregion
	}
}