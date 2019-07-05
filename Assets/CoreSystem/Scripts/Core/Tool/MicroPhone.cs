using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MicroPhone : Singleton<MicroPhone>
{
    public int m_Frequency = 44100;
    public int m_Lenght = 1;
    public bool m_IsLoop = true;

    protected AudioSource m_AudioSource;
    protected string m_DeviceName;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    new protected void OnDestroy()
    {
        base.OnDestroy();

        if (m_AudioSource != null && m_AudioSource.clip != null)
        {
            m_AudioSource.clip.UnloadAudioData();
        }

        Microphone.End(m_DeviceName);
    }

    // Use this for initialization
    void Start ()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.Log("No Microphone");
        }

        foreach (string _DeviceName in Microphone.devices)
        {
            if (_DeviceName.Contains("HTC"))
            {
                m_DeviceName = _DeviceName;
                break;
            }
        }
    }

    public void StartCaptureVoice()
    {
        if (Microphone.devices == null || Microphone.devices.Length == 0)
            return;

        if (m_AudioSource.clip != null)
            m_AudioSource.clip.UnloadAudioData();

        m_AudioSource.clip = Microphone.Start(m_DeviceName, m_IsLoop, m_Lenght, m_Frequency);
        m_AudioSource.loop = m_IsLoop;
        while (Microphone.GetPosition(m_DeviceName) < 0)
        {

        }
        m_AudioSource.Play();
        m_AudioSource.mute = true;
    }

    public void StopCaptureVoice(string iFileName)
    {
        if (Microphone.IsRecording(m_DeviceName) == false)
            return;

        int _Pos = Microphone.GetPosition(m_DeviceName);
        //計算目前錄音取樣的陣列大小
        float[] _Samples = new float[_Pos * m_AudioSource.clip.channels];

        m_AudioSource.clip.GetData(_Samples, 0);
        AudioClip _AC = AudioClip.Create("Temp", _Samples.Length, m_AudioSource.clip.channels, m_Frequency, false);
        _AC.SetData(_Samples, 0);

        //SavWav.Save(iFileName, _AC);
        AudioClipToFile.SaveToWave(System.IO.Directory.GetCurrentDirectory() + "\\Captures", iFileName + "2", _AC, 0);

        Microphone.End(m_DeviceName);
        m_AudioSource.Stop();

        _AC.UnloadAudioData();
        Destroy(_AC);
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (Microphone.IsRecording(m_DeviceName) == false)
        {
            if (GUILayout.Button("Start MC Record"))
            {
                StartCaptureVoice();
            }
        }
        else
        {
            if (GUILayout.Button("Stop MC Record"))
            {
                StopCaptureVoice("VoiceRecordTest");
            }
        }
    }
#endif

}
