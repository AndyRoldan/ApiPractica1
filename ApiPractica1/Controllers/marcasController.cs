using ApiPractica1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPractica1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcasController : ControllerBase
    {

        private readonly equiposContext _equiposContexto;

        public marcasController(equiposContext equiposContexto)
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
            List<marcas> ListadoMarcas = (from e in _equiposContexto.marcas
                                                  select e).ToList();

            if (ListadoMarcas.Count == 0)
            {
                return NotFound();
            }
            return Ok(ListadoMarcas);
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
            marcas? marcas = (from e in _equiposContexto.marcas
                                              where e.id_marcas == id
                                              select e).FirstOrDefault();

            if (marcas == null)
            {
                return NotFound();
            }
            return Ok(marcas);
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
            marcas? marcas = (from e in _equiposContexto.marcas
                                              where e.nombre_marca.Contains(filtro)
                                              select e).FirstOrDefault();

            if (marcas == null)
            {
                return NotFound();
            }
            return Ok(marcas);
        }


        /// EndPoint que CREA los registros de una tablas 

        [HttpPost]
        [Route("AGREGAR")]
        public IActionResult GuardarEquipo([FromBody] marcas marcas)
        {
            try
            {
                _equiposContexto.marcas.Add(marcas);
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
        public IActionResult ActualizarEquipo(int id, [FromBody] marcas marcasModificas)
        {
            ///Para actualizar un regisro, se obriene el registro original de la base de datos
            ///al cual alteraremos alguna propiedad
            marcas? equipoActual = (from e in _equiposContexto.marcas
                                            where e.id_marcas == id
                                            select e).FirstOrDefault();

            ///Verificamos que estisa el registro segun su ID
            if (equipoActual == null)
            { return NotFound(); }

            ///Si se encuentra el registro, se alteran los campos modificables
            equipoActual.nombre_marca = marcasModificas.nombre_marca;
            equipoActual.estados = marcasModificas.estados;


            ///Se marca el registro como modificado en el contexto
            ///y se envia la modificacion a la base de datos
            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            return Ok(marcasModificas);

        }


        /// EndPoint que ELIMANR los registros de una tablas
        /// 

        [HttpDelete]
        [Route("ELIMINAR")]
        public IActionResult EliminarEqipo(int id)
        {
            ///Para actualizar un regisro, se obriene el registro original de la base de datos
            ///al cual Eliminaremos
            marcas? marcas = (from e in _equiposContexto.marcas
                                              where e.id_marcas == id
                                              select e).FirstOrDefault();

            ///Verificamos que exista el registro segun su ID
            if (marcas == null)
            { return NotFound(); }


            ///Ejecutamos la accion de elimnar el registro

            _equiposContexto.marcas.Attach(marcas);
            _equiposContexto.marcas.Remove(marcas);
            _equiposContexto.SaveChanges();

            return Ok(marcas);

        }

    }
}
