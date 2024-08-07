package crc64e52ac986f94dea9c;


public class MediaPlayerService_MediaSessionCallback
	extends android.media.session.MediaSession.Callback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPause:()V:GetOnPauseHandler\n" +
			"n_onPlay:()V:GetOnPlayHandler\n" +
			"n_onSkipToNext:()V:GetOnSkipToNextHandler\n" +
			"n_onSkipToPrevious:()V:GetOnSkipToPreviousHandler\n" +
			"n_onStop:()V:GetOnStopHandler\n" +
			"";
		mono.android.Runtime.register ("MauiAudio.Platforms.Android.MediaPlayerService+MediaSessionCallback, MauiAudio", MediaPlayerService_MediaSessionCallback.class, __md_methods);
	}


	public MediaPlayerService_MediaSessionCallback ()
	{
		super ();
		if (getClass () == MediaPlayerService_MediaSessionCallback.class) {
			mono.android.TypeManager.Activate ("MauiAudio.Platforms.Android.MediaPlayerService+MediaSessionCallback, MauiAudio", "", this, new java.lang.Object[] {  });
		}
	}

	public MediaPlayerService_MediaSessionCallback (crc64e52ac986f94dea9c.MediaPlayerServiceBinder p0)
	{
		super ();
		if (getClass () == MediaPlayerService_MediaSessionCallback.class) {
			mono.android.TypeManager.Activate ("MauiAudio.Platforms.Android.MediaPlayerService+MediaSessionCallback, MauiAudio", "MauiAudio.Platforms.Android.MediaPlayerServiceBinder, MauiAudio", this, new java.lang.Object[] { p0 });
		}
	}


	public void onPause ()
	{
		n_onPause ();
	}

	private native void n_onPause ();


	public void onPlay ()
	{
		n_onPlay ();
	}

	private native void n_onPlay ();


	public void onSkipToNext ()
	{
		n_onSkipToNext ();
	}

	private native void n_onSkipToNext ();


	public void onSkipToPrevious ()
	{
		n_onSkipToPrevious ();
	}

	private native void n_onSkipToPrevious ();


	public void onStop ()
	{
		n_onStop ();
	}

	private native void n_onStop ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
