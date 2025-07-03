using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;
using static TravelAgency.GCommon.ValidationConstants.Destination;

namespace TravelAgency.Data.Configuration
{
    public class DestinationConfiguration : IEntityTypeConfiguration<Destination>
    {
        public void Configure(EntityTypeBuilder<Destination> entity)
        {
            entity
                .HasKey(d => d.Id);

            entity
                .Property(d => d.CountryName)
                .IsRequired()
                .HasMaxLength(MaxLenghtCountryName);

            entity
                .Property(d => d.Description)
                .IsRequired()
                .HasMaxLength(MaxLenghtDescription);

            entity
                .Property(d => d.ImageUrl)
                .IsRequired(false);

            entity
                .Property(d => d.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasQueryFilter(d => d.IsDeleted == false);

            entity
                .HasData(
                new Destination
                {
                    Id = Guid.Parse("c304fab0-e8e7-4d06-9ffb-607c502be4e6"),
                    CountryName = "Japan",
                    ImageUrl = "https://www.smartmeetings.com/wp-content/uploads/2018/03/japan-1.jpg",
                    Description = "Japan is a captivating blend of ancient traditions and modern innovation, offering diverse experiences for travelers. From bustling cityscapes like Tokyo and Osaka to serene temples and stunning natural landscapes, there's something for everyone. Visitors can explore historic castles and shrines, indulge in delicious cuisine, and experience the unique culture of Japan."
                },
                new Destination 
                {
                    Id = Guid.Parse("bba3d9da-4843-448e-b0f0-ba9c60e91b08"),
                    CountryName = "United States of America",
                    ImageUrl = "https://miro.medium.com/v2/resize:fit:800/1*93v0YEC9A7Q8-8PxPwrstA.jpeg",
                    Description = "A tourist in the USA can experience a vast range of attractions, from natural wonders like national parks and coastlines to vibrant cities and diverse cultural experiences. The country offers something for every type of traveler, including those interested in history, food, outdoor adventures, and the arts."
                },
                new Destination 
                {
                    Id = Guid.Parse("f2af18bd-0c37-45dd-b545-3ff2beea2473"),
                    CountryName = "Egypt",
                    ImageUrl = "https://www.traveltalktours.com/wp-content/smush-webp/2023/02/Great-Sphinx-of-Giza.jpeg.webp",
                    Description = "Egypt is a captivating country known for its rich ancient history, vibrant culture, and stunning natural landscapes. Tourists are drawn to explore the iconic pyramids and temples, cruise along the Nile River, and relax on the beaches of the Red Sea. The country offers a blend of historical exploration, cultural immersion, and opportunities for adventure and relaxation."
                },
                new Destination 
                {
                    Id = Guid.Parse("530d919a-fd28-43f9-89e0-8fc5bd00f3fe"),
                    CountryName = "France",
                    ImageUrl = "https://res.klook.com/image/upload/q_85/c_fill,w_750/v1718112298/klyzxawxgytpixrvsgem.jpg",
                    Description = "France, the world's most visited country, offers a rich tapestry of experiences for tourists, from iconic landmarks and vibrant cities to picturesque countryside and stunning coastlines. It's a land of diverse landscapes, from the lavender fields of Provence to the snowy peaks of the Alps, and boasts a wealth of historical sites, including numerous UNESCO World Heritage sites. "
                },
                new Destination 
                {
                    Id = Guid.Parse("7060a6cc-6942-4346-b2c0-0011337cbaff"),
                    CountryName = "Italy",
                    ImageUrl = "https://imgcld.yatra.com/ytimages/image/upload/t_holidays_srplist_tablet_hc/v5879035672/Holidays/Greenland/Venice_ycTyfo.jpg",
                    Description = "Italy is a popular tourist destination known for its rich history, art, architecture, cuisine, and diverse landscapes. Visitors are drawn to iconic cities like Rome, Florence, and Venice, as well as the scenic beauty of the Amalfi Coast, Lake Como, and the Italian Alps. Italy also boasts a vibrant culture, delicious food, and friendly people, making it a captivating destination for a wide range of travelers."
                },
                new Destination 
                {
                    Id = Guid.Parse("75779581-666c-446d-a9a8-5b30743d6738"),
                    CountryName = "Hawaii",
                    ImageUrl = "https://www.acg.aaa.com/content/contenthub/us/en/blogs/4c/travel/say-aloha-to-your-ultimate-hawaii-travel-guide/_jcr_content/root/container/container/blog-details/image.coreimg.jpeg/1734336851769/hero-ultimate-hawaii-travel-guide-seo.jpeg",
                    Description = "Hawaii, known as the Aloha State, is a captivating destination celebrated for its stunning natural beauty, vibrant culture, and unique experiences. It offers diverse landscapes, from volcanic wonders and lush rainforests to golden beaches and dramatic canyons. Visitors can explore volcanic national parks, hike to breathtaking viewpoints, enjoy world-class surfing, and immerse themselves in the rich Polynesian heritage."
                },
                new Destination 
                {
                    Id = Guid.Parse("1d29ae99-3840-4ae9-aee7-ad0d4dd94cff"),
                    CountryName = "Spain",
                    ImageUrl = "https://www.connollycove.com/wp-content/uploads/2023/05/Depositphotos_171005292_XL.webp",
                    Description = "Spain is a vibrant and diverse country, offering a rich tapestry of cultural experiences, stunning landscapes, and delicious cuisine. From bustling cities like Madrid and Barcelona to the sun-kissed beaches of the Mediterranean coast, Spain caters to a wide range of travel preferences. Its history is evident in its architecture, with influences from Roman, Moorish, and Christian cultures, leaving behind iconic landmarks like the Alhambra and numerous cathedrals."
                },
                new Destination 
                {
                    Id = Guid.Parse("0e0e2740-9aa7-4316-bc76-5ac57c91ab0d"),
                    CountryName = "Greece",
                    ImageUrl = "https://www.cuddlynest.com/blog/wp-content/uploads/2022/10/top-attractions-in-greece-athens.jpg",
                    Description = "Greece is a popular tourist destination known for its stunning landscapes, ancient ruins, vibrant culture, and delicious cuisine. The country offers a mix of experiences, from exploring historical sites like the Acropolis in Athens to relaxing on beautiful beaches in the Greek islands. Greece is also famous for its culinary delights, with fresh ingredients and flavorful dishes like moussaka and souvlaki."
                },
                new Destination 
                {
                    Id = Guid.Parse("bc2131b6-9529-40c5-a963-628410fa0c41"),
                    CountryName = "Brazil",
                    ImageUrl = "https://www.mappr.co/wp-content/uploads/2021/04/image-132.jpeg",
                    Description = "Brazil, the largest country in South America, is a vibrant land known for its diverse landscapes, rich culture, and lively atmosphere. From the iconic beaches of Rio de Janeiro and the lush Amazon rainforest to the historic cities and vibrant festivals, Brazil offers a unique and unforgettable travel experience."
                },
                new Destination
                {
                    Id = Guid.Parse("2127672f-0baf-4033-9279-7aa70698dca4"),
                    CountryName = "India",
                    ImageUrl = "https://static.investindia.gov.in/s3fs-public/2023-06/1.jpg",
                    Description = "India, a vibrant and diverse country, is a popular tourist destination known for its stunning natural wonders, rich cultural heritage, and historical sites. From bustling cities to serene backwaters, India offers a wide range of experiences for every traveler. Its unique blend of ancient traditions and modern life, combined with warm hospitality, makes it an unforgettable destination."
                }
                );
        }
    }
}
