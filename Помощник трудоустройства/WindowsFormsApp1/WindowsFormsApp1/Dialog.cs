using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace WindowsFormsApp1
{
    class Dialog
    {
        Yandex_speechAPI yandex = new Yandex_speechAPI();
        LOG lg_new = new LOG();
        //Form1 form;


        string label3_text;
        string vopros;
        string name = "";
        string otvet1 = ""; 
        bool names = false; //флаг запрещает повторный вызов операторов if с переменнй vopros
        public void dialoging()
        {
            label3_text = Data1.Texting_label3;
            vopros = Data1.Texting_vopros;
            bool nnn = false;

            ProcessStartInfo prc1 = new ProcessStartInfo();
            prc1.FileName = @"MicrosoftSpeechPlatformSDK.msi";

            ProcessStartInfo prc2 = new ProcessStartInfo();
            prc2.FileName = @"MSSpeech_SR_ru-RU_TELE.msi";

            ProcessStartInfo prc3 = new ProcessStartInfo();
            prc3.FileName = @"SpeechPlatformRuntime.msi";

            ProcessStartInfo prc4 = new ProcessStartInfo();
            prc4.FileName = @"rezume.docx";


            if ((!label3_text.Contains("выход") | !label3_text.Contains("закрыть") | !label3_text.Contains("закрой")) & names == false & vopros.Contains("Выполнить установку данных программ") & (label3_text.Contains("\bда\b") | label3_text.Contains("выполни") | label3_text.Contains("установи") | label3_text.Contains("устанавливай") | label3_text.Contains("действуй")))
            {                
                vopros = "";
                names = true;
                try
                {
                    if (nnn == false)
                    {
                        nnn = true;
                        Process.Start(prc1);
                        lg_new.write("\r\n" + "Выполнена установка MicrosoftSpeechPlatformSDK.msi" + "\r\n");
                    }
                }
                catch { }

                nnn = false;
                try
                {
                    if (nnn == false)
                    {
                        nnn = true;
                        Process.Start(prc2);
                        lg_new.write("\r\n" + "ВMSSpeech_SR_ru-RU_TELE.msi" + "\r\n");
                    }
                }
                catch { }

                nnn = false;
                try
                {
                    if (nnn == false)
                    {
                        nnn = false;
                        Process.Start(prc3);
                        lg_new.write("\r\n" + "SpeechPlatformRuntime.msi" + "\r\n");
                    }
                }
                catch { }

            }


            if ((!label3_text.Contains("выход") | !label3_text.Contains("закрыть") | !label3_text.Contains("закрой")) & vopros.Contains("имя") & label3_text != "" & names == false)
            {
                vopros = "";
                names = true;
                name = label3_text;
                name = name.Substring(0, name.IndexOf(','));    //удаляем все после первого символа ","
                name = Regex.Replace(name, @"[^а-яА-Я!@#]+", " ");
                name = Regex.Replace(name, "Текст", " ", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, "Гайка", " ", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"зовут", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"зови", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"привет", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"здравствуй", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"назывй", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"называют", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"не ошибешься", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"пусть", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"будет", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"будешь", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"тебя", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"\bимя\b", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"\bмое\b", "", RegexOptions.IgnoreCase);
                name = Regex.Replace(name, @"\bменя\b", "", RegexOptions.IgnoreCase);

                lg_new.write("\r\n" + "Имя: "+ name + "\r\n");
                yandex.PlayMp3FromUrl("Приятно познакомиться " + name + "Теперь я готова ответить на все ваши вопросы");
            }



            if ((label3_text.Contains("какой") | label3_text.Contains("какие")| label3_text.Contains("твои") | label3_text.Contains("свои") | label3_text.Contains("свой")) & (label3_text.Contains("функци") | label3_text.Contains("возможности") | label3_text.Contains("задачи")))
            {
                yandex.PlayMp3FromUrl("Моя основная задача помочь трудоустроиться автору данной программы");
            }

            if ((label3_text.Contains("какой") | label3_text.Contains("сколько")) & (label3_text.Contains("лет") | label3_text.Contains("возраст")))
            {
                lg_new.write("\r\n" + "Возраст" + "\r\n");
                yandex.PlayMp3FromUrl("28 лет");
            }

            if ((label3_text.Contains("какой") | label3_text.Contains("есть")) & (label3_text.Contains("опыт") | label3_text.Contains("стаж")) & (label3_text.Contains("работы") | label3_text.Contains("рабочий")))
            {
                lg_new.write("\r\n" + "Опыт работы" + "\r\n");
                yandex.PlayMp3FromUrl("К сожалению опыта работы в программировании нету, но он в свободное время занимается практикой, ставит себе различные задачи и решает их. Я тому пример.");
            }

            if ((label3_text.Contains("какой") | label3_text.Contains("каком") | label3_text.Contains("чем")) & (label3_text.Contains("язык") | label3_text.Contains("сред") | label3_text.Contains("программ")) )
            {
                lg_new.write("\r\n" + "что изучал" + "\r\n");
                yandex.PlayMp3FromUrl("Изучал Си шарп, Делфи");
            }

            if ((label3_text.Contains("работает") | label3_text.Contains("трудится")) & (label3_text.Contains("где")) )
            {
                lg_new.write("\r\n" + "Работа на данный момент" + "\r\n");
                yandex.PlayMp3FromUrl("На данный момент работает инженером на Калужском Турбинном Заводе");
            }

            if ((label3_text.Contains("какие") | label3_text.Contains("чем") | label3_text.Contains("какое")) & (label3_text.Contains("увлекается") | label3_text.Contains("увлечения") | label3_text.Contains("хобби")))
            {
                lg_new.write("\r\n" + "Увлечения" + "\r\n");
                yandex.PlayMp3FromUrl("В свободное время занимается программированием. Так же увлекается каратэ");
            }

            if ((label3_text.Contains("каких") | label3_text.Contains("какие") | label3_text.Contains("какой") | label3_text.Contains("чего")) & (label3_text.Contains("результат") | label3_text.Contains("успех") | label3_text.Contains("достиг")) & (label3_text.Contains("карат") | label3_text.Contains("спорт")))
            {
                lg_new.write("\r\n" + "Пояс по каратэ" + "\r\n");
                yandex.PlayMp3FromUrl("Обладает оранжевым поясом");
            }

            if ((label3_text.Contains("каких") | label3_text.Contains("какие") | label3_text.Contains("какой") | label3_text.Contains("чего")) & (label3_text.Contains("результат") | label3_text.Contains("успех") | label3_text.Contains("достиг")) & (label3_text.Contains("в изучении") | label3_text.Contains("программировании")))
            {
                lg_new.write("\r\n" + "Разработанные проекты" + "\r\n");
                yandex.PlayMp3FromUrl("Последний проект - голосовой асистент, аналогичный данной программе, но с большим функциналом.");
            }

            if ((label3_text.Contains("открой") | label3_text.Contains("открыть") | label3_text.Contains("покажи") | label3_text.Contains("показать")) & (label3_text.Contains("резюме")))
            {
                nnn = false;
                try
                {
                    if (nnn == false)
                    {
                        nnn = true;
                        lg_new.write("\r\n" + "Запуск стандартного резюме" + "\r\n");
                        Process.Start(prc4);
                    }
                }
                catch { }
            }

            if ((label3_text.Contains("почему") | label3_text.Contains("зачем")) & (label3_text.Contains("резюме") | label3_text.Contains("програм")) & (label3_text.Contains("прислал") | label3_text.Contains("отправил")))
            {
                lg_new.write("\r\n" + "показать навыки" + "\r\n");
                yandex.PlayMp3FromUrl("для того чтобы сразу показать свои навыки, так как кадровики увидев отсутствие опыта прекращают дальнейшее общение.");
            }

            if (label3_text.Contains("контакт") | label3_text.Contains("телефон") | label3_text.Contains("связаться")) 
            {
                lg_new.write("\r\n" + "контакт" + "\r\n");
                yandex.PlayMp3FromUrl("8-953-323-29-28");
                MessageBox.Show("8-953-323-29-28");
            }

            if (label3_text.Contains("выход") | label3_text.Contains("закрыть") | label3_text.Contains("закрой"))
            {
                Application.Exit();
            }

            if ((label3_text.Contains("как") | label3_text.Contains("какое") | label3_text.Contains("назови")) & (label3_text.Contains("имя") | label3_text.Contains("зовут")))
            {
                lg_new.write("\r\n" + "мое имя" + "\r\n");
                yandex.PlayMp3FromUrl("Горбачев Олег Олегович");
            }
        }


        public string otvet()
        {
            return otvet1;
        }
    }
}
