using NSoft.Promo;

namespace VSNRM_Kompas.PromoCods
{
    /// <summary>
    /// Промокоды BOM-Report Kompas. Общая логика - NSoft.Promo.PromoClient,
    /// здесь только привязка к программе и к её настройкам.
    /// </summary>
    public static class PromoLicense
    {
        public static readonly PromoClient Client = new PromoClient(
            PromoProducts.BomReportKompas,
            MainForm.ProgramID,
            () => SaveDXF.Body.KompasVersion,
            () => global::DetectVirtualMachine_Class.DetectVirtualMachine());

        /// <summary>Выставляет флаги функций по действующим промокодам. Флаги в настройках сами ничего не включают: при запуске они пересчитываются.</summary>
        public static void Apply(global::Option_Class options)
        {
            if (options == null) return;
            if (options.IVC == null) options.IVC = new global::Option_Class.VisibleControls();
            global::Option_Class.VisibleControls v = options.IVC;
            ushort f = Client.ActiveFeatures(Client.LoadCodes());
            v.CutLength = Client.Has(f, "cut");
            v.PropertyTranslation = Client.Has(f, "translate");
            v.ProjectClone = Client.Has(f, "clone");
            v.Check_ProfileValue = Client.Has(f, "profile");
            v.EditOn = Client.Has(f, "edit");
            //галочка «Base64 из экспорт.рис.» видна по промокоду, её включение - выбор пользователя
            v.GetBase64FromImageForDrawingPanelVis = Client.Has(f, "base64");
            if (!v.GetBase64FromImageForDrawingPanelVis) v.GetBase64FromImageForDrawing = false;
            v.SpecialPan = v.CutLength || v.PropertyTranslation || v.Check_ProfileValue;
        }
    }
}
