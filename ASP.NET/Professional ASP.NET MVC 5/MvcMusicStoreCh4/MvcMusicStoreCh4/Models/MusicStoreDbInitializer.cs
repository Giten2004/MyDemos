using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcMusicStoreCh4.Models
{
    public class MusicStoreDbInitializer : DropCreateDatabaseAlways<MusicStoreDB>
    {
        protected override void Seed(MusicStoreDB context)
        {
            context.Artists.Add(new Artist { Name = "Al Di Meola" });
            context.Genres.Add(new Genre { Name = "Jazz" });
            context.Albums.Add(new Album
            {
                Artist = new Artist { Name = "Rush" },
                Genre = new Genre { Name = "Rock" },
                Price = 9.99m,
                Title = "Caravan"
            });

            base.Seed(context);
        }
    }
}