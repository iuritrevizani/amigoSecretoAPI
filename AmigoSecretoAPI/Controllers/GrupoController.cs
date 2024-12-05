using Microsoft.AspNetCore.Mvc;
using AmigoSecretoAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace AmigoSecretoAPI.Controllers
{
    [ApiController]
    [Route("api/grupos")]
    public class GrupoController : ControllerBase
    {
        private static List<Grupo> grupos = new List<Grupo>();
        private static int contadorId = 1;

        // POST: api/grupos
        [HttpPost]
        public IActionResult CriarGrupo([FromBody] string nome)
        {
            var grupo = new Grupo
            {
                Id = contadorId++, // Incrementa o ID do grupo
                Nome = nome        // Define o nome do grupo
            };

            grupos.Add(grupo); // Adiciona o grupo à lista
            return Ok(grupo);  // Retorna o grupo criado
        }

        // PATCH: api/grupos/{id}/participantes
        [HttpPatch("{id}/participantes")]
        public IActionResult AdicionarParticipantes(int id, [FromBody] List<string> participantes)
        {
            var grupo = grupos.FirstOrDefault(g => g.Id == id);
            if (grupo == null) return NotFound("Grupo não encontrado.");

            grupo.Participantes.AddRange(participantes);
            return Ok(grupo);
        }

        // POST: api/grupos/{id}/matches
        [HttpPost("{id}/matches")]
        public IActionResult GerarMatches(int id)
        {
            var grupo = grupos.FirstOrDefault(g => g.Id == id);
            if (grupo == null) return NotFound("Grupo não encontrado.");
            if (grupo.Participantes.Count < 2) return BadRequest("É necessário pelo menos 2 participantes.");

            var random = new Random();

            // Embaralha os participantes
            var participantes = grupo.Participantes.OrderBy(p => random.Next()).ToList();

            // Gera os Matches fechando o ciclo
            for (int i = 0; i < participantes.Count; i++)
            {
                var presenteador = participantes[i];
                var presenteado = participantes[(i + 1) % participantes.Count]; // O último presenteia o primeiro
                grupo.Matches[presenteador] = presenteado;
            }

            return Ok(grupo.Matches);
        }

        // GET: api/grupos/{id}/matches/quem-presenteio
        [HttpGet("{id}/matches/quem-presenteio")]
        public IActionResult QuemPresenteio(int id, [FromQuery] string participante)
        {
            var grupo = grupos.FirstOrDefault(g => g.Id == id);
            if (grupo == null) return NotFound("Grupo não encontrado.");
            if (!grupo.Matches.ContainsKey(participante)) return NotFound("Participante não encontrado no grupo.");

            return Ok(new { Presenteado = grupo.Matches[participante] });
        }
    }
}
