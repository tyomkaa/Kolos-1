using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/medicaments")]
    [ApiController]
    public class MedicamentController : ControllerBase
    {
        private readonly IDBService dBService;

        public MedicamentController(IDBService dBService)
        {
            this.dBService = dBService;
        }

        [HttpGet("{IdMedicament}")]
        public async Task<IActionResult> GetMedicament(int idMedicament)
        {
            List<Medicament> meds = null;

            try
            {
                meds = (List<Medicament>)dBService.GetMedicament(idMedicament);
            }catch(Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
            return Ok(meds);
        }
    }
}
