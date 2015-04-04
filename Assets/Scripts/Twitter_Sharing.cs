using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

public class Twitter_Sharing : MonoBehaviour
{
	public Texture2D MyImage;

	public void OnEnable ()
	{
		ScreenshotHandler.ScreenshotFinishedSaving += ScreenshotSaved;
		
	}
	
	void OnDisable()
	{
		ScreenshotHandler.ScreenshotFinishedSaving -= ScreenshotSaved;
		
	}


	public void OnTwitterTextSharingClick()
	{
#if  UNITY_ANDROID
		AndroidJavaObject fbTwitterSharing = null;
		AndroidJavaObject activityContext = null;

		if (fbTwitterSharing == null)
		{
			using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			}

			using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.tag.fb_twitter.Fb_Twitter"))
			{
				if (pluginClass != null)
				{
					fbTwitterSharing = pluginClass.CallStatic<AndroidJavaObject>("instance");
					fbTwitterSharing.Call("setContext", activityContext);

					//Text Sharing
					fbTwitterSharing.Call("ShareTextOnTwitter", "Tweet Something");

					//Image Sharing with text
					fbTwitterSharing.Call("PostImageOnTwitter", ScreenshotHandler.savedImagePath, "Image Sharing");
				}
			}
		}
#endif


	}
	public void OniOSTwitterMediaSharing()
	{
		Debug.Log("Media Share");
		byte[] bytes = MyImage.EncodeToPNG();
		string path = Application.persistentDataPath + "/MyImage.png";
		File.WriteAllBytes(path, bytes);
		
		string path_ =  "MyImage.png";
		StartCoroutine(ScreenshotHandler.Save(path_, "Media Share", true));
	}
	
	void ScreenshotSaved ()
	{
		
		#if UNITY_ANDROID
				ShareImageInAndroid();
		#endif
		
	}
	void ShareImageInAndroid()
	{
		#if UNITY_ANDROID
		AndroidJavaObject fbTwitterSharing = null;
		AndroidJavaObject activityContext = null;

		if (fbTwitterSharing == null)
		{
			using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			}

			using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.tag.fb_twitter.Fb_Twitter"))
			{
				if (pluginClass != null)
				{
					fbTwitterSharing = pluginClass.CallStatic<AndroidJavaObject>("instance");
					fbTwitterSharing.Call("setContext", activityContext);
				
					fbTwitterSharing.Call("PostImageOnTwitter", ScreenshotHandler.savedImagePath, "Image Sharing");
					
				}
			}
		}
		#endif
	}
}
