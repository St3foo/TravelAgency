using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.Data.Models;
using static TravelAgency.GCommon.ValidationConstants.Landmark;

namespace TravelAgency.Data.Configuration
{
    public class LandmarkConfiguration : IEntityTypeConfiguration<Landmark>
    {
        public void Configure(EntityTypeBuilder<Landmark> entity)
        {
            entity
                .HasKey(l => l.Id);

            entity
                .Property(l => l.LocationName)
                .IsRequired()
                .HasMaxLength(MaxLenghtLocation);

            entity
                .Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(MaxLenghtName);

            entity
                .Property(l => l.Description)
                .IsRequired()
                .HasMaxLength(MaxLenghtDescription);

            entity
                .Property(l => l.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(l => l.Destination)
                .WithMany(d => d.Landmarks)
                .HasForeignKey(l => l.DestinationId);

            entity
                .Property(l => l.ImageUrl)
                .IsRequired(false);

            entity
                .HasQueryFilter(l => l.IsDeleted == false);

            entity
                .HasData(
                new Landmark
                {
                    Id = Guid.Parse("5a883558-99eb-4c1a-8250-fcd2af1049bf"),
                    LocationName = "Kyoto",
                    Name = "Fushimi Inari Taisha",
                    ImageUrl = "https://dskyoto.s3.amazonaws.com/gallery/full/8514/5559/7797/08-20131216_FushimiInari_Mainspot-307.jpg",
                    Description = "Fushimi Inari-taisha, located in Kyoto, Japan, is a prominent Shinto shrine dedicated to Inari, the god of rice and prosperity. It is famous for its thousands of vibrant vermilion torii gates that wind up a mountain path, creating a mesmerizing tunnel-like effect. The shrine is the head shrine for all Inari shrines in Japan and is a popular destination for both tourists and locals. ",
                    DestinationId = Guid.Parse("c304fab0-e8e7-4d06-9ffb-607c502be4e6")
                },
                new Landmark
                {
                    Id = Guid.Parse("26d2d910-86f5-419a-a88a-a5b57206a72b"),
                    LocationName = "Island of Honshu",
                    Name = "Mount Fuji",
                    ImageUrl = "https://www.beautifulworld.com/wp-content/uploads/2020/11/Mount-Fuji-Japan.jpg",
                     Description = "Mount Fuji, also known as Fujisan, is Japan's highest mountain and a globally recognized symbol of the country. It's a dormant stratovolcano with a near-perfect symmetrical cone shape, often capped with snow. Rising to 3,776 meters (12,389 feet), it's a popular destination for climbers and a source of artistic inspiration.",
                    DestinationId = Guid.Parse("c304fab0-e8e7-4d06-9ffb-607c502be4e6")
                },
                new Landmark
                {
                    Id = Guid.Parse("f87bbd97-a72e-47ff-a03f-487d07394e48"),
                    LocationName = "Kyoto",
                    Name = "Arashiyama Bamboo Forest",
                    ImageUrl = "https://arashiyamabambooforest.com/wp-content/uploads/2024/09/arashiyama-bamboo-forest-sagano-kyoto-dense-towering-grove-1.jpeg",
                    Description = "The Arashiyama Bamboo Forest, also known as Sagano Bamboo Forest, is a captivating natural attraction in Kyoto, Japan, renowned for its towering bamboo stalks and serene atmosphere. Visitors can stroll along pathways that wind through the grove, experiencing the unique sights and sounds of the rustling bamboo leaves.",
                    DestinationId = Guid.Parse("c304fab0-e8e7-4d06-9ffb-607c502be4e6")
                },
                new Landmark
                {
                    Id = Guid.Parse("257877c3-1bf9-4da2-bd67-134c7aaebe9b"),
                    LocationName = "Keystone",
                    Name = "Mount Rushmore",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/87/Mount_Rushmore_detail_view_%28100MP%29.jpg/1200px-Mount_Rushmore_detail_view_%28100MP%29.jpg",
                    Description = "Mount Rushmore is a massive sculpture carved into the granite face of Mount Rushmore in the Black Hills of South Dakota. It features the 60-foot-high carved faces of four U.S. presidents: George Washington, Thomas Jefferson, Theodore Roosevelt, and Abraham Lincoln. The monument, known as the \"Shrine of Democracy\", symbolizes the birth, growth, development, and preservation of the United States.",
                    DestinationId = Guid.Parse("bba3d9da-4843-448e-b0f0-ba9c60e91b08")
                },
                new Landmark
                {
                    Id = Guid.Parse("847d8970-36dd-40da-af65-6f0d19162eb7"),
                    LocationName = "Ontario",
                    Name = "Niagara Falls",
                    ImageUrl = "https://cdn.britannica.com/30/94430-050-D0FC51CD/Niagara-Falls.jpg",
                    Description = "Niagara Falls is a collective name for a group of three waterfalls located on the Niagara River, straddling the border between Ontario, Canada, and New York, USA.",
                    DestinationId = Guid.Parse("bba3d9da-4843-448e-b0f0-ba9c60e91b08")
                },
                new Landmark
                {
                    Id = Guid.Parse("c61d4179-3893-43f4-bdbd-0df0d5d1a777"),
                    LocationName = "New York",
                    Name = "Statue of Liberty",
                    ImageUrl = "https://cdn-imgix.headout.com/tour/30357/TOUR-IMAGE/6cdcf542-452d-4897-beed-76cf68f154e4-1act-de005e04-05d9-4715-96b0-6a089d5c3460.jpg?auto=format&w=1222.3999999999999&h=687.6&q=90&ar=16%3A9&crop=faces&fit=crop",
                    Description = "The Statue of Liberty is a colossal neoclassical sculpture on Liberty Island in New York Harbor. It depicts Libertas, the Roman goddess of liberty, as a woman holding a torch in her right hand and a tablet inscribed with the date of the American Declaration of Independence (July 4, 1776) in her left. The statue, a gift from France to the United States, is a symbol of freedom and democracy. ",
                    DestinationId = Guid.Parse("bba3d9da-4843-448e-b0f0-ba9c60e91b08")
                },
                new Landmark
                {
                    Id = Guid.Parse("e896a347-9756-43b4-9791-dca8c5aeb84b"),
                    LocationName = "Egypt",
                    Name = "Piramids of Giza",
                    ImageUrl = "https://cdn-imgix.headout.com/media/images/e3e4b92772a00bf08922a79dd5a874d7-Giza.jpg",
                    Description = "The Giza pyramid complex (also called the Giza necropolis) in Egypt is home to the Great Pyramid, the pyramid of Khafre, and the pyramid of Menkaure, along with their associated pyramid complexes and the Great Sphinx. All were built during the Fourth Dynasty of the Old Kingdom of ancient Egypt, between c. 2600 – c.",
                    DestinationId = Guid.Parse("f2af18bd-0c37-45dd-b545-3ff2beea2473")
                },
                new Landmark
                {
                    Id = Guid.Parse("8273a188-b39f-44ca-a663-33ba6a262083"),
                    LocationName = "Luxor",
                    Name = "Karnak",
                    ImageUrl = "https://images.memphistours.com/xlarge/839487522_Karnak%20Temple.jpg",
                    Description = "Karnak, located in Luxor, Egypt, is the largest religious complex in the world, dedicated to the Theban Triad of Amun, Mut, and Khonsu. Construction began in the Middle Kingdom and continued into the Ptolemaic period, with most of the current structures dating to the New Kingdom. The site is known for its colossal statues, towering columns, and vast Hypostyle Hall. ",
                    DestinationId = Guid.Parse("f2af18bd-0c37-45dd-b545-3ff2beea2473")
                },
                new Landmark
                {
                    Id = Guid.Parse("e1264931-2355-4271-a612-398ca2f84b15"),
                    LocationName = "Luxor",
                    Name = "Valley of the Kings",
                    ImageUrl = "https://www.cleopatraegypttours.com/wp-content/uploads/2020/01/Valley-of-the-Kings.jpg",
                    Description = "The Valley of the Kings is a historical site in Egypt, known as the burial ground for many pharaohs and powerful nobles of the New Kingdom period (1539-1075 BC). It is located on the west bank of the Nile River, opposite Luxor, and is characterized by its numerous rock-cut tombs, some adorned with intricate hieroglyphs and vivid decorations. ",
                    DestinationId = Guid.Parse("f2af18bd-0c37-45dd-b545-3ff2beea2473")
                },
                new Landmark
                {
                    Id = Guid.Parse("4b8e0894-f680-4fa7-bfdb-ac92f3061788"),
                    LocationName = "Paris",
                    Name = "Eiffel Tower",
                    ImageUrl = "https://planetrail.co.uk/wp-content/uploads/Eiffel-Tower-Paris-resized.jpg",
                    Description = "The Eiffel Tower, a wrought iron lattice tower, is a globally recognized symbol of Paris and a testament to engineering and architectural boldness. Standing at 330 meters (1,083 feet) tall, including the antenna, it was the tallest structure in the world for over four decades. The tower features three levels for visitors, offering panoramic views of the city, restaurants, and shops.",
                    DestinationId = Guid.Parse("530d919a-fd28-43f9-89e0-8fc5bd00f3fe")
                },
                new Landmark
                {
                    Id = Guid.Parse("f1bb2a61-7d0a-4ebc-b37e-c8ee0caeaaea"),
                    LocationName = "Paris",
                    Name = "Louvre",
                    ImageUrl = "https://www.franceguide.info/wp-content/uploads/sites/18/paris-louvre-pyramid-hd.jpg",
                    Description = "The Louvre, or Musée du Louvre, is one of the world's largest and most famous art museums, located in Paris, France. Originally a fortress and then a royal palace, it now houses a vast collection of art and artifacts, spanning various historical periods and cultures. The museum is renowned for its iconic masterpieces like the Mona Lisa and the Venus de Milo.",
                    DestinationId = Guid.Parse("530d919a-fd28-43f9-89e0-8fc5bd00f3fe")
                },
                new Landmark
                {
                    Id = Guid.Parse("4a656a15-efcd-43eb-88f9-a2c24e3bd820"),
                    LocationName = "Paris",
                    Name = "Arc de Triomphe",
                    ImageUrl = "https://media.cntraveler.com/photos/5a93281d8087c02669a7dc07/16:9/w_2560,c_limit/Arc%20de%20Triomphe_GettyImages-615063063.jpg",
                    Description = "The Arc de Triomphe, located in Paris, is a monument commissioned by Napoleon Bonaparte in 1806 to honor his military victories. Inspired by ancient Roman arches, it stands as a symbol of French military prowess and national unity. The monument features intricate sculptures, inscriptions of battles and generals, and the Tomb of the Unknown Soldier.",
                    DestinationId = Guid.Parse("530d919a-fd28-43f9-89e0-8fc5bd00f3fe")
                },
                new Landmark
                {
                    Id = Guid.Parse("0a5bd069-b91b-49f7-acfe-2f0df5985028"),
                    LocationName = "Rome",
                    Name = "Colosseum",
                    ImageUrl = "https://res.cloudinary.com/dtljonz0f/image/upload/c_auto,ar_4:3,w_3840,g_auto/f_auto/q_auto/v1/roman-coliseum-under-clouds-sunset-summer?_a=BAVAZGE70",
                    Description = "The Colosseum, also known as the Flavian Amphitheatre, is a massive elliptical structure in Rome, Italy, famous for its historical significance as a venue for gladiatorial contests and public spectacles. Built with travertine stone, tuff, and brick-faced concrete, it's the largest amphitheater ever constructed, with an oval shape measuring 189 meters long and 156 meters wide.",
                    DestinationId = Guid.Parse("7060a6cc-6942-4346-b2c0-0011337cbaff")
                },
                new Landmark
                {
                    Id = Guid.Parse("6824566a-9f3d-406e-b74a-f49f8aab1196"),
                    LocationName = "Pisa",
                    Name = "Leaning Tower of Pisa",
                    ImageUrl = "https://static.toiimg.com/photo/112466783.cms",
                    Description = "The Leaning Tower of Pisa is a renowned bell tower (campanile) in Pisa, Italy, notable for its unintended tilt caused by unstable ground. It's a cylindrical, eight-story structure of white marble, part of the Pisa Cathedral complex, and features colonnaded balconies and arches.",
                    DestinationId = Guid.Parse("7060a6cc-6942-4346-b2c0-0011337cbaff")
                },
                new Landmark
                {
                    Id = Guid.Parse("b4a25684-2851-44c5-bcde-b488530526a1"),
                    LocationName = "Venice",
                    Name = "Rialto Bridge",
                    ImageUrl = "https://csengineermag.com/wp-content/uploads/2022/11/AdobeStock_232079420.jpeg",
                    Description = "The Rialto Bridge, or Ponte di Rialto, is a historic stone arch bridge in Venice, Italy, spanning the Grand Canal. It's the oldest bridge across the canal and a major tourist attraction. Built between 1588 and 1591, it replaced earlier wooden structures and is known for its single-span design, arcades, and shops. ",
                    DestinationId = Guid.Parse("7060a6cc-6942-4346-b2c0-0011337cbaff")
                },
                new Landmark
                {
                    Id = Guid.Parse("2a9bd06c-b5e6-4b86-a412-49f2f2c1873e"),
                    LocationName = "Honolulu",
                    Name = "Waikiki Beach",
                    ImageUrl = "https://www.alamoanahotelhonolulu.com/Portals/alamoanahotelhonolulu.com/Images/HeroImages/waikiki-beach_1920x720.jpg?ver=2018-09-07-111100-177",
                    Description = "Waikiki Beach is a world-famous, two-mile stretch of white sand in Honolulu, Hawaii, known for its vibrant atmosphere and beautiful turquoise waters. It's a popular destination for swimming, surfing, and sunbathing, and is lined with palm trees, high-rise hotels, and resorts.",
                    DestinationId = Guid.Parse("75779581-666c-446d-a9a8-5b30743d6738")
                },
                new Landmark
                {
                    Id = Guid.Parse("cc77e510-98d0-4ef9-967e-1b5f04e6a9c1"),
                    LocationName = "Kailua",
                    Name = "Lanikai Beach",
                    ImageUrl = "https://loveoahu.org/wp-content/uploads/Lanikai-beach-mokes.jpg",
                    Description = "Lanikai Beach, also known as Ka'ōhao Beach, is a world-renowned beach on the windward coast of Oahu, Hawaii, known for its soft, white sand and calm, turquoise waters. It's often ranked as one of the best beaches in the world and is celebrated for its stunning views of the Mokulua Islands.",
                    DestinationId = Guid.Parse("75779581-666c-446d-a9a8-5b30743d6738")
                },
                new Landmark
                {
                    Id = Guid.Parse("85bdc262-b92f-440e-be00-b753edfb2d3c"),
                    LocationName = "Honolulu",
                    Name = "Hanauma Bay",
                    ImageUrl = "https://www.hawaiiactivities.com/travelguide/wp-content/uploads/Hawaii_Oahu_Hanauma-Bay_shutterstock_2376889275.jpg",
                    Description = "Hanauma Bay is a protected marine embayment on the southeast coast of Oʻahu, Hawaii, formed within a tuff ring of a volcanic cone. It's renowned for its vibrant coral reef and abundant marine life, making it a popular destination for snorkeling and swimming. ",
                    DestinationId = Guid.Parse("75779581-666c-446d-a9a8-5b30743d6738")
                },
                new Landmark
                {
                    Id = Guid.Parse("b2c81222-382e-4266-9ebc-488bba633004"),
                    LocationName = "Barcelona",
                    Name = "Sagrada Família",
                    ImageUrl = "https://media.tacdn.com/media/attractions-splice-spp-674x446/0b/e1/0a/1e.jpg",
                    Description = "The Sagrada Familia, also known as the Basilica i Temple Expiatori de la Sagrada Família, is a large, unfinished Roman Catholic church in Barcelona, Spain, designed by Antoni Gaudí. It is a UNESCO World Heritage Site and an iconic symbol of Barcelona.",
                    DestinationId = Guid.Parse("1d29ae99-3840-4ae9-aee7-ad0d4dd94cff")
                },
                new Landmark
                {
                    Id = Guid.Parse("acf38f44-653f-48d0-9c3f-76b7da78fcf3"),
                    LocationName = "Palma de Mallorca",
                    Name = "La Seu",
                    ImageUrl = "https://exoticholiday.bg/img/POBEKTI/BIG_shutterstock_15288900951170.jpg",
                    Description = "Palma Cathedral, also known as La Seu, is a magnificent Gothic Roman Catholic cathedral located in Palma de Mallorca, Spain. It is a prominent landmark overlooking the city and the sea, known for its impressive size, intricate architecture, and historical significance.",
                    DestinationId = Guid.Parse("1d29ae99-3840-4ae9-aee7-ad0d4dd94cff")
                },
                new Landmark
                {
                    Id = Guid.Parse("19670860-5d97-4e3e-a7a4-e79107e4b948"),
                    LocationName = "Island of Ibiza",
                    Name = "Ibiza",
                    ImageUrl = "https://www.invisahoteles.com/uploads/cms_apps/imagenes/san_antoni.png",
                    Description = "Ibiza, also known as Eivissa in Catalan, is a Spanish island in the Balearic Islands archipelago, renowned for its vibrant nightlife and stunning beaches. It's a popular tourist destination, attracting visitors with its unique blend of cosmopolitan atmosphere, natural beauty, and historical sites like Dalt Vila, a UNESCO World Heritage site.",
                    DestinationId = Guid.Parse("1d29ae99-3840-4ae9-aee7-ad0d4dd94cff")
                },
                new Landmark
                {
                    Id = Guid.Parse("e8ef0ae9-80b9-414a-8851-23fd5e9a7625"),
                    LocationName = "Athens",
                    Name = "Acropolis of Athens",
                    ImageUrl = "https://cdn.britannica.com/99/255999-159-73560D25/Parthenon-temple-at-the-Acropolis-of-Athens-Greece-built-5th-century-BC.jpg",
                    Description = "The Acropolis of Athens is an ancient citadel located on a rocky hill above the city. It's famous for its collection of historical buildings, including the Parthenon, and is considered a symbol of classical Greek civilization and the birthplace of Western civilization.",
                    DestinationId = Guid.Parse("0e0e2740-9aa7-4316-bc76-5ac57c91ab0d")
                },
                new Landmark
                {
                    Id = Guid.Parse("9318ddf7-c4f4-4deb-9480-82566d12da56"),
                    LocationName = "Trikala",
                    Name = "Meteora",
                    ImageUrl = "https://siskata.com/wp-content/uploads/2013/07/DSC_0181.jpg",
                    Description = "Meteora, meaning \"suspended in the air\", is a unique rock formation in central Greece, famous for its monasteries perched atop towering sandstone pillars. These monasteries, built by monks beginning in the 14th century, represent a significant example of Byzantine architecture and are a UNESCO World Heritage site.",
                    DestinationId = Guid.Parse("0e0e2740-9aa7-4316-bc76-5ac57c91ab0d")
                },
                new Landmark
                {
                    Id = Guid.Parse("b57bf2bd-d373-4b40-8144-8ecf1b8a8dfb"),
                    LocationName = "Nafplio",
                    Name = "Palamidi",
                    ImageUrl = "https://ucarecdn.com/6cccfe0a-0316-40df-bce6-a7429f58415d/-/crop/4000x2098/0,75/-/resize/1200x630/-/resize/x300/",
                    Description = "Palamidi Fortress is a Venetian-built castle located on a hill overlooking Nafplio in the Peloponnese region of Greece. It is known for its impressive baroque architecture and strategic location, offering panoramic views of the town and the Argolic Gulf.",
                    DestinationId = Guid.Parse("0e0e2740-9aa7-4316-bc76-5ac57c91ab0d")
                },
                new Landmark
                {
                    Id = Guid.Parse("6826870b-52e3-4e87-9b74-e8840020db59"),
                    LocationName = "Rio de Janeiro",
                    Name = "Christ the Redeemer",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4f/Christ_the_Redeemer_-_Cristo_Redentor.jpg",
                    Description = "The Christ the Redeemer statue in Brazil is a colossal Art Deco-style statue of Jesus Christ, located atop Corcovado Mountain in Rio de Janeiro. It stands 30 meters (98 feet) tall, with a 8-meter (26-foot) pedestal, and has arms that stretch 28 meters (92 feet) wide.",
                    DestinationId = Guid.Parse("bc2131b6-9529-40c5-a963-628410fa0c41")
                },
                new Landmark
                {
                    Id = Guid.Parse("2846bf55-4630-4834-a3cb-9188ae9ebb2f"),
                    LocationName = "Rio de Janeiro",
                    Name = "Escadaria Selarón",
                    ImageUrl = "https://freewalkertours.com/wp-content/uploads/Escalera-Selaron5.jpeg",
                    Description = "Escadaria Selarón, also known as the Selarón Steps, is a vibrant and iconic set of 215 steps in Rio de Janeiro, Brazil. It's a masterpiece of public art created by Chilean-born artist Jorge Selarón, who dedicated it as a tribute to the Brazilian people.",
                    DestinationId = Guid.Parse("bc2131b6-9529-40c5-a963-628410fa0c41")
                },
                new Landmark
                {
                    Id = Guid.Parse("1be37f07-c443-4dfb-8400-f78f6a667de7"),
                    LocationName = "Rio de Janeiro",
                    Name = "Leblon Beach",
                    ImageUrl = "https://mediaim.expedia.com/destination/1/41c8d7b4508417f56a0654d0384603ff.jpg",
                    Description = "Leblon Beach, located in Rio de Janeiro, is known for its upscale atmosphere, family-friendly environment, and beautiful scenery. It's situated in the wealthy Leblon neighborhood, adjacent to Ipanema, but offers a more tranquil and sophisticated experience. ",
                    DestinationId = Guid.Parse("bc2131b6-9529-40c5-a963-628410fa0c41")
                },
                new Landmark
                {
                    Id = Guid.Parse("eb08ee39-2927-4861-bc8c-05c2f1b1a3d0"),
                    LocationName = "Agra",
                    Name = "Taj Mahal",
                    ImageUrl = "https://www.travelandleisure.com/thmb/wdUcyBQyQ0wUVs4wLahp0iWgZhc=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/taj-mahal-agra-india-TAJ0217-9eab8f20d11d4391901867ed1ce222b8.jpg",
                    Description = "The Taj Mahal is a magnificent white marble mausoleum located in Agra, India, renowned for its stunning architecture and as a symbol of enduring love. Built by Mughal emperor Shah Jahan in memory of his wife Mumtaz Mahal, it stands as a masterpiece of Indo-Islamic design.",
                    DestinationId = Guid.Parse("2127672f-0baf-4033-9279-7aa70698dca4")
                },
                new Landmark
                {
                    Id = Guid.Parse("8236660d-2616-425c-82dc-99bb0863307b"),
                    LocationName = "Madurai",
                    Name = "Meenakshi Amman Temple",
                    ImageUrl = "https://i0.wp.com/naedin.click/wp-content/uploads/2021/10/Meenakshi-Amman-Temple-Tamil-Nadu-India-10.jpg?resize=696%2C522&ssl=1",
                    Description = "The Meenakshi Amman Temple in Madurai, India, is a renowned example of Dravidian architecture, dedicated to Goddess Meenakshi and Lord Sundareshwarar. The temple complex is vast, featuring towering gopurams (gateway towers), intricate sculptures, and the famous Hall of a Thousand Pillars.",
                    DestinationId = Guid.Parse("2127672f-0baf-4033-9279-7aa70698dca4")
                },
                new Landmark
                {
                    Id = Guid.Parse("7fd83bf7-24e8-45fb-b1e5-877a0452ba43"),
                    LocationName = "Jaipur",
                    Name = "Amber Palace",
                    ImageUrl = "https://cdn.britannica.com/50/152850-050-2DB7645E/Wall-centre-background-Amer-Palace-Sun-Gate.jpg",
                    Description = "The Amber Palace, also known as Amer Fort, is a stunning example of Rajput architecture located 11 kilometers from Jaipur, India. Built primarily from pale yellow and pink sandstone with white marble, it stands on a hill overlooking Maota Lake.",
                    DestinationId = Guid.Parse("2127672f-0baf-4033-9279-7aa70698dca4")
                }
                );
        }
    }
}
