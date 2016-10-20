using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class AndroidDialog : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		this.UpdateAsObservable ().Where (_ => Application.platform == RuntimePlatform.Android && Input.GetKey (KeyCode.Escape))
			.Subscribe (_ => DispExitDialog());
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void DispExitDialog() {
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject> ("currentActivity");

		activity.Call ("runOnUiThread", new AndroidJavaRunnable (() => {
			//Create a AlertDialog
			AndroidJavaObject alertDialogBuilder = new AndroidJavaObject ("android.app.AlertDialog$Builder", activity);
			alertDialogBuilder.Call<AndroidJavaObject> ("setMessage", "message");
			alertDialogBuilder.Call<AndroidJavaObject> ("setCancelable", true);
			alertDialogBuilder.Call<AndroidJavaObject> ("setPositiveButton", "OK", new PositiveButtonListner (this));
			alertDialogBuilder.Call<AndroidJavaObject> ("setNegativeButton", "Cancel", new NegativeButtonListner(this));
			AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject> ("create");
			dialog.Call ("show");
		}));
	}

}

class PositiveButtonListner :AndroidJavaProxy
{
	public PositiveButtonListner (Object d) : base ("android.content.DialogInterface$OnClickListener")
	{
		//It is called when you create a listener
	}

	public void onClick (AndroidJavaObject obj, int value)
	{
		//It is called when you push a button
		Application.Quit ();
	}
}

class NegativeButtonListner :AndroidJavaProxy
{
	public NegativeButtonListner (Object d) : base ("android.content.DialogInterface$OnClickListener")
	{
		//It is called when you create a listener
	}

	public void onClick (AndroidJavaObject obj, int value)
	{
		//It is called when you push a button
	}
}




