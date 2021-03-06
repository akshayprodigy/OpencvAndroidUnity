using System.Runtime.InteropServices;
using System;
using UnityEngine;

public class NativeLibAdapter 
{
#if !UNITY_EDITOR
	[DllImport("native-lib")]
	private static extern int InitCV_Internal(int width, int height);

	[DllImport("native-lib")]
	private static extern IntPtr SubmitFrame_Internal(int width, int height, IntPtr bufferAddr);

	[DllImport("native-lib")]
	private static extern int FooTestFunction_Internal();
#endif

	public static int InitCV(int width, int height)
	{
#if !UNITY_EDITOR
		int result = InitCV_Internal(width, height);
#else
		int result = -1;
#endif
		Debug.LogWarning("[NativeLibAdapter] InitCV " + (result == 0 ? "No Errors" : "Error Code : " + result));


		return result;
	}

	public static IntPtr SubmitFrame(int width, int height, IntPtr bufferAddr)
	{
#if !UNITY_EDITOR
		IntPtr ret = SubmitFrame_Internal(width, height, bufferAddr);
#else
		IntPtr ret = IntPtr.Zero;
#endif
		return ret;
	}

	public static int FooTest()
	{
#if !UNITY_EDITOR
		return FooTestFunction_Internal();
#else
		return -1;
#endif
	}
}
