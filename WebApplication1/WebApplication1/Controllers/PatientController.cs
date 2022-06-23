using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IDBService dBService;

        public PatientController(IDBService dBService)
        {
            this.dBService = dBService;
        }

        [HttpDelete("{idPatient}")]
        public async Task<IActionResult> Deletepatient(int idPatient)
        {
            try
            {
                dBService.DeletePatient(idPatient);
            }catch(Exception ex)
            {
                return StatusCode(404, ex.Message);
            }

            return Ok("Patient has been deleted");
        }
    }
}
