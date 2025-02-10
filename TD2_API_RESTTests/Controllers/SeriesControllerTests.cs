using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TD2_API_REST.Controllers;
using TD2_API_REST.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace TD2_API_REST.Tests
{
    [TestClass]
    public class SeriesControllerTests
    {
        private SeriesDbContext _context;
        private SeriesController _controller;

        [TestInitialize]
        public void Setup()
        {


            var builder = new DbContextOptionsBuilder<SeriesDbContext>().UseNpgsql("SeriesDB");
            SeriesDbContext context = new SeriesDbContext(builder.Options);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Series.AddRange(new List<Serie>
            {
                new Serie { Serieid = 1, Titre = "Breaking Bad", Resume = "Walter White devient criminel." },
                new Serie { Serieid = 2, Titre = "Game of Thrones", Resume = "La lutte pour le trône de fer." },
                new Serie { Serieid = 3, Titre = "Stranger Things", Resume = "Enfants vs créatures surnaturelles." }
            });

            _context.SaveChanges();

            _controller = new SeriesController(_context);
        }

        [TestMethod]
        public async Task GetSeries_ReturnsCorrectData()
        {
            var result = await _controller.GetSeries();

            Assert.IsNotNull(result.Value);
            var seriesList = result.Value.Where(s => s.Serieid <= 3).ToList();
            Assert.AreEqual(3, seriesList.Count);
            Assert.AreEqual("Breaking Bad", seriesList[0].Titre);
            Assert.AreEqual("Game of Thrones", seriesList[1].Titre);
            Assert.AreEqual("Stranger Things", seriesList[2].Titre);
        }

        [TestMethod]
        public async Task GetSerie_WithValidId_ReturnsSerie()
        {
            
            var result = await _controller.GetSerie(1);
            var value = result.Result as OkObjectResult;
            var serie = value?.Value as Serie;

            
            Assert.IsNotNull(serie);
            Assert.AreEqual(1, serie.Serieid);
            Assert.AreEqual("Breaking Bad", serie.Titre);
        }

        [TestMethod]
        public async Task GetSerie_WithInvalidId_ReturnsNotFound()
        {
            var result = await _controller.GetSerie(99);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task DeleteSerie_WithValidId_RemovesSerie()
        {
            var result = await _controller.DeleteSerie(1);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsFalse(_context.Series.Any(s => s.Serieid == 1));
        }

        [TestMethod]
        public async Task DeleteSerie_WithInvalidId_ReturnsNotFound()
        {
            var result = await _controller.DeleteSerie(99);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PostSerie_ValidData_CreatesSerie()
        {
            var newSerie = new Serie { Titre = "The Witcher", Resume = "Geralt de Riv chasse des monstres." };

            var result = await _controller.PostSerie(newSerie);
            var createdAtAction = result.Result as CreatedAtActionResult;
            var createdSerie = createdAtAction?.Value as Serie;

            Assert.IsNotNull(createdSerie);
            Assert.AreEqual("The Witcher", createdSerie.Titre);
            Assert.AreEqual(4, createdSerie.Serieid);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public async Task PostSerie_MissingTitle_ThrowsException()
        {
            var newSerie = new Serie { Resume = "Pas de titre" };

            await _controller.PostSerie(newSerie);
        }

        [TestMethod]
        public async Task PutSerie_ValidUpdate_UpdatesSerie()
        {
            var updatedSerie = new Serie { Serieid = 2, Titre = "GOT Updated", Resume = "Mise à jour du résumé" };

            var result = await _controller.PutSerie(2, updatedSerie);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.AreEqual("GOT Updated", _context.Series.Find(2).Titre);
        }

        [TestMethod]
        public async Task PutSerie_InvalidId_ReturnsNotFound()
        {
            var updatedSerie = new Serie { Serieid = 99, Titre = "Inexistante", Resume = "N'existe pas" };

            var result = await _controller.PutSerie(99, updatedSerie);

            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
