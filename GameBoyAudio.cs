using System;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameBoy
{
    public class GameBoyAudio
    {
        private const int bytesPerSample = 2;
        private const int samplesPerBuffer = short.MaxValue / 4;
        private readonly int sampleRate;    //Hz, samples per second
        private readonly int channels;      //1 = mono, 2 = stereo
        private readonly DynamicSoundEffectInstance sound;
        private readonly byte[] sampleBuffer;

        public GameBoyAudio(int sampleRate = 44100, AudioChannels channels = AudioChannels.Mono)
        {
            this.sampleRate = sampleRate;
            this.channels = channels == AudioChannels.Stereo ? 2 : 1;
            sound = new DynamicSoundEffectInstance(sampleRate, channels);
            sampleBuffer = new byte[samplesPerBuffer * bytesPerSample * this.channels];
        }

        public void Play() => sound.Play();
        public float Volume { get => sound.Volume; set => sound.Volume = value; }
        private void SubmitSampleBuffer() => sound.SubmitBuffer(sampleBuffer);

        /// <summary>
        /// Poll for updates since the BufferNeeded event doesn't seem to fire until
        /// the buffer is empty, causing choppy sound. :(
        /// </summary>
        /// <remarks>
        /// Even though DynamicSoundEffectInstance's source seems to have a built-in
        /// threshold like this...?
        /// <see cref="https://github.com/MonoGame/MonoGame/blob/72919dc8185e70734507b58bd9a8e03bfe9a0d96/MonoGame.Framework/Audio/DynamicSoundEffectInstance.cs"/>
        /// </remarks>
        public void Update()
        {
            while(sound.PendingBufferCount < 3)
            {
                Console.WriteLine("BufferNeeded");
                FillSampleBuffer();
                SubmitSampleBuffer();
            }
        }

        private void FillSampleBuffer()
        {
            for (int i = 0; i < sampleBuffer.Length; i += 2)
            {
                short sample = (short)((i * 100) % short.MaxValue);
                sampleBuffer[i] = (byte)(sample & 0xFF);
                sampleBuffer[i + 1] = (byte)(sample >> 8);
            }
        }
    }
}
