using NSoft.Promo;
using System.Collections.Generic;

namespace VSNRM_Kompas.PromoCods
{
    public class IPromo_Class
    {
        public global::Option_Class IOption_Class;

        public PromoClient Client => PromoLicense.Client;

        /// <summary>Создаёт файл-запрос промокода (.datq), который клиент отправляет разработчику.</summary>
        public void CreateRequestFile(string path)
        {
            Client.WriteRequestFile(path, Client.LoadCodes());
        }

        /// <summary>
        /// Загружает файл промокода (.dat), проверяет и сохраняет код. Функции включаются сразу, перезапуск не нужен.
        /// </summary>
        public bool ImportPromoFile(string path, out string message)
        {
            List<string> codes = Client.LoadCodes();
            if (!Client.ImportPromoFile(path, codes, out message)) return false;
            Client.SaveCodes(codes);
            if (IOption_Class != null) PromoLicense.Apply(IOption_Class);
            return true;
        }

        public string ActiveFeaturesText() => Client.ActiveFeaturesText(Client.LoadCodes());
    }
}
