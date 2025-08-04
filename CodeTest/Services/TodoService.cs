using CodeTest.Models;
using CodeTest.Models.Dto;

namespace CodeTest.Services
{
    public class TodoService
    {

        private readonly List<TodoItem> _todoItems = new List<TodoItem> {
            new TodoItem { Id = 1, Title = "Do code testing", IsCompleted = false, DueDate =DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)) },
            new TodoItem { Id = 2, Title = "Walk the dog", IsCompleted = false, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) },
            new TodoItem { Id = 3, Title = "Play tennis", IsCompleted = true, DueDate = DateOnly.FromDateTime(DateTime.UtcNow) },
            new TodoItem { Id = 4, Title = "Read a book", IsCompleted = false, DueDate =DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)) },
            new TodoItem { Id =5, Title = "Buy groceries", IsCompleted = false, DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)) },
            new TodoItem { Id = 6, Title = "Clean the house", IsCompleted = true, DueDate = DateOnly.FromDateTime(DateTime.UtcNow) },
        };


        public List<TodoItem> GetAll(bool? isCompleted = null, DateOnly? startDueDate = null, DateOnly? endDueDate = null)
        {
            var query = _todoItems.AsQueryable();

            if (isCompleted.HasValue)
            {
                query = query.Where(item => item.IsCompleted == isCompleted.Value);
            }

            if (startDueDate.HasValue)
            {
                query = query.Where(item => item.DueDate >= startDueDate.Value);
            }

            if (endDueDate.HasValue)
            {
                query = query.Where(item => item.DueDate <= endDueDate.Value);
            }

            return query.ToList();


        }
        public bool updateTodo(int id, TodoUpdateDTO todoDto)
        {
            var item = _todoItems.FirstOrDefault(x => x.Id == id);
            if (item == null) return false;

            item.Title = todoDto.Title;
            item.IsCompleted = todoDto.IsCompleted;

            return true;
        }

        public TodoItem createDodo(TodoCreateDTO todoCreateDTO)
        {
            int currentMaxId = _todoItems.Count > 0 ? _todoItems.Max(x => x.Id) : 0;
            var newItem = new TodoItem
            {
                Id = currentMaxId + 1,
                Title = todoCreateDTO.Title,
                IsCompleted = false,
                DueDate = todoCreateDTO.DueDate
            };
            _todoItems.Add(newItem);
            return newItem;
        }

        public bool deleteTodo(int id)
        {
            var item = _todoItems.FirstOrDefault(x => x.Id == id);
            if (item == null) return false;
            _todoItems.Remove(item);
            return true;
        }
    }
}
