package crc64e52ac986f94dea9c;


public class MediaPlayerServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MauiAudio.Platforms.Android.MediaPlayerServiceBinder, MauiAudio", MediaPlayerServiceBinder.class, __md_methods);
	}


	public MediaPlayerServiceBinder ()
	{
		super ();
		if (getClass () == MediaPlayerServiceBinder.class) {
			mono.android.TypeManager.Activate ("MauiAudio.Platforms.Android.MediaPlayerServiceBinder, MauiAudio", "", this, new java.lang.Object[] {  });
		}
	}


	public MediaPlayerServiceBinder (java.lang.String p0)
	{
		super (p0);
		if (getClass () == MediaPlayerServiceBinder.class) {
			mono.android.TypeManager.Activate ("MauiAudio.Platforms.Android.MediaPlayerServiceBinder, MauiAudio", "System.String, System.Private.CoreLib", this, new java.lang.Object[] { p0 });
		}
	}

	public MediaPlayerServiceBinder (crc64e52ac986f94dea9c.MediaPlayerService p0)
	{
		super ();
		if (getClass () == MediaPlayerServiceBinder.class) {
			mono.android.TypeManager.Activate ("MauiAudio.Platforms.Android.MediaPlayerServiceBinder, MauiAudio", "MauiAudio.Platforms.Android.MediaPlayerService, MauiAudio", this, new java.lang.Object[] { p0 });
		}
	}

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
