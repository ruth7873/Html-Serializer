using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace pc2
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
   
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                HtmlElement element = queue.Dequeue();
                yield return element;
                foreach (HtmlElement child in element.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement element = this;
            while (element != null)
            {
                yield return element;
                element = element.Parent;
            }
        }
        public IEnumerable<HtmlElement> Query(Selector selector)
        {
            var set = new HashSet<HtmlElement>();
            FindElementBySelector(selector, set, this.Descendants());
            return set;
        }
        void FindElementBySelector(Selector selector, HashSet<HtmlElement> list, IEnumerable<HtmlElement> elements)
        {
            if (selector == null)
                return;

            foreach (var item in elements)
            {
                if (CheckSelector(item, selector))
                {
                   if (selector.Child == null)
                        list.Add(item);
                    FindElementBySelector(selector.Child, list, item.Descendants());
                }
            }
        }

        public bool CheckSelector(HtmlElement element, Selector selector)
        {
             var s = "\"" + selector.Id + "\"";
            if(selector.Id != ""&& !s.Equals(element.Id))
                return false;
            if (selector.TagName != element.Name)
                return false;
               
            foreach (var c in selector.Classes)
                if (element.Classes.Count>0&&!element.Classes.Contains(c))
                    return false;
            return true;
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Id: {Id}");
            stringBuilder.AppendLine($"Name: {Name}");
            stringBuilder.AppendLine("Attributes:");
            foreach (var attribute in Attributes)
            {
                stringBuilder.AppendLine($"   {attribute.Key}: {attribute.Value}");
            }
            stringBuilder.AppendLine("Classes:");
            foreach (var className in Classes)
            {
                stringBuilder.AppendLine($"   {className}");
            }
            stringBuilder.AppendLine($"InnerHtml: {InnerHtml}");
            stringBuilder.AppendLine($"Parent: {Parent?.Id ?? "null"}"); // Parent might be null
            stringBuilder.AppendLine("Children:");
            foreach (var child in Children)
            {
                stringBuilder.AppendLine($"   {child.Id}");
            }

            return stringBuilder.ToString();
        }

    }

}
