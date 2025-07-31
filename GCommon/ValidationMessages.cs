namespace TravelAgency.GCommon
{
    public static class ValidationMessages
    {
        public static class Destination 
        {
            public const string NameIsRequerd = "Destination name is requred.";
            public const string NameMinLenghtRequired = "Name must be at least 3 characters.";
            public const string NameMaxLenghtRequired = "Name cannot exceed 60 characters.";

            public const string DescriptionIsRequerd = "Description is requred.";
            public const string DescriptionMinLenghtRequired = "Description must be at least 10 characters.";
            public const string DescriptionMaxLenghtRequired = "Description cannot exceed 500 characters.";
        }

        public static class Hotel
        {
            public const string HotelNameIsRequerd = "Hotel name is requred.";
            public const string HotelNameMinLenghtRequired = "Name must be at least 5 characters.";
            public const string HotelNameMaxLenghtRequired = "Name cannot exceed 100 characters.";

            public const string DescriptionIsRequerd = "Description is requred.";
            public const string DescriptionMinLenghtRequired = "Description must be at least 10 characters.";
            public const string DescriptionMaxLenghtRequired = "Description cannot exceed 500 characters.";

            public const string CityNameIsRequerd = "City name is requred.";
            public const string CityNameMinLenghtRequired = "Name must be at least 3 characters.";
            public const string CityNameMaxLenghtRequired = "Name cannot exceed 50 characters.";

            public const string DestinationIdIsRequered = "Destination is requred.";
        }

        public static class Landmark 
        {
            public const string NameIsRequerd = "Landmark name is requred.";
            public const string NameMinLenghtRequired = "Name must be at least 5 characters.";
            public const string NameMaxLenghtRequired = "Name cannot exceed 100 characters.";

            public const string DescriptionIsRequerd = "Description is requred.";
            public const string DescriptionMinLenghtRequired = "Description must be at least 10 characters.";
            public const string DescriptionMaxLenghtRequired = "Description cannot exceed 500 characters.";

            public const string LocationNameIsRequerd = "Location name is requred.";
            public const string LocationNameMinLenghtRequired = "Location name must be at least 3 characters.";
            public const string LocationNameMaxLenghtRequired = "Location name cannot exceed 50 characters.";

            public const string DestinationIdIsRequered = "Destination is requred.";
        }

        public static class Tour 
        {
            public const string NameIsRequerd = "Tour name is requred.";
            public const string NameMinLenghtRequired = "Name must be at least 3 characters.";
            public const string NameMaxLenghtRequired = "Name cannot exceed 100 characters.";

            public const string DescriptionIsRequerd = "Description is requred.";
            public const string DescriptionMinLenghtRequired = "Description must be at least 10 characters.";
            public const string DescriptionMaxLenghtRequired = "Description cannot exceed 500 characters.";
        }
    }
}
