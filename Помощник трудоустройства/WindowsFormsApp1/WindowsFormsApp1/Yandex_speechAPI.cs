using System;
using System.Threading;
using System.IO;
using System.Net;
using NAudio.Wave;


namespace WindowsFormsApp1
{
    class Yandex_speechAPI
    {
        WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback());
        private const string API_URL = "https://tts.voicetech.yandex.net/generate?";    //адрес отправки запроса на перевод текста в речь
        private const string API_KEY = "236319ab-f446-4705-93bd-a023ac620be4";      //ключ пользователя яндекса (скача с инета, с какого-то рпоекта)  
        bool music_flag = false;    //переменная для избежания запуска нескольких экземпляров одного потока
        public void PlayMp3FromUrl(string text)      //функция воспроизведения аудио-файла
        {
            Thread thread1 = new Thread(golos1);

            void golos1()
            {
                string url = API_URL + "text=" + text + "&format=mp3&lang=ru-RU&speaker=oksana&emotion=good&speed=1.0&key=" + API_KEY;
                music_flag = true;
                using (Stream ms = new MemoryStream())
                {
                    using (Stream stream = WebRequest.Create(url).GetResponse().GetResponseStream())
                    {
                        byte[] buffer = new byte[32768];
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                    }
                    ms.Position = 0;
                    using (WaveStream blockAlignedStream =
                        new BlockAlignReductionStream(
                            WaveFormatConversionStream.CreatePcmStream(
                                new Mp3FileReader(ms))))
                    {

                        waveOut.Init(blockAlignedStream);
                        waveOut.Play();

                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            Thread.Sleep(100);
                        }

                    }
                }
                music_flag = false;

            }

            if (music_flag == false)
            {
                thread1.Start();
            }

        }   //Воспроизведение заранее сгенерированного звукового файла

        public bool start()
        {
            return music_flag;
        }
    }
}
