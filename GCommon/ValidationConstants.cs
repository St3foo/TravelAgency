namespace TravelAgency.GCommon
{
    public static class ValidationConstants
    {
        public static class Destination
        {
            public const int MinLenghtCountryName = 3;
            public const int MaxLenghtCountryName = 60;

            public const int MinLenghtDescription = 10;
            public const int MaxLenghtDescription = 500;
        }

        public static class Hotel
        {
            public const int MinLenghtCityName = 3;
            public const int MaxLenghtCityName = 50;

            public const int MinLenghtHotelName = 5;
            public const int MaxLenghtHotelName = 100;

            public const int MinLenghtDescription = 10;
            public const int MaxLenghtDescription = 500;
        }

        public static class Landmark
        {
            public const int MinLenghtLocation = 3;
            public const int MaxLenghtLocation = 50;

            public const int MinLenghtName = 5;
            public const int MaxLenghtName = 100;

            public const int MinLenghtDescription = 10;
            public const int MaxLenghtDescription = 500;
        }

        public static class Tour 
        {
            public const int MinLenghtTourName = 3;
            public const int MaxLenghtTourName = 100;

            public const int MinLenghtDescription = 10;
            public const int MaxLenghtDescription = 500;
        }
    }
}

