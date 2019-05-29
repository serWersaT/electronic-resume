using System;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
using System.Diagnostics;




namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public bool record = false;
        public bool starting = false;
        public bool start_dialog = false;
        public string[] spesok_speech = new string[100];
        public string status = "";
        public string vopros = "";

        public Form1()
        {
            InitializeComponent();
        }

        Spisok_programm spisok1 = new Spisok_programm();    //вызов конструктора класса Spisok_programm, для проверки наличия программ для подключения распознавания голоса
        Yandex_speechAPI golos1 = new Yandex_speechAPI();   //вызов конструктора класса Yandex_speechAPI, для отправки get запроса на сервер яндекса 
        enternet_ping ping1 = new enternet_ping(); //вызов конструктора класса enternet_ping, для проверки подключения к интернету
        LOG applog = new LOG(); //вызов конструктора класса LOG, для записи выполненных действий программы
        google_voice voice1 = new google_voice(); //вызов конструктора класса google_voice, для записи и распознавания голоса
        Dialog dialog1 = new Dialog();  //вызов конструктора класса Dialog, для отетов на задаваемые вопросы
        Mail mailing = new Mail();  //вызов конструктора класса Mail, для отправлки лог фоайла при закрытии программы


        public static int tick_form = 0;   //переменная используется для продолжения записи звука при нулевом уровне звука.
        int dead_tick = 0;  //переменная для вызова сборщика мусора по таймеру
        private void Form1_Load(object sender, EventArgs e)
        {
            applog.write("[Старт]");
            ping1.ping();   //выполняем проверку подключени к интернету
            label1.Text = ping1.ret();  //передаем статус подключения в label1
            if (label1.Text == "Статус подключения к сети интернет: Подключено")    //программа работает при наличии подключения к сети
            {
                string programms = spisok1.spisok();
                /*данные библиотеки скачены с сайта microsoft, необходимы для распознавания голоса*/
                if (!programms.Contains("Microsoft Speech Platform SDK") && !programms.Contains("Microsoft Server Speech Recognition Language") && !programms.Contains("Microsoft Server Speech Platform Runtime"))
                {
                    MessageBox.Show("Внимание! Голосовое управление программой не возможно. Скачайте следующие дополнения: Microsoft Speech Platform SDK (x86) v11.0, Microsoft Server Speech Recognition Language - TELE (ru-RU) и Microsoft Server Speech Platform Runtime (x86)");
                    spesok_speech[0] = "Голосовое управление программой не возможно. Но вы можете управлять программой с помощью командной строки или скачайте следующие дополнения: Microsoft Speech Platform SDK (x86) v11.0, Microsoft Server Speech Recognition Language - TELE (ru-RU) и Microsoft Server Speech Platform Runtime (x86)";
                    spesok_speech[1] = "Выполнить установку данных программ?";
                    vopros = "Выполнить установку данных программ?";

                    /*Data1.Texting_vopros = vopros;
                    vopros = "";*/
                }
                else
                {
                    i = 0;  //сбрасываем счетчик списка предложений на ноль
                    spesok_speech[0] = "Приветствую тебя, пользователь";
                    spesok_speech[1] = "Я электронное резюме. Как я могу к вам обращаться?";

                    vopros = "имя";
                    /*Data1.Texting_vopros = vopros;
                   vopros = "";*/
                }
            }
        }


        int i = 0;
        string newtext = "";
        private void timer1_Tick(object sender, EventArgs e)
        {
            ///////////////////////////условия записи голоса ///////////////////////////////////////////////
            bool recording = voice1.record;
            if (newtext != voice1.status)
            {
                label3.Text = voice1.status;
            }

            newtext = voice1.status;

            tick_form++;
            Data1.sValue = tick_form;
            if (tick_form == 35)
            {
                if (recording == false)
                {
                    try
                    {
                        voice1.RecordStart();
                    }
                    catch { }
                }
                tick_form = 0;

                bool timer = voice1.timer1_enabled;
                if (timer == true)
                {
                    voice1.time2();
                    start_dialog = false;
                }

            }


            ///////////////////////////условия воспроизведения голоса ///////////////////////////////////////////////
            starting = golos1.start();  //переменная запрещающаяя воспроизведение звука при уже запущенном воспроизведении
            if (spesok_speech[i] != null & starting == false)
            {
                golos1.PlayMp3FromUrl(spesok_speech[i]);
                spesok_speech[i] = null;
                i++;
            }

            /////////////////////////// вызов сборщика мусора ///////////////////////////////////////////////
            dead_tick++;
            if (dead_tick == 200)   //каждые 20 сек вызываем сборшик мусора
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                dead_tick = 0;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            label3.Text = voice1.status;
        }

        private void label3_TextChanged(object sender, EventArgs e)
        {
            if (start_dialog == false)  //для избежания повторного запуска экземпляра класса Dialog
            {
                start_dialog = true;

                if (label3.Text.Contains("final") & label3.Text.Contains("transcript") & label3.Text.Contains("confidence") & label1.Text.Contains("Подключено"))
                {
                    Data1.Texting_vopros = vopros;
                    vopros = "";

                    label3.Text = label3.Text.ToLower();
                    Data1.Texting_label3 = label3.Text;    //передаем значение label3 в класс Dialog
                    dialog1.dialoging();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                label3.Text = textBox1.Text;
            }
        }


        public void SetLabelText(string text)
        {
            label3.Text = text;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (label1.Text == "Статус подключения к сети интернет: Подключено")    //программа работает при наличии подключения к сети
            {
                mailing.SendMail();
            }
            else
            {
                applog.write("Закрыли программу без подключения к интернету");   
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & textBox1.Text != "")        //срабатывание по нажитию кнопки enter
            {
                if (textBox1.Text != "")
                {
                    label3.Text = textBox1.Text;
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }


    public static class Data1   //статический класс для передачи данных в другие классы 
    {
        public static int sValue    //пеедаем данные типа int
        {
            get { return svalue; }
            set { svalue = value; if (SomeEvent != null) SomeEvent(null, EventArgs.Empty); }
        }
        static int svalue;
        public static string Texting_label3    //передаем данные типа string
        {
            get { return texting; }
            set { texting = value; if (SomeEvent != null) SomeEvent(null, EventArgs.Empty); }
        }
        static string texting;

        public static string Texting_vopros    //передаем данные типа string
        {
            get { return texting1; }
            set { texting1 = value; if (SomeEvent != null) SomeEvent(null, EventArgs.Empty); }
        }
        static string texting1;

        public static event EventHandler SomeEvent;
    }
}
