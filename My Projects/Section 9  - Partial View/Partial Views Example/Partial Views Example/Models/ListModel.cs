using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Partial_Views_Example.Models
{
    public class ListModel
    {
        public string ListTitle { get; set; } = "";
        public List<string> ListItems { get; set; } = new List<string>();

       
    }
}
