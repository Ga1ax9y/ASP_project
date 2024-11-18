using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253505_Stanishewski.API.Data;
using WEB_253505_Stanishewski.API.Services.GameService;
using WEB_253505_Stanishewski.Domain.Entities;
using WEB_253505_Stanishewski.Domain.Models;

namespace WEB_253505_Stanishewski.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // GET: api/Games/category/{category}?pageNo=1&pageSize=3
        [HttpGet("category/{category}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<List<Game>>>> GetGamesByCategory(string category, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _gameService.GetProductListAsync(
                category,
                pageNo,
                pageSize));
        }

        // GET: api/Games
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<List<Game>>>> GetGames(string? category,int pageNo = 1,int pageSize = 3)
        {
            return Ok(await _gameService.GetProductListAsync(
           category,
           pageNo,
           pageSize));
        }


        // GET: api/Games/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _gameService.GetProductByIdAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }
            await _gameService.UpdateProductAsync(id, game);

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<Game>>> PostGame(Game game)
        {
            var result = await _gameService.CreateProductAsync(game);
            return Ok(result);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            await _gameService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/SaveImage")]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<string>>> SaveImage(int id, IFormFile formFile)
        {
            var result = await _gameService.SaveImageAsync(id, formFile);
            return Ok(result);
        }
    }
}
