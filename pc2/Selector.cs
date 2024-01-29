using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pc2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector ConvertToSelector(string str)
        {
            string[] arr = str.Split(' ');
            Selector root = new Selector();
            Selector current = root;
            string pattern = @"(?<tag>\w+)(?:#(?<id>\w+))?(?:\.(?<class>\w+))*";
            foreach (string s in arr)
            {
                // מציאת ההתאמות באמצעות regex
                Match match = Regex.Match(s, pattern);
                // חלוץ את התגית
                string tagName = match.Groups["tag"].Value;
                // חלוץ את המזהה
                string id = match.Groups["id"].Value;
                // חלוץ את הקלאסים
                string classes = string.Join(" ", match.Groups["class"].Captures.Cast<Capture>().Select(c => c.Value));
                string[]arr_classes=classes.Split(" ");
                current.Child = new Selector();
                current = current.Child;
                if(HtmlHelper.Instance.HtmlTags.Contains(tagName) || HtmlHelper.Instance.HtmlVoidTags.Contains(tagName))
                current.TagName = tagName;
                current.Id = id;
                foreach (string c in arr_classes)
                {
                    current.Classes.Add(c);
                }
            }

            return root.Child;
        }
    }
}