namespace NinthWebApplication.Models
{
    public class WeatherForecastHolder
    {
        private List<WeatherForecast> _values;

        public WeatherForecastHolder()
        {
            _values = new List<WeatherForecast>();
        }

        public void Add(DateTime dateTime, int temperatureC)
        {
            WeatherForecast forecast = new WeatherForecast();
            forecast.TemperatureC = temperatureC;
            forecast.Date = dateTime;

            _values.Add(forecast);
        }
        public bool Update(DateTime date, int temperatureC)
        {
            foreach (WeatherForecast item in _values)
            {
                if (item.Date == date)
                {
                    item.TemperatureC = temperatureC;
                    return true;
                }
            }
            return false;
        }

        public List<WeatherForecast> Get(DateTime dateFrom, DateTime dateTo)
        { 
            List<WeatherForecast> list = new List<WeatherForecast>();
            foreach (WeatherForecast item in _values)
            {
                if (item.Date >= dateFrom && item.Date <= dateTo)
                {
                    list.Add(item);
                }
            }
                return list;
        }
        public bool Delete(DateTime date)
        {
            return false;
        }
    }
}