namespace TreeService.Domain.Shared;

public static class Errors
{
    public static class Node
    {
        /// <summary>
        /// Нода не найдена
        /// </summary>
        /// <param name="id">Номер узла</param>
        /// <returns>Возвращает ошибку типа 404</returns>
        public static Error NotFound(Guid id)
        {
            return Error.NotFound("node.not.found", $"Node with ID '{id}' was not found.");
        }

        /// <summary>
        /// Нода родителя не найдена
        /// </summary>
        /// <param name="id">Номер ноды</param>
        /// <returns>Возвращает ошибку типа 400</returns>
        public static Error ParentNotFound(Guid id)
        {
            return Error.Validation("node.parent.not.found", $"Parent node with ID '{id}' does not exist.");
        }

        /// <summary>
        /// Циклическая ссылка
        /// </summary>
        /// <param name="id">Номер узла</param>
        /// <returns>Возвращает ошибку типа 400</returns>
        public static Error CircularReference(Guid id)
        {
            return Error.Validation("node.circular.reference", $"Node with ID '{id}' cannot reference itself as a parent.");
        }

        /// <summary>
        /// Дочерний узел уже существует
        /// </summary>
        /// <param name="id">Номер узла</param>
        /// <returns>Возвращает ошибку типа 409</returns>
        public static Error ChildAlreadyExists(Guid id)
        {
            return Error.Conflict("node.child.already.exists", $"Child node with ID '{id}' already exists in the tree.");
        }

        /// <summary>
        /// Обязательное поле
        /// </summary>
        /// <param name="field">Название поля</param>
        /// <returns>Возвращает ошибку типа 400</returns>
        public static Error ValueIsRequired(string? field = null)
        {
            var name = field ?? "field";
            return Error.Validation("value.required", $"The {name} is required.");
        }

        /// <summary>
        /// Неверное значение
        /// </summary>
        /// <param name="field">Название поля</param>
        /// <param name="message">Описание ошибки</param>
        /// <returns></returns>
        public static Error ValueIsInvalid(string? field = null, string? message = null)
        {
            var name = field ?? "field";
            var messageText = message ?? "The value is invalid.";
            return Error.Validation("value.invalid", $"{name} is invalid. {messageText}");

        }
    }
}
