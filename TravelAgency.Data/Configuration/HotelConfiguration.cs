using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;
using static TravelAgency.GCommon.ValidationConstants.Hotel;

namespace TravelAgency.Data.Configuration
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> entity)
        {
            entity
                .HasKey(h => h.Id);

            entity
                .Property(h => h.CityName)
                .IsRequired()
                .HasMaxLength(MaxLenghtCityName);

            entity
                .Property(h => h.HotelName)
                .IsRequired()
                .HasMaxLength(MaxLenghtHotelName);

            entity
                .Property(h => h.ImageUrl)
                .IsRequired(false);

            entity
                .Property(h => h.Description)
                .IsRequired()
                .HasMaxLength(MaxLenghtDescription);

            entity
                .Property(h => h.Price)
                .HasPrecision(18, 2);

            entity
                .Property(h => h.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(h => h.Destination)
                .WithMany(d => d.Hotels)
                .HasForeignKey(h => h.DestinationId);

            entity
                .HasQueryFilter(h => h.IsDeleted == false);

            entity
                .HasData(
                new Hotel 
                {
                    Id = Guid.Parse("f8e2efd0-0e54-4c6d-8582-3651fa46f7b3"),
                    CityName = "Tokyo",
                    HotelName = "Sheraton Grande",
                    ImageUrl = "https://cache.marriott.com/is/image/marriotts7prod/si-tyosi-tyosi-exterior-9518-28621:Wide-Hor?wid=1336&fit=constrain",
                    Description = "The Sheraton Grande Tokyo Bay Hotel is a luxurious urban resort hotel located near Tokyo Disneyland and Tokyo DisneySea.",
                    Price = 1200,
                    DaysStay = 5,
                    DestinationId = Guid.Parse("c304fab0-e8e7-4d06-9ffb-607c502be4e6")
                },
                new Hotel
                {
                    Id = Guid.Parse("873c1c6c-c0aa-4a3c-b7fc-fc93cb31254b"),
                    CityName = "Las Vegas",
                    HotelName = "MGM Grand",
                    ImageUrl = "https://thelibrary.mgmresorts.com/transform/M2h6tISLUZ61/MGM103112227.tif",
                    Description = "The MGM Grand Las Vegas is a massive hotel and casino resort located on the Las Vegas Strip, known for its size, entertainment options, and vibrant atmosphere. It boasts a huge casino floor, numerous restaurants, a large pool complex, and is home to the MGM Grand Garden Arena, a popular venue for concerts and sporting events.",
                    Price = 1250,
                    DaysStay = 5,
                    DestinationId = Guid.Parse("bba3d9da-4843-448e-b0f0-ba9c60e91b08")
                },
                new Hotel
                {
                    Id = Guid.Parse("d857605f-1a16-4539-afaf-5e42f604413b"),
                    CityName = "Cairo",
                    HotelName = "Fairmont Nile City",
                    ImageUrl = "https://m.ahstatic.com/is/image/accorhotels/aja_p_5280-04:8by10?fmt=jpg&op_usm=1.75,0.3,2,0&resMode=sharp2&iccEmbed=true&icc=sRGB&dpr=on,1.5&wid=335&hei=418&qlt=80",
                    Description = "The hotel features mainly Art Deco and contemporary design features within its overall interior decoration style. It is situated between the two towers of the Nile City Complex. It is also located about 23 km away from tourist attractions including the Giza pyramid complex.",
                    Price = 1070,
                    DaysStay = 7,
                    DestinationId = Guid.Parse("f2af18bd-0c37-45dd-b545-3ff2beea2473")
                },
                new Hotel
                {
                    Id = Guid.Parse("eb99a8e9-1f43-4cf9-bb47-b398177bfc73"),
                    CityName = "Paris",
                    HotelName = "Baume",
                    ImageUrl = "https://live.staticflickr.com/65535/52533798631_1a273b2e86_c_d.jpg",
                    Description = "Hotel Baume is a 4-star, 35-room boutique hotel in Paris with a 1930s Art Deco design, located in the Saint-Germain-des-Prés neighborhood.",
                    Price = 900,
                    DaysStay = 7,
                    DestinationId = Guid.Parse("530d919a-fd28-43f9-89e0-8fc5bd00f3fe")
                },
                new Hotel
                {
                    Id = Guid.Parse("e27408c7-41dc-4504-b9c9-79f1cfe92f83"),
                    CityName = "Rome",
                    HotelName = "Artemide",
                    ImageUrl = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/0a/96/aa/1f/exterior-hotel-facade.jpg?w=900&h=500&s=1",
                    Description = "Hotel Artemide is a 4-star hotel located in the heart of Rome, known for its central location and elegant accommodations. It features a rooftop terrace, a spa, and an Italian cuisine restaurant.",
                    Price = 1100,
                    DaysStay = 5,
                    DestinationId = Guid.Parse("7060a6cc-6942-4346-b2c0-0011337cbaff")
                },
                new Hotel
                {
                    Id = Guid.Parse("35b584ce-0897-44af-b86a-aa0401abc87e"),
                    CityName = "Hilo",
                    HotelName = "Hilo Hawaiian Hotel",
                    ImageUrl = "https://www.hilohawaiian.com/wp-content/uploads/2024/11/Hilo-Hawaiian-008-copy-1030x687.jpg",
                    Description = "The Hilo Hawaiian Hotel, an oceanfront resort on Hilo Bay, offers a blend of authentic Hawaiian charm and modern amenities.",
                    Price = 1600,
                    DaysStay = 5,
                    DestinationId = Guid.Parse("75779581-666c-446d-a9a8-5b30743d6738")
                },
                new Hotel
                {
                    Id = Guid.Parse("a21e6f7e-009a-4202-be2e-abdaed81df7e"),
                    CityName = "Madrid",
                    HotelName = "Catalonia Goya",
                    ImageUrl = "https://storage.googleapis.com/app-engine-imagenes-pro/pro/styles/talla_uno/cloud-storage/2024-09/Hero_10.jpg.webp",
                    Description = "Hotel Catalonia Goya is a 4-star hotel located in the Salamanca district of Madrid. It's situated in a 19th-century building and offers modern, comfortable rooms with wooden floors and elegant dark furniture.",
                    Price = 1050,
                    DaysStay = 5,
                    DestinationId = Guid.Parse("1d29ae99-3840-4ae9-aee7-ad0d4dd94cff")
                },
                new Hotel
                {
                    Id = Guid.Parse("64459ca2-a1fc-475f-a51b-87bb20a66e01"),
                    CityName = "Santorini",
                    HotelName = "Katikies Santorini",
                    ImageUrl = "https://cms.inspirato.com/ImageGen.ashx?image=%2Fmedia%2F9396609%2Fsantorini-greece_katikies-hotel_exterior2.jpg&width=1081.5",
                    Description = "Katikies Santorini is a luxury hotel located on the cliffs of Oia, Santorini, Greece. It's renowned for its stunning views of the caldera and the Aegean Sea, and its unique, cubist-style architecture integrated into the cliffside. ",
                    Price = 2000,
                    DaysStay = 10,
                    DestinationId = Guid.Parse("0e0e2740-9aa7-4316-bc76-5ac57c91ab0d")
                },
                new Hotel
                {
                    Id = Guid.Parse("4e01fe6f-7ca9-4d5d-a13b-ca513d490a8e"),
                    CityName = "Rio de Janeiro",
                    HotelName = "The Fairmont",
                    ImageUrl = "https://assets.simpleviewinc.com/simpleview/image/upload/c_limit,q_75,w_1200/v1/crm/iglta/111988_9c002b48-5056-854c-b6bdad1a03141233.jpg",
                    Description = "The Fairmont Rio de Janeiro Copacabana is a 5-star beachfront hotel located on Copacabana Beach in Rio de Janeiro, Brazil. It offers stunning views of the beach, Sugarloaf Mountain, and Rodrigo de Freitas Lagoon.",
                    Price = 1750,
                    DaysStay = 7,
                    DestinationId = Guid.Parse("bc2131b6-9529-40c5-a963-628410fa0c41")
                },
                new Hotel
                {
                    Id = Guid.Parse("2f7a05d6-d490-4f67-8c3b-ada3127ea585"),
                    CityName = "New Delhi",
                    HotelName = "Leela Palace",
                    ImageUrl = "https://5.imimg.com/data5/VM/JJ/GLADMIN-9169594/the-leela-palace-5-star-luxury-hotel.jpg",
                    Description = "The Leela Palaces, Hotels and Resorts is an Indian luxury hotel chain known for its opulent properties that blend traditional Indian aesthetics with modern comforts.",
                    Price = 1800,
                    DaysStay = 7,
                    DestinationId = Guid.Parse("2127672f-0baf-4033-9279-7aa70698dca4")
                }
                );
        }
    }
}
