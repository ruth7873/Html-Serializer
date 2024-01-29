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
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        //public HtmlElement()
        //{
        //    this.Children = new List<HtmlElement>();
        //    this.Attributes = new Dictionary<string, string>();
        //}

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(root);
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
            HtmlElement element = root;
            while (element != null)
            {
                yield return element;
                element = element.Parent;
            }
        }
        public bool CheckSelector(HtmlElement element, Selector selector)
        {
            if (selector.Id != element.Id || selector.TagName != element.Name)
                return false;
            foreach (var c in selector.Classes)
                if (!element.Classes.Contains(c))
                    return false;
            return true;
        }
        public IEnumerable<HtmlElement> FindElementBySelector(Selector selector, List<HtmlElement> list)
        {
           
            FindElementBySelector(selector.Child, list);
            return list;
        }
        public IEnumerable<HtmlElement> FindElementBySelectorShellFunction(Selector selector)
        {
                return FindElementBySelector(selector,this.Descendants().ToList().FindAll(d => CheckSelector(d, selector)));
        }
    }
}
