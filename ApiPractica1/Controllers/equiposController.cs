using ApiPractica1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ApiPractica1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto; 

        public equiposController(equiposContext equiposContexto)
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
            List<equipos> listadoEquipo = (from e in _equiposContexto.equipos 
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
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
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
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

  
        /// EndPoint que CREA los registros de una tablas 
     
        [HttpPost]
        [Route("AGREGAR")]
        public IActionResult GuardarEquipo([FromBody]equipos equipo)
        {
            try
            { 
                _equiposContexto.equipos.Add(equipo);
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
        public IActionResult ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            ///Para actualizar un regisro, se obriene el registro original de la base de datos
            ///al cual alteraremos alguna propiedad
            equipos? equipoActual = (from e in _equiposContexto.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();

            ///Verificamos que estisa el registro segun su ID
            if (equipoActual == null)
            { return NotFound(); }

            ///Si se encuentra el registro, se alteran los campos modificables
            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.id_marcas = equipoModificar.id_marcas;
            equipoActual.id_tipo_equipo = equipoModificar.id_tipo_equipo;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;

            ///Se marca el registro como modificado en el contexto
            ///y se envia la modificacion a la base de datos
            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(equipoModificar);

        }


        /// EndPoint que ELIMANR los registros de una tablas
        /// 

        [HttpDelete]
        [Route("ELIMINAR")]
        public IActionResult EliminarEqipo(int id)
        {
            ///Para actualizar un regisro, se obriene el registro original de la base de datos
            ///al cual Eliminaremos
            equipos? equipo = (from e in _equiposContexto.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();

            ///Verificamos que exista el registro segun su ID
            if (equipo == null)
            { return NotFound(); }

       
            ///Ejecutamos la accion de elimnar el registro
     
            _equiposContexto.equipos.Attach(equipo);
            _equiposContexto.equipos.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);

        }



    }
   
}
