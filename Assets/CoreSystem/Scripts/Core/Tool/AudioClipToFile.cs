using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class AudioClipToFile
{
    const int WAVE_HEADER_SIZE = 44;
    const System.UInt16 BITS_16 = 16;

    public static void SaveToWave(string iFilePath, string iFileName, AudioClip iClip, int iLength)
    {
        if (iFileName.EndsWith(".wav", System.StringComparison.OrdinalIgnoreCase) == false)
        {
            iFileName += ".wav";
        }

        //取得路徑並建立資料夾
        string _FilePath = Path.Combine(iFilePath, iFileName);
        Directory.CreateDirectory(Path.GetDirectoryName(_FilePath));

        using (FileStream _FS = CreateFileStream(_FilePath))
        {
            WriteWaveHeader(_FS, iClip);
            AudioClipWriteToWaveFile(_FS, iClip);
            _FS.Close();
        }
    }

    public static FileStream CreateFileStream(string iFilePath)
    {
        FileStream _FileStream = new FileStream(iFilePath, FileMode.Create);
        return _FileStream;
    }

    public static void WriteWaveHeader(FileStream iFileStream, AudioClip iClip)
    {
        if (iFileStream == null || iClip == null)
            return;

        //Wave Hader File
        //位元組   | 區塊名稱                   | 區塊大小 | 端序         | 內容
        //0 -- 3   | ChunkID        區塊編號    | 4bytes   | BigEndian    | "RIFF"
        //4 -- 7   | ChunkSize      總區塊大小  | 4bytes   | LittleEndian | N + 36
        //8 -- 11  | Format         檔案格式    | 4bytes   | BigEndian    | "WAVE"
        //12 -- 15 | Subchunk1ID    子區塊1標籤 | 4bytes   | BigEndian    | "fmt "
        //16 -- 19 | Subchunk1Size  子區塊1大小 | 4bytes   | LittleEndian | 16
        //20 -- 21 | AudioFormat    音訊格式    | 2bytes   | LittleEndian | 1(PCM)
        //22 -- 23 | NumChannels    聲道數量    | 2bytes   | LittleEndian | 1(單聲道) 2(立體聲)
        //24 -- 27 | SampleRate     取樣頻率    | 4bytes   | LittleEndian | 取樣點/秒(Hz)
        //28 -- 31 | ByteRate       位元組率    | 4bytes   | LittleEndian | 取樣頻率 * 聲道數量 * 位元深度 / 8  (SampleRate * NumChannels * BitsPerSample / 8)
        //32 -- 33 | BlockAlign     區塊對齊    | 2bytes   | LittleEndian | NumChannels * BitsPerSample / 8
        //34 -- 35 | BitsPerSample  位元深度    | 2bytes   | LittleEndian | 取樣位元深度, 8 bits = 8, 16 bits = 16
        //36 -- 39 | Subchunk2ID    子區塊2標籤 | 4bytes   | BigEndian    | "data"
        //40 -- 43 | Subchunk2Size  子區塊2大小 | 4bytes   | LittleEndian | N = 取樣數 * 聲道數量 * 位元深度 / 8  (NumSamples * NumChannels * BitsPerSample / 8)
        //44 --    | Data           音訊資料    | N        | LittleEndian | 音訊資料

        int _N = iClip.samples * iClip.channels * BITS_16 / 8;

        byte[] _ChunkID = System.Text.Encoding.UTF8.GetBytes("RIFF");
        iFileStream.Write(_ChunkID, 0, 4);

        byte[] _ChunkSize = System.BitConverter.GetBytes(_N + 36);
        iFileStream.Write(_ChunkSize, 0, 4);

        byte[] _Format = System.Text.Encoding.UTF8.GetBytes("WAVE");
        iFileStream.Write(_Format, 0, 4);

        byte[] _Subchunk1ID = System.Text.Encoding.UTF8.GetBytes("fmt ");
        iFileStream.Write(_Subchunk1ID, 0, 4);

        byte[] _Subchunk1Size = System.BitConverter.GetBytes(16);
        iFileStream.Write(_Subchunk1Size, 0, 4);

        byte[] _AudioFormat = System.BitConverter.GetBytes((System.UInt16)1);
        iFileStream.Write(_AudioFormat, 0, 2);

        byte[] _NumChannels = System.BitConverter.GetBytes((System.UInt16)iClip.channels);
        iFileStream.Write(_NumChannels, 0, 2);

        byte[] _SampleRate = System.BitConverter.GetBytes(iClip.frequency);
        iFileStream.Write(_SampleRate, 0, 4);

        byte[] _ByteRate = System.BitConverter.GetBytes(iClip.frequency * iClip.channels * BITS_16 / 8);
        iFileStream.Write(_ByteRate, 0, 4);

        byte[] _BlockAlign = System.BitConverter.GetBytes((System.UInt16)iClip.channels * BITS_16 / 8);
        iFileStream.Write(_BlockAlign, 0, 2);

        byte[] _BitsPerSample = System.BitConverter.GetBytes(BITS_16);
        iFileStream.Write(_BitsPerSample, 0, 2);

        byte[] _Subchunk2ID = System.Text.Encoding.UTF8.GetBytes("data");
        iFileStream.Write(_Subchunk2ID, 0, 4);

        byte[] _Subchunk2Size = System.BitConverter.GetBytes(_N);
        iFileStream.Write(_Subchunk2Size, 0, 4);
    }

    public static void AudioClipWriteToWaveFile(FileStream iFileStream, AudioClip iClip)
    {
        if (iFileStream == null || iClip == null)
            return;

        float[] _Samples = new float[iClip.samples * iClip.channels];
        iClip.GetData(_Samples, 0);
        byte[] _ByteArray = new byte[iClip.samples * iClip.channels * BITS_16 / 8];

        for (int i = 0; i < iClip.samples; ++i)
        {
            byte[] _TempByte = System.BitConverter.GetBytes((System.Int16)(_Samples[i] * 32767));
            _TempByte.CopyTo(_ByteArray, i * 2);
        }

        iFileStream.Write(_ByteArray, 0, _ByteArray.Length);
    }
}
