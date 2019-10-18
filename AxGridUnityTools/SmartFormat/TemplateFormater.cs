using System;
using SmartFormat;
using SmartFormat.Core.Extensions;


namespace AxGrid.SmartFormat
{
    /// <summary>
    /// Форматирование шаблонное
    /// </summary>
    public class TemplateFormater  : IFormatter
    {
        public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
        {
            var iCanHandleThisInput = formattingInfo.CurrentValue is int || 
                                      formattingInfo.CurrentValue is long || 
                                      formattingInfo.CurrentValue is double ||
                                      formattingInfo.CurrentValue is float;
            if (!iCanHandleThisInput)
                return false;
            
            double val = Convert.ToInt64(formattingInfo.CurrentValue);
            
            var opts = formattingInfo.FormatterOptions != null
                ? formattingInfo.FormatterOptions.Split(',')
                : new string[0];

            var templateName = opts.Length > 0 && opts[0] != null ? opts[0] : null;
            var templateNameAlt = opts.Length > 1 && opts[1] != null ? opts[1] : null;

            if (templateName == null)
            {
                formattingInfo.Write(val.ToString());
                return true;
            }

            try
            {
                var template = Text.Text.GetFormat(templateName);
                if (string.IsNullOrEmpty(template))
                    template = Text.Text.GetFormat(templateNameAlt);
                formattingInfo.Write(Smart.Format(template, val));
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return false;
            }
        }

        private string[] names = {"tp", "template"};
        public string[] Names { get { return names; } set { names = value; } }
    }
}