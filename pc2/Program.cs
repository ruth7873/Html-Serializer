// See https://aka.ms/new-console-template for more information
using pc2;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

Selector.ConvertToSelector("div#mydiv.class_name \"div#yourdiv.class_name222”\r\n");
//var html = await Load("http://hebrewbooks.org/beis");

//var cleanHtml = new Regex("\\s[^ ]").Replace(html, "");
//var html = await Load("https://mail.google.com/mail/u/0/#inbox");

var html = await Load("https://chani-k.co.il/sherlok-game/");
html = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
var htmlLines = new Regex("<(.*?)>").Split(html).Where(s => s.Length > 0);//divide the html to tags
HtmlElement Serialize(IEnumerable<string> htmlLines)
{

    HtmlElement root = new HtmlElement();
    HtmlElement current = root;
    foreach (var line in htmlLines)
    {
        string[] words = line.Split(' ');
        if (!words[0].Equals("/html"))//the end of the html
        {
            if (words[0].StartsWith("/"))//תווית סוגרת
                current = current.Parent;
            else if (HtmlHelper.Instance.HtmlTags.Contains(words[0]) || HtmlHelper.Instance.HtmlVoidTags.Contains(words[0]))//תגית פותחת
            {
                HtmlElement newElement = new HtmlElement();
                newElement.Parent = current;
                newElement.Name = words[0];
                current.Children.Add(newElement);
                current = newElement;
                if (line.IndexOf(' ') > 0)//there is attributes
                {
                    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line.Substring(line.IndexOf(' '))).ToList();
                    foreach (var attribute in attributes)
                    {
                        string[] arr = attribute.ToString().Split("=");
                        if (arr[0].Equals("id"))
                            current.Id = arr[1];
                        else if (arr[0].Equals("class"))
                        {
                            current.Classes = arr[1].Split(" ").ToList();
                        }
                        else
                            current.Attributes.Add(arr[0], arr[1]);
                    }
                }
                if (HtmlHelper.Instance.HtmlVoidTags.Contains(words[0]))//אם הסגירה היא באותה שורה, ללא תווית סוגרת
                {
                    current = current.Parent;
                }
            }
            else//the context of the tag
            {
                current.InnerHtml = line;
            }
        }
    }
    return root;
}

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
