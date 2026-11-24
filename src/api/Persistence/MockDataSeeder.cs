using MediaArtArchive.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace MediaArtArchive.Api.Persistence;

public static class MockDataSeeder
{
    private const string SeedName = "mock-content-v1";

    public static async Task SeedAsync(ArchiveDbContext db, CancellationToken ct = default)
    {
        if (await db.DataSeeds.AnyAsync(x => x.Name == SeedName, ct)) return;

        var strategy = db.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await db.Database.BeginTransactionAsync(ct);
            var artists = new Dictionary<string, Artist>();
            foreach (var seed in ArtistSeeds)
            {
            var artist = await db.Artists.SingleOrDefaultAsync(x => x.DisplayName == seed.DisplayName, ct);
            if (artist is null)
            {
                artist = new Artist
                {
                    DisplayName = seed.DisplayName,
                    SortName = seed.DisplayName,
                    Biography = seed.Biography,
                    Location = seed.Location,
                    ActiveYears = seed.ActiveYears
                };
                db.Artists.Add(artist);
            }

            artist.DisplayNameUk = seed.DisplayNameUk;
            artist.SortNameUk = seed.DisplayNameUk;
            artist.BiographyUk = seed.BiographyUk;
            artist.LocationUk = seed.LocationUk;
            artist.ActiveYearsUk = seed.ActiveYearsUk;
                artists[seed.Key] = artist;
            }

        await db.SaveChangesAsync(ct);

        foreach (var seed in WorkSeeds)
        {
            var artist = artists[seed.ArtistKey];
            var work = new Work
            {
                Title = seed.Title,
                TitleUk = seed.TitleUk,
                CreationDate = seed.CreationDate,
                Medium = seed.Medium,
                MediumUk = seed.MediumUk,
                Description = seed.Description,
                DescriptionUk = seed.DescriptionUk,
                City = seed.City,
                CityUk = seed.CityUk,
                Context = seed.Context,
                ContextUk = seed.ContextUk,
                Accent = seed.Accent,
                Featured = seed.Featured
            };
            work.Artists.Add(new WorkArtist { Work = work, Artist = artist, ArtistId = artist.Id, SortOrder = 0 });
            db.Works.Add(work);
        }

        db.Articles.AddRange(
            new Article { Title = "Reading Unstable Media", TitleUk = "Читаючи нестабільні медіа", Byline = "Kateryna S.", BylineUk = "Катерина С.", PublishedAt = new(2025, 5, 14), Category = "Essay", CategoryUk = "Есе", Excerpt = "A placeholder essay on preserving works whose technologies, interfaces, and contexts continue to change.", ExcerptUk = "Макетне есе про збереження робіт, технології, інтерфейси та контексти яких продовжують змінюватися." },
            new Article { Title = "Networks of Care: On Archiving Digital Practice", TitleUk = "Мережі турботи: про архівування цифрової практики", Byline = "Andrii P.", BylineUk = "Андрій П.", PublishedAt = new(2025, 3, 28), Category = "Research note", CategoryUk = "Дослідницька нотатка", Excerpt = "How can an archive record not only an artwork, but also the relationships and acts of maintenance around it?", ExcerptUk = "Як архів може зафіксувати не лише твір, а й стосунки та практики підтримки довкола нього?" },
            new Article { Title = "A Conversation About Transmission", TitleUk = "Розмова про передавання", Byline = "Marta D.", BylineUk = "Марта Д.", PublishedAt = new(2025, 2, 7), Category = "Interview", CategoryUk = "Інтерв’ю", Excerpt = "A representative conversation about radio, listening, distance, and the social life of technical media.", ExcerptUk = "Демонстраційна розмова про радіо, слухання, відстань і суспільне життя технічних медіа." }
        );

        db.Events.AddRange(
            new ArchiveEvent { Title = "Archive Assembly: Describing Media Art", TitleUk = "Архівна асамблея: опис медіамистецтва", EventDate = new(2026, 10, 12), Location = "Kyiv", LocationUk = "Київ", Venue = "Placeholder venue", VenueUk = "Макетний майданчик", Status = "Upcoming", Description = "A working session on language, metadata, and responsible description in media art archives.", DescriptionUk = "Робоча сесія про мову, метадані та відповідальний опис в архівах медіамистецтва." },
            new ArchiveEvent { Title = "Software as Cultural Memory", TitleUk = "Програмне забезпечення як культурна пам’ять", EventDate = new(2026, 11, 21), Location = "Online", LocationUk = "Онлайн", Venue = "Public programme", VenueUk = "Публічна програма", Status = "Upcoming", Description = "A public conversation about conserving software-based and networked artworks.", DescriptionUk = "Публічна розмова про збереження програмних і мережевих творів мистецтва." },
            new ArchiveEvent { Title = "Moving Image: Early Experiments", TitleUk = "Рухоме зображення: ранні експерименти", EventDate = new(2025, 4, 18), Location = "Lviv", LocationUk = "Львів", Venue = "Placeholder institution", VenueUk = "Макетна інституція", Status = "Past", Description = "A representative screening and discussion programme for the historical-events layout.", DescriptionUk = "Демонстраційний показ і дискусійна програма для макета історичних подій." }
        );

        db.NewsItems.AddRange(
            new NewsItem { PublishedAt = new(2025, 6, 2), Title = "Cataloguing pilot begins", TitleUk = "Розпочато пілотне каталогування", Summary = "The revival project begins testing a new descriptive framework with a small set of placeholder records.", SummaryUk = "Команда відновлення тестує нову систему опису на невеликій групі макетних записів." },
            new NewsItem { PublishedAt = new(2025, 4, 19), Title = "Research notes now available", TitleUk = "Опубліковано дослідницькі нотатки", Summary = "The first group of methodological notes has been added to the developing archive.", SummaryUk = "До архіву, що розвивається, додано першу групу методологічних нотаток." },
            new NewsItem { PublishedAt = new(2025, 2, 3), Title = "Archive revival project announced", TitleUk = "Оголошено проєкт відновлення архіву", Summary = "A new phase of research and technical work begins around the Ukrainian Media Art Archive.", SummaryUk = "Розпочинається новий етап дослідницької та технічної роботи над Українським Медіа Арт Архівом." }
        );

        db.DataSeeds.Add(new DataSeed { Name = SeedName });
        await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        });
    }

    private static readonly ArtistSeed[] ArtistSeeds =
    [
        new("olena-m", "Olena M.", "Олена М.", "Kyiv, Ukraine", "Київ, Україна", "Active since 2014", "Працює з 2014 року", "Placeholder biography for a multidisciplinary artist working with generative moving image, installation, and digital archives.", "Макетна біографія міждисциплінарної мисткині, яка працює з генеративним рухомим зображенням, інсталяцією та цифровими архівами."),
        new("taras-k", "Taras K.", "Тарас К.", "Odesa, Ukraine", "Одеса, Україна", "Active since 2011", "Працює з 2011 року", "Placeholder biography for an artist and researcher exploring sound, transmission, and coastal ecologies.", "Макетна біографія митця й дослідника, який вивчає звук, передавання та прибережні екології."),
        new("ira-b", "Ira B.", "Іра Б.", "Lviv, Ukraine", "Львів, Україна", "Active since 2017", "Працює з 2017 року", "Placeholder biography for an artist whose web-based and photographic practice considers memory and unstable collections.", "Макетна біографія мисткині, чиї веброботи та фотографічна практика осмислюють пам’ять і нестабільні колекції."),
        new("maksym-h", "Maksym H.", "Максим Г.", "Kharkiv, Ukraine", "Харків, Україна", "Active since 2009", "Працює з 2009 року", "Placeholder biography for a moving-image artist documenting technological systems and everyday urban space.", "Макетна біографія митця рухомого зображення, який документує технологічні системи та повсякденний міський простір.")
    ];

    private static readonly WorkSeed[] WorkSeeds =
    [
        new("olena-m", "Signal Garden", "Сад сигналів", "2018", "Generative video", "Генеративне відео", "Kyiv", "Київ", "Independent practice", "Незалежна практика", "A placeholder record for a moving-image work built from generative signals and fragments of an imagined urban broadcast.", "Макетний запис про рухоме зображення, побудоване з генеративних сигналів і фрагментів уявного міського мовлення.", "#9fb4aa", true),
        new("taras-k", "Black Sea Frequency", "Чорноморська частота", "2020", "Sound installation", "Звукова інсталяція", "Odesa", "Одеса", "Research residency", "Дослідницька резиденція", "A placeholder sound work considering distance, transmission, and the shifting acoustic landscape of the Black Sea coast.", "Макетна звукова робота про відстань, передавання та мінливий акустичний ландшафт чорноморського узбережжя.", "#73828f", true),
        new("ira-b", "Soft Archive", "М’який архів", "2022", "Interactive web work", "Інтерактивна вебробота", "Lviv", "Львів", "Online commission", "Онлайн-замовлення", "An imagined browser-based archive where personal memory is reorganised through touch, repetition, and disappearance.", "Уявний браузерний архів, де особиста пам’ять реорганізується через дотик, повторення та зникнення.", "#b59a86", true),
        new("maksym-h", "Field Notes № 04", "Польові нотатки № 04", "2016", "Single-channel video", "Одноканальне відео", "Kharkiv", "Харків", "Artist-run space", "Простір, керований митцями", "A placeholder documentary study of overlooked technical and civic infrastructures.", "Макетне документальне дослідження непомітних технічних і міських інфраструктур.", "#8e8979", false),
        new("olena-m", "Common Ground", "Спільна основа", "2019", "Interactive installation", "Інтерактивна інсталяція", "Kyiv", "Київ", "Museum exhibition", "Музейна виставка", "An imagined interactive environment responding to the movement and proximity of visitors.", "Уявне інтерактивне середовище, що реагує на рух і близькість відвідувачів.", "#b3aa91", false),
        new("ira-b", "Afterimage", "Післяобраз", "2023", "Digital photography", "Цифрова фотографія", "Lviv", "Львів", "Independent practice", "Незалежна практика", "A placeholder photographic sequence about image persistence and mediated recollection.", "Макетна фотосерія про тривалість образу та опосередковане пригадування.", "#9898a7", false)
    ];

    private sealed record ArtistSeed(string Key, string DisplayName, string DisplayNameUk, string Location, string LocationUk,
        string ActiveYears, string ActiveYearsUk, string Biography, string BiographyUk);
    private sealed record WorkSeed(string ArtistKey, string Title, string TitleUk, string CreationDate, string Medium,
        string MediumUk, string City, string CityUk, string Context, string ContextUk, string Description,
        string DescriptionUk, string Accent, bool Featured);
}
