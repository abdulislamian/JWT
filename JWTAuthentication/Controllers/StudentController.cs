using AutoMapper;
using JWTAuthentication.Data;
using JWTAuthentication.Models;
using JWTAuthentication.Models.DTO;
using JWTAuthentication.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly JWTDbContext dbContext;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public StudentController(JWTDbContext dbContext, IStudentRepository studentRepository, IMapper mapper,
            UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository,IHttpContextAccessor _httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            _userManager = userManager;
            this.tokenRepository = tokenRepository;
            httpContextAccessor = _httpContextAccessor;
        }

        #region GellAllStudent
        [HttpGet]
        [Authorize(Policy = "AllUser")]
        public async Task<IActionResult> GetAll()
        {
            var students = await studentRepository.GetAllAsync();

            //Populate DTO with Domain Model (Student)
            var studentDTO = mapper.Map<List<StudentDTO>>(students);

            //Return DTO
            return Ok(studentDTO);
        }
        #endregion

        #region GetStudentById
        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Policy = "AllUser")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var studentDomain = await studentRepository.GetByIdAsync(id);
            if (studentDomain == null)
            {
                return NotFound();
            }
            //Map Student Domain with Student DTO
            //var studentDTO = new StudentDTO
            //{
            //    Id = studentDomain.Id,
            //    Name = studentDomain.Name,
            //    Address = studentDomain.Address
            //};
            return Ok(mapper.Map<StudentDTO>(studentDomain));
        }
        #endregion

        #region CreateStudent
        //POST
        [HttpPost]
        //[ValidateModel]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Create([FromBody] AddStudentRequestDto AddStudentRequestDto)
        {
            var studentDomainModel = mapper.Map<Student>(AddStudentRequestDto);

            //Use Domain Model to create Region
            studentDomainModel = await studentRepository.CreateAsync(studentDomainModel);

            //Convert Back Region to regionDTO
            var studentDTO = mapper.Map<StudentDTO>(studentDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = studentDTO.Id }, studentDTO);
        }
        #endregion

        #region UpdateStudent
        //PUT: https://localhost:7296/api/student/{id}
        [HttpPut]
        [Route("{id:int}")]
        //[ValidateModel]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStudentRequestDTO updateStudentRequestDTO)
        {
            //Map DTO to Domain Model
            var studentDomainModel = mapper.Map<Student>(updateStudentRequestDTO);
            await studentRepository.UpdateAsync(id, studentDomainModel);
            if (studentDomainModel == null)
            {
                return NotFound();
            }

            //Convert Domain Model to DTO
            var studentDTO = mapper.Map<StudentDTO>(studentDomainModel);
            return Ok(studentDTO);
        }
        #endregion

        #region DeleteStudent
        //Delete : https://localhost:7296/api/student/{id}
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Policy = "OnlyAdmin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var studentDomainModel = await studentRepository.DeleteAsync(id);
            if (studentDomainModel == null)
            {
                return NotFound();
            }

            //If want you can also return deleted Object Back
            var regionDTO = mapper.Map<StudentDTO>(studentDomainModel);
            return Ok(regionDTO);

        }
        #endregion
    }
}
