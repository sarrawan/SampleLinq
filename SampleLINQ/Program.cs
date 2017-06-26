using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SampleLINQ
{
	class Program
	{
		static void Main(string[] args)
		{
			//LessonOne();
			//LessonTwo();
			//Console.ReadLine();


			var assetFlow = new AssetFlowsByYearMonth();
			bool original;
			assetFlow.HaveSecurityAssetFlowsInNonInitialMonth(201706, out original);
			Console.WriteLine($"This is the value from the original method: {original}");

			bool linqAny;
			assetFlow.HaveSecurityLinqAny(201706, out linqAny);
			Console.WriteLine($"This is the value from the linq any method: {linqAny}");

			bool linqSelect;
			assetFlow.HaveSecurityLinqSelectMany(201706, out linqSelect);
			Console.WriteLine($"This is the value from the line select many method: {linqSelect}");

			Console.ReadLine();
		}

		private static void LessonTwo()
		{
			QueryingCustomTypes();
			CreatingCustomProjectionsWithTheDataSourceType();
			CreatingCustomProjectionsOnDifferentType();
			ProjectingAnonymousTypes();
		}

		private static void LessonOne()
		{
			string[] musicalArtists = {"Adele", "Maroon 5", "Avril Lavigne"};

			IEnumerable<string> aArtists =
				from artist in musicalArtists
				where artist.StartsWith("A")
				select artist;

			IEnumerable<string> aArtistsFluent = musicalArtists.Where(a => a.StartsWith("A"));

			foreach (var artist in aArtists)
			{
				Console.WriteLine(artist);
			}

			foreach (var artist in aArtistsFluent)
			{
				Console.WriteLine(artist);
			}
		}

		static List<MusicalArtist> GetMusicalArtists()
		{
			return new List<MusicalArtist>
			{
				new MusicalArtist
				{
					Name = "Adele",
					Genre = "Pop",
					LatestHit = "Someone Like You",
					Albums = new List<Album>
					{
						new Album { Name = "21", Year = "2011" },
						new Album { Name = "19", Year = "2008" },
					}
				},
				new MusicalArtist
				{
					Name = "Maroon 5",
					Genre = "Adult Alternative",
					LatestHit = "Moves Like Jaggar",
					Albums = new List<Album>
					{
						new Album { Name = "Misery", Year = "2010" },
						new Album { Name = "It Won't Be Soon Before Long", Year = "2008" },
						new Album { Name = "Wake Up Call", Year = "2007" },
						new Album { Name = "Songs About Jane", Year = "2006" },
					}
				},
				new MusicalArtist
				{
					Name = "Lady Gaga",
					Genre = "Pop",
					LatestHit = "You And I",
					Albums = new List<Album>
					{
						new Album { Name = "The Fame", Year = "2008" },
						new Album { Name = "The Fame Monster", Year = "2009" },
						new Album { Name = "Born This Way", Year = "2011" },
					}
				}
			};
		}

		static void QueryingCustomTypes()
		{
			List<MusicalArtist> artistsDataSource = GetMusicalArtists();

			IEnumerable<MusicalArtist> artistsResult =
				from artist in artistsDataSource
				select artist;

			IEnumerable<MusicalArtist> artistsResultFluent = artistsDataSource;

			Console.WriteLine("\nQuerying Custom Types");
			Console.WriteLine("---------------------\n");

			foreach (MusicalArtist artist in artistsResult)
			{
				Console.WriteLine(
					"Name: {0}\nGenre: {1}\nLatest Hit: {2}\n",
					artist.Name,
					artist.Genre,
					artist.LatestHit);
			}

			foreach (MusicalArtist artist in artistsResultFluent)
			{
				Console.WriteLine(
					"Name: {0}\nGenre: {1}\nLatest Hit: {2}\n",
					artist.Name,
					artist.Genre,
					artist.LatestHit);
			}
		}

		static void CreatingCustomProjectionsWithTheDataSourceType()
		{
			List<MusicalArtist> artistsDataSource = GetMusicalArtists();

			IEnumerable<MusicalArtist> artistsResult =
				from artist in artistsDataSource
				select new MusicalArtist
				{
					Name = artist.Name,
					LatestHit = artist.LatestHit
				};

			IEnumerable<MusicalArtist> artistsResultFluent = artistsDataSource.Select(a => 
				new MusicalArtist()
				{
					Name = a.Name,
					LatestHit = a.LatestHit
				});

			Console.WriteLine("\nCustom Projection With Data Source Type");
			Console.WriteLine("---------------------------------------\n");

			foreach (MusicalArtist artist in artistsResult)
			{
				Console.WriteLine(
					"Name: {0}\nLatest Hit: {1}\n",
					artist.Name,
					artist.LatestHit);
			}

			foreach (MusicalArtist artist in artistsResultFluent)
			{
				Console.WriteLine(
					"Name: {0}\nLatest Hit: {1}\n",
					artist.Name,
					artist.LatestHit);
			}
		}

		static void CreatingCustomProjectionsOnDifferentType()
		{
			List<MusicalArtist> artistsDataSource = GetMusicalArtists();

			IEnumerable<ArtistViewModel> artistsResult =
				from artist in artistsDataSource
				select new ArtistViewModel
				{
					ArtistName = artist.Name,
					Song = artist.LatestHit
				};

			IEnumerable<ArtistViewModel> artistsResultFluent = artistsDataSource.Select(a =>
				new ArtistViewModel()
				{
					ArtistName = a.Name,
					Song = a.LatestHit
				});

			Console.WriteLine("\nCustom Projection On a Different Type");
			Console.WriteLine("-------------------------------------\n");

			foreach (ArtistViewModel artist in artistsResult)
			{
				Console.WriteLine(
					"Artist Name: {0}\nSong: {1}\n",
					artist.ArtistName,
					artist.Song);
			}

			foreach (ArtistViewModel artist in artistsResultFluent)
			{
				Console.WriteLine(
					"Artist Name: {0}\nSong: {1}\n",
					artist.ArtistName,
					artist.Song);
			}
		}

		private static void ProjectingAnonymousTypes()
		{
			List<MusicalArtist> artistsDataSource = GetMusicalArtists();

			var artistsResult =
				from artist in artistsDataSource
				select new
				{
					Name = artist.Name,
					NumberOfAlbums = artist.Albums.Count
				};

			var artistsResultFluent = artistsDataSource.Select(a =>
				new
				{
					Name = a.Name,
					NumberOfAlbums = a.Albums.Count
				});

			Console.WriteLine("\nProjecting Anonymous Types");
			Console.WriteLine("--------------------------\n");

			foreach (var artist in artistsResult)
			{
				Console.WriteLine(
					"Artist Name: {0}\nNumber of Albums: {1}\n",
					artist.Name,
					artist.NumberOfAlbums);
			}

			foreach (var artist in artistsResultFluent)
			{
				Console.WriteLine(
					"Artist Name: {0}\nNumber of Albums: {1}\n",
					artist.Name,
					artist.NumberOfAlbums);
			}
		}
	}

	public class Album
	{
		public string Name { get; set; }
		public string Year { get; set; }

		public string RecordingLabel { get; set; }
	}

	public class MusicalArtist
	{
		public string Name { get; set; }
		public string Genre { get; set; }
		public string LatestHit { get; set; }
		public List<Album> Albums { get; set; }
	}

	public class ArtistViewModel
	{
		public string ArtistName { get; set; }

		public string Song { get; set; }
	}
}
