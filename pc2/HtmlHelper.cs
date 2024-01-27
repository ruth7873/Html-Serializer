using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace pc2
{
    public class HtmlHelper
    {
        private static readonly HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        private HtmlHelper()
        {
            var HtmlTags = File.ReadAllText("tagsList/HtmlTags.json");
            var HtmlVoidTags = File.ReadAllText("tagsList/HtmlVoidTags.json");
            this.HtmlTags = JsonSerializer.Deserialize<string[]>(HtmlTags);
            this.HtmlVoidTags = JsonSerializer.Deserialize<string[]>(HtmlVoidTags);
        }
    }
}
