﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController: CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenerosController(ApplicationDbContext context,
            IMapper mapper)
            :base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            return await Get<Genero, GeneroDTO>();
        }

        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            return await Get<Genero, GeneroDTO>(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(entidad);
            await context.SaveChangesAsync();
            
            var generoDTO = mapper.Map<GeneroDTO>(entidad);
            return new CreatedAtRouteResult("obtenerGenero", new { id = generoDTO.Id }, generoDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);
            if (!existe) return NotFound();

            context.Remove(new Genero { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
