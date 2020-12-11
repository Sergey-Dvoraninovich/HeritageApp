using System;

namespace HeritageWebApplication.Services
{
    public class InfoService : IInfoService
    {
        private DateTime _time;

        public InfoService()
        {
            _time = DateTime.Now;
        }
        public DateTime StartTime()
        {
            return _time;
        }
    }
}