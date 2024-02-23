using ApiPractica1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPractica1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_equipoController : ControllerBase
    {

        private readonly equiposContext _equiposContexto;

        public estados_equipoController(equiposContext equiposContexto)
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
            List<estados_equipo> listadoEquipo = (from e in _equiposContexto.estados_equipo
                                           select e).ToList();

            if (listadoEquipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
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
            estados_equipo? estados_equipo = (from e in _equiposContexto.estados_equipo
                               where e.id_estados_equipo == id
                               select e).FirstOrDefault();

            if (estados_equipo == null)
            {
                return NotFound();
            }
            return Ok(estados_equipo);
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
            estados_equipo? estados_equipo = (from e in _equiposContexto.estados_equipo
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();

            if (estados_equipo == null)
            {
                return NotFound();
            }
            return Ok(estados_equipo);
        }


        /// EndPoint que CREA los registros de una tablas 

        [HttpPost]
        [Route("AGREGAR")]
        public IActionResult GuardarEquipo([FromBody] estados_equipo estados_equipo)
        {
            try
            {
                _equiposContexto.estados_equipo.Add(estados_equipo);
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
        public IActionResult ActualizarEquipo(int id, [FromBody] estados_equipo estados_equipoModificar)
        {
            ///Para actualizar un regisro, se obriene el registro original de la base de datos
            ///al cual alteraremos alguna propiedad
            estados_equipo? equipoActual = (from e in _equiposContexto.estados_equipo
                                     where e.id_estados_equipo == id
                                     select e).FirstOrDefault();

            ///Verificamos que estisa el registro segun su ID
            if (equipoActual == null)
            { return NotFound(); }

            ///Si se encuentra el registro, se alteran los campos modificables
            equipoActual.descripcion = estados_equipoModificar.descripcion;
            equipoActual.estado = estados_equipoModificar.estado;
            

            ///Se marca el registro como modificado en el contexto
            ///y se envia la modificacion a la base de datos
            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(estados_equipoModificar);

        }


        /// EndPoint que ELIMANR los registros de una tablas
        /// 

        [HttpDelete]
        [Route("ELIMINAR")]
        public IActionResult EliminarEqipo(int id)
        {
            ///Para actualizar un regisro, se obriene el registro original de la base de datos
            ///al cual Eliminaremos
            estados_equipo? estados_equipo = (from e in _equiposContexto.estados_equipo
                               where e.id_estados_equipo == id
                               select e).FirstOrDefault();

            ///Verificamos que exista el registro segun su ID
            if (estados_equipo == null)
            { return NotFound(); }


            ///Ejecutamos la accion de elimnar el registro

            _equiposContexto.estados_equipo.Attach(estados_equipo);
            _equiposContexto.estados_equipo.Remove(estados_equipo);
            _equiposContexto.SaveChanges();

            return Ok(estados_equipo);

        }

    }
}
