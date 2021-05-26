using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace PSO2_Scratch_Parser
{
    public class ScratchList
    {
        public int Count { get
            {
                return m_prizeList.Count;
            } 
        }

        private readonly List<Prize> m_prizeList;
        private readonly string Prize_Url;
        private ScratchList(string url = null) 
        {
            m_prizeList = new List<Prize>();
            Prize_Url = url;
        }

        private string GetImageUrl(string rel_url)
        {
            if (String.IsNullOrEmpty(rel_url))
            {
                return "";
            } 
            else if (String.IsNullOrEmpty(Prize_Url))
            {
                return rel_url;
                
            }

            var uri = new Uri(Prize_Url);
            uri = new Uri(uri, rel_url);
            return uri.AbsoluteUri;
        }

        private void parseHTMLDoc(HtmlDocument htmlDoc)
        {

            var prizes = htmlDoc.DocumentNode.SelectNodes("//dl[@class='item-list-l']");

            foreach (var prize in prizes)
            {
                var prize_name = prize.SelectSingleNode("dt").InnerText;
                var concept_url = prize.SelectSingleNode(".//a[@title='設定画']")?.GetAttributeValue("href", "");
                concept_url = GetImageUrl(concept_url);

                var prize_details = prize.SelectNodes(".//td");

                var prize_list = prize.SelectSingleNode(".//ul[@class='image']");

                if (prize_list != null)
                {
                    var parse_list = new List<PrizeBoxItem>();
                    var prize_contents = prize_list.SelectNodes(".//li");

                    foreach(var item in prize_contents)
                    {
                        Match name_match = Regex.Match(item.InnerText, "「(.*?)」");
                        var item_name = name_match.Success ? name_match.Groups?[1].Value : "";

                        Match genre_match = Regex.Match(item.InnerText, "（(.*?)）");
                        var item_genre = genre_match.Success ? genre_match.Groups?[1].Value : "";

                        var item_image = GetImageUrl(prize.SelectSingleNode($".//a[@title='{item_name}']")?.GetAttributeValue("href", ""));


                        parse_list.Add(new PrizeBoxItem
                        {
                            Name_jp = item_name,
                            Name_en = "",
                            Image_url = item_image,
                            Genre_jp = item_genre,
                            Genre_en = ""
                        });
                    }

                    m_prizeList.Add(new Prize
                    {
                        Name_jp = prize_name,
                        Name_en = "",
                        Concept_art = concept_url,
                        Genre_jp = prize_details[0]?.InnerText,
                        Genre_en = "",
                        Rate = prize_details[1]?.InnerText,
                        Contents = parse_list
                    });
                } 
                else
                {
                    var item_image = GetImageUrl(prize.SelectSingleNode($".//a[@title='{prize_name}']")?.GetAttributeValue("href", ""));

                    m_prizeList.Add(new Prize
                    {
                        Name_jp = prize_name,
                        Name_en = "",
                        Concept_art = concept_url,
                        Image_url = item_image,
                        Genre_jp = prize_details[0]?.InnerText,
                        Genre_en = "",
                        Rate = prize_details[1]?.InnerText
                    });
                }
            }
            Write(@"C:\Users\Jimmy\Desktop\test2.json");
        }
        
        public void Write(string fileName)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };
            var jsonString = JsonConvert.SerializeObject(m_prizeList, settings);
            File.WriteAllText(fileName, jsonString);
        }
        
        public static ScratchList parseFromWebsiteURL(string url)
        {
            var scratchList = new ScratchList(url);

            HtmlWeb web = new HtmlWeb() { OverrideEncoding = Encoding.UTF8 };
            var htmlDoc = web.Load(url);
            scratchList.parseHTMLDoc(htmlDoc);

            return scratchList;
        }

        public static ScratchList parseFromHTMLFile(string filename)
        {
            var scratchList = new ScratchList();

            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(filename);
            scratchList.parseHTMLDoc(htmlDoc);

            return scratchList;
        }
    }
}
