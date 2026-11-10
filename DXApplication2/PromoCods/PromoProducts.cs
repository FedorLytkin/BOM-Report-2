using System;
using System.Collections.Generic;
using System.Linq;

namespace NSoft.Promo
{
    /// <summary>Функция программы, которую включает промокод.</summary>
    public sealed class PromoFeatureInfo
    {
        /// <summary>Бит в маске функций. НЕ МЕНЯТЬ: зашит в уже выданные коды.</summary>
        public ushort Bit { get; }
        /// <summary>Короткое имя для Tools\PromoKeyGen (--features merge,pdf).</summary>
        public string Key { get; }
        public string Name { get; }

        public PromoFeatureInfo(ushort bit, string key, string name)
        {
            Bit = bit;
            Key = key;
            Name = name;
        }
    }

    /// <summary>Программа, для которой выписываются промокоды.</summary>
    public sealed class PromoProduct
    {
        /// <summary>Код программы в файле-запросе. НЕ МЕНЯТЬ.</summary>
        public string Id { get; }
        public string Name { get; }
        /// <summary>Домен подписи: код другой программы не пройдёт проверку. НЕ МЕНЯТЬ.</summary>
        public string Domain { get; }
        /// <summary>Первая строка файла промокода .dat. НЕ МЕНЯТЬ.</summary>
        public string ResponseHeader { get; }
        public IReadOnlyList<PromoFeatureInfo> Features { get; }

        public PromoProduct(string id, string name, string domain, string responseHeader, params PromoFeatureInfo[] features)
        {
            Id = id;
            Name = name;
            Domain = domain;
            ResponseHeader = responseHeader;
            Features = features;
        }

        public PromoProduct(string id, string name, params PromoFeatureInfo[] features)
            : this(id, name, "NSoft.Promo.v1/" + id, "NSOFT-PROMO/" + id, features)
        {
        }

        public string Describe(ushort features)
        {
            return string.Join(", ", Features.Where(f => (features & f.Bit) != 0).Select(f => f.Name));
        }

        /// <summary>Биты, которых нет у этой программы.</summary>
        public ushort UnknownBits(ushort features)
        {
            ushort known = 0;
            foreach (PromoFeatureInfo f in Features) known |= f.Bit;
            return (ushort)(features & ~known);
        }

        public override string ToString() => Name;
    }

    /// <summary>
    /// Каталог программ и их функций. ОБЩИЙ файл: копии во всех программах и в LicManager2.0 должны совпадать.
    /// Новую программу или функцию - только ДОБАВЛЯТЬ, существующие Id/Bit/Domain/ResponseHeader не менять.
    /// </summary>
    public static class PromoProducts
    {
        static PromoFeatureInfo F(ushort bit, string key, string name) => new PromoFeatureInfo(bit, key, name);

        //DXF-Auto Kompas - первая программа с этой схемой: домен и заголовок исторические
        public static readonly PromoProduct DxfAutoKompas = new PromoProduct("DXFAUTO-KOMPAS", "DXF-Auto Kompas",
            "NSoft.DXFAuto.Promo.v1", "NSOFT-DXFAUTO-PROMO",
            F(1, "cut", "Длина реза, гибы и отверстия"),
            F(2, "report", "Отчёт заказа"),
            F(4, "merge", "Объединение DXF"));

        public static readonly PromoProduct DxfAutoInventor = new PromoProduct("DXFAUTO-INVENTOR", "DXF-Auto Inventor",
            F(1, "merge", "Объединение DXF"),
            F(2, "holes", "Проверка диаметров отверстий"),
            F(4, "bend", "Чертежи для гибки"),
            F(8, "engraving", "Гравировка"));

        public static readonly PromoProduct DxfAutoSw = new PromoProduct("DXFAUTO-SW", "DXF-Auto SW",
            F(1, "merge", "Объединение DXF"));

        public static readonly PromoProduct BomReportKompas = new PromoProduct("BOMREPORT-KOMPAS", "BOM-Report Kompas",
            F(1, "cut", "Параметры листового тела (длина реза, гибы, отверстия)"),
            F(2, "translate", "Трансляция свойств в узлы"),
            F(4, "clone", "Копирование проекта"),
            F(8, "profile", "Проверка заполненности длины профиля"),
            F(16, "edit", "Редактирование"),
            F(32, "base64", "Base64 из рисунков чертежей"));

        public static readonly PromoProduct BomReportInventor = new PromoProduct("BOMREPORT-INVENTOR", "BOM-Report Inventor",
            F(1, "fan", "Ведомость для вентиляции"));

        public static readonly PromoProduct BomReportSw = new PromoProduct("BOMREPORT-SW", "BOM-Report SW",
            F(1, "pdf", "Сохранение PDF"),
            F(2, "material", "Калькуляция материала"),
            F(4, "erp", "Ведомость ERP"));

        public static readonly PromoProduct DrawingExportKompas = new PromoProduct("DRAWINGEXPORT-KOMPAS", "Drawing-Export Kompas",
            F(1, "copydrw", "Создание чертежа по шаблону"));

        public static readonly PromoProduct DrawingExportInventor = new PromoProduct("DRAWINGEXPORT-INVENTOR", "Drawing-Export Inventor",
            F(1, "copydrw", "Создание чертежа по шаблону"));

        public static readonly PromoProduct DrawingExportSw = new PromoProduct("DRAWINGEXPORT-SW", "Drawing-Export SW",
            F(1, "copydrw", "Создание чертежа по шаблону"));

        public static readonly IReadOnlyList<PromoProduct> All = new[]
        {
            DxfAutoKompas, DxfAutoInventor, DxfAutoSw,
            BomReportKompas, BomReportInventor, BomReportSw,
            DrawingExportKompas, DrawingExportInventor, DrawingExportSw,
        };

        /// <summary>Программа запросов без строки кода программы (DXF-Auto Kompas 25.112.26.265).</summary>
        public const string LegacyProductId = "DXFAUTO-KOMPAS";

        public static PromoProduct Find(string id) =>
            All.FirstOrDefault(p => string.Equals(p.Id, id?.Trim(), StringComparison.OrdinalIgnoreCase));

        public static PromoProduct FindByHeader(string header) =>
            All.FirstOrDefault(p => p.ResponseHeader == header?.Trim());
    }
}
