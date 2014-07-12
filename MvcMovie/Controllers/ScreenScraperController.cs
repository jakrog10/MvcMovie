using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using System.Xml.XPath;
using MvcMovie.Models;
using System.Text.RegularExpressions;

namespace MvcMovie.Controllers
{
    public class ScreenScraperController : Controller
    {
        //
        // GET: /ScreenScraper/

        public ActionResult Index()
        {
            //this.ViewBag.Message = ScreenScrapeSGAmmo("http://sgammo.com/catalog/rimfire-ammunition/22-lr");
            this.ViewBag.Message = this.ViewBag.Message + 
                ScrapePalmettoStateArmory("http://palmettostatearmory.com/index.php/ammunition/rimfire-ammunition/22-long-rifle.html?limit=25&mode=list");
            return View();
        }

        public static string ScrapePalmettoStateArmory(string str)
        {

            string outPutString = System.String.Empty;
            var getHtmlWeb = new HtmlWeb();
            var doc = getHtmlWeb.Load(str);

            HtmlAgilityPack.HtmlNodeCollection items = doc.DocumentNode.SelectNodes
            ("//li[contains(@class, 'item')]");

            var ammoCollection = new List<Ammo> { };
            foreach (var i in items)
            {
                Ammo currentAmmo = new Ammo();
                string DescriptionString =
                    ".//h2[contains(@class,'product-name')]//a";
                string PriceString = ".//span[@class='price']";
                string numInStock = ".//td[3]";

                currentAmmo.Description = i.SelectSingleNode(DescriptionString).OuterHtml;
                try
                {
                    currentAmmo.Price = i.SelectSingleNode(PriceString).InnerText;
                }
                catch
                { 
                 currentAmmo.Price = "caught an error trying to locate price on " + currentAmmo.Description;
                }
                
                  
                //currentAmmo.NumberInSock = r.SelectSingleNode(numInStock).InnerText;

                //outPutString += i.OuterHtml + "<br />";
                outPutString += "Description is: " + currentAmmo.Description + "<br />";
                outPutString += "Price is: " + currentAmmo.Price + "<br />";
            }
            
            
            
            
            if (outPutString == string.Empty)
            {
                outPutString = "<br />baseline ScrapePalmettoStateArmory Method with return null";
            }
            
            return outPutString;

        }

        public static string GenericAmmoSearch(string url)
        {
            string outPutString = System.String.Empty;
            var getHtmlWeb = new HtmlWeb();
            var doc = getHtmlWeb.Load(url);

         

            

            return outPutString;
        }



