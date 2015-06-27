using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Demo.Api.Controllers
{
    public class Favorite
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public int Stars { get; set; }
    }

    [RoutePrefix("favorites")]
    public class FavoritesApiController : AuthorizedApiController
    {
        [Route]
        [HttpGet]
        public IHttpActionResult Get()
        {
            Thread.Sleep(1500); //intentially slow this down for demo purposes
            return Ok(new List<Favorite>
            {
                new Favorite{Id = 0, Link = "http://www.candywarehouse.com/brands/wonka-candy", Stars = 2, Title = "Buy Candy Online"},
                new Favorite{Id = 1, Link = "https://news.google.com", Stars = 5, Title = "Latest News"},
                new Favorite{Id = 2, Link = "http://www.nps.gov/grca/index.htm", Stars = 5, Title = "Grand Canyon Tours"},
                new Favorite{Id = 3, Link = "https://www.netflix.com/", Stars = 4, Title = "Watch Movies"},
                new Favorite{Id = 4, Link = "http://www.nhl.com", Stars = 2, Title = "Hockey"},
                new Favorite{Id = 5, Link = "http://www.cigarsinternational.com", Stars = 4, Title = "Cigars"}
            }.OrderBy(f => f.Stars));
        }
    }
}
