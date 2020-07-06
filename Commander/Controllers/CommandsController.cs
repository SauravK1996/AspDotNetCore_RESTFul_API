using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commander.Controllers
{
    //api/commands
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        //private readonly MockCommanderRepo _repository;
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/commands/{id}
        [HttpGet]
        public ActionResult <IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommand();
            if (commandItems == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        //GET api/commands/{id}
        [HttpGet("{id}",Name = "GetCommandById")]
        public ActionResult <Command> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem == null)
                return NotFound();

            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        //POST api/commands
        [HttpPost]
        public ActionResult <CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);

            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);

            //return Ok(commandReadDto);
        }

        //PUT api/command/{5}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommadUpdateDto commadUpdateDto)
        {
            var commandModelFormRepo = _repository.GetCommandById(id);

            if(commandModelFormRepo == null)
            {
                return NotFound();
            }


            _mapper.Map(commadUpdateDto, commandModelFormRepo);

            _repository.UpdateCommand(commandModelFormRepo);

            _repository.SaveChanges();

            return NoContent();

        }

        //PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommadUpdateDto> patchDoc)
        {
            var commandModelFormRepo = _repository.GetCommandById(id);

            if (commandModelFormRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommadUpdateDto>(commandModelFormRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFormRepo);

            _repository.UpdateCommand(commandModelFormRepo);

            _repository.SaveChanges();

            return NoContent();
        }       
    
        
        //DELETE api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFormRepo = _repository.GetCommandById(id);

            if (commandModelFormRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteCommand(commandModelFormRepo);

            _repository.SaveChanges();

            return NoContent();
        }
    
    }
}
