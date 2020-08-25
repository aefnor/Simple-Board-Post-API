using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleBoardAPI.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SimpleBoardAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Postings : ControllerBase
    {
        public static List<Posting> postings = new List<Posting>();

        [HttpGet("/{queryString}")]
        public string Get(string queryString)
        {
            var query = HttpContext.Request.Query;
            Dictionary<string, Microsoft.Extensions.Primitives.StringValues> parameters = HttpContext.Request.Query.Keys.Cast<string>()
            .ToDictionary(k => k, v => HttpContext.Request.Query[v]);
            var current_filter = "";
            List<Posting> postings_filtered = postings;

            if (query.Count == 0)
            {
                return JsonConvert.SerializeObject(postings.ToArray());
            }

            foreach(var pair in query)
            {
                try
                {
                    if (pair.Key == "EQUAL")
                    {
                        if (pair.Value[0].Contains("id"))
                            postings_filtered.Select(n => n).Where(o => o.id.ToLower().Trim() == pair.Value[1].ToLower().Trim());
                        else if (pair.Value[0].Contains("title"))
                        {
                            postings_filtered.Where(o => o.title.ToLower().Trim() == pair.Value[1].ToLower().Trim()).Select(n => n);
                        }
                        else if (pair.Value[0].Contains("content"))
                        {
                            postings_filtered.Where(o => o.content.ToLower().Trim() == pair.Value[1].ToLower().Trim()).Select(n => n);
                        }
                        else if (pair.Value[0].Contains("views"))
                        {
                            postings_filtered.Where(o => o.view.ToString().ToLower().Trim() == pair.Value[1].ToLower().Trim()).Select(n => n);
                        }
                        else if (pair.Value[0].Contains("timestamp"))
                        {
                            postings_filtered.Where(o => o.timestamp.ToString().ToLower().Trim() == pair.Value[1].ToLower().Trim()).Select(n => n);
                        }

                    }
                    else if (query.ContainsKey("NOT")) {

                        if (pair.Value[0].Contains("id")) { 
                            postings_filtered.Select(n => n).Where(o => o.id.ToLower().Trim() != pair.Value[1].ToLower().Trim());
                        }
                        else if (pair.Value[0].Contains("title"))
                        {
                            postings_filtered.Where(o => o.title.ToLower().Trim() != pair.Value[1].ToLower().Trim()).Select(n => n);
                        }
                        else if (pair.Value[0].Contains("content"))
                        {
                            postings_filtered.Where(o => o.content.ToLower().Trim() != pair.Value[1].ToLower().Trim()).Select(n => n);
                        }
                        else if (pair.Value[0].Contains("views"))
                        {
                            postings_filtered.Where(o => o.view.ToString().ToLower().Trim() != pair.Value[1].ToLower().Trim()).Select(n => n);
                        }
                        else if (pair.Value[0].Contains("timestamp"))
                        {
                            postings_filtered.Where(o => o.timestamp.ToString().ToLower().Trim() != pair.Value[1].ToLower().Trim()).Select(n => n);
                        }
                    }
                    //if (query.ContainsKey("AND") {

                    //}
                    //if (query.ContainsKey("OR") {

                    //}

                    //if (query.ContainsKey("GREATER_THAN") {

                    //}
                    //if (query.ContainsKey("LESS_THAN") {

                    //}
                }
                catch(Exception e)
                {
                    Console.WriteLine("Failed to filter - " + e.Message);
                }
            }
            
            return JsonConvert.SerializeObject(postings_filtered.ToArray());
        }
        [HttpPost]
        public string Post(Posting model)
        {
            model.timestamp = DateTime.Now.Ticks;
            //check if id exists if so dont add update
            
            if(postings.Count == 0)
            {
                postings.Add(model);
            }
            foreach(var item in postings.ToList<Posting>())
            {
                if (item.id == model.id)
                {
                    item.title = model.title;
                    item.content = model.content;
                    return JsonConvert.SerializeObject(model);
                }
            }
            postings.Add(model);
            return JsonConvert.SerializeObject(model);
        }
    }
}
