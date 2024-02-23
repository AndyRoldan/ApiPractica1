using ApiPractica1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPractica1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipo_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public tipo_equipoController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        ///<sumary>
        /// EndPoint que retorna el liustado de todos los equipos existentes
        /// </sumary>
        /// <returns></returns>
        [HttpGet]
        [Route("OBTENER TODO")]
        public IActionResult Get()
        {
            List<tipo_equipo> tipo_Equipos = (from e in _equiposContexto.tipo_equipo
                                          select e).ToList();

            if (tipo_Equipos.Count == 0)
            {
                return NotFound();
            }
            return Ok(tipo_Equipos);
        }

        ///<sumary>
        /// EndPoint que retorna los registros de una tablas filtradas por su ID
        /// </sumary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("BUSCAR POR ID")]
        public IActionResult Get(int id)
        {
            tipo_equipo? tipo = (from e in _equiposContexto.tipo_equipo
                              where e.id_tipo_equipo == id
                              select e).FirstOrDefault();

            if (tipo == null)
            {
                return NotFound();
            }
            return Ok(tipo);
        }


        ///<sumary>
        /// EndPoint que retorna los registros de una tablas filtradas por su descripcion
        /// </sumary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("BUSCAR POR DESCRIPCION")]
        public IActionResult FindByDescription(string filtro)
        {
            tipo_equipo? tipo = (from e in _equiposContexto.tipo_equipo
                              where e.descripcion.Contains(filtro)
                              select e).FirstOrDefault();

            if (tipo == null)
            {
                return NotFound();
            }
            return Ok(tipo);
        }


        /// EndPoint que CREA los registros de una tablas 

        [HttpPost]
        [Route("AGREGAR")]
        public IActionResult GuardarEquipo([FromBody] tipo_equipo tipo_equipo)
        {
            try
            {
                _equiposContexto.tipo_equipo.Add(tipo_equipo);
                _equiposContexto.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// EndPoint que MODIFICAR los registros de una tablas
        /// 


        [HttpPut]
        [Route("ACTUALIZAR")]
        public IActionResult ActualizarEquipo(int id, [FromBody] tipo_equipo tipoModificar)
        {
            ///Para actualizar un regisro, se obriene el registro original de la base de datos
            ///al cual alteraremos alguna propiedad
            tipo_equipo? equipoActual = (from e in _equiposContexto.tipo_equipo
                                    where e.id_tipo_equipo == id
                                    select e).FirstOrDefault();

            ///Verificamos que estisa el registro segun su ID
            if (equipoActual == null)
            { return NotFound(); }

            ///Si se encuentra el registro, se alteran los campos modificables
            equipoActual.descripcion = tipoModificar.descripcion;
            equipoActual.estado = tipoModificar.estado;


            ///Se marca el registro como modificado en el contexto
            ///y se envia la modificacion a la base de datos
            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(tipoModificar);

        }


        /// EndPoint que ELIMANR los registros de una tablas
        /// 

        [HttpDelete]
        [Route("ELIMINAR")]
        public IActionResult EliminarEqipo(int id)
        {
            ///Para actualizar un regisro, se obriene el registro original de la base de datos
            ///al cual Eliminaremos
            tipo_equipo? tipo_equipo = (from e in _equiposContexto.tipo_equipo
                              where e.id_tipo_equipo == id
                              select e).FirstOrDefault();

            ///Verificamos que exista el registro segun su ID
            if (tipo_equipo == null)
            { return NotFound(); }


            ///Ejecutamos la accion de elimnar el registro

            _equiposContexto.tipo_equipo.Attach(tipo_equipo);
            _equiposContexto.tipo_equipo.Remove(tipo_equipo);
            _equiposContexto.SaveChanges();

            return Ok(tipo_equipo);

        }
    }
}