        public static string ScreenScrapeSGAmmo(string str)
        {
            Uri baseURI = new Uri(str);
           
            string outPutString = System.String.Empty;
            // do some work
            var getHtmlWeb = new HtmlWeb();
            var doc = getHtmlWeb.Load(str);
            //string searchString = "/product/aguila/500-round-brick-22-lr-colibri-powderless-low-noise-ammo-20-grain-bullet-not-semi-auto";
            string searchString = "/product/";
            int counter = 0;

            //**********************************************
            //////System.Xml.XPath.XPathNavigator x = doc.CreateNavigator();
            //////XPathNodeIterator it = x.Select(bug);
            //////while (it.MoveNext())
            //////{

            //////    outPutString = outPutString + counter + " ." + it.Current.Value + "<br />";
            //////}
            //**********************************************

            //************** get rowes **************************************
            HtmlAgilityPack.HtmlNodeCollection rows = doc.DocumentNode.SelectNodes
                ("//tr[td[a[contains(@href, '/product/')]]]");
            
            var ammoCollection = new List<Ammo>{};
           
            foreach (var r in rows)
            {
                string DescriptionString = 
                    ".//td//a[contains(@href,'" + searchString + "') and contains(.,'22') and not (img)]";
                string PriceString = ".//td[@class='price-cell']//span";
                string numInStock = ".//td[3]";
               // outPutString = outPutString + r.OuterHtml + "<br />";
                Ammo currentAmmo = new Ammo();
                
                


                currentAmmo.Description = r.SelectSingleNode(DescriptionString).OuterHtml;

                //************ put host url into relative path supplied by host ***********
                string HRefPattern= "href\\s*=[\"]";
                string replacement = "href=\"" + baseURI.Scheme + "://" + baseURI.Host;
                Regex rgx = new Regex(HRefPattern);
                string result = rgx.Replace(currentAmmo.Description, replacement);
                //************ end host url replacement logic ******************************
                currentAmmo.Description = result;
                currentAmmo.Price = r.SelectSingleNode(PriceString).InnerText;
                currentAmmo.NumberInSock = r.SelectSingleNode(numInStock).InnerText;
                string[] w = System.Text.RegularExpressions.Regex.Split(currentAmmo.Description, "[0-9]+");
                currentAmmo.NumberRounds = Regex.Match(currentAmmo.Description, @"\d+").ToString();
                ammoCollection.Add(currentAmmo);



            }

            foreach (var a in ammoCollection)
            {
                outPutString = outPutString + "<br />" + "description is: " + a.Description + "<br />";
                outPutString = outPutString + "price is " + a.Price + "<br />";
                outPutString = outPutString + "number in stock is " + a.NumberInSock + "<br />";
                outPutString = outPutString + "number of rounds is " + a.NumberRounds + "<br />";
                outPutString = outPutString + "output uri is " + getHtmlWeb.ResponseUri + "<br />";
            }

            outPutString = outPutString + "****************** end rows ********* <br />";
            //************** end rowes ***************************************

            outPutString = outPutString + "****************** begin cols ********* <br />";
            for (int i = 0; i < rows.Count; ++i) 
            
            {

                HtmlNodeCollection cols = rows[i].SelectNodes(".//td");
                for (int j = 0; j < cols.Count; ++j)
                {
                    string value = cols[j].OuterHtml;
                    //outPutString = outPutString + value + "<br />";

                }

            }
            outPutString = outPutString + "****************** end cols ********* <br />";
            //************** description from anchors ***********************
            string descString = "//tr//td//a[contains(@href,'" + searchString + "') and contains(.,'22') and not (img)]";
            var data = doc.DocumentNode.SelectNodes(descString);
         
            foreach (var item in data)
            {
                counter += 1;
                //outPutString = outPutString + counter + ". " + item.InnerText + "<br />";
                // *** last one used - outPutString = outPutString + counter + ". " + item.SelectSingleNode("//a").OuterHtml;
            }
            //****************************************************************
            
            
            //var aTags = document.DocumentNode.SelectNodes("//td");
              
            IEnumerable<HtmlNode> paragraphs = doc.DocumentNode.Descendants().Where(p => p.Name.ToLower() == "p");
           //IEnumerable<HtmlNode> tr = doc.DocumentNode.Descendants().Where(t => t.Name.ToLower() == "tr");

            //try linq()
            var names = from y in doc.DocumentNode.Descendants().Where(n => n.Name == "td")
                        select y;
                              //from td in tr.Descendants("td").Where(x => x.Attributes["href"].Value == 
                              //    "/product/winchester/50-rd-box-22-lr-number-12-shot-shells-rat-or-snake-shot-ammo")
                              //where td.InnerText.Trim().Length > 0
                              //select tr;
           
            //////////var data =
            //////////    from
            //////////        tr in doc.DocumentNode.Descendants().Where(td => td.Name.ToLower() == "tr")
                
            //////////       //tr.Descendants("td").Where(a => a.Attributes["href"].Value.Contains(searchString))
            //////////    from
            //////////        td in tr.Descendants().Where(t => t.Name.ToLower() == "td")
            //////////    from
            //////////        a in td.Descendants("a").Where(a => a.Attributes["href"].Value.Contains(searchString) &!
            //////////        a.InnerHtml.Contains("img"))
            //////////    select tr;
            
            
           

            

            ////////if (data != null)
            ////////{
               
            ////////    foreach (var t in data)
            ////////    {
                    
            ////////        //outPutString = "url inner text = " + lnk.InnerText + "<br />";
            ////////        //col = t.SelectNodes("//td[2]");
            ////////        //outPutString = outPutString + counter + " ." + t.InnerHtml + "<br />";
                    
                   
            ////////        //string s = t.InnerHtml.ToString();
                  
                 
            ////////    }


            ////////}
            
            ////if (aTags != null)
            ////{
            ////    foreach (var aTag in aTags)
            ////    {
            ////        counter += 1;
            ////        //outPutString = outPutString + counter + "found one" + "\t" + "<br />";
            ////        // outPutString = outPutString + aTag.InnerHtml + " - " + aTag.Attributes["href"] + "\t" + "<br />";
            ////       // ** last working outPutString = outPutString + counter + ". " + aTag.InnerHtml + "<br />";
                   
                    
            ////           // "found one" + "\t" + "<br />";
            ////        //outPutString += counter + ". " + aTag.InnerHtml + " - " +
            ////        //  aTag.Attributes["href"].Value + "\t" + "<br />";
            ////        //counter++;
            ////    }
            ////}

            return outPutString;
        }
        
        public static string ScreenScrapeOld(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                // set properties of the client 
                return client.DownloadString(url);
            }
        }

    }
}
