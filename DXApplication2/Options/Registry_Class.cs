using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveDXF.OptionsFold
{
    class Registry_Class
    {
        public void AddNewKey(string NodeName, string KeyName, string KeyValue)
        {
            // В этот раз открываем специально для записи.
            RegistryKey myKey = Registry.LocalMachine;

            // Второй аргумент (true) говорит о том, что будет совершаться запись.
            RegistryKey newKey = myKey.OpenSubKey($@"Software\Microsoft\{NodeName}", true);
            if (newKey == null) newKey = myKey.CreateSubKey($@"Software\Microsoft\{NodeName}");
            try
            {
                newKey.GetValue(KeyName, KeyValue);
            }
            catch (Exception e){}
            finally
            {
                // Закрыть ключ нужно обязательно.
                myKey.Close();
            }
        }
        public string GetKeyValue(string NodeName, string KeyName)
        {
            // Совершаем навигацию по реестру и открываем ключ для чтения,
            // можно сразу указать весь путь, а не совершать навигацию поэтапно.
            RegistryKey myKey = Registry.LocalMachine;
            RegistryKey wKey = myKey.OpenSubKey($@"Software\Microsoft\{NodeName}");
            if (wKey == null) return null;
            // Читаем данные и конвертируем в нужный формат.
            string Str = wKey.GetValue(KeyName) as string;

            wKey.Close();
            return Str;
        }
        public string GetKeyValue(string NodeName, string KeyName, string DefaultKeyValue)
        {
            // Совершаем навигацию по реестру и открываем ключ для чтения,
            // можно сразу указать весь путь, а не совершать навигацию поэтапно.
            RegistryKey myKey = Registry.LocalMachine;
            RegistryKey wKey = myKey.OpenSubKey($@"Software\Microsoft\{NodeName}");
            if (wKey == null)
            {
                myKey.OpenSubKey($@"Software\Microsoft\{NodeName}", true);
                wKey = myKey.CreateSubKey($@"Software\Microsoft\{NodeName}"); //($@"Software\Microsoft\{NodeName}");
                AddNewKey(NodeName, KeyName, DefaultKeyValue);
            }
            // Читаем данные и конвертируем в нужный формат.
            string Str = wKey.GetValue(KeyName, DefaultKeyValue) as string ;

            wKey.Close();
            return Str;
        }
        public void SetKeyValue(string NodeName, string KeyName, string KeyValue)
        {
            RegistryKey myKey = Registry.LocalMachine;
            RegistryKey wKey = myKey.OpenSubKey($@"Software\Microsoft\{NodeName}");
            if (wKey == null) return;
            wKey.SetValue(KeyName, KeyValue);
            myKey.Close();
        }

        public void DeleteKey(string NodeName, string KeyName)
        {
            RegistryKey myKey = Registry.LocalMachine;

            // Для удаления тоже нужно иметь права редактирования.
            RegistryKey wKey = myKey.OpenSubKey($@"Software\Microsoft\{NodeName}", true);
            if (wKey == null) return;
            try
            {
                wKey.DeleteSubKey(KeyName);
            }
            catch (Exception e){}
            finally
            {
                myKey.Close();
            } 
        }
    }
}
