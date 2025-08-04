using System.Net;
using CodeTest.Models.Dto;
using CodeTest.Services;
using MCodeTest.Models;
using Microsoft.AspNetCore.Mvc;


namespace CodeTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoService _todoService;
        protected APIResponse _response;

        public TodoController(TodoService todoService)
        {
            _todoService = todoService;
            this._response = new();
        }

        [HttpGet]
        public ActionResult<APIResponse> GetAll([FromQuery] bool? isCompleted, [FromQuery] DateOnly? startDueDate, [FromQuery] DateOnly? endDueDate)
        {

            try
            {
                var todoItems = _todoService.GetAll(isCompleted, startDueDate, endDueDate);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = todoItems.GroupBy(p => p.DueDate)
                                            .Select(p => new
                                            {
                                                dueDate = p.Key,
                                                items = p.ToList()
                                            })
                                            .OrderBy(p => p.dueDate)
                                            .ToList();

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }
            return _response;

        }

        [HttpPut("{id:int}", Name = "updateTodo")]
        public ActionResult<APIResponse> UpdateTodo(int id, [FromBody] TodoUpdateDTO todoDto)
        {

            if (todoDto == null || id != todoDto.Id)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage.Add("Invalid Todo item data.");
                return BadRequest(_response);
            }
            try
            {
                var isUpdated = _todoService.updateTodo(id, todoDto);
                if (!isUpdated)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage.Add("Todo item not found.");
                    return NotFound(_response);
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return NoContent();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessage.Add(ex.Message);
                return StatusCode((int)_response.StatusCode, _response);
            }
        }


        [HttpPost]
        public ActionResult<APIResponse> CreateTodo([FromBody] TodoCreateDTO todoCreateDTO)
        {
            if (todoCreateDTO == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessage.Add("Invalid Todo item data.");
                return BadRequest(_response);
            }
            try
            {
                var createdTodoItem = _todoService.createDodo(todoCreateDTO);
                if (createdTodoItem == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    _response.ErrorMessage.Add("Failed to create Todo item.");
                    return StatusCode((int)_response.StatusCode, _response);
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = createdTodoItem;
                return Created("", _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessage.Add(ex.Message);
                return StatusCode((int)_response.StatusCode, _response);
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<APIResponse> DeleteTodo(int id)
        {


            try
            {
                var isDeleted = _todoService.deleteTodo(id);
                if (!isDeleted)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage.Add("Todo item not found.");
                    return NotFound(_response);
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return NoContent();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessage.Add(ex.Message);
                return StatusCode((int)_response.StatusCode, _response);
            }
        }
    }
}
