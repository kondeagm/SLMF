using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SLMFAPI.Models;
using SLMFAPI.Models.Comunidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace SLMFAPI.Controllers
{
    public class ComunidadController : ApiController
    {
        private SlmfDBEntities db = new SlmfDBEntities();
        private List<SocialNetPost> lAllPosts = new List<SocialNetPost>();

        [Route("api/comunityfeed")]
        [HttpPost]
        [CacheOutput(ClientTimeSpan = 120, ServerTimeSpan = 120)]
        public IHttpActionResult GetFeedSocialNets()
        {
            try
            {
                JObject joSocialNets = CreateJsonSocialNets();
                return Ok(joSocialNets);
            }
            catch
            {
                return InternalServerError();
            }
        }

        private JObject CreateJsonSocialNets()
        {
            lAllPosts.Clear();
            JObject joResponse = new JObject();
            var bdRedes = from data in db.RedSocial.Include(p => p.ContenidoEstatico)
                          select data;
            foreach (var bdRed in bdRedes)
            {
                JProperty jpRed = new JProperty(bdRed.Nombre.ToLower(),
                    new JObject(
                        new JProperty("url", bdRed.URL),
                        new JProperty("title", bdRed.APPId),
                        new JProperty("slides", CreateJsonFeedsSocialNet(bdRed))
                    )
                );
                joResponse.Add(jpRed);
            }
            JProperty jpAllRed = new JProperty("all", CreateJsonAllFeedsSocialNet());
            joResponse.Add(jpAllRed);
            return joResponse;
        }

        private JArray CreateJsonFeedsSocialNet(RedSocial pBdRed)
        {
            JArray jaResponse = new JArray();
            switch (pBdRed.Nombre.ToLower().Trim())
            {
                case "youtube":
                    jaResponse = FeedsYoutube(pBdRed);
                    break;

                case "twitter":
                    jaResponse = FeedsTwitter(pBdRed);
                    break;

                case "instagram":
                    jaResponse = FeedsInstagram(pBdRed);
                    break;
            }
            return jaResponse;
        }

        private JArray FeedsYoutube(RedSocial pBdRed)
        {
            JArray jaResponse = new JArray();
            List<SocialNetPost> lVideoPosts = new List<SocialNetPost>();
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = pBdRed.APIKey,
                ApplicationName = this.GetType().ToString()
            });
            var searchListRequest = youtubeService.Search.List("snippet");
            if (pBdRed.ContenidoEstatico.Count > 0)
            {
                searchListRequest.MaxResults = 1;
                foreach (var bdContenido in pBdRed.ContenidoEstatico)
                {
                    searchListRequest.Q = bdContenido.Identificador;
                    var searchListResponse = searchListRequest.Execute();
                    foreach (var searchResult in searchListResponse.Items)
                    {
                        SocialNetPost video = new SocialNetPost();
                        video.ID = searchResult.Id.VideoId;
                        video.URL = "https://www.youtube.com/watch?v=" + searchResult.Id.VideoId + "&feature=youtube_gdata_player";
                        video.Thumb = searchResult.Snippet.Thumbnails.High.Url;
                        video.Date = Convert.ToDateTime(searchResult.Snippet.PublishedAt);
                        video.Title = searchResult.Snippet.Title;
                        video.Description = searchResult.Snippet.Description;
                        video.Name = "youtube";
                        lVideoPosts.Add(video);
                        lAllPosts.Add(video);
                    }
                }
            }
            else
            {
                searchListRequest.MaxResults = pBdRed.NoPost;
                searchListRequest.Q = pBdRed.APPId;
                var searchListResponse = searchListRequest.Execute();
                foreach (var searchResult in searchListResponse.Items)
                {
                    SocialNetPost video = new SocialNetPost();
                    video.ID = searchResult.Id.VideoId;
                    video.URL = "https://www.youtube.com/watch?v=" + searchResult.Id.VideoId + "&feature=youtube_gdata_player";
                    video.Thumb = searchResult.Snippet.Thumbnails.High.Url;
                    video.Date = Convert.ToDateTime(searchResult.Snippet.PublishedAt);
                    video.Title = searchResult.Snippet.Title;
                    video.Description = searchResult.Snippet.Description;
                    video.Name = "youtube";
                    lVideoPosts.Add(video);
                    lAllPosts.Add(video);
                }
            }
            foreach (var VideoPost in lVideoPosts.OrderByDescending(s => s.Date))
            {
                JObject joVideo =
                    new JObject(
                        new JProperty("url", VideoPost.URL),
                        new JProperty("thumbnail", VideoPost.Thumb),
                        new JProperty("day", VideoPost.Date.ToString("dd", CultureInfo.InvariantCulture)),
                        new JProperty("month", VideoPost.Date.ToString("MMM", CultureInfo.CreateSpecificCulture("es-MX")).ToUpper()),
                        new JProperty("year", VideoPost.Date.ToString("yyyy", CultureInfo.InvariantCulture)),
                        new JProperty("title", VideoPost.Title),
                        new JProperty("description", VideoPost.Description)
                    );
                jaResponse.Add(joVideo);
            }
            return jaResponse;
        }

        private JArray FeedsTwitter(RedSocial pBdRed)
        {
            JArray jaResponse = new JArray();
            List<SocialNetPost> lTweetPosts = new List<SocialNetPost>();
            var twitter = new Twitter(pBdRed.APPId, pBdRed.APIKey, "150706984-SLW2aTZi9xzQ2Jf7lnCM7CuOsojfiKAo9aI2ME2z", "xbvbnZ72usfVen5cnk3OOiajEp78bNCeaU5AZi0GyoQVH");
            var response = twitter.GetTweets(pBdRed.Identificador.Replace("@", "").ToLower(), pBdRed.NoPost);
            dynamic timeline = System.Web.Helpers.Json.Decode(response);
            foreach (var tweet in timeline)
            {
                SocialNetPost twett = new SocialNetPost();
                twett.ID = tweet["id_str"].ToString();
                twett.URL = "https://twitter.com/" + pBdRed.Identificador.Replace("@", "").ToLower() + "/status/" + tweet["id_str"].ToString();
                twett.Thumb = "";
                twett.Date = DateTime.ParseExact(tweet["created_at"].ToString(), "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                twett.Title = "";
                twett.Description = tweet["text"].ToString();
                twett.Name = "twitter";
                lTweetPosts.Add(twett);
                lAllPosts.Add(twett);
            }
            foreach (var Tweet in lTweetPosts.OrderByDescending(s => s.Date))
            {
                JObject joTweet =
                    new JObject(
                        new JProperty("url", Tweet.URL),
                        new JProperty("thumbnail", Tweet.Thumb),
                        new JProperty("day", Tweet.Date.ToString("dd", CultureInfo.InvariantCulture)),
                        new JProperty("month", Tweet.Date.ToString("MMM", CultureInfo.CreateSpecificCulture("es-MX")).ToUpper()),
                        new JProperty("year", Tweet.Date.ToString("yyyy", CultureInfo.InvariantCulture)),
                        new JProperty("title", Tweet.Title),
                        new JProperty("description", Tweet.Description)
                    );
                jaResponse.Add(joTweet);
            }
            return jaResponse;
        }

        private JArray FeedsInstagram(RedSocial pBdRed)
        {
            JArray jaResponse = new JArray();
            List<SocialNetPost> lInstaPosts = new List<SocialNetPost>();
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://api.instagram.com/v1/users/" +
                pBdRed.APPId + "/media/recent?count=" + pBdRed.NoPost + "&access_token=" + pBdRed.APIKey);
            request.Method = "GET";
            String json = String.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                json = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
            }
            dynamic dyn = JsonConvert.DeserializeObject(json);
            foreach (var data in dyn.data)
            {
                SocialNetPost instapost = new SocialNetPost();
                instapost.ID = Convert.ToString(data.id);
                instapost.URL = Convert.ToString(data.link);
                instapost.Thumb = Convert.ToString(data.images.thumbnail.url);
                instapost.Date = UnixTimeStampToDateTime((double)data.created_time);
                instapost.Title = "";
                if (data.caption != null)
                {
                    instapost.Description = Convert.ToString(data.caption.text);
                }
                instapost.Name = "instagram";
                lInstaPosts.Add(instapost);
                lAllPosts.Add(instapost);
            }
            foreach (var Instapost in lInstaPosts.OrderByDescending(s => s.Date))
            {
                JObject joTweet =
                    new JObject(
                        new JProperty("url", Instapost.URL),
                        new JProperty("thumbnail", Instapost.Thumb),
                        new JProperty("day", Instapost.Date.ToString("dd", CultureInfo.InvariantCulture)),
                        new JProperty("month", Instapost.Date.ToString("MMM", CultureInfo.CreateSpecificCulture("es-MX")).ToUpper()),
                        new JProperty("year", Instapost.Date.ToString("yyyy", CultureInfo.InvariantCulture)),
                        new JProperty("title", Instapost.Title),
                        new JProperty("description", Instapost.Description)
                    );
                jaResponse.Add(joTweet);
            }
            return jaResponse;
        }

        private JArray CreateJsonAllFeedsSocialNet()
        {
            JArray jaResponse = new JArray();
            foreach (var SocialNetPost in lAllPosts.OrderByDescending(s => s.Date))
            {
                JObject joPost =
                    new JObject(
                        new JProperty("name", SocialNetPost.Name),
                        new JProperty("day", SocialNetPost.Date.ToString("dd", CultureInfo.InvariantCulture)),
                        new JProperty("month", SocialNetPost.Date.ToString("MMM", CultureInfo.CreateSpecificCulture("es-MX")).ToUpper()),
                        new JProperty("year", SocialNetPost.Date.ToString("yyyy", CultureInfo.InvariantCulture)),
                        new JProperty("description", SocialNetPost.Description),
                        new JProperty("thumbnail", SocialNetPost.Thumb),
                        new JProperty("title", SocialNetPost.Title),
                        new JProperty("url", SocialNetPost.URL)
                    );
                jaResponse.Add(joPost);
            }
            return jaResponse;
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}