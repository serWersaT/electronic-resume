using System;
using System.IO;
using System.Net;
using NAudio.Wave;
using Microsoft.Speech.Recognition;

namespace WindowsFormsApp1
{
    class google_voice
    {
        LOG applog = new LOG();
        public bool record = false;   
        bool ON = false;
        public bool timer1_enabled = false; //переменная передается на form1 для управления элементом таймер
        public string status = "";


        WaveIn waveIn;
        WaveFileWriter writer;
        string outputFilename = "demo.wav";
        enternet_ping ping1 = new enternet_ping();

       
        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern
            Boolean PlaySound(string lpszName, int hModule, int dwFlags);

        public WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();

        void waveIn_DataAvailable(object sender, WaveInEventArgs e1)
        {
            writer.WriteData(e1.Buffer, 0, e1.BytesRecorded);
        }
        void waveIn_RecordingStopped(object sender, EventArgs e)        //остановка записи звука для отправления на сервер гугл
        {
            try
            {
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
            }
            catch (Exception err)
            {
                applog.write("ошибка: " + err);
            }
        }

        void sre_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e1)    //Процедура начала записи звукового файла для распознавания речи
        {
            record = true;
            if (e1.AudioLevel > 1.1 & ON == false)
            {
                try
                {
                    waveIn = new WaveIn();
                    waveIn.DeviceNumber = 0;
                    waveIn.DataAvailable += waveIn_DataAvailable;
                    waveIn.RecordingStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(waveIn_RecordingStopped);
                    waveIn.WaveFormat = new WaveFormat(16000, 1);

                    writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat); 

                    status = "Идет запись...";
                    try
                    {
                        waveIn.StartRecording();
                    }
                    catch (Exception err)
                    {
                        applog.write("ошибка: " + err);
                    }
                    ON = true;
                }
                catch
                {
                    waveIn.StopRecording();
                    ON = false;
                    record = false;
                }

            }
            int tick_time = Data1.sValue;

            if (e1.AudioLevel < 0.1 & ON == true & waveIn != null & tick_time>=34)
            {
                record = false;
                waveIn.StopRecording();
                status = "Стоп запись";
                ON = false;
                timer1_enabled = true;  
            }
            record = false;
        }

        public void RecordStart()   
        {
            record = false;
            ping1.ping();   //проверяем наличие подключения к интернету
            string enternet = ping1.ret();

            if (enternet == "Статус подключения к сети интернет: Подключено")
            {
                record = true;
                try
                {
                    System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-ru");
                    SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);
                    sre.SetInputToDefaultAudioDevice();

                    //sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);        //распозавание голосовых команд.  Будет использоваться при отсутствии интернета
                    sre.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(sre_AudioLevelUpdated);       //проверка громкости входного звука

                    Choices numbers = new Choices();    //Инициализирует новый экземпляр класса Choices, содержащий пустой набор вариантов.
                    numbers.Add(new string[] { "один", "два", "три", "четыре", "пять" });   //список команд для оффлайн работы

                    GrammarBuilder gb = new GrammarBuilder();
                    gb.Culture = ci;
                    gb.Append(numbers);

                    Grammar g = new Grammar(gb);
                    sre.LoadGrammar(g);

                    sre.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch (Exception err)
                {
                    applog.write("ошибка: " + err);
                }
                record = false;
            }
        }
        public void time2() //отправка POST запроса для перевода звука в текст
        {
            try
            {
                record = true;
                WebRequest request = WebRequest.Create("https://www.google.com/speech-api/v2/recognize?output=json&lang=ru-RU&key=AIzaSyBOti4mM-6x9WDnZIjIeyEU21OpBXqWBgw");
                request.Method = "POST";
                byte[] byteArray = File.ReadAllBytes(outputFilename);
                request.ContentType = "audio/l16; rate=16000"; //"16000";
                request.ContentLength = byteArray.Length;
                request.GetRequestStream().Write(byteArray, 0, byteArray.Length);
                // Получить ответ.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Открытие потока, используя StreamReader для легкого доступа.
                StreamReader reader = new StreamReader(response.GetResponseStream());
                //Читает содержание.
                status ="Текст: "+ reader.ReadToEnd();
                // Очистите потоки.
                reader.Close();
                response.Close();

                record = false;
            }
            catch
            {
                record = false;
            }
            timer1_enabled = false;
        }
    }
}
