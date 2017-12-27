
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace steemFollowers
{
    class Followers
    {
        
        static string Source(string apiUrl)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
                myRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                myRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                myRequest.Method = "GET";
                myRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                myRequest.Timeout = 5000;
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();
                return result;
            }
            catch (Exception ex) { return ex.Message; }

        }
        static JObject countJsonData(string user)
        {
            return JObject.Parse(Source("https://api.steemjs.com/get_follow_count?account=" + user));
        }
        //we create a Tuple type function to return more than one object
        public static Tuple<int,int,List<follow>> getFollowers(string user)
        {
            try { 
                JObject res = countJsonData(user);// first we want to reach the subscriber numbers of the user
                int follower = (int)res["follower_count"];
                int following = (int)res["following_count"];
            string src = Source("https://api.steemjs.com/get_followers?following=" + user + "&limit=1000"); // it will return last 1000 followers
            List<follow> allFollowers = new List<follow>();
                foreach (Match m in Regex.Matches(src, "{(.*?)}", RegexOptions.Singleline))
                {
                    //we process each subscription separately and save it as a follower object on the list
                    string jsonLine = m.Groups[1].Value.ToString().Replace("[", "").Replace("]", "");
                    follow fo = new follow();
                    fo.follower = Regex.Match(jsonLine, "follower\":\"(.*?)\"", RegexOptions.Singleline).Groups[1].Value;
                    fo.following = Regex.Match(jsonLine, "following\":\"(.*?)\"", RegexOptions.Singleline).Groups[1].Value;
                    fo.type = Regex.Match(jsonLine, "what\":\"(.*?)\"", RegexOptions.Singleline).Groups[1].Value;
                    allFollowers.Add(fo);
                }

                return Tuple.Create(follower,following,allFollowers);
           }
            catch { }return Tuple.Create(0, 0, new List<follow>());
        }
        
    }
}
