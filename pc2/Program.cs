using pc2;
using System.Text.RegularExpressions;

var html = await Load("https://chani-k.co.il/sherlok-game/");//loading html from website 

html = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
var htmlLines = new Regex("<(.*?)>").Split(html).Where(s => s.Length > 0);//divide the html to tags
var root = Serialize(htmlLines);
Console.WriteLine("============print the tree=======");
PrintTree(root);

Console.WriteLine("===========check 1 - div#copyright==============");
Check(Selector.ConvertToSelector("div#copyright"));
Console.WriteLine("===========check 2 - div img====================");
Check(Selector.ConvertToSelector("div img"));
Console.WriteLine("===========check 3 - a img======================");
Check(Selector.ConvertToSelector("a img"));
void Check(Selector selector)
{
    var result = root.Query(selector);
    result.ToList().ForEach(element => { Console.WriteLine(element); });
}
void PrintTree(HtmlElement root)
{
    if (root == null)
        return;
    Console.WriteLine(root.ToString());
    for (int i = 0; i < root.Children.Count; i++) { PrintTree(root.Children[i]); }
}
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

Console.ReadKey();

