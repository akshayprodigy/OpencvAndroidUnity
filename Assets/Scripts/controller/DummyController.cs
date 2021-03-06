using UnityEngine;
using UnityEngine.UI;

using System.Runtime.InteropServices;
using System;
public class DummyController : MonoBehaviour
{
    // Start is called before the first frame update
    public float RotateSpeed = 0.5f;

    public RawImage InImage;
    public RawImage OutImage;

    WebCamTexture wct;
    Texture2D outTexture;

    void Awake()
    {
#if UNITY_EDITOR
        int width = 1280;
        int height = 720;
#else
        int width = 720;
        int height = 480;
#endif

        NativeLibAdapter.InitCV(width, height);

        outTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        wct = new WebCamTexture(width, height);
        wct.Play();

        Debug.LogWarning("Foo Value in C++ is " + NativeLibAdapter.FooTest());
    }

    void Update()
    {
        this.transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);

        if (wct.width > 100 && wct.height > 100)
        {
            Color32[] pixels = wct.GetPixels32();
            GCHandle pixelHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);

            IntPtr results = NativeLibAdapter.SubmitFrame(wct.width, wct.height, pixelHandle.AddrOfPinnedObject());
            int bufferSize = wct.width * wct.height * 4;
            byte[] rawData = new byte[bufferSize];

            if (results != IntPtr.Zero)
            {
                Marshal.Copy(results, rawData, 0, bufferSize);

                outTexture.LoadRawTextureData(rawData);
                outTexture.Apply();
            }

            InImage.texture = wct;
            OutImage.texture = outTexture;

            rawData = null;
            pixelHandle.Free();
        }
    }
}
