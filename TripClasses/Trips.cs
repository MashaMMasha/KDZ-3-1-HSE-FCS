using System.Collections.Generic;

namespace TripClasses
{
    public class Trips
    {
        // Объявляем поля класса.
        private string _tripId;
        private string _destination;
        private string _startDate;
        private string _endDate;
        private List<string> _travelers;
        private string _accommodation;
        private List<string> _activities;

        // По ТЗ они должны быть открыты только для чтения поэтому сеттеры не пишем.
        public string TripId
        {
            get => _tripId;
        }
        
        public string Destination
        {
            get => _destination;
        }
        
        public string StartDate
        {
            get => _startDate;
        }
        
        public string EndDate
        {
            get => _endDate;
        }

        public List<string> Travelers
        {
            get => _travelers;
        }
        
        public string Accommodation
        {
            get => _accommodation;
        }
        
        public List<string> Activities
        {
            get => _activities;
        }
        
        // Созадем пустой конструктор.
        public Trips()
        {
            _tripId = "";
            _destination = "";
            _startDate = "";
            _endDate = "";
            _travelers = new List<string>();
            _accommodation = "";
            _activities = new List<string>();
        }

        // Конструктор с параметрами.
        public Trips(string tripId, string destination, string startDate, string endDate, 
            List<string> travelers, string accommodation, List<string> activities)
        {
            _tripId = tripId;
            _destination = destination;
            _startDate = startDate;
            _endDate = endDate;
            _travelers = travelers;
            _accommodation = accommodation;
            _activities = activities;
        }
    }
}