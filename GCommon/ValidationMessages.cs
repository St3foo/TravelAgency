namespace TravelAgency.GCommon
{
    public static class ValidationMessages
    {
        public static class Destination 
        {
            public const string NameIsRequerd = "Destination name is requred.";
            public const string NameMinLenght = "Name must be at least 3 characters.";
            public const string NameMaxLenght = "Name cannot exceed 60 characters.";

            public const string DescriptionIsRequerd = "Description is requred.";
            public const string DescriptionMinLenght = "Description must be at least 10 characters.";
            public const string DescriptionMaxLenght = "Description cannot exceed 500 characters.";
        }

        public static class Hotel
        {
            public const string HotelNameIsRequerd = "Hotel name is requred.";
            public const string HotelNameMinLenght = "Name must be at least 5 characters.";
            public const string HotelNameMaxLenght = "Name cannot exceed 100 characters.";

            public const string DescriptionIsRequerd = "Description is requred.";
            public const string DescriptionMinLenght = "Description must be at least 10 characters.";
            public const string DescriptionMaxLenght = "Description cannot exceed 500 characters.";

            public const string CityNameIsRequerd = "City name is requred.";
            public const string CityNameMinLenght = "Name must be at least 3 characters.";
            public const string CityNameMaxLenght = "Name cannot exceed 50 characters.";

            public const string DestinationIdIsRequered = "Destination is requred.";
        }

        public static class Landmark 
        {
            public const string NameIsRequerd = "Landmark name is requred.";
            public const string NameMinLenght = "Name must be at least 5 characters.";
            public const string NameMaxLenght = "Name cannot exceed 100 characters.";

            public const string DescriptionIsRequerd = "Description is requred.";
            public const string DescriptionMinLenght = "Description must be at least 10 characters.";
            public const string DescriptionMaxLenght = "Description cannot exceed 500 characters.";

            public const string LocationNameIsRequerd = "Location name is requred.";
            public const string LocationNameMinLenght = "Location name must be at least 3 characters.";
            public const string LocationNameMaxLenght = "Location name cannot exceed 50 characters.";

            public const string DestinationIdIsRequered = "Destination is requred.";
        }
    }
}
