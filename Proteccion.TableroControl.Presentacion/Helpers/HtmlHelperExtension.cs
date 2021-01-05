using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Proteccion.TableroControl.Presentacion.Helpers
{
    public static class HtmlHelperExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static HtmlString HtmlConvertToJson(this IHtmlHelper htmlHelper, object model)
        {
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = Formatting.Indented;

            return new HtmlString(JsonConvert.SerializeObject(model, settings));
        }
    }
}
