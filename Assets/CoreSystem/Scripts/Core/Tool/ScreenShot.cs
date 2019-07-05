using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShot
{
    static public void CaptureScreen(string iFolderName, string iFileName, int iSuperSize = 1)
    {
        string _Folder = Application.dataPath + "/" + iFolderName;

        if (!Directory.Exists(_Folder))
            Directory.CreateDirectory(_Folder);

        string _FilePath = _Folder + "/" + iFileName + ".png";
        ScreenCapture.CaptureScreenshot(_FilePath, iSuperSize);
    }

    static public Texture CaptureScreenToTexture()
    {
        Texture2D _Savetexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _Savetexture.name = "ScreenShot";
        _Savetexture.ReadPixels(new Rect(0.0f, 0.0f, Screen.width, Screen.height), 0, 0);
        _Savetexture.Apply();

        return _Savetexture;
    }

    static public void SaveTextureToFolder(Texture2D iTexture, string iFolderName)
    {
        if (iTexture == null)
            return;

        string _Folder = Application.dataPath + "/" + iFolderName;

        if (!Directory.Exists(_Folder))
            Directory.CreateDirectory(_Folder);

        byte[] _Bytes = iTexture.EncodeToPNG();

        FileStream _FS = new FileStream(_Folder + "/" + iTexture.name + ".png", FileMode.Create);
        BinaryWriter _BW = new BinaryWriter(_FS);
        _BW.Write(_Bytes);
        _BW.Close();
        _FS.Close();
    }

    static public Texture CaptureScreenFromRenderTexture(RenderTexture iRT, Texture2D iTargetTexture)
    {
        if (iRT == null)
            return null;

        Texture2D _Savetexture = iTargetTexture;
        if (_Savetexture == null)
            _Savetexture = new Texture2D(iRT.width, iRT.height, TextureFormat.RGB24, false, true);

        _Savetexture.name = "ScreenShot";
        _Savetexture.ReadPixels(new Rect(0.0f, 0.0f, iRT.width, iRT.height), 0, 0);
        _Savetexture.Apply();

        return _Savetexture;
    }
}
