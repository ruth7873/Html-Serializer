// See https://aka.ms/new-console-template for more information
using pc2;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

var html = await Load("http://hebrewbooks.org/beis");

var cleanHtml = new Regex("\\s[^ ]").Replace(html, "");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);

var htmlElement = "<div> id=\"my-id\" class=\"my-class-1 my-class-2\" width=\"100%\">text</div>";
var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);

HtmlElement root = new HtmlElement();
HtmlElement current = new HtmlElement();
bool flag=false;
foreach (var line in htmlLines)
{
    if (line.Equals("/html"))
        Console.WriteLine();
    else
    {
        if (line.StartsWith("/"))//תווית סוגרת
        {
            current=current.Parent;
            continue;  
        }
        if (HtmlHelper.Instance.HtmlTags.Contains(line)|| HtmlHelper.Instance.HtmlVoidTags.Contains(line))//תגית פותחת
        {
            flag = true;
            HtmlElement newElement=new HtmlElement();
            current.Children.Add(newElement);
            current = newElement;
            continue;
        }
        //if ()
        //{
        //    flag=true;
        //    HtmlElement newElement = new HtmlElement();
        //    current.Children.Add(newElement);
        //    current = newElement;
        //}
        else
            {
            //current.InnerHtml = line;
            var attribute = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
            //foreach (string attr in attribute)
            //{
            //    if (attr.StartsWith("id"))
            //        current.Id = attr;
            //    else if(attr.StartsWith("name"))
            //        current.Name = attr;
            //    else if(attr.StartsWith("class"))
            //    {
            //        var a=attr.Split(" ");
            //        current.Classes.Add(attr);
            //    }
            //    else
            //        current.Attributes.Add(attr);
            //}
            
        }
    }
}


Console.ReadLine();


async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
