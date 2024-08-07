#if __ANDROID__
using Plugin.Maui.Audio;
#endif

namespace AppEmpresa.Suporte
{
    public class AudioColetor
    {
#if __ANDROID__
        private readonly IAudioManager _audioManager;
#endif

        public AudioColetor()
        {
#if __ANDROID__
            _audioManager = AudioManager.Current;
#endif
        }

        public async Task SomAcerto()
        {
#if __ANDROID__
            var audioStream = await FileSystem.OpenAppPackageFileAsync("acerto.mp3");
            var audioAcerto = _audioManager.CreatePlayer(audioStream);
            audioAcerto.Volume = 1;
            audioAcerto.Play();
#endif
        }

        public async Task SomErro()
        {
#if __ANDROID__
            var audioStream = await FileSystem.OpenAppPackageFileAsync("erro.mp3");
            var audioErro = _audioManager.CreatePlayer(audioStream);
            audioErro.Volume = 1;
            audioErro.Play();
#endif
        }
    }
}
