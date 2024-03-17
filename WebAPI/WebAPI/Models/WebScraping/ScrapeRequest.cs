using WebAPI.Enums;

namespace WebAPI.Models.WebScraping
{
    public class ScrapeRequest (ScrapeRequestType type, int days)
    {
        public ScrapeRequestType Type => type;

        public int Days => days;
    }
}
