using System;
using HtmlAgilityPack;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScratchParser
{
    public class ScratchData
    {

        ScratchData()
        {

        }
        
        public void parseURL(string url)
        {
            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(url);
        }

        public void parseHTML(string filename)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(filename);
        }
    }
}
